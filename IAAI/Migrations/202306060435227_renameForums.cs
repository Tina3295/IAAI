namespace IAAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameForums : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Fora", newName: "Forums");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Forums", newName: "Fora");
        }
    }
}
