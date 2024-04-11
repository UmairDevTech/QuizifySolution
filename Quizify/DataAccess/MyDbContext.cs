using Microsoft.EntityFrameworkCore;
using Quizify.DataAccess.Entities;

namespace Quizify.DataAccess
{
    public class MyDbContext : DbContext
    {
        public DbSet<ContentInfo> Content { get; set; }
        public DbSet<NetworkInfo> Network { get; set; }
        public DbSet<UserInfo> User { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
}
