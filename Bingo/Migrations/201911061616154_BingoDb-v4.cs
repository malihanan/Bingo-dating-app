namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "displayBirthdate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "displayBirthdate");
        }
    }
}
