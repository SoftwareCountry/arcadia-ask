namespace Arcadia.Ask.Questions.Exceptions
{
    using System;

    public class QuestionNotApprovedException : Exception
    {
        public QuestionNotApprovedException(Guid questionId)
            : base($"Question with id {questionId} was not approved, yet")
        {
        }
    }
}