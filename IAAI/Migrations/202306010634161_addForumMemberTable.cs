namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForumMemberTable : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ForumMemberExperienceId)
                .ForeignKey("dbo.ForumMembers", t => t.ForumMemberId, cascadeDelete: true)
                .Index(t => t.ForumMemberId);
            
            CreateTable(
                "dbo.ForumMembers",
                c => new
                    {
                        ForumMemberId = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 10),
                        Gender = c.Int(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        ApplicationType = c.Int(nullable: false),
                        BusinessPhone = c.String(maxLength: 20),
                        CellPhone = c.String(maxLength: 20),
                        Address = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Membership = c.String(),
                        NowUnit = c.String(maxLength: 50),
                        JobTitle = c.String(maxLength: 30),
                        HighestEducation = c.String(maxLength: 30),
                        InitDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumMemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumMemberExperiences", "ForumMemberId", "dbo.ForumMembers");
            DropIndex("dbo.ForumMemberExperiences", new[] { "ForumMemberId" });
            DropTable("dbo.ForumMembers");
            DropTable("dbo.ForumMemberExperiences");
        }
    }
}
