namespace Arcadia.Ask.Hubs
{
    using System.Threading.Tasks;

    using Arcadia.Ask.Questions;

    using Microsoft.AspNetCore.SignalR;

    public class QuestionsHub : Hub<IQuestionsClient>
    {
        public async Task Send()
        {
            //await this.Clients.All();
        }

        public async Task<Question[]> GetQuestions()
        {
            return new[] { new Question() { Text = "test" } };
        }
    }
}