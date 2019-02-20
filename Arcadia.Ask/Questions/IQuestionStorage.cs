namespace Arcadia.Ask.Questions
{
    using System.Threading.Tasks;

    public interface IQuestionStorage
    {
        Task<Question[]> GetQuestions();
        
        Task<Question> UpsertQuestion(Question question);

        Task DeleteQuestion(string questionId);
    }
}