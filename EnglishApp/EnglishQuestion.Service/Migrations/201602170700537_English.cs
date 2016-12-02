namespace EnglishQuestion.Service.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class English : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 4000),
                        IsAnswer = c.Boolean(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Extend1 = c.String(maxLength: 4000),
                        Extend2 = c.String(maxLength: 4000),
                        Extend3 = c.String(maxLength: 4000),
                        Extend4 = c.String(maxLength: 4000),
                        Extend5 = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 4000),
                        Level = c.String(maxLength: 4000),
                        Purpose = c.String(maxLength: 4000),
                        Section = c.String(maxLength: 4000),
                        TestLevel = c.String(maxLength: 4000),
                        TimeDone = c.Int(nullable: false),
                        CanMixAnswer = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 4000),
                        ParagraphId = c.Int(nullable: false),
                        Type = c.String(maxLength: 4000),
                        Title = c.String(maxLength: 4000),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Extend1 = c.String(maxLength: 4000),
                        Extend2 = c.String(maxLength: 4000),
                        Extend3 = c.String(maxLength: 4000),
                        Extend4 = c.String(maxLength: 4000),
                        Extend5 = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Paragraph", t => t.ParagraphId, cascadeDelete: true)
                .Index(t => t.ParagraphId);
            
            CreateTable(
                "dbo.Paragraph",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 4000),
                        Level = c.String(maxLength: 4000),
                        Purpose = c.String(maxLength: 4000),
                        Section = c.String(maxLength: 4000),
                        TestLevel = c.String(maxLength: 4000),
                        TimeDone = c.Int(nullable: false),
                        Status = c.String(maxLength: 4000),
                        FileInfo = c.String(maxLength: 4000),
                        Type = c.String(maxLength: 4000),
                        Title = c.String(maxLength: 4000),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Extend1 = c.String(maxLength: 4000),
                        Extend2 = c.String(maxLength: 4000),
                        Extend3 = c.String(maxLength: 4000),
                        Extend4 = c.String(maxLength: 4000),
                        Extend5 = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.B1B2ConfigValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 4000),
                        Value = c.String(maxLength: 4000),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Extend1 = c.String(maxLength: 4000),
                        Extend2 = c.String(maxLength: 4000),
                        Extend3 = c.String(maxLength: 4000),
                        Extend4 = c.String(maxLength: 4000),
                        Extend5 = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubTest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestId = c.Int(nullable: false),
                        XmlWritingStructure = c.String(),
                        XmlListeningStructure = c.String(),
                        WritingTestContent = c.String(),
                        ListeningTestContent = c.String(),
                        No = c.String(maxLength: 4000),
                        Name = c.String(maxLength: 4000),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Extend1 = c.String(maxLength: 4000),
                        Extend2 = c.String(maxLength: 4000),
                        Extend3 = c.String(maxLength: 4000),
                        Extend4 = c.String(maxLength: 4000),
                        Extend5 = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Test", t => t.TestId, cascadeDelete: true)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(maxLength: 4000),
                        Name = c.String(maxLength: 4000),
                        ClassNo = c.String(maxLength: 4000),
                        Level = c.String(maxLength: 4000),
                        TotalTime = c.Int(nullable: false),
                        RealTestTime = c.Int(nullable: false),
                        TotalQuestion = c.Int(nullable: false),
                        NumOfSubTest = c.Int(nullable: false),
                        ConfigStructure = c.String(maxLength: 4000),
                        Purpose = c.String(maxLength: 4000),
                        IsChoice = c.Boolean(nullable: false),
                        TestDate = c.DateTime(nullable: false),
                        EndTestDate = c.DateTime(nullable: false),
                        IsGuess = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Extend1 = c.String(maxLength: 4000),
                        Extend2 = c.String(maxLength: 4000),
                        Extend3 = c.String(maxLength: 4000),
                        Extend4 = c.String(maxLength: 4000),
                        Extend5 = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubTest", "TestId", "dbo.Test");
            DropForeignKey("dbo.Question", "ParagraphId", "dbo.Paragraph");
            DropForeignKey("dbo.Answer", "QuestionId", "dbo.Question");
            DropIndex("dbo.SubTest", new[] { "TestId" });
            DropIndex("dbo.Question", new[] { "ParagraphId" });
            DropIndex("dbo.Answer", new[] { "QuestionId" });
            DropTable("dbo.Test");
            DropTable("dbo.SubTest");
            DropTable("dbo.B1B2ConfigValue");
            DropTable("dbo.Paragraph");
            DropTable("dbo.Question");
            DropTable("dbo.Answer");
        }
    }
}
