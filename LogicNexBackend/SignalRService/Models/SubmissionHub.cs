using Microsoft.AspNetCore.SignalR;
using SignalRService.Interfaces;

namespace SignalRService.Models
{
    public class SubmissionHub : Hub<ISubmissionHub>
    {
        public async Task AddGroup(string groupID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,groupID);
        }
    }
}
