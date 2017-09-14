namespace UxtrataWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using UxtrataWeb.Models;
    using UxtrataWeb.Util;

    internal sealed class Configuration : DbMigrationsConfiguration<UxtrataWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            // register mysql code generator
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(UxtrataWeb.Models.ApplicationDbContext context)
        {
            //Constants c = new Constants();
            context.BusinessType.AddOrUpdate(
                bt => bt.BusinessTypeID,
                new BusinessType() { BusinessTypeID = 1, Name = Constants.BusinessType.Enroll_Student.ToString()},
                new BusinessType() { BusinessTypeID = 2, Name = Constants.BusinessType.Deposit.ToString()},
                new BusinessType() { BusinessTypeID = 3, Name = Constants.BusinessType.Withdraw.ToString()}
            );
        }
    }
}
