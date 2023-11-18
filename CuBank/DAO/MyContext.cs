using CuBank.Models;
using Microsoft.EntityFrameworkCore;

namespace CuBank.DAO
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Operacion> Operaciones { get; set; }
    }
}
