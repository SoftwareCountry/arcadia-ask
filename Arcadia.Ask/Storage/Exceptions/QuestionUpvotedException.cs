namespace Arcadia.Ask.Storage.Exceptions
{
    using System;

    public class QuestionUpvotedException : Exception
    {
        public QuestionUpvotedException(Guid questionId) : base($"Question with id {questionId} already upvoted by user")
        {
        }
    }
}