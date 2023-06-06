namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForumReplyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumReplies",
                c => new
                    {
                        ForumReplyId = c.Int(nullable: false, identity: true),
                        ReplyContentHtml = c.String(),
                        ForumId = c.Int(nullable: false),
                        ForumMemberId = c.String(nullable: false),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumReplyId)
                .ForeignKey("dbo.Forums", t => t.ForumId, cascadeDelete: true)
                .Index(t => t.ForumId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumReplies", "ForumId", "dbo.Forums");
            DropIndex("dbo.ForumReplies", new[] { "ForumId" });
            DropTable("dbo.ForumReplies");
        }
    }
}
