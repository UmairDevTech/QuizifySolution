using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizify.Controllers.Common;
using Quizify.DataAccess;
using Quizify.DataAccess.Entities;

namespace Quizify.Controllers
{
    public class UserController : ApplicationService
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<string> CreateOrUpdate(UserInfo input)
        {
            if (string.IsNullOrWhiteSpace(input.Username))
                throw new UserFriendlyException("Username can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.FirstName))
                throw new UserFriendlyException("First name can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.LastName))
                throw new UserFriendlyException("Last name can't be null or empty.");
            if (input.ThreeDigitNumber == 0)
                throw new UserFriendlyException("Three digit number can't be 0.");

            var user = await _context.User.FirstOrDefaultAsync(x => x.Username == input.Username);
            if (user != null)
                throw new UserFriendlyException($"Username '{input.Username}' already exists.");

            if (input.Id == 0)
            {
                await _context.User.AddAsync(input);
                await _context.SaveChangesAsync();
                return "User Added Successfully.";
            }
            else
            {
                var old_entity = await _context.User.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (old_entity == null)
                    throw new UserFriendlyException($"UserId: '{input.Id}' is invalid.");

                _context.User.Update(old_entity);
                await _context.SaveChangesAsync();
                return "User Updated Successfully.";
            }
        }

        [HttpGet]
        public async Task<UserInfo> Get(int Id)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
                throw new UserFriendlyException($"UserId: '{Id}' is invalid.");

            return user;
        }

        [HttpGet]
        public async Task<List<UserInfo>> GetAll(string? Username, string? FirstName, string? LastName, int? ThreeDigitNumber, int? Wins, int? Losses)
        {
            var query = _context.User.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Username))
                query = query.Where(x => x.Username.Contains(Username));
            if (!string.IsNullOrWhiteSpace(FirstName))
                query = query.Where(x => x.FirstName.Contains(FirstName));
            if (!string.IsNullOrWhiteSpace(LastName))
                query = query.Where(x => x.LastName.Contains(LastName));
            if (ThreeDigitNumber != null)
                query = query.Where(x => x.ThreeDigitNumber == ThreeDigitNumber);
            if (Wins >= 0)
                query = query.Where(x => x.Wins == Wins);
            if (Losses >= 0)
                query = query.Where(x => x.Losses == Losses);

            return await query.ToListAsync();
        }

        [HttpDelete]
        public async Task<string> Delete(int Id)
        {
            var entity = await Get(Id);
            _context.User.Remove(entity);
            await _context.SaveChangesAsync();
            return "User Deleted Successfully.";
        }
    }
}
