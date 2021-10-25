namespace Mini_Tekstac_Question_Paper_Generation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin_CredentialTb",
                c => new
                    {
                        username = c.String(nullable: false, maxLength: 128),
                        password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.username);
            
            CreateTable(
                "dbo.UserDetailsTb",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        First_Name = c.String(nullable: false),
                        Last_Name = c.String(),
                        DoB = c.DateTime(nullable: false),
                        Gender = c.String(),
                        Contact_Number = c.Long(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 50),
                        ConfirmPassword = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserDetailsTb");
            DropTable("dbo.Admin_CredentialTb");
        }
    }
}
