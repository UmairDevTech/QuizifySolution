using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizify.Controllers.Common;
using Quizify.DataAccess;
using Quizify.DataAccess.Entities;

namespace Quizify.Controllers
{
    public class ContentController : ApplicationService
    {
        private readonly MyDbContext context;

        public ContentController(MyDbContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public async Task<string> CreateOrUpdate(ContentInfo input)
        {
            var user = context.User.FirstOrDefaultAsync(x => x.Id == input.UserId);

            if (string.IsNullOrWhiteSpace(input.Question))
                throw new UserFriendlyException("Question can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.OptionA))
                throw new UserFriendlyException("OptionA can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.OptionB))
                throw new UserFriendlyException("OptionB can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.OptionC))
                throw new UserFriendlyException("OptionC can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.OptionD))
                throw new UserFriendlyException("OptionD can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.CorrectAnswer))
                throw new UserFriendlyException("CorrectAnswer can't be null or empty.");
            if (input.Image == null || input.Image.Length == 0)
                throw new UserFriendlyException("Image can't be null or empty.");
            if (user == null)
                throw new UserFriendlyException($"UserId: '{input.UserId}' is invalid.");

            if (input.Id == 0)
            {
                await context.Content.AddAsync(input);
                await context.SaveChangesAsync();
                return "Question Added Successfully.";
            }
            else
            {
                var old_entity = await context.Content.FirstOrDefaultAsync(x => x.Id == input.Id)!;
                if (old_entity == null)
                    throw new UserFriendlyException($"QuestionId: '{input.Id}' is invalid.");

                context.Content.Update(old_entity);
                await context.SaveChangesAsync();
                return "Question Updated Successfully.";
            }
        }

        [HttpGet]
        public async Task<ContentInfo> Get(int Id)
        {
            var content = await context.Content.FirstOrDefaultAsync(x => x.Id == Id);
            if (content == null)
                throw new UserFriendlyException($"QuestionId: '{Id}' is invalid.");

            return content;
        }

        [HttpGet]
        public async Task<List<ContentInfo>> GetAll(string? Question, string? OptionA, string? OptionB, string? OptionC, string? OptionD, string? CorrectAnswer, int? UserId)
        {
            var query = context.Content.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Question))
                query = query.Where(x => x.Question.Contains(Question));
            if (!string.IsNullOrWhiteSpace(OptionA))
                query = query.Where(x => x.OptionA.Contains(OptionA));
            if (!string.IsNullOrWhiteSpace(OptionB))
                query = query.Where(x => x.OptionB.Contains(OptionB));
            if (!string.IsNullOrWhiteSpace(OptionC))
                query = query.Where(x => x.OptionC.Contains(OptionC));
            if (!string.IsNullOrWhiteSpace(OptionD))
                query = query.Where(x => x.OptionD.Contains(OptionD));
            if (!string.IsNullOrWhiteSpace(CorrectAnswer))
                query = query.Where(x => x.CorrectAnswer.Contains(CorrectAnswer));
            if (UserId != 0)
                query = query.Where(x => x.UserId == UserId);

            return await query.ToListAsync();
        }

        [HttpDelete]
        public async Task<string> Delete(int Id)
        {
            var entity = await Get(Id);
            context.Content.Remove(entity);
            await context.SaveChangesAsync();
            return "Question Deleted Successfully.";
        }
    }
}
