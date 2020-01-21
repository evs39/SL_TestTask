namespace SportLigue.MainApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRentalFees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "RentalFees", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "RentalFees");
        }
    }
}
