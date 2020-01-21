namespace SportLigue.MainApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRentalFees : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Movies", "RentalFees");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "RentalFees", c => c.Int(nullable: false));
        }
    }
}
