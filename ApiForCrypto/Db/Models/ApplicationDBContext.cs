using Microsoft.EntityFrameworkCore;

namespace ApiForCrypto.Db.Models
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
