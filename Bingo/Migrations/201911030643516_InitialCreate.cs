namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        EmailId = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Bio = c.String(),
                        Likes = c.String(),
                        Dislikes = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
