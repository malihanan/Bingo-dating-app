namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Matches", "ReceiverTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Matches", "ReceiverTime", c => c.DateTime(nullable: false));
        }
    }
}
