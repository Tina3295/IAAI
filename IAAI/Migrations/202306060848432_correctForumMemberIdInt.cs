namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctForumMemberIdInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForumReplies", "ForumMemberId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumReplies", "ForumMemberId", c => c.String(nullable: false));
        }
    }
}
