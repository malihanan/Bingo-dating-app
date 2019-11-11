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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BingoDbContext, Bingo.Migrations.Configuration>());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
    }
}