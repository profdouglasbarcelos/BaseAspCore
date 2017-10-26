namespace Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

    public class BaseAspCoreContext : DbContext
    {
        public BaseAspCoreContext(DbContextOptions<BaseAspCoreContext> options) : base(options)
        {

        }
    }
}
