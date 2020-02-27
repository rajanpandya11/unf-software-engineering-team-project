namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "LastName", c => c.String(nullable: false));
            //CreateTable(
            //   "dbo.AspNetUsers",
            //   c => new
            //   {
            //       Id = c.String(nullable: false, maxLength: 128),
            //       FirstName = c.String(nullable: false),
            //       LastName = c.String(nullable: false),
            //       UserName = c.String(nullable: false, maxLength: 256),
            //       Email = c.String(maxLength: 256),
            //       EmailConfirmed = c.Boolean(nullable: false),
            //       PasswordHash = c.String(),
            //       Salary = c.Long(nullable: false),
            //       SecurityStamp = c.String(),
            //       PhoneNumber = c.String(),
            //       PhoneNumberConfirmed = c.Boolean(nullable: false),
            //       TwoFactorEnabled = c.Boolean(nullable: false),
            //       LockoutEndDateUtc = c.DateTime(),
            //       LockoutEnabled = c.Boolean(nullable: false),
            //       AccessFailedCount = c.Int(nullable: false),
            //   })
            //  .PrimaryKey(t => t.Id)
            //  .Index(t => t.UserName, unique: true, name: "UserNameIndex");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "LastName", c => c.String());
            AlterColumn("dbo.Employees", "FirstName", c => c.String());
        }
    }
}
