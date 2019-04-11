namespace Arcadia.Ask.Models.DTO
{
    using System;
    using System.Linq;

    using Arcadia.Ask.Models.Entities;

    public class QuestionForSpecificUserDto
    {
        public QuestionDto Metadata { get; set; }

        public bool DidVote { get; set; }

        public QuestionForSpecificUserDto(QuestionEntity entity, Guid userId)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.Metadata = new QuestionDto(entity);
            this.DidVote = entity.Votes?.Any(v => v.UserId == userId) ?? false;
        }
    }
}