using System;

namespace Arcadia.Ask.Storage.Exceptions
{
    public class QuestionNotFoundException : Exception
    {
        public QuestionNotFoundException(Guid questionId) : base($"Question with id {questionId} was not found") { }
    }
}
