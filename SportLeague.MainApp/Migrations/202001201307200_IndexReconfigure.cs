namespace SportLigue.MainApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndexReconfigure : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Movies", new[] { "Name" });
            CreateIndex("dbo.Movies", "Id", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Movies", new[] { "Id" });
            CreateIndex("dbo.Movies", "Name", unique: true);
        }
    }
}
