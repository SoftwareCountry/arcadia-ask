namespace Arcadia.Ask.Questions
{
    using System;
    using System.Threading.Tasks;

    public interface IQuestionStorage
    {
        Task<Question[]> GetQuestions();

        Task<Question> GetQuestion(Guid questionId);
        
        Task<Question> UpsertQuestion(Question question);

        Task DeleteQuestion(Guid questionId);

        Task<Question> ApproveQuestion(Guid questionId);

        Task<Question> ChangeVotes(Guid questionId, int diff);
    }
}