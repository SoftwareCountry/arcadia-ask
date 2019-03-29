using System;

namespace Arcadia.Ask.Storage.Exceptions
{
    public class QuestionVotedException : Exception
    {
        public QuestionVotedException(Guid questionId) : base($"Question with id {questionId} already upvoted by user") { }
    }
}
