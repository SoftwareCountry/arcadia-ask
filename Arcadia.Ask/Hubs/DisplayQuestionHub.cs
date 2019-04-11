namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
    using Arcadia.Ask.Models.DTO;
    using Arcadia.Ask.Questions;
    using Arcadia.Ask.Questions.Exceptions;
    using Arcadia.Ask.Storage.Questions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    public class DisplayQuestionHub : Hub<IDisplayQuestionClient>
    {
        private readonly IDisplayedQuestion displayedQuestion;
        private readonly IQuestionStorage questionsStorage;

        public DisplayQuestionHub(
            IDisplayedQuestion displayedQuestion,
            IQuestionStorage questionsStorage
        )
        {
            this.displayedQuestion = displayedQuestion;
            this.questionsStorage = questionsStorage;
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
            await this.Clients.All.DisplayedQuestionChanged(new QuestionDto(question));
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public async Task HideQuestion()
        {
            this.displayedQuestion.CurrentDisplayedQuestionId = Guid.Empty;

            await this.Clients.All.DisplayedQuestionHidden();
        }

        public async Task<QuestionDto> GetDisplayedQuestion()
        {
            try
            {
                var displayedQuestionId = this.displayedQuestion.CurrentDisplayedQuestionId;
                var question = await this.questionsStorage.GetQuestion(displayedQuestionId);

                return new QuestionDto(question);
            }
            catch (QuestionNotFoundException)
            {
                await this.Clients.All.DisplayedQuestionHidden();
                return null;
            }
        }
    }
}