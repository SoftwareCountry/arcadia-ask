﻿namespace Arcadia.Ask.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class VoteEntity
    {
        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }

        [MaxLength(50)]
        public string UserId { get; set; }

        /// <summary>
        ///     This value is true if question is upvoted and false if downvoted.
        /// </summary>
        [Required]
        public bool IsUpvoted { get; set; }

        public virtual QuestionEntity Question { get; set; }
    }
}