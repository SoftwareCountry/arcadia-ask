using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.Ask.Storage.Exceptions
{
    public class QuestionUpvotedException : Exception
    {
        public QuestionUpvotedException(Guid questionId) : base($"Question with id {questionId} already upvoted by user") { }
    }
}
