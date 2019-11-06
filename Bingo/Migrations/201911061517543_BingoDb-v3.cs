namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Contact", c => c.String());
            AddColumn("dbo.Users", "Birthdate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Age", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "Birthdate");
            DropColumn("dbo.Users", "Contact");
        }
    }
}
