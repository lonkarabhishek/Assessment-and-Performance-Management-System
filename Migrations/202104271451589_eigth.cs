namespace Mini_Tekstac_Question_Paper_Generation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eigth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamsAndAssessmentGivenTb", "percentageMarks", c => c.Single(nullable: false));
            DropColumn("dbo.ExamsAndAssessmentGivenTb", "Marks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExamsAndAssessmentGivenTb", "Marks", c => c.Int(nullable: false));
            DropColumn("dbo.ExamsAndAssessmentGivenTb", "percentageMarks");
        }
    }
}
