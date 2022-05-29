namespace ADO_Exam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_fk : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Games SET GameModeId = 1 WHERE GameModeId IS NULL");
            Sql("UPDATE dbo.Games SET CopiesSold = 1 WHERE CopiesSold IS NULL");
            AlterColumn("dbo.Games", "GameModeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Games", "CopiesSold", c => c.Int(nullable: false));
            CreateIndex("dbo.Games", "GameModeId");
            AddForeignKey("dbo.Games", "GameModeId", "dbo.GameModes", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Games", "GameModeId", "dbo.GameModes");
            DropIndex("dbo.Games", new[] { "GameModeId" });
        }
    }
}
