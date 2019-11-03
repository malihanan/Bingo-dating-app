namespace Bingo.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class BingoDbContext : DbContext
    {
        public BingoDbContext()
            : base("name=BingoDbContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}