namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Questions;

    using Microsoft.AspNetCore.SignalR;

    public class QuestionsHub : Hub<IQuestionsClient>
    {
        private readonly IQuestionStorage questionsStorage;
        private Guid currentUserGuid;

        public QuestionsHub(IQuestionStorage questionsStorage)
        {
            this.questionsStorage = questionsStorage;
        }

        public override Task OnConnectedAsync()
        {
            this.currentUserGuid = Guid.NewGuid();
            return Task.CompletedTask;
        }

        public async Task CreateQuestion(string text)
        {
            var question = await this.questionsStorage.UpsertQuestion(new QuestionDTO()
            {
                Text = text,
                Author = "author",
                IsApproved = false,
                PostedAt = DateTimeOffset.Now
            });
            await this.Clients.All.QuestionIsChanged(question);
        }

        public async Task ApproveQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.ApproveQuestion(questionId);
            await this.Clients.All.QuestionIsApproved(questionId);
        }

        public async Task RemoveQuestion(Guid questionId)
        {
            await this.questionsStorage.DeleteQuestion(questionId);
            await this.Clients.All.QuestionIsRemoved(questionId);
        }

        public async Task UpvoteQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.UpvoteQuestion(questionId, this.currentUserGuid);
            await this.Clients.All.QuestionVotesAreChanged(question.QuestionId, question.Votes);
        }

        public async Task DownvoteQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.DownvoteQuestion(questionId, this.currentUserGuid);
            await this.Clients.AllExcept(this.Context.ConnectionId).QuestionVotesAreChanged(question.QuestionId, question.Votes);
            await this.Clients.Client(this.Context.ConnectionId).QuestionIsChanged(question);
        }

        public Task<IEnumerable<QuestionDTO>> GetQuestions()
        {
            return this.questionsStorage.GetQuestionsForSpecificUser(this.currentUserGuid);
        }
    }
}