namespace UxtrataWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeToAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Code", c => c.String(nullable: false, maxLength: 30, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Code");
        }
    }
}
