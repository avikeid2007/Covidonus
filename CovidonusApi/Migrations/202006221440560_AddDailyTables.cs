namespace CovidonusApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDailyTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyDistrictWiseDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        District = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Confirmed = c.Int(nullable: false),
                        Deceased = c.Int(nullable: false),
                        Recovered = c.Int(nullable: false),
                        Tested = c.Int(nullable: false),
                        StateCode = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        DailyStateWiseDataId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        CreatedBy = c.String(unicode: false),
                        Modified = c.DateTime(precision: 0),
                        ModifiedBy = c.String(unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DailyStateWiseDatas", t => t.DailyStateWiseDataId, cascadeDelete: true)
                .Index(t => t.DailyStateWiseDataId);
            
            CreateTable(
                "dbo.DailyStateWiseDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        StateCode = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        Confirmed = c.Int(nullable: false),
                        Deaths = c.Int(nullable: false),
                        Recovered = c.Int(nullable: false),
                        Tested = c.Int(nullable: false),
                        StateNotes = c.String(unicode: false),
                        LastUpdatedtime = c.DateTime(precision: 0),
                        StateLogo = c.String(maxLength: 50, storeType: "nvarchar"),
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
            DropForeignKey("dbo.DailyDistrictWiseDatas", "DailyStateWiseDataId", "dbo.DailyStateWiseDatas");
            DropIndex("dbo.DailyDistrictWiseDatas", new[] { "DailyStateWiseDataId" });
            DropTable("dbo.DailyStateWiseDatas");
            DropTable("dbo.DailyDistrictWiseDatas");
        }
    }
}
