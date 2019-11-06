namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Gender", c => c.String());
            AddColumn("dbo.Users", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "City", c => c.String());
            AddColumn("dbo.Users", "Occupation", c => c.String());
            AddColumn("dbo.Users", "Hobbies", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Hobbies");
            DropColumn("dbo.Users", "Occupation");
            DropColumn("dbo.Users", "City");
            DropColumn("dbo.Users", "Age");
            DropColumn("dbo.Users", "Gender");
        }
    }
}
