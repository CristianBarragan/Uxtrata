namespace UxtrataWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBusinessType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessTypes",
                c => new
                    {
                        BusinessTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.BusinessTypeID);
            
            AddColumn("dbo.Transactions", "BusinessTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Transactions", "BusinessTypeID");
            AddForeignKey("dbo.Transactions", "BusinessTypeID", "dbo.BusinessTypes", "BusinessTypeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "BusinessTypeID", "dbo.BusinessTypes");
            DropIndex("dbo.Transactions", new[] { "BusinessTypeID" });
            DropColumn("dbo.Transactions", "BusinessTypeID");
            DropTable("dbo.BusinessTypes");
        }
    }
}
