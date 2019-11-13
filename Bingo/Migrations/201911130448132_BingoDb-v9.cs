namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv9 : DbMigration
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
            
            AddColumn("dbo.Users", "MalePreference", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "FemalePreference", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "OtherPreference", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "Gender", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Gender", c => c.String());
            DropColumn("dbo.Users", "OtherPreference");
            DropColumn("dbo.Users", "FemalePreference");
            DropColumn("dbo.Users", "MalePreference");
            DropTable("dbo.Conversations");
        }
    }
}
