namespace SportLigue.MainApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedReleaseDateType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "ReleaseDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "ReleaseDate", c => c.DateTime(nullable: false));
        }
    }
}
