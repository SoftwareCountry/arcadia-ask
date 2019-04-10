namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Models.Entities;
    using Arcadia.Ask.Questions;
    using Arcadia.Ask.Questions.Exceptions;
    using Arcadia.Ask.Storage.Questions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class QuestionsHub : Hub<IQuestionsClient>
    {
        private readonly IQuestionStorage questionsStorage;
        private readonly IDisplayedQuestion displayedQuestion;

        private const string ModeratorsGroupName = "ModeratorsGroup";

        public QuestionsHub(
            IQuestionStorage questionsStorage,
            IDisplayedQuestion displayedQuestion
        )
        {
            this.questionsStorage = questionsStorage;
            this.displayedQuestion = displayedQuestion;
        }

        public override async Task OnConnectedAsync()
        {
            if (this.Context.User.IsInRole(RoleNames.Moderator))
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, ModeratorsGroupName);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (this.Context.User.IsInRole(RoleNames.Moderator))
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, ModeratorsGroupName);
            }
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

            await this.Clients.Group(ModeratorsGroupName).QuestionIsChanged(this.EntityToDto(question));
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public async Task ApproveQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.ApproveQuestion(questionId);
            await this.Clients.All.QuestionIsChanged(this.EntityToDto(question));
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public async Task RemoveQuestion(Guid questionId)
        {
            await this.questionsStorage.DeleteQuestion(questionId);
            await this.Clients.All.QuestionIsRemoved(questionId);
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public async Task DisplayQuestion(Guid questionId)
        {
            var question = await this.questionsStorage.GetQuestion(questionId);

            if (!question.IsApproved)
            {
                throw new QuestionNotApprovedException(questionId);
            }

            this.displayedQuestion.CurrentDisplayedQuestionId = question.QuestionId;
            await this.Clients.All.DisplayedQuestionChanged(questionId);
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public async Task HideQuestion()
        {
            this.displayedQuestion.CurrentDisplayedQuestionId = Guid.Empty;

            await this.Clients.All.DisplayedQuestionHidden();
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
            questions = this.Context.User.IsInRole(RoleNames.Moderator) ? questions : questions.Where(q => q.IsApproved);

            return questions.Select(q => this.EntityToDtoForSpecificUser(q, this.CurrentUserGuid));
        }

        private async Task VotesAreChanged(QuestionDto question)
        {
            await this.Clients.All.QuestionIsChanged(question);
            await this.Clients.Caller.QuestionIsVoted(question.QuestionId);
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
                Votes = entity.Votes?.Count ?? 0,
                IsDisplayed = this.displayedQuestion.CurrentDisplayedQuestionId == entity.QuestionId
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
                Metadata = this.EntityToDto(entity),
                DidVote = entity.Votes?.Any(v => v.UserId == userId) ?? false
            };
        }

        private Guid CurrentUserGuid => Guid.Parse(this.Context.User.Identity.Name);
    }
}