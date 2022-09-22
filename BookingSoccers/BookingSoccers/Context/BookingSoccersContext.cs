using System;
using BookingSoccers.Models;
using Microsoft.EntityFrameworkCore;
using SoccerBookings.Models;
#nullable disable
namespace BookingSoccers.Context 
{
    public class BookingSoccersContext : DbContext
    {

        public BookingSoccersContext(DbContextOptions<BookingSoccersContext> options) :
            base(options)
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

