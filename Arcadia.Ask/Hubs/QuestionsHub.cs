namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Models.DTO;
    using Storage.Questions;

    public class QuestionsHub : Hub<IQuestionsClient>
    {
        private readonly Guid currentUserGuid;
        private readonly IQuestionStorage questionsStorage;

        public QuestionsHub(IQuestionStorage questionsStorage)
        {
            this.questionsStorage = questionsStorage;
            currentUserGuid = Guid.NewGuid();
        }

        public async Task CreateQuestion(string text)
        {
            var question = await questionsStorage.UpsertQuestion(new QuestionDto
            {
                Text = text,
                Author = "author",
                IsApproved = false,
                PostedAt = DateTimeOffset.Now
            });
            await Clients.All.QuestionIsChanged(question);
        }

        public async Task ApproveQuestion(Guid questionId)
        {
            await questionsStorage.ApproveQuestion(questionId);
            await Clients.All.QuestionIsApproved(questionId);
        }

        public async Task RemoveQuestion(Guid questionId)
        {
            await questionsStorage.DeleteQuestion(questionId);
            await Clients.All.QuestionIsRemoved(questionId);
        }

        public async Task UpvoteQuestion(Guid questionId)
        {
            var question = await questionsStorage.UpvoteQuestion(questionId, currentUserGuid);
            await Clients.AllExcept(Context.ConnectionId).QuestionVotesAreChanged(question.QuestionId, question.Votes);
            await Clients.Client(Context.ConnectionId).QuestionIsChanged(question);
        }

        public async Task DownvoteQuestion(Guid questionId)
        {
            var question = await questionsStorage.DownvoteQuestion(questionId, currentUserGuid);
            await Clients.AllExcept(Context.ConnectionId).QuestionVotesAreChanged(question.QuestionId, question.Votes);
            await Clients.Client(Context.ConnectionId).QuestionIsChanged(question);
        }

        public Task<IEnumerable<QuestionDto>> GetQuestions()
        {
            return questionsStorage.GetQuestionsForSpecificUser(currentUserGuid);
        }
    }
}