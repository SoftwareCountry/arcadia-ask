namespace Arcadia.Ask.Hubs
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Models.DTO;
    using Models.Entities;
    using Storage.Questions;

    public class QuestionsHub : Hub<IQuestionsClient>
    {
        private readonly Guid currentUserGuid;
        private readonly IQuestionStorage questionsStorage;

        public QuestionsHub(IQuestionStorage questionsStorage)
        {
            this.questionsStorage = questionsStorage;
            currentUserGuid = Guid.NewGuid();
        }

        public async Task CreateQuestion(string text)
        {
            var question = await questionsStorage.UpsertQuestion(new QuestionEntity()
            {
                Text = text,
                Author = "author",
                IsApproved = false,
                PostedAt = DateTimeOffset.Now
            });
            await Clients.All.QuestionIsChanged(EntityToDto(question));
        }

        public async Task ApproveQuestion(Guid questionId)
        {
            await questionsStorage.ApproveQuestion(questionId);
            await Clients.All.QuestionIsApproved(questionId);
        }

        public async Task RemoveQuestion(Guid questionId)
        {
            await questionsStorage.DeleteQuestion(questionId);
            await Clients.All.QuestionIsRemoved(questionId);
        }

        public async Task UpvoteQuestion(Guid questionId)
        {
            var question = await questionsStorage.UpvoteQuestion(questionId, currentUserGuid);
            var questionDto = EntityToDtoForSpecificUser(question, currentUserGuid);

            await VotesAreChanged(questionDto);
        }

        public async Task DownvoteQuestion(Guid questionId)
        {
            var question = await questionsStorage.DownvoteQuestion(questionId, currentUserGuid);
            var questionDto = EntityToDtoForSpecificUser(question, currentUserGuid);

            await VotesAreChanged(questionDto);
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestions()
        {
            var questions = await questionsStorage.GetQuestions();
            return questions.Select(q => EntityToDtoForSpecificUser(q, currentUserGuid));
        }

        private async Task VotesAreChanged(QuestionDto question)
        {
            await Clients.AllExcept(Context.ConnectionId).QuestionVotesAreChanged(question.QuestionId, question.Votes);
            await Clients.Client(Context.ConnectionId).QuestionIsChanged(question);
        }

        private QuestionDto EntityToDto(QuestionEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new QuestionDto()
            {
                QuestionId = entity.QuestionId,
                Author = entity.Author,
                Text = entity.Text,
                PostedAt = entity.PostedAt,
                IsApproved = entity.IsApproved,
                Votes = entity.Votes?.Count ?? 0,
            };
        }

        private QuestionDto EntityToDtoForSpecificUser(QuestionEntity entity, Guid userId)
        {
            var dto = EntityToDto(entity);
            dto.DidVote = entity.Votes.Count(v => v.UserId == userId) > 0;

            return dto;
        }
    }
}