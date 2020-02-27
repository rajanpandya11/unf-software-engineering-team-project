namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedInProjectModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsRequest", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Projects", "Budget", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Budget", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "Name", c => c.String());
            DropColumn("dbo.Projects", "IsRequest");
        }
    }
}
