namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdminTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 100),
                        UserName = c.String(nullable: false, maxLength: 10),
                        ProfilePicture = c.String(),
                        InitDate = c.DateTime(),
                        Permission = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.AdminId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Admins");
        }
    }
}
