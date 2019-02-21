namespace Arcadia.Ask.Questions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionStorage : IQuestionStorage
    {
        private readonly ConcurrentDictionary<Guid, Question> allQuestions = new ConcurrentDictionary<Guid, Question>();

        public async Task<Question[]> GetQuestions()
        {
            return this.allQuestions.ToArray().Select(x => x.Value).ToArray();
        }

        public async Task<Question> GetQuestion(Guid questionId)
        {
            return this.allQuestions.TryGetValue(questionId, out var value) ? value : null;
        }

        public async Task<Question> UpsertQuestion(Question question)
        {
            return this.allQuestions.AddOrUpdate(question.QuestionId, question, (guid, oldQuestion) => question);
        }

        public Task DeleteQuestion(Guid questionId)
        {
            this.allQuestions.TryRemove(questionId, out _);
            return Task.CompletedTask;
        }

        private Question ThrowQuestionNotFound(Guid _) => throw new InvalidOperationException("No question found");

        public async Task<Question> ApproveQuestion(Guid questionId)
        {
            return this.allQuestions.AddOrUpdate(
                questionId,
                this.ThrowQuestionNotFound,
                (_, oldQuestion) => oldQuestion.Approve());
        }

        public async Task<Question> ChangeVotes(Guid questionId, int diff)
        {
            return this.allQuestions.AddOrUpdate(
                questionId,
                this.ThrowQuestionNotFound,
                (_, oldQuestion) => oldQuestion.ChangeVotes(diff));
        }
    }
}