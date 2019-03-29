namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Questions;

    using Microsoft.AspNetCore.SignalR;

    public class QuestionsHub : Hub<IQuestionsClient>
    {
        private readonly IQuestionStorage questionsStorage;

        public QuestionsHub(IQuestionStorage questionsStorage)
        {
            this.questionsStorage = questionsStorage;
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
            await this.Clients.All.QuestionIsChanged(question);
        }

        public async Task RemoveQuestion(Guid questionId)
        {
            await this.questionsStorage.DeleteQuestion(questionId);
            await this.Clients.All.QuestionIsRemoved(questionId);
        }

        public async Task UpvoteQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.UpvoteQuestion(questionId, Guid.NewGuid());
            await this.Clients.All.QuestionIsChanged(question);
        }

        public async Task DownvoteQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.DownvoteQuestion(questionId, Guid.NewGuid());
            await this.Clients.All.QuestionIsChanged(question);
        }

        public Task<IEnumerable<QuestionDTO>> GetQuestions()
        {
            return this.questionsStorage.GetQuestions();
        }
    }
}