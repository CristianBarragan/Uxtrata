namespace UxtrataWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEntitiesModifications2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "Name", c => c.String(nullable: false, maxLength: 30, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Name", c => c.String(unicode: false));
        }
    }
}
