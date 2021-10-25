namespace Mini_Tekstac_Question_Paper_Generation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ninth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TopicsTb",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        TopicName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TopicId);
            
            AddColumn("dbo.AssessmentTb", "SkillSet", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssessmentTb", "SkillSet");
            DropTable("dbo.TopicsTb");
        }
    }
}
