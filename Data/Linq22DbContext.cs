using Microsoft.EntityFrameworkCore;
using ThisLinq.Models;

namespace ThisLinq.Data
{
    public class Linq22DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public Linq22DbContext(DbContextOptions<Linq22DbContext> options) : base(options)
        {

        }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ConnectionLink> Connections { get; set; }
    }
}
