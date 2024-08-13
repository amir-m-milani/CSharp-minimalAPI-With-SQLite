namespace Athenticate.Database;
using Microsoft.EntityFrameworkCore;
using Athenticate.Model;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}