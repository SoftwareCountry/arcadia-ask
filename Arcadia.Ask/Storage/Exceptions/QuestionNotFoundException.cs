using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.Ask.Storage.Exceptions
{
    public class QuestionNotFoundException : Exception
    {
        public QuestionNotFoundException(Guid questionId) : base($"Question with id {questionId} was not found") { }
    }
}
