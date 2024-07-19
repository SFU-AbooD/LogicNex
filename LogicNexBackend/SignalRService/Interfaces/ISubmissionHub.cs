using SignalRService.Models;
namespace SignalRService.Interfaces
{
    public interface ISubmissionHub
    {
       public Task UpdateSubmission(JudgeResponse response);
    }
}
