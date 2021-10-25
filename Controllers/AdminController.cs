using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mini_Tekstac_Question_Paper_Generation.Models;
using System.Data.Entity;
using System.Globalization;
using iTextSharp;
using iTextSharp.awt;
using iTextSharp.text;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;

namespace Mini_Tekstac_Question_Paper_Generation.Controllers
{
    public class AdminController : Controller
    {
        //database of admin call
        QPGDbContext db;
        public AdminController()
        {
            db = new QPGDbContext();
        }
        // admin login
        public ActionResult Login()
        {
            return View();
        }
        

        //admin login check
        [HttpPost]
        public ActionResult Login(AdminModel model)
        {
            var credentialCheck = db.Admin.Where(x => x.username == model.username
            && x.password == model.password).FirstOrDefault();
            var usernameCheck = db.Admin.Where(x => x.username != model.username
            && x.password != model.password).FirstOrDefault();
            var passwordMismatch = db.Admin.Where(x => x.username == model.username
            && x.password != model.password).FirstOrDefault();

            if (credentialCheck != null)
            {
                Session["AdminUsername"] = credentialCheck.username;
                return RedirectToAction("Home");
            }
            else if (usernameCheck != null)
            {
                ViewBag.Message = "This user Id " + model.username + " is not present";
                return View();
            }
            else if (passwordMismatch != null)
            {
                ViewBag.Message = "Password not matching";
            }
            else
            {
                ModelState.AddModelError("", "**Invalid credentials**");
            }
            return View();
        }


