using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApplication9.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectContext context = new ProjectContext();
        private Dictionary<int, int> selectedAnswers = new Dictionary<int, int>();
        public IActionResult Index(int id = 0)
        {
            //using (ProjectContext context = new ProjectContext())
            //{

              
                ViewBag.Message = "You have to Login to do exercise ! ";
                ViewBag.exam = context.Exams.ToList();
                return View();
            //}
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User model)
        {

                var user = context.Users.SingleOrDefault(l => l.Username == model.Username && l.Password == model.Password);
                if (user != null)
                {
                    int usid = user.UserId;
                    int id = 0;
                HttpContext.Session.SetInt32("UserId", usid);
                return RedirectToAction("Home", new { id = id, userId = usid });

                }
                else
                {
                    ViewBag.LoginFailMessage = "Username or Password is incorrect";

                    return View(model);
                }
        }

        public IActionResult Home(int id,int userId)
        {
            //using (ProjectContext context = new ProjectContext())
            //{
                var questions = context.Questions.Include(q => q.Answers).Where(q => q.ExamId == id).ToList();
                ViewBag.exam = context.Exams.ToList();
                ViewBag.abc = userId;
                ViewBag.examname = context.Exams.Where(e => e.ExamId == id).Select(e => e.ExamName).FirstOrDefault();
             
                return View(questions);
            //}
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(User user)
        {
            //using (ProjectContext context = new ProjectContext())
            //{
                var existing = context.Users.FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);

                if (existing != null)
                {
                    ViewBag.Error = "Username or emaul already exists. Please choose a different username or email.";
                    return View(user);
                }
                string gender = Request.Form["Gender"];
                if (gender == "Male")
                {
                    user.Gender = true;
                }
                else
                {
                    user.Gender = false;
                }
                context.Add(user);
                context.SaveChanges();
                return RedirectToAction("Login");

            //}
        }
        [HttpPost]
        public IActionResult Submitted(Dictionary<int, int> answers)
        {
            if (answers == null || !answers.Any())
            {
                // Handle the case where no answers were submitted
                return RedirectToAction("Index");
            }
            int totalScore = 0;
            
            //using (ProjectContext context = new ProjectContext())
            //{
                foreach (var questionId in answers.Keys)
                {
                    int selectedAnswerId = answers[questionId];
                    var question = context.Questions.FirstOrDefault(q => q.QuestionId == questionId);

                    if (question != null)
                    {
                        var correctAnswer = context.Answers.FirstOrDefault(a => a.AnswerId == selectedAnswerId && a.IsCorrect);

                        if (correctAnswer != null)
                        {
                            totalScore++;
                            selectedAnswers[questionId] = selectedAnswerId; 
                        }
                        else
                        {
                            selectedAnswers[questionId] = selectedAnswerId; 
                        }
                    }
                }
                //save score to Db
                var result = new Result
                {
                    UserId = HttpContext.Session.GetInt32("UserId"),
                    ExamId = 4,
                    TotalScore = totalScore, 
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
                context.Results.Add(result);
                context.SaveChanges();
           // }
            // return score to quizview
            //var resultModel = new ResultViewModel { TotalScore = totalScore , SelectedAnswers = selectedAnswers};

            return RedirectToAction("QuizResult", new { totalScore = totalScore});
        }

        public IActionResult QuizResult(int totalScore)
        {
            // Retrieve the questions again (assuming you have a method to fetch the questions from the database)
                List<Question> questions = context.Questions.Include(q => q.Answers).Where(q => q.ExamId == 4).ToList();
                
                // Create the ViewModel and populate its properties
                var viewModel = new ResultViewModel
                {
                    TotalScore = totalScore,
                    Questions = questions,
                    SelectedAnswers = selectedAnswers
                };

                // Pass the ViewModel to the view
                return View(viewModel);
        }
        public IActionResult LogOut()
        {
            return RedirectToAction("Index");
        }
        public IActionResult Information(int id)
        {
            User user = new User();
            //using (ProjectContext _context = new ProjectContext())
            //{
                 user = context.Users.SingleOrDefault(u=> u.UserId == id);
                 return View(user);
               
            //}
            //return RedirectToAction("Information", new { user = user });
        }

    }
}
