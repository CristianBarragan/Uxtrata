namespace UxtrataWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBusinessToTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "BusinessID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "CourseSelectionID", "dbo.CourseSelections");
            DropIndex("dbo.Transactions", new[] { "CourseSelectionID" });
            DropColumn("dbo.Transactions", "CourseSelectionID");
        }
    }
}