        //admin logout
        public ActionResult Logout()
        {

            Session["AdminUsername"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }
        //confirm logout
        public ActionResult ConfirmLogout()
        {
            if (Session["AdminUsername"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //home begins..
        public ActionResult Home()
        {
            try
            {
                if (Session["AdminUsername"] != null)
                {
                    var adminuser = Session["AdminUsername"].ToString().FirstOrDefault();

                    var examsGiven = db.ExamaAndAssessmentGivens.Include(x => x.AssessmentModel).ToList();

                    List<int> reparttions = new List<int>();

                    reparttions.Add(examsGiven.Where(x => x.percentageMarks > 80).Count());
                    reparttions.Add(examsGiven.Where(x => x.percentageMarks > 60 && x.percentageMarks <= 80).Count());
                    reparttions.Add(examsGiven.Where(x => x.percentageMarks <= 60).Count());

                    ViewBag.REP = reparttions.ToList();

                    // Most popular skills

                    Dictionary<string, int> skillFrequency = new Dictionary<string, int>();

                    foreach (var examGiven in examsGiven)
                    {
                        string[] skillsOfTheExam = examGiven.AssessmentModel.AssessmentName.Split(',');

                        foreach (string skill in skillsOfTheExam)
                        {
                            skillFrequency[skill] = skillFrequency.ContainsKey(skill) ? skillFrequency[skill] + 1 : 1;
                        }
                    }

                    var topThreeSkills = skillFrequency.OrderByDescending(x => x.Value).Take(3);

                    var topThreeSkillNames = topThreeSkills.Select(x => x.Key).ToList();

                    var topThreeSkillFreq = topThreeSkills.Select(x => x.Value).ToList();

                    ViewBag.TOPSKILLNAMES = topThreeSkillNames;

                    ViewBag.TOPSKILLFREQ = topThreeSkillFreq;

                    return View();
                }
                else
                { return RedirectToAction("Login"); }
            }
            catch
            {
                ViewBag.Message = "Something went wrong, please try again..";
                return View();
            }
        }

        //add feedback questionnare
        public ActionResult FeedbackQuestionnare()
        {
            if (Session["AdminUsername"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult FeedbackQuestionnare(FBQuestions model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.FeedbackQuestions.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = "Question added successfully!";
                    return View();
                }
                catch
                {
                    ViewBag.Message1 = "Something went wrong, please try again later...";
                }

            }
            return View();
        }

        //display feedback questions
        public ActionResult DisplayFeedbackQuestions()
        {
            if (Session["AdminUsername"] != null)
            {
                List<FBQuestions> Questions = db.FeedbackQuestions.ToList();

                return View(Questions);
            }
            else return RedirectToAction("Login");
        }

        //delete feedback question
        public ActionResult Delete(int id)
        {
            var SearchQuestion = db.FeedbackQuestions.Where(x => x.QuestionNo == id).FirstOrDefault();
            return View(SearchQuestion);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int Id)
        {
            if (ModelState.IsValid)
            {
                var SearchQuestion = db.FeedbackQuestions.Where(x => x.QuestionNo == Id).FirstOrDefault();
                db.FeedbackQuestions.Remove(SearchQuestion);
                db.SaveChanges();
                ViewBag.Message = "Successfully deleted the feedback question";
                return View();
            }
            return View();
        }

        //display tickets raised and a link to solve them
        public ActionResult HelpTickets()
        {
            if (Session["AdminUsername"] != null)
            {

                var data = db.Help.Include(x => x.UserModel).ToList();
                var display = data.OrderByDescending(x => x.TicketId);
                return View(display);
            }

            else
            {
                return RedirectToAction("Login");
            }

        }

        //resolve the tickets
        public ActionResult Resolve(int id)
        {
            Session["userId"] = id;
            if (Session["AdminUsername"] != null)
            {
                var ticket = db.Help.Where(x => x.TicketId == id).FirstOrDefault();
                return View(ticket);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        public ActionResult Resolve(HelpTickets model)
        {
            int id = int.Parse(Session["userId"].ToString());

            try
            {


                ModelState.Clear();
                var helpData = db.Help.Where(x => x.TicketId == id).FirstOrDefault();
                helpData.Resolve = model.Resolve;
                helpData.TicketStatus = model.TicketStatus;
                db.Entry(helpData).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Successfully resolved the issue";
                return View();



            }
            catch (Exception e)
            {
                ViewBag.Message1 = "Something's broken, let us check " + e;
                return View();
            }
        }

        //display all feedback
        public ActionResult DisplayAllFeedback()
        {

            if (Session["AdminUsername"] != null)
            {
                var questions = db.Feedback.Include(y => y.FBQuestions).ToList();
                var assessname = db.Feedback.Include(x => x.AssessmentModel).ToList();
                var allFeedbacks = db.Feedback.Include(x => x.UserModel).ToList();
                var orderedFeedbacks = allFeedbacks.OrderByDescending(x => x.feedbackGivenDate);

                return View(orderedFeedbacks);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        //add assessment questions
        public ActionResult AddAssessmentQuestions()
        {
            if (Session["AdminUsername"] != null)
            {
                var data = (from x in db.Assessments select x.AssessmentName).ToList();
                ViewBag.AssessmentNames = new SelectList(data);
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult AddAssessmentQuestions(QuestionsAndAnswersSheet model)
        {
            if (Session["AdminUsername"] != null)
            {
                var data = (from x in db.Assessments select x.AssessmentName).ToList();
                ViewBag.AssessmentNames = new SelectList(data);
                if (ModelState.IsValid)
                {
                    var searchAssessmentCode = db.Assessments.Where(x => x.AssessmentName == model.AssessmentName).FirstOrDefault();


                    var info = searchAssessmentCode.AssessmentCode;
                    model.AssessmentCode = info;
                    db.QuestionAndAnswers.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = "Question added successfully for " + model.AssessmentName;
                    return View();

                }
                else return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //display all assessment Questions
        public ActionResult DisplayAssessmentQuestions()
        {
            if (Session["AdminUsername"] != null)
            {
                var x = db.QuestionAndAnswers.ToList();

                return View(x);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult DisplayAssessmentQuestions(string AssessmentName)
        {
            if (Session["AdminUsername"] != null)
            {

                var x = db.QuestionAndAnswers.ToList();
                var searchedDisplay = x.Where(y => y.AssessmentName.Equals(AssessmentName)).ToList();
                return View(searchedDisplay);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //add assessment
        public ActionResult AddAssessment()
        {
            if (Session["AdminUsername"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult AddAssessment(AssessmentModel model)
        {
            if (Session["AdminUsername"] != null)
            {
                if (model.AssessmentDate > DateTime.Now)
                {
                    db.Assessments.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = "Assessment added successfully";
                    return View();
                }
                else
                {
                    ViewBag.Message1 = " * date should be in the future";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        //display assessments 
        public ActionResult DisplayAssements()
        {
            if (Session["AdminUsername"] != null)
            {
                var assessments = db.Assessments.ToList();
                return View(assessments);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //Add Exam Details
        public ActionResult AddExamDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddExamDetails(ExamDetails model)
        {
            try
            {
                if (Session["AdminUsername"] != null)
                {
                    db.ExamDetails.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = "Your Details are submitted successfully!";
                    return View();
                }
                else return RedirectToAction("Login");
            }
            catch
            {
                ViewBag.Message1 = "Failed to submit the Exam Details!!";

                return View();
            }
        }

        //Display Exam Details
        public ActionResult DisplayExamDetails()
        {

            AssessmentModel assessment = new AssessmentModel();
            if (Session["AdminUsername"] != null)
            {

                var display = db.ExamDetails.Include(x => x.AssessmentModel).ToList();
                return View(display);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        //update exam details
        public ActionResult Edit(int id, int assescode)
        {
            if (Session["AdminUsername"] != null)
            {
                Session["examId"] = id;
                Session["assescode"] = assescode;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult Edit(ExamDetails model)
        {
            if (Session["AdminUsername"] != null)
            {
                int examid = int.Parse(Session["examId"].ToString());
                int assesscode = int.Parse(Session["assescode"].ToString());
                try
                {
                    ModelState.Clear();
                    var findExam = db.ExamDetails.Where(x => x.ExamId == examid).FirstOrDefault();
                    findExam.AssessmentCode = assesscode;
                    findExam.ExamName = model.ExamName;
                    findExam.DateOfExam = model.DateOfExam;
                    findExam.Competency = model.Competency;
                    db.Entry(findExam).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Successfully saved the update";
                    return View();

                }
                catch (Exception e)
                {
                    ViewBag.Message1 = "Something's broken, let us check " + e;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login");
            }


        }

        //delete exam details
        public ActionResult RemoveExam(int examid)
        {
            if (Session["AdminUsername"] != null)
            {

                Session["examId"] = examid;
                var include = db.ExamDetails.Include(x => x.AssessmentModel);
                var SearchExam = include.Where(x => x.ExamId == examid).FirstOrDefault();
                return View(SearchExam);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public ActionResult RemoveExam()
        {
            if (Session["AdminUsername"] != null)
            {

                try
                {
                    int examId = int.Parse(Session["examId"].ToString());
                    var SearchExam = db.ExamDetails.Where(x => x.ExamId == examId).FirstOrDefault();
                    db.ExamDetails.Remove(SearchExam);
                    db.SaveChanges();
                    ViewBag.Message = "Successfully deleted the exam";
                    return View();
                }
                catch
                {
                    ViewBag.Message1 = "something went wrong..";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult GenerateReport()
        {
            if (Session["AdminUsername"] != null)
            {
                var displayall = db.ExamaAndAssessmentGivens.Include(x => x.AssessmentModel).Include(x => x.ExamDetails).Include(x => x.UserModel);
                var assesscodes = displayall.ToList();
                return View(assesscodes);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult Report(int AssessmentCode)
        {
            if (Session["AdminUsername"] != null)
            {
                var assName = db.Assessments.Where(x => x.AssessmentCode == AssessmentCode).FirstOrDefault();
                var asssName = assName.AssessmentName;
                var assessment = db.ExamaAndAssessmentGivens.Where(x => x.AssessmentCode == AssessmentCode).ToList();
                var UsersGivenAssessmentCount = assessment.GroupBy(x => x.User_Id).Select(y => new { Count = y.Select(x => x.User_Id).Distinct().Count() }).Count();
                var NoOfPassedUsers = assessment.Where(x => x.percentageMarks > 40).Distinct().Count();
                var NoOfFailedUsers = assessment.Where(x => x.percentageMarks < 40).Distinct().Count();
                var HighestScore = assessment.OrderByDescending(x => x.Score).First().Score;
                ViewBag.UserCount = UsersGivenAssessmentCount;
                ViewBag.PassCount = NoOfPassedUsers;
                ViewBag.FailCount = NoOfFailedUsers;
                ViewBag.HighestScore = HighestScore;
                List<string> report = new List<string>();
                report.Add(UsersGivenAssessmentCount.ToString());
                report.Add(NoOfPassedUsers.ToString());
                report.Add(NoOfFailedUsers.ToString());
                report.Add(HighestScore.ToString());
                report.Add(AssessmentCode.ToString());
                report.Add(asssName.ToString());

                Session["data"] = report;

                return View();

            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //CREATE PDF FOR DATE RANGE

        public FileResult CreatePdfForDateRange()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("Result for Assessment" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 7 columns  
            PdfPTable tableLayout = new PdfPTable(7);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   
            doc.Add(Add_Content(tableLayout));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }

        protected PdfPTable Add_Content(PdfPTable tableLayout)
        {

            float[] headers = { 50, 24, 45, 35, 50, 35, 50 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            //  List<BookPackage> bookings = context.BookPackages.ToList<BookPackage>();
            var Data = Session["Data"];
            List<string> report = Data as List<string>;


            tableLayout.AddCell(new PdfPCell(new Phrase("REPORT FOR ASSESMENT(S)", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "Assessment Code");
            AddCellToHeader(tableLayout, "Assessment Name");
            AddCellToHeader(tableLayout, "Number of users given the assessment");
            AddCellToHeader(tableLayout, "Number of user passed");
            AddCellToHeader(tableLayout, "Number of user failed");
            AddCellToHeader(tableLayout, "Highest score");
            AddCellToHeader(tableLayout, "Date");


            ////Add body  


            AddCellToBody(tableLayout, report[4]);
            AddCellToBody(tableLayout, report[5]);
            AddCellToBody(tableLayout, report[0]);
            AddCellToBody(tableLayout, report[1]);
            AddCellToBody(tableLayout, report[2]);
            AddCellToBody(tableLayout, report[3]);
            AddCellToBody(tableLayout, DateTime.Now.ToString());
            



            return tableLayout;
        }
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }

        
    }
}