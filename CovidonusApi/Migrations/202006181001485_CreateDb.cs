namespace CovidonusApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeltaDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Confirmed = c.Int(nullable: false),
                        Deceased = c.Int(nullable: false),
                        Recovered = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DistrictWiseDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        District = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Notes = c.String(unicode: false),
                        Active = c.Int(nullable: false),
                        Confirmed = c.Int(nullable: false),
                        Deceased = c.Int(nullable: false),
                        Recovered = c.Int(nullable: false),
                        StateWiseDataId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        CreatedBy = c.String(unicode: false),
                        Modified = c.DateTime(precision: 0),
                        ModifiedBy = c.String(unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        Delta_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeltaDatas", t => t.Delta_Id)
                .ForeignKey("dbo.StateWiseDatas", t => t.StateWiseDataId, cascadeDelete: true)
                .Index(t => t.StateWiseDataId)
                .Index(t => t.Delta_Id);
            
            CreateTable(
                "dbo.StateWiseDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        StateCode = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        Created = c.DateTime(nullable: false, precision: 0),
                        CreatedBy = c.String(unicode: false),
                        Modified = c.DateTime(precision: 0),
                        ModifiedBy = c.String(unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistrictWiseDatas", "StateWiseDataId", "dbo.StateWiseDatas");
            DropForeignKey("dbo.DistrictWiseDatas", "Delta_Id", "dbo.DeltaDatas");
            DropIndex("dbo.DistrictWiseDatas", new[] { "Delta_Id" });
            DropIndex("dbo.DistrictWiseDatas", new[] { "StateWiseDataId" });
            DropTable("dbo.StateWiseDatas");
            DropTable("dbo.DistrictWiseDatas");
            DropTable("dbo.DeltaDatas");
        }
    }
}
