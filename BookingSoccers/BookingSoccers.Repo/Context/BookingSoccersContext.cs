using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace BookingSoccers.Repo.Context
{
    public class BookingSoccersContext : DbContext
    {
        //protected readonly IConfiguration configuration;

        public BookingSoccersContext(DbContextOptions<BookingSoccersContext> options) :
            base(options)
        {
            //this.configuration = configuration;
        }

        protected BookingSoccersContext()
        {
        }

        public DbSet<SoccerField> SoccerFields { get; set; }

        public DbSet<Zone> Zones { get; set; }

        public DbSet<ZoneSlot> ZoneSlots { get; set; }

        public DbSet<ZoneType> ZoneTypes { get; set; }

        public DbSet<PriceItem> PriceItems { get; set; }

        public DbSet<PriceMenu> PriceMenus { get; set; }

        public DbSet<ImageFolder> ImageFolders { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(configuration.GetConnectionString("DatabaseConnectionString"));
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}

