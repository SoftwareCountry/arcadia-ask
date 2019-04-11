namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Arcadia.Ask.Auth.Roles;
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
            await this.Clients.All.DisplayedQuestionChanged(questionId);
        }

        [Authorize(Roles = RoleNames.Moderator)]
        public async Task HideQuestion()
        {
            this.displayedQuestion.CurrentDisplayedQuestionId = Guid.Empty;

            await this.Clients.All.DisplayedQuestionHidden();
        }

        public Task<Guid> GetDisplayedQuestionId()
        {
            return Task.FromResult(this.displayedQuestion.CurrentDisplayedQuestionId);
        }

    }
}
