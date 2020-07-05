namespace CovidonusApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CasesTimeSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DailyConfirmed = c.Int(nullable: false),
                        DailyDeceased = c.Int(nullable: false),
                        DailyRecovered = c.Int(nullable: false),
                        Date = c.String(maxLength: 20, storeType: "nvarchar"),
                        TotalConfirmed = c.Int(nullable: false),
                        TotalDeceased = c.Int(nullable: false),
                        TotalRecovered = c.Int(nullable: false),
                        DateFull = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        DeltaId = c.Int(nullable: false),
                        StateCode = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        StateWiseDataId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        CreatedBy = c.String(unicode: false),
                        Modified = c.DateTime(precision: 0),
                        ModifiedBy = c.String(unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeltaDatas", t => t.DeltaId, cascadeDelete: true)
                .ForeignKey("dbo.StateWiseDatas", t => t.StateWiseDataId, cascadeDelete: true)
                .Index(t => t.DeltaId)
                .Index(t => t.StateWiseDataId);
            
            CreateTable(
                "dbo.StateWiseDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        StateCode = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        Active = c.Int(nullable: false),
                        Confirmed = c.Int(nullable: false),
                        Deaths = c.Int(nullable: false),
                        Recovered = c.Int(nullable: false),
                        DeltaActive = c.Int(nullable: false),
                        DeltaConfirmed = c.Int(nullable: false),
                        DeltaDeaths = c.Int(nullable: false),
                        DeltaRecovered = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.Testeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndividualsTestedPerConfirmedCase = c.String(maxLength: 12, storeType: "nvarchar"),
                        PositiveCasesFromSamplesReported = c.String(maxLength: 12, storeType: "nvarchar"),
                        SampleReportedToday = c.String(maxLength: 12, storeType: "nvarchar"),
                        Source = c.String(unicode: false),
                        TestPositivityRate = c.String(maxLength: 12, storeType: "nvarchar"),
                        TestsConductedByPrivateLabs = c.String(maxLength: 12, storeType: "nvarchar"),
                        TestsPerConfirmedCase = c.String(maxLength: 12, storeType: "nvarchar"),
                        TestsPerMillion = c.String(maxLength: 12, storeType: "nvarchar"),
                        TotalIndividualsTested = c.String(maxLength: 12, storeType: "nvarchar"),
                        TotalPositiveCases = c.String(maxLength: 12, storeType: "nvarchar"),
                        TotalSamplesTested = c.String(maxLength: 12, storeType: "nvarchar"),
                        UpdateTimeStamp = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistrictWiseDatas", "StateWiseDataId", "dbo.StateWiseDatas");
            DropForeignKey("dbo.DistrictWiseDatas", "DeltaId", "dbo.DeltaDatas");
            DropIndex("dbo.DistrictWiseDatas", new[] { "StateWiseDataId" });
            DropIndex("dbo.DistrictWiseDatas", new[] { "DeltaId" });
            DropTable("dbo.Testeds");
            DropTable("dbo.StateWiseDatas");
            DropTable("dbo.DistrictWiseDatas");
            DropTable("dbo.DeltaDatas");
            DropTable("dbo.CasesTimeSeries");
        }
    }
}
