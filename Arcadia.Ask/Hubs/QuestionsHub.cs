namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Models.DTO;
    using Models.Entities;

    using Storage.Questions;

    public class QuestionsHub : Hub<IQuestionsClient>
    {
        private readonly Guid currentUserGuid;
        private readonly IQuestionStorage questionsStorage;

        public QuestionsHub(IQuestionStorage questionsStorage)
        {
            this.questionsStorage = questionsStorage;
            this.currentUserGuid = Guid.NewGuid();
        }

        public async Task CreateQuestion(string text)
        {
            var question = await this.questionsStorage.UpsertQuestion(new QuestionEntity
            {
                Text = text,
                Author = "author",
                IsApproved = false,
                PostedAt = DateTimeOffset.Now
            });
            await this.Clients.All.QuestionIsChanged(this.EntityToDto(question));
        }

        public async Task ApproveQuestion(Guid questionId)
        {
            await this.questionsStorage.ApproveQuestion(questionId);
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
            var questionDto = this.EntityToDtoForSpecificUser(question, this.currentUserGuid);

            await this.VotesAreChanged(questionDto);
        }

        public async Task DownvoteQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.DownvoteQuestion(questionId, this.currentUserGuid);
            var questionDto = this.EntityToDtoForSpecificUser(question, this.currentUserGuid);

            await this.VotesAreChanged(questionDto);
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestions()
        {
            var questions = await this.questionsStorage.GetQuestions();
            return questions.Select(q => this.EntityToDtoForSpecificUser(q, this.currentUserGuid));
        }

        private async Task VotesAreChanged(QuestionDto question)
        {
            await this.Clients.AllExcept(this.Context.ConnectionId).QuestionVotesAreChanged(question.QuestionId, question.Votes);
            await this.Clients.Client(this.Context.ConnectionId).QuestionIsChanged(question);
        }

        private QuestionDto EntityToDto(QuestionEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new QuestionDto
            {
                QuestionId = entity.QuestionId,
                Author = entity.Author,
                Text = entity.Text,
                PostedAt = entity.PostedAt,
                IsApproved = entity.IsApproved,
                Votes = entity.Votes?.Count ?? 0
            };
        }

        private QuestionDto EntityToDtoForSpecificUser(QuestionEntity entity, Guid userId)
        {
            var dto = this.EntityToDto(entity);
            dto.DidVote = entity.Votes.Count(v => v.UserId == userId) > 0;

            return dto;
        }
    }
}