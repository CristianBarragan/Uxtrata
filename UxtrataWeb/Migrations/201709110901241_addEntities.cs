namespace UxtrataWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.AccountID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        CourseName = c.String(unicode: false),
                        AccountID = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CourseID)
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.CourseSelections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        CourseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Age = c.Int(nullable: false),
                        AccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentID)
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        DebitAccountID = c.Int(nullable: false),
                        CreditAccountID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditAccount_AccountID = c.Int(),
                        DebitAccount_AccountID = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.Accounts", t => t.CreditAccount_AccountID)
                .ForeignKey("dbo.Accounts", t => t.DebitAccount_AccountID)
                .Index(t => t.CreditAccount_AccountID)
                .Index(t => t.DebitAccount_AccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "DebitAccount_AccountID", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CreditAccount_AccountID", "dbo.Accounts");
            DropForeignKey("dbo.CourseSelections", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Students", "AccountID", "dbo.Accounts");
            DropForeignKey("dbo.CourseSelections", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Courses", "AccountID", "dbo.Accounts");
            DropIndex("dbo.Transactions", new[] { "DebitAccount_AccountID" });
            DropIndex("dbo.Transactions", new[] { "CreditAccount_AccountID" });
            DropIndex("dbo.Students", new[] { "AccountID" });
            DropIndex("dbo.CourseSelections", new[] { "CourseID" });
            DropIndex("dbo.CourseSelections", new[] { "StudentID" });
            DropIndex("dbo.Courses", new[] { "AccountID" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Students");
            DropTable("dbo.CourseSelections");
            DropTable("dbo.Courses");
            DropTable("dbo.Accounts");
        }
    }
}
