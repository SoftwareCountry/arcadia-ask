namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.DTO;
    using Models.Entities;
    using Storage.Questions;

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
            var question = await questionsStorage.UpsertQuestion(new QuestionEntity()
            {
                Text = text,
                Author = "author",
                IsApproved = false,
                PostedAt = DateTimeOffset.Now
            });
            await Clients.All.QuestionIsChanged(EntityToDto(question));
        }

        public async Task ApproveQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.ApproveQuestion(questionId);
            await this.Clients.All.QuestionIsChanged(EntityToDto(question));
        }

        public async Task RemoveQuestion(Guid questionId)
        {
            await this.questionsStorage.DeleteQuestion(questionId);
            await this.Clients.All.QuestionIsRemoved(questionId);
        }

        public async Task UpvoteQuestion(Guid questionId)
        {
            var question = await questionsStorage.UpvoteQuestion(questionId, Guid.NewGuid());
            var questionDto = EntityToDto(question);

            await VotesAreChanged(questionDto);
        }

        public async Task DownvoteQuestion(Guid questionId)
        {
            var question = await questionsStorage.DownvoteQuestion(questionId, Guid.NewGuid());
            var questionDto = EntityToDto(question);

            await VotesAreChanged(questionDto);
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestions()
        {
            var questions = await questionsStorage.GetQuestions();
            return questions.Select(q => EntityToDto(q));
        }

        private async Task VotesAreChanged(QuestionDto question)
        {
            await Clients.All.QuestionIsChanged(question);
        }

        private QuestionDto EntityToDto(QuestionEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new QuestionDto()
            {
                QuestionId = entity.QuestionId,
                Author = entity.Author,
                Text = entity.Text,
                PostedAt = entity.PostedAt,
                IsApproved = entity.IsApproved,
                Votes = entity.Votes?.Count ?? 0,
            };
        }
    }
}