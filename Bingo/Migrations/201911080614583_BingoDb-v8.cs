namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Matches", "ReceiverResult", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Matches", "ReceiverResult", c => c.Boolean(nullable: false));
        }
    }
}
