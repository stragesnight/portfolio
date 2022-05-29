namespace ADO_Exam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.CompanyBranches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenreId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        RealeaseDate = c.DateTime(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.GenreId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Games", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.Games", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyBranches", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyBranches", "CityId", "dbo.Cities");
            DropIndex("dbo.Games", new[] { "CompanyId" });
            DropIndex("dbo.Games", new[] { "GenreId" });
            DropIndex("dbo.CompanyBranches", new[] { "CityId" });
            DropIndex("dbo.CompanyBranches", new[] { "CompanyId" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropTable("dbo.Countries");
            DropTable("dbo.Genres");
            DropTable("dbo.Games");
            DropTable("dbo.Companies");
            DropTable("dbo.CompanyBranches");
            DropTable("dbo.Cities");
        }
    }
}
