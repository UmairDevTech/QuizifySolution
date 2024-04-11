using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizify.Controllers.Common;
using Quizify.DataAccess;
using Quizify.DataAccess.Entities;

namespace Quizify.Controllers
{
    public class NetworkController : ApplicationService
    {
        private readonly MyDbContext _context;

        public NetworkController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<string> CreateOrUpdate(NetworkInfo input)
        {
            if (string.IsNullOrWhiteSpace(input.Code))
                throw new UserFriendlyException("Code can't be null or empty.");
            if (string.IsNullOrWhiteSpace(input.NetworkName))
                throw new UserFriendlyException("Network name can't be null or empty.");

            var network = await _context.Network.FirstOrDefaultAsync(x => x.Code == input.Code);
            if (network != null)
                throw new UserFriendlyException($"Network with code '{input.Code}' already exists.");

            if (input.Id == 0)
            {
                await _context.Network.AddAsync(input);
                await _context.SaveChangesAsync();
                return "Network Added Successfully.";
            }
            else
            {
                var old_entity = await _context.Network.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (old_entity == null)
                    throw new UserFriendlyException($"NetworkId: '{input.Id}' is invalid.");

                _context.Network.Update(old_entity);
                await _context.SaveChangesAsync();
                return "Network Updated Successfully.";
            }
        }

        [HttpGet]
        public async Task<NetworkInfo> Get(int Id)
        {
            var network = await _context.Network.FirstOrDefaultAsync(x => x.Id == Id);
            if (network == null)
                throw new UserFriendlyException($"NetworkId: '{Id}' is invalid.");

            return network;
        }

        [HttpGet]
        public async Task<List<NetworkInfo>> GetAll(string? Code, string? NetworkName)
        {
            var query = _context.Network.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Code))
                query = query.Where(x => x.Code.Contains(Code));
            if (!string.IsNullOrWhiteSpace(NetworkName))
                query = query.Where(x => x.NetworkName.Contains(NetworkName));

            return await query.ToListAsync();
        }

        [HttpDelete]
        public async Task<string> Delete(int Id)
        {
            var entity = await Get(Id);
            _context.Network.Remove(entity);
            await _context.SaveChangesAsync();
            return "Network Deleted Successfully.";
        }
    }
}
