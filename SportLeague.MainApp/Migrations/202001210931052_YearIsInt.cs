namespace SportLigue.MainApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YearIsInt : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Movies", "ReleaseDate");
            AddColumn("dbo.Movies", "ReleaseDate", c => c.Int(nullable: false, defaultValue: 1895));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "ReleaseDate");
            AddColumn("dbo.Movies", "ReleaseDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
