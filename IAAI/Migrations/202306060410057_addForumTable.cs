namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForumTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fora",
                c => new
                    {
                        ForumId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ContentHtml = c.String(),
                        ForumMemberId = c.Int(nullable: false),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumId)
                .ForeignKey("dbo.ForumMembers", t => t.ForumMemberId, cascadeDelete: true)
                .Index(t => t.ForumMemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fora", "ForumMemberId", "dbo.ForumMembers");
            DropIndex("dbo.Fora", new[] { "ForumMemberId" });
            DropTable("dbo.Fora");
        }
    }
}
