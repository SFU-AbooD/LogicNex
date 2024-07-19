using LogicNexBackend.Dbcontexts;
using LogicNexBackend.Models;
using LogicNexBackend.utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace LogicNexBackend.Repositories
{
    public enum Status { 
        NOT_UNIQUE,
        NOT_FOUND,
        OK,
        EXISTS,
        INSERT_ISSUE
    }
    public class UserRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<MongoDbuser> _userCollection;
        private readonly IMongoCollection<emailConfirmation> _emailConfirmationCollection;
        public UserRepository(MongoDbContext context)
        {
            _context = context;
            _userCollection = _context._database.GetCollection<MongoDbuser>("Users");
            _emailConfirmationCollection = _context._database.GetCollection<emailConfirmation>("EmailConfirmations");
        }
        public async Task<MongoDbuser?> getUsername(string email) {
            FilterDefinition<MongoDbuser> find_by_email = Builders<MongoDbuser>
                                               .Filter.Eq(x => x.Email, email);
            MongoDbuser? user = await _userCollection.Find(find_by_email).FirstOrDefaultAsync();
            if(user == null)
                return null;
            else
                return user;
        }
            public async Task<Status> RegisterUser(MongoDbuser user,IEmailSender EmailSender) {
                try
                {
                     await _userCollection.InsertOneAsync(user);
                     string confirmation_id = Utils.Generate_random_email_validation();
                     emailConfirmation store_confirm = new emailConfirmation() {
                         _id = user._id,
                         confirmation_key = confirmation_id,
                };
                      await _emailConfirmationCollection.InsertOneAsync(store_confirm);
                      await EmailSender.SendEmailAsync(user.Email,"Email Confirmation", $"click here to activate your account <a href='https://localhost:7179/api/User/confirm?link={confirmation_id}'>click here</a>");
                      return Status.OK;
                }
                catch (Exception) {
                    return Status.INSERT_ISSUE;
                }
            }
        public async Task<bool> confirmUserEmail(string token) {
            try
            {
                FilterDefinition<emailConfirmation> find_by_token = Builders<emailConfirmation>
                                                                    .Filter.Eq(x => x.confirmation_key, token);
                emailConfirmation row = await _emailConfirmationCollection.Find(find_by_token).FirstAsync();
                FilterDefinition<MongoDbuser> find_user = Builders<MongoDbuser>.Filter.Eq(x => x._id, row._id);
                UpdateDefinition<MongoDbuser> updated_user = Builders<MongoDbuser>.Update.Set(x => x.Email_Confimerd, true);
                UpdateResult result = await _userCollection.UpdateOneAsync(find_user, updated_user);
                await _emailConfirmationCollection.DeleteOneAsync(find_by_token);
                return result.ModifiedCount == 1 ? true : false;
            }
            catch (Exception) {
                return false;
            }
        }
        public async Task<MongoDbuser?> Get_User(string email)
        {
            FilterDefinition<MongoDbuser> get_user_filter = Builders<MongoDbuser>.Filter.
                Eq(x=>x.Email, email);
            ProjectionDefinition<MongoDbuser> limit_data = Builders<MongoDbuser>.Projection
                .Exclude(f=>f.Refresh_tokens)
                .Exclude(f=>f._id);
            MongoDbuser user = await _userCollection.Find(get_user_filter).Project<MongoDbuser>(limit_data).FirstOrDefaultAsync();
            return user ?? null;
        }
        public async Task<MongoDbuser?> User_Login(UserLogin User_formation) {
            MongoDbuser? user = await Get_User(User_formation.email);
            if (user == null)
                return null;
            else {
                if (user.Password_hash == User_formation.password)
                    return user;
                else { 
                    return null;
                }
            }
        }
        public async Task<refreshTokenModel?> getRefreshToken(string email, string token_key) {
            FilterDefinition<MongoDbuser> find_token = Builders<MongoDbuser>.Filter.Eq(x=>x.Email,email) & Builders<MongoDbuser>.Filter.
                Where(x => x.Refresh_tokens.Any(c => c.Key == token_key));
            MongoDbuser? model = await _userCollection.Find(find_token).FirstOrDefaultAsync();
            if (model != null)
            {
                return model.Refresh_tokens[0];
            }
            else {
                return null;
            }
        }
      //  public async Task<bool> DeleteRefreshToken(string email, string refreshToken, string token_key)
       // {/

        //}
            public async Task<bool> addRefreshToken(string email,string refreshToken,string token_key)
        {
            FilterDefinition<MongoDbuser> filter = Builders<MongoDbuser>.Filter
                .Eq(x => x.Email, email);
            refreshTokenModel refresh_token = new()
            {
                Key = token_key,
                Refresh_token = refreshToken,
                ExpireAt = DateTime.UtcNow.AddDays(7) // TTL would remove this if we did not remove it by using it!
            };
            UpdateDefinition<MongoDbuser> update = Builders<MongoDbuser>.Update
              .Push(x => x.Refresh_tokens , refresh_token);
            UpdateResult result = await _userCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount == 1 ? true : false;
        }
    }
}
