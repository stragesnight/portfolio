namespace ADO_Exam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_gamemode_and_copiessold : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameModes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Games", "GameModeId", c => c.Int(nullable: true));
            AddColumn("dbo.Games", "CopiesSold", c => c.Int(nullable: true));
            //CreateIndex("dbo.Games", "GameModeId");
            //AddForeignKey("dbo.Games", "GameModeId", "dbo.GameModes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Games", "GameModeId", "dbo.GameModes");
            //DropIndex("dbo.Games", new[] { "GameModeId" });
            DropColumn("dbo.Games", "CopiesSold");
            DropColumn("dbo.Games", "GameModeId");
            DropTable("dbo.GameModes");
        }
    }
}
