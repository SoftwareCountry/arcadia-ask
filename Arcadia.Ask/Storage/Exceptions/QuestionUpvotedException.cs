using System;

namespace Arcadia.Ask.Storage.Exceptions
{
    public class QuestionUpvotedException : Exception
    {
        public QuestionUpvotedException(Guid questionId) : base($"Question with id {questionId} already upvoted by user") { }
    }
}
