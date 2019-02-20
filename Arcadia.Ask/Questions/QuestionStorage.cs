namespace Arcadia.Ask.Questions
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionStorage : IQuestionStorage
    {
        private ConcurrentDictionary<string, Question> allQuestions = new ConcurrentDictionary<string, Question>();

        public async Task<Question[]> GetQuestions()
        {
            return this.allQuestions.ToArray().Select(x => x.Value).ToArray();
        }

        public async Task<Question> UpsertQuestion(Question question)
        {
            this.allQuestions[question.QuestionId] = question;
            return question;
        }

        public async Task DeleteQuestion(string questionId)
        {
            this.allQuestions.TryRemove(questionId, out _);
        }
    }
}