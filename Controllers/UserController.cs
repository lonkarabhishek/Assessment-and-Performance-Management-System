using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using Mini_Tekstac_Question_Paper_Generation.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Web.Services3.Addressing;
using iTextSharp;
using iTextSharp.awt;
using iTextSharp.text;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;

namespace Mini_Tekstac_Question_Paper_Generation.Controllers
{
    public class UserController : Controller
    {
        //database call
        QPGDbContext db;
        public UserController()
        {
            db = new QPGDbContext();
        }

        


        //New User registration/signing up
        public ActionResult Registration()
        {
            return View();
        }
        //sending user details to database after authenticating
        [HttpPost]
        public ActionResult Registration(UserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.DoB < DateTime.Now)
                    {
                        db.Users.Add(model);
                        db.SaveChanges();
                        ViewBag.Message = "Congratulations! You registered successfully, you can now go to login to use mini tekstac!";
                    }
                    /*else
                    {
                        ViewBag.Message =
                    }*/

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        ViewBag.Message1 = "User id already exists, please try a different user id other than " + model.User_Id;
                    }
                    else throw;
                }
                catch (Exception)
                {
                    ViewBag.Message1 = "Problem in registering you, please contact system admin!" +
                        " \n You can try using different user_id while signing up apart from " + model.User_Id;
                }
                return View();
            }
            return View();
        }


        //login for the user
        public ActionResult Login()
        {
            return View();
        }
        //login authentication
        [HttpPost]
        public ActionResult Login(UserModel model)
        {


            var credentialCheck = db.Users.Where(x => x.User_Id.Equals(model.User_Id)
           || x.Password == model.Password).FirstOrDefault(); //authenticate user

            var passwordMismatch = db.Users.Where(x => x.User_Id.Equals(model.User_Id)
            && x.Password != model.Password).FirstOrDefault();

            var idNotPresent = db.Users.Where(x => x.User_Id != model.User_Id
                || x.Password == model.Password).FirstOrDefault();

            if (credentialCheck != null)
            {
                Session["UserId"] = credentialCheck.User_Id.ToString();
                return RedirectToAction("Dashboard");
            }
            else if (passwordMismatch != null)
            {
                ViewBag.Message = "Password mismatch, please try again..";
            }
            else if (idNotPresent != null)
            {
                ViewBag.Message = "Entered Id not present, please register if you have'nt already!";
            }
            else
            {
                ModelState.AddModelError("", "**Invalid Username or Password**");

            }
            return View();


        }

        // forget password section, updates password
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(UserModel model)
        {
            try
            {

                int id = model.User_Id;
                var result = db.Users.Where(x => x.User_Id == id).FirstOrDefault();
                if (result != null)
                {
                    var nickName = db.Users.Where(x => x.SecurityQuestionNickName == model.SecurityQuestionNickName).FirstOrDefault();
                    var friendName = db.Users.Where(x => x.SecurityQuestionFriendName == model.SecurityQuestionFriendName).FirstOrDefault();
                    var schoolName = db.Users.Where(x => x.SecurityQuestionSchoolName == model.SecurityQuestionSchoolName).FirstOrDefault();
                    if (nickName != null && friendName != null && schoolName != null)
                    {

                        result.Password = model.Password;
                        result.ConfirmPassword = model.ConfirmPassword;
                        db.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Message = "Password updated successfully";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message1 = "You got some questions wrong";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message1 = "User not registered with Mini Tekstac";

                    return View();
                }
            }
            catch
            {
                ViewBag.Message1 = "Something's broken, don't worry we are working on it and you will be up and running soon";
                return View();
            }


        }

        //forget user id
        public ActionResult ForgetUserId()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetUserId(UserModel model)
        {


            var User = db.Users.Where(x => x.Email == model.Email).FirstOrDefault();
            if (User != null)
            {
                var nickName = db.Users.Where(x => x.SecurityQuestionNickName == model.SecurityQuestionNickName).FirstOrDefault();
                var friendName = db.Users.Where(x => x.SecurityQuestionFriendName == model.SecurityQuestionFriendName).FirstOrDefault();
                var schoolName = db.Users.Where(x => x.SecurityQuestionSchoolName == model.SecurityQuestionSchoolName).FirstOrDefault();
                var checkFirstName = db.Users.Where(x => x.First_Name == model.First_Name).FirstOrDefault();
                var checkLastName = db.Users.Where(x => x.Last_Name == model.Last_Name).FirstOrDefault();
                var checkContactNumber = db.Users.Where(x => x.Contact_Number == model.Contact_Number).FirstOrDefault();

                if (nickName != null && friendName != null && schoolName != null &&
                    checkFirstName != null && checkLastName != null && checkContactNumber != null)
                {
                    ViewBag.Message = "Congrats, you've been authenticated, your user id is " + User.User_Id;
                    return View();
                }
                else
                {
                    ViewBag.Message1 = "Couldn't verify you, please try again...";
                    return View();
                }
            }
            else
            {
                ViewBag.Message1 = "User with given email Id not present";
                return View();
            }

        }

        //user logout
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                Session["UserId"] = null;
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult ConfirmLogout()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        //dashboard to follow
        /*[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]*/
        public ActionResult Dashboard()
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    int id = int.Parse(Session["UserId"].ToString());
                    var checkExamGiven = db.ExamaAndAssessmentGivens.Where(x => x.User_Id == id).FirstOrDefault();
                    if (checkExamGiven != null)
                    {
                        var list = db.ExamaAndAssessmentGivens.OrderByDescending(x => x.AssessmentGiven);
                        var checkAssessment = list.Where(x => x.User_Id == id).FirstOrDefault();
                        Session["AssessmentCode"] = checkAssessment.AssessmentCode;

                        var list1 = db.Feedback.OrderByDescending(x => x.feedbackGivenDate);
                        var feedbackCheck = list1.Where(x => x.User_Id == id).FirstOrDefault();

                        if (checkAssessment != null)//check if this user has taken an exam or not then check if has given feedback
                        {
                            if (feedbackCheck != null)
                            {
                                if (checkAssessment.User_Id == feedbackCheck.User_Id && checkAssessment.AssessmentCode == feedbackCheck.AssessmentCode
                                    && checkAssessment.FeedbackGive == true)
                                {

                                    var user = db.Users.Where(x => x.User_Id == id).FirstOrDefault();
                                    ViewBag.username = user.First_Name;



                                    var searchExams = db.ExamaAndAssessmentGivens.Include(x => x.AssessmentModel).ToList();
                                    var examsGiven = searchExams.Where(x => x.User_Id == id).ToList();

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

                                    


                                    return View(user);
                                }
                                else
                                {
                                    return RedirectToAction("TriggerFeedback");
                                }
                            }
                            else if (feedbackCheck == null || checkAssessment != null)
                            {

                                return RedirectToAction("TriggerFeedback");
                            }
                            else
                            {
                                ViewBag.noexamsgiven = "Hey, you haven't given any assessment yet";
                                var user = db.Users.Where(x => x.User_Id == id).FirstOrDefault();
                                ViewBag.username = user.First_Name;
                                return View(user);
                            }

                        }
                        else
                        {
                            ViewBag.noexamsgiven = "Hey, you haven't given any assessment yet";
                            var user = db.Users.Where(x => x.User_Id == id).FirstOrDefault();
                            ViewBag.username = user.First_Name;
                            return View(user);
                        }
                    }
                    else
                    {
                        ViewBag.noexamsgiven = "Hey, you haven't given any assessment yet";
                        var user = db.Users.Where(x => x.User_Id == id).FirstOrDefault();
                        ViewBag.username = user.First_Name;
                        return View(user);
                    }

                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Something went wrong, try logging in again " + e;
                return View();
            }
        }

        //user help 
        public ActionResult Help()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        public ActionResult Help(HelpTickets model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ModelState.Clear();
                    model.Date_of_Ticket = DateTime.Now;
                    model.TicketStatus = "Active";
                    model.User_Id = int.Parse(Session["UserId"].ToString());
                    db.Help.Add(model);
                    db.SaveChanges();
                    ViewBag.Message = "Ticket raised successfully with ticket id " + model.TicketId;
                    return View();
                }
                else
                    return View();
            }
            catch
            {
                ViewBag.Message1 = "Something went wrong, please try again later...";
                return View();
            }
        }

        //display all help tickets raised
        public ActionResult DisplayAllHelpTickets()
        {
            if (Session["UserId"] != null)
            {

                int id = int.Parse(Session["UserId"].ToString());
                var ticketDetails = db.Help.Where(x => x.User_Id == id).ToList();
                if(ticketDetails!=null)
                {
                    var display = ticketDetails.OrderByDescending(x => x.TicketId);
                    var searchname = db.Users.Where(x => x.User_Id == id).FirstOrDefault();
                    ViewBag.Username = searchname.First_Name;
                    return View(display);

                }
                else
                {
                    ViewBag.Msg = "You haven't raised any help tickets yet";
                    return View();
                }
                
            }
            else return RedirectToAction("Login");
        }

        //user feedback


        public ActionResult Feedback(int id/*, int assessmentId */ )
        {
            ViewBag.Id = id;
            if (Session["UserId"] != null)
            {
                /*Session["AssessmentId"] = assessmentId;*/
                int idss = int.Parse(Session["UserId"].ToString());
                var searchFeedback = db.ExamaAndAssessmentGivens.Where(x => x.User_Id == idss && x.FeedbackGive == false).FirstOrDefault();

                int i = 0;
                do
                {

                    int assessCode = int.Parse(Session["AssessmentCode"].ToString());

                    var assessmentName = db.Assessments.Where(x => x.AssessmentCode == assessCode).FirstOrDefault();
                    ViewBag.assessmentName = assessmentName.AssessmentName;
                    var p = db.ExamaAndAssessmentGivens.Where(x=>x.AssessmentCode==assessCode).FirstOrDefault();
                    ViewBag.assessmentDate = p.AssessmentGiven.ToShortDateString();
                    int ids = int.Parse(Session["UserId"].ToString());
                    var question = db.FeedbackQuestions.ToArray();

                    ViewBag.question = question[id].Question;
                    Session["QuestionNo"] = question[id].QuestionNo;
                    i++;
                    return View();

                } while (i <= db.FeedbackQuestions.Count());
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult Feedback(FeedbackModel model)
        {

            if (Session["UserId"] != null)
            {

                int assessCode = int.Parse(Session["AssessmentCode"].ToString());
                int id = int.Parse(Session["UserId"].ToString());
                var searchUser = db.Feedback.Where(x => x.User_Id == id).ToArray();
                var assessmentName = db.Assessments.Where(x => x.AssessmentCode == assessCode).FirstOrDefault();
                ViewBag.assessmentName = assessmentName.AssessmentName;

                var p = db.ExamaAndAssessmentGivens.Where(x => x.AssessmentCode == assessCode).FirstOrDefault();
                ViewBag.assessmentDate = p.AssessmentGiven.ToShortDateString();
                var question = db.FeedbackQuestions.ToArray();

                model.User_Id = id;
                model.QuestionNo = int.Parse(Session["QuestionNo"].ToString());
                model.AssessmentCode = assessCode;
                model.feedbackGivenDate = DateTime.Now;
                int count = db.FeedbackQuestions.Count();
                ViewBag.fbquestionsCount = count;
                var list3 = db.ExamaAndAssessmentGivens.OrderByDescending(x => x.AssessmentGiven);
                var searchStatus = list3.Where(x => x.User_Id == id).FirstOrDefault();
                searchStatus.FeedbackGive = true;
                db.Entry(searchStatus).State = EntityState.Modified;
                db.Feedback.Add(model);
                db.SaveChanges();
                ViewBag.message = "Successfully given feedback";
                return View();

            }
            else
            {
                return RedirectToAction("Login");
            }


        }


        /*pubblic ActionResult GiveFeedback()
        {
            if (Session["UserId"] != null)
            {
                if (Session["feedbackGiven"].Equals("true"))
                {
                    ViewBag.FeedbackStatus = "Feedback not available as of now";
                    return View();
                }
                else
                {
                    int assessCode = int.Parse(Session["AssessmentCode"].ToString());
                    ViewBag.AssessmentName = db.Assessments.Where(x => x.AssessmentCode == assessCode);
                    var feedbackQuestions = db.FeedbackQuestions.ToList();
                    return View(feedbackQuestions);
                }
            }
            else return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult GiveFeedback(FeedbackModel model)
        {


            return View();

        }
*/

        //trigger feedback
        public ActionResult TriggerFeedback()
        {
            int id = int.Parse(Session["UserId"].ToString());
            var searchUsername = db.Users.Where(x => x.User_Id == id).FirstOrDefault();
            ViewBag.name = searchUsername.First_Name;
            int assessCode = int.Parse(Session["AssessmentCode"].ToString());
            var searchAssessment = db.Assessments.Where(x => x.AssessmentCode == assessCode).FirstOrDefault();
            ViewBag.assessName = searchAssessment.AssessmentName;
            return View();
        }

        //take assessment
        public ActionResult TakeAssessmet()
        {

            if (Session["UserId"] != null)
            {
                var displayExams = db.ExamDetails.Include(x => x.AssessmentModel);
                var display = displayExams.ToList();
                var availableExams = display.Where(x => x.DateOfExam <= DateTime.Now);
                return View(availableExams);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult TakeAssessmet(ExamDetails model, string AssessmentName)
        {

            if (Session["UserId"] != null)
            {

                var displayExams = db.ExamDetails.Include(x => x.AssessmentModel);
                var display = displayExams.ToList();
                var availableExams = display.Where(x => x.DateOfExam <= DateTime.Now);
                var filterDisplay = availableExams.Where(x => x.AssessmentCode == model.AssessmentCode).ToList();


                if (AssessmentName != null)
                {
                    var filteredSkills = availableExams.Where(x => x.AssessmentModel.AssessmentName == AssessmentName).ToList();
                    return View(filteredSkills);
                }
                else return View(filterDisplay);
            
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
       
        //before starting assessment
        public ActionResult StartAssessment(int ExamId, int AssessmentCode, string Competency)
        {
            int id = ExamId;
            int asid = AssessmentCode;
            string c = Competency;
            var questions = db.QuestionAndAnswers.Where(x => x.Competency == Competency && x.AssessmentCode == AssessmentCode).ToList();
            ViewBag.questionscount = questions.Count();
            if (Session["UserId"] != null)
            {
                Session["ExamId"] = id.ToString();
                Session["AssessmentCode"] = asid.ToString();
                Session["Competency"] = c;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        //live assessment
        public ActionResult LiveAssessment()
        {
            if (Session["UserId"] != null)
            {
                int examId = int.Parse(Session["ExamId"].ToString());
                int AssessCode = int.Parse(Session["AssessmentCode"].ToString());
                string competency = Session["Competency"].ToString();

                var questions = db.QuestionAndAnswers.Where(x => x.Competency == competency && x.AssessmentCode == AssessCode).ToList();
                var searchass = db.Assessments.Where(x => x.AssessmentCode == AssessCode).FirstOrDefault();
                ViewBag.assName = "Assessment: " + searchass.AssessmentName;
                ViewBag.msg = "Total Questions: " + questions.Count;

                ViewBag.Count = questions.Count;

                return View(questions);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

   
        public ActionResult AfterSubmitAssessment(int score)
        {
            if (Session["UserId"] != null)
            {
                int examId = int.Parse(Session["ExamId"].ToString());
                int AssessCode = int.Parse(Session["AssessmentCode"].ToString());
                string competency = Session["Competency"].ToString();
                int userId = int.Parse(Session["UserId"].ToString());
                var userName = db.Users.Where(x => x.User_Id == userId).FirstOrDefault();
                var usname = userName.First_Name +" "+ userName.Last_Name;
                var assname = db.Assessments.Where(x => x.AssessmentCode == AssessCode).FirstOrDefault();
                var AsssName = assname.AssessmentName;
                var questions = db.QuestionAndAnswers.Where(x => x.Competency == competency && x.AssessmentCode == AssessCode).ToList();

                ExamaAndAssessmentGiven model = new ExamaAndAssessmentGiven();
                List<string> report = new List<string>();
                model.User_Id = userId;
                model.AssessmentCode = AssessCode;
                model.ExamId = examId;
                model.FeedbackGive = false;
                model.AssessmentGiven = DateTime.Now;
                model.Score = score;
                report.Add(usname);
                report.Add(AsssName);
                report.Add(score.ToString());

                float totalQuestions = questions.Count;
                float y = (score / totalQuestions) * 100;
                model.percentageMarks = (int)Math.Round(y);
                int marks = (int)Math.Round(y);
                report.Add(marks.ToString());
                Session["report"] = report;
                db.ExamaAndAssessmentGivens.Add(model);
                db.SaveChanges();
                ViewBag.Message = "Successfully taken assessment, you got " + score + " correct answers from a total of " + questions.Count + " questions";
                ViewBag.Marks = "Total Percentage of marks scored: ";
                ViewBag.M = model.percentageMarks + " %";
                if (model.percentageMarks >= 40)
                {
                    ViewBag.CertificateStatusPass = "You have successfully cleared the assessment, please download your certificate from below :)";
                }
                else
                {
                    ViewBag.CertificateStatusFail = "We're sorry, but you didn't cleared the exams :(";
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //report generation
        public FileResult CreatePdfForDateRange()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("Report" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 7 columns  
            PdfPTable tableLayout = new PdfPTable(4);
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

            float[] headers = { 50, 24, 45, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            //  List<BookPackage> bookings = context.BookPackages.ToList<BookPackage>();
            var Data = Session["report"];
            List<string> report = Data as List<string>;


            tableLayout.AddCell(new PdfPCell(new Phrase("Result of Assessment", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "User Name");
            AddCellToHeader(tableLayout, "Assessment Name");
            AddCellToHeader(tableLayout, "Score");
            AddCellToHeader(tableLayout, "Percentage Marks");



            ////Add body  



            AddCellToBody(tableLayout, report[0]);
            AddCellToBody(tableLayout, report[1]);
            AddCellToBody(tableLayout, report[2]);
            AddCellToBody(tableLayout, report[3]);





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


        //assessment report
        public ActionResult AssessmentReport()
        {
            if (Session["UserId"] != null)
            {
                int id = int.Parse(Session["UserId"].ToString());
                var s = db.ExamaAndAssessmentGivens.Include(x => x.UserModel).Include(x => x.AssessmentModel).Include(x => x.ExamDetails);
                var listAssessmentTaken = s.OrderByDescending(x=>x.AssessmentGiven).Where(x => x.User_Id == id).ToList();

                return View(listAssessmentTaken);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public FileResult CreatePdfForDateRanges(string username, string assname, int score, int marks)
        {
            Session["usname"] = username;
            Session["assname"] = assname;
            Session["score"] = score;
            Session["marks"] = marks;
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("Report" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 7 columns  
            PdfPTable tableLayout = new PdfPTable(4);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   
            doc.Add(Adds_Content(tableLayout));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }

        protected PdfPTable Adds_Content(PdfPTable tableLayout)
        {

            float[] headers = { 50, 24, 45, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            //  List<BookPackage> bookings = context.BookPackages.ToList<BookPackage>();
            var Data = Session["report"];
            List<string> report = Data as List<string>;


            tableLayout.AddCell(new PdfPCell(new Phrase("Result of Assessment", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "User Name");
            AddCellToHeader(tableLayout, "Assessment Name");
            AddCellToHeader(tableLayout, "Score");
            AddCellToHeader(tableLayout, "Percentage Marks");
            string usname = Session["usname"].ToString();
            string assname = Session["assname"].ToString();
            string score = Session["score"].ToString();
            string marks = Session["marks"].ToString();


            ////Add body  



            AddCellToBody(tableLayout, usname);
            AddCellToBody(tableLayout, assname);
            AddCellToBody(tableLayout, score);
            AddCellToBody(tableLayout, marks);





            return tableLayout;
        }

        //user details display
        public ActionResult Profile()
        {
            if (Session["UserId"] != null)
            {
                int userid = int.Parse(Session["UserId"].ToString());
                var userdetails = db.Users.Where(x => x.User_Id == userid).FirstOrDefault();
                ViewBag.pass = userdetails.Password;
                return View(userdetails);

            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}