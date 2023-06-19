namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPermissionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 50),
                        RootId = c.Int(),
                        Code = c.String(maxLength: 5),
                        URL = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.RootId)
                .Index(t => t.RootId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "RootId", "dbo.Permissions");
            DropIndex("dbo.Permissions", new[] { "RootId" });
            DropTable("dbo.Permissions");
        }
    }
}
