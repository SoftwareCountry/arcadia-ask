namespace Arcadia.Ask.Storage.Exceptions
{
    using System;

    public class QuestionNotFoundException : Exception
    {
        public QuestionNotFoundException(Guid questionId) : base($"Question with id {questionId} was not found")
        {
        }
    }
}