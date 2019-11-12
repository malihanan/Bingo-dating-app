namespace Bingo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BingoDbv9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "MalePreference", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "FemalePreference", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "OtherPreference", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "OtherPreference", c => c.Boolean());
            AlterColumn("dbo.Users", "FemalePreference", c => c.Boolean());
            AlterColumn("dbo.Users", "MalePreference", c => c.Boolean());
        }
    }
}
