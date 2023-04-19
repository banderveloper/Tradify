using Microsoft.EntityFrameworkCore;

namespace Tradify.Identity.Persistence
{
    public class DatabaseInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }
    }
}
