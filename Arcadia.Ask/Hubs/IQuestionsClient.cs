namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Models.DTO;

    public interface IQuestionsClient
    {
        Task QuestionIsChanged(QuestionDto question);

        Task QuestionIsRemoved(Guid questionId);

        Task QuestionVotesAreChanged(Guid questionId, int votes);

        Task QuestionIsApproved(Guid questionId);
    }
}