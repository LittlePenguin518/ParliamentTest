using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Domain;

namespace UKParliament.CodeTest.Data
{
    public class RoomBookingsContext : DbContext
    {
        public RoomBookingsContext(DbContextOptions<RoomBookingsContext> ContextOptions)
            : base(ContextOptions)
        {
        }

        public DbSet<Person> People { get; set; }

        public DbSet<Room> Room { get; set; }

        public DbSet<RoomBooking> RoomBookings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(x => x.Id);
            modelBuilder.Entity<Room>().HasKey(y => y.Id);

            modelBuilder.Entity<RoomBooking>().HasKey(z => z.Id);

        }
    }
}