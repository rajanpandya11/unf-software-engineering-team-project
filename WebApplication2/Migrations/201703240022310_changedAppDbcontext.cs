namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedAppDbcontext : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Salary", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Salary");
        }
    }
}
