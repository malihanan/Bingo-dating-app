namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trupti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sender_id = c.Int(nullable: false),
                        receiver_id = c.Int(nullable: false),
                        message = c.String(),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Conversations");
        }
    }
}