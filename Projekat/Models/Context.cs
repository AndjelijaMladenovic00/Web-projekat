using Microsoft.EntityFrameworkCore;

namespace Models
{

    public class Context : DbContext
    {
        public DbSet<Hotel> Hoteli { get; set; }
        public DbSet<Soba> Sobe { get; set; }
        public DbSet<Gost> Gosti { get; set; }
        public DbSet<Recepcioner> Recepcioneri { get; set; }

        public Context(DbContextOptions options):base(options) {}

       
    }
}