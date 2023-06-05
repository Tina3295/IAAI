namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delExperienceTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ForumMemberExperiences", "ForumMemberId", "dbo.ForumMembers");
            DropIndex("dbo.ForumMemberExperiences", new[] { "ForumMemberId" });
            AddColumn("dbo.ForumMembers", "HistoryUnit1", c => c.String(maxLength: 50));
            AddColumn("dbo.ForumMembers", "HistoryJobTitle1", c => c.String(maxLength: 30));
            AddColumn("dbo.ForumMembers", "StartYear1", c => c.Int());
            AddColumn("dbo.ForumMembers", "StartMonth1", c => c.Int());
            AddColumn("dbo.ForumMembers", "EndYear1", c => c.Int());
            AddColumn("dbo.ForumMembers", "EndMonth1", c => c.Int());
            AddColumn("dbo.ForumMembers", "HistoryUnit2", c => c.String(maxLength: 50));
            AddColumn("dbo.ForumMembers", "HistoryJobTitle2", c => c.String(maxLength: 30));
            AddColumn("dbo.ForumMembers", "StartYear2", c => c.Int());
            AddColumn("dbo.ForumMembers", "StartMonth2", c => c.Int());
            AddColumn("dbo.ForumMembers", "EndYear2", c => c.Int());
            AddColumn("dbo.ForumMembers", "EndMonth2", c => c.Int());
            AddColumn("dbo.ForumMembers", "HistoryUnit3", c => c.String(maxLength: 50));
            AddColumn("dbo.ForumMembers", "HistoryJobTitle3", c => c.String(maxLength: 30));
            AddColumn("dbo.ForumMembers", "StartYear3", c => c.Int());
            AddColumn("dbo.ForumMembers", "StartMonth3", c => c.Int());
            AddColumn("dbo.ForumMembers", "EndYear3", c => c.Int());
            AddColumn("dbo.ForumMembers", "EndMonth3", c => c.Int());
            DropTable("dbo.ForumMemberExperiences");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ForumMemberExperiences",
                c => new
                    {
                        ForumMemberExperienceId = c.Int(nullable: false, identity: true),
                        Unit = c.String(maxLength: 50),
                        JobTitle = c.String(maxLength: 30),
                        StartYear = c.Int(),
                        StartMonth = c.Int(),
                        EndYear = c.Int(),
                        EndMonth = c.Int(),
                        ForumMemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ForumMemberExperienceId);
            
            DropColumn("dbo.ForumMembers", "EndMonth3");
            DropColumn("dbo.ForumMembers", "EndYear3");
            DropColumn("dbo.ForumMembers", "StartMonth3");
            DropColumn("dbo.ForumMembers", "StartYear3");
            DropColumn("dbo.ForumMembers", "HistoryJobTitle3");
            DropColumn("dbo.ForumMembers", "HistoryUnit3");
            DropColumn("dbo.ForumMembers", "EndMonth2");
            DropColumn("dbo.ForumMembers", "EndYear2");
            DropColumn("dbo.ForumMembers", "StartMonth2");
            DropColumn("dbo.ForumMembers", "StartYear2");
            DropColumn("dbo.ForumMembers", "HistoryJobTitle2");
            DropColumn("dbo.ForumMembers", "HistoryUnit2");
            DropColumn("dbo.ForumMembers", "EndMonth1");
            DropColumn("dbo.ForumMembers", "EndYear1");
            DropColumn("dbo.ForumMembers", "StartMonth1");
            DropColumn("dbo.ForumMembers", "StartYear1");
            DropColumn("dbo.ForumMembers", "HistoryJobTitle1");
            DropColumn("dbo.ForumMembers", "HistoryUnit1");
            CreateIndex("dbo.ForumMemberExperiences", "ForumMemberId");
            AddForeignKey("dbo.ForumMemberExperiences", "ForumMemberId", "dbo.ForumMembers", "ForumMemberId", cascadeDelete: true);
        }
    }
}
