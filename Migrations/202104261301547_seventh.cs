namespace Mini_Tekstac_Question_Paper_Generation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seventh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamsAndAssessmentGivenTb", "Score", c => c.Int(nullable: false));
            AddColumn("dbo.ExamsAndAssessmentGivenTb", "Marks", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExamsAndAssessmentGivenTb", "Marks");
            DropColumn("dbo.ExamsAndAssessmentGivenTb", "Score");
        }
    }
}
