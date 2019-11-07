namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        MatchId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        SenderResult = c.Boolean(nullable: false),
                        SenderTime = c.DateTime(nullable: false),
                        ReceiverResult = c.Boolean(nullable: false),
                        ReceiverTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MatchId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Matches");
        }
    }
}
