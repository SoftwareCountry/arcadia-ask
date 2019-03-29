namespace Arcadia.Ask.Models.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;


    public class VoteEntity
    {
        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }

        public Guid UserId { get; set; }

        /// <summary>
        ///     This value is true if question is upvoted and false if downvoted.
        /// </summary>
        [Required]
        public bool IsUpvoted { get; set; }

        public virtual QuestionEntity Question { get; set; }
    }
}
