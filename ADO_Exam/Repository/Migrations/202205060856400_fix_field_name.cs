namespace ADO_Exam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_field_name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "ReleaseDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Games", "RealeaseDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "RealeaseDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Games", "ReleaseDate");
        }
    }
}
