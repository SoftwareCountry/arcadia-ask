﻿namespace Arcadia.Ask.Storage.Exceptions
{
    using System;

    public class QuestionVotedException : Exception
    {
        public QuestionVotedException(Guid questionId) : base($"Question with id {questionId} already upvoted by user")
        {
        }
    }
}