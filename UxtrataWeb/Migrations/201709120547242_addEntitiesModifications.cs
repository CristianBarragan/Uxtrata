namespace UxtrataWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEntitiesModifications : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "CourseName", c => c.String(nullable: false, maxLength: 30, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "CourseName", c => c.String(unicode: false));
        }
    }
}
