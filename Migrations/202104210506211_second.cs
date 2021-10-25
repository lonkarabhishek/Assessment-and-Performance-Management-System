namespace Mini_Tekstac_Question_Paper_Generation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionAndAnswersTb",
                c => new
                    {
                        AssessmentQuestionNo = c.Int(nullable: false, identity: true),
                        AssessmentCode = c.Int(nullable: false),
                        AssessmentName = c.String(),
                        Question = c.String(nullable: false),
                        Answer1 = c.String(nullable: false),
                        Answer2 = c.String(nullable: false),
                        Answer3 = c.String(nullable: false),
                        Answer4 = c.String(nullable: false),
                        RightAnswer = c.String(nullable: false),
                        Competency = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AssessmentQuestionNo)
                .ForeignKey("dbo.AssessmentTb", t => t.AssessmentCode, cascadeDelete: true)
                .Index(t => t.AssessmentCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionAndAnswersTb", "AssessmentCode", "dbo.AssessmentTb");
            DropIndex("dbo.QuestionAndAnswersTb", new[] { "AssessmentCode" });
            DropTable("dbo.QuestionAndAnswersTb");
        }
    }
}
