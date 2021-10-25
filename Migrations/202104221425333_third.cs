namespace Mini_Tekstac_Question_Paper_Generation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExamsAndAssessmentGivenTb",
                c => new
                    {
                        ExamsGivenid = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        AssessmentCode = c.Int(nullable: false),
                        ExamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExamsGivenid)
                .ForeignKey("dbo.AssessmentTb", t => t.AssessmentCode, cascadeDelete: false)
                .ForeignKey("dbo.ExamDetailsTb", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.UserDetailsTb", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.AssessmentCode)
                .Index(t => t.ExamId);
            
            AddColumn("dbo.AssessmentTb", "UserModel_User_Id", c => c.Int());
            CreateIndex("dbo.AssessmentTb", "UserModel_User_Id");
            AddForeignKey("dbo.AssessmentTb", "UserModel_User_Id", "dbo.UserDetailsTb", "User_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExamsAndAssessmentGivenTb", "User_Id", "dbo.UserDetailsTb");
            DropForeignKey("dbo.AssessmentTb", "UserModel_User_Id", "dbo.UserDetailsTb");
            DropForeignKey("dbo.ExamsAndAssessmentGivenTb", "ExamId", "dbo.ExamDetailsTb");
            DropForeignKey("dbo.ExamsAndAssessmentGivenTb", "AssessmentCode", "dbo.AssessmentTb");
            DropIndex("dbo.ExamsAndAssessmentGivenTb", new[] { "ExamId" });
            DropIndex("dbo.ExamsAndAssessmentGivenTb", new[] { "AssessmentCode" });
            DropIndex("dbo.ExamsAndAssessmentGivenTb", new[] { "User_Id" });
            DropIndex("dbo.AssessmentTb", new[] { "UserModel_User_Id" });
            DropColumn("dbo.AssessmentTb", "UserModel_User_Id");
            DropTable("dbo.ExamsAndAssessmentGivenTb");
        }
    }
}
