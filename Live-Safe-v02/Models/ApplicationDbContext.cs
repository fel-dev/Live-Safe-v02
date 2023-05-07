using Microsoft.EntityFrameworkCore;
using Live_Safe_v02.Models;

namespace Live_Safe_v02.Models {
    public class ApplicationDbContext : DbContext {

        // constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // properties
        public DbSet<Expostos> Expostos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
