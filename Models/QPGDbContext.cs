using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
namespace Mini_Tekstac_Question_Paper_Generation.Models
{
    public class QPGDbContext: DbContext
    {
        public QPGDbContext():base("Name = SqlCon")
        {

        }
        public virtual DbSet<UserModel>Users { get; set; }
        public virtual DbSet<AdminModel>Admin { get; set; }
        public virtual DbSet<FBQuestions>FeedbackQuestions { get; set; }
        public virtual DbSet<HelpTickets>Help { get; set; }
        public virtual DbSet<FeedbackModel>Feedback { get; set; }
        public virtual DbSet<AssessmentModel>Assessments { get; set; }
        public virtual DbSet<QuestionsAndAnswersSheet>QuestionAndAnswers { get; set; }
        public virtual DbSet<ExamDetails> ExamDetails { get; set; }
        public virtual DbSet<ExamaAndAssessmentGiven> ExamaAndAssessmentGivens { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }

    }
}