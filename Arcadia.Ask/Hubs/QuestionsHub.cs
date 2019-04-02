namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Storage.Questions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class QuestionsHub : Hub<IQuestionsClient>
    {
        private readonly IQuestionStorage questionsStorage;

        public QuestionsHub(IQuestionStorage questionsStorage)
        {
            this.questionsStorage = questionsStorage;
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
            var question = await this.questionsStorage.ApproveQuestion(questionId);
            await this.Clients.All.QuestionIsChanged(this.EntityToDto(question));
        }

        public async Task RemoveQuestion(Guid questionId)
        {
            await this.questionsStorage.DeleteQuestion(questionId);
            await this.Clients.All.QuestionIsRemoved(questionId);
        }

        public async Task UpvoteQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.UpvoteQuestion(questionId, this.CurrentUserGuid);
            var questionDto = this.EntityToDto(question);

            await this.VotesAreChanged(questionDto);
        }

        public async Task DownvoteQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.DownvoteQuestion(questionId, this.CurrentUserGuid);
            var questionDto = this.EntityToDto(question);

            await this.VotesAreChanged(questionDto);
        }

        public async Task<IEnumerable<QuestionForSpecificUserDto>> GetQuestions()
        {
            var questions = await this.questionsStorage.GetQuestions();
            return questions.Select(q => this.EntityToDtoForSpecificUser(q, this.CurrentUserGuid));
        }

        private async Task VotesAreChanged(QuestionDto question)
        {
            await this.Clients.All.QuestionIsChanged(question);
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

        private QuestionForSpecificUserDto EntityToDtoForSpecificUser(QuestionEntity entity, Guid userId)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new QuestionForSpecificUserDto
            {
                Question = new QuestionDto
                {
                    QuestionId = entity.QuestionId,
                    Author = entity.Author,
                    Text = entity.Text,
                    PostedAt = entity.PostedAt,
                    IsApproved = entity.IsApproved,
                    Votes = entity.Votes?.Count ?? 0
                },
                DidVote = entity.Votes.Count(v => v.UserId == userId) > 0
            };
        }

        private Guid CurrentUserGuid => Guid.Parse(this.Context.User.Identity.Name);
    }
}