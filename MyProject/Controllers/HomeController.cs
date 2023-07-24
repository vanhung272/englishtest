using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using Microsoft.AspNetCore.Http;
namespace WebApplication9.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int id = 0)
        {
            using (ProjectContext context = new ProjectContext())
            {

              
                ViewBag.Message = "You have to Login to do exercise ! ";
                ViewBag.exam = context.Exams.ToList();
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User model)
        {
            using (ProjectContext context = new ProjectContext())
            {

                var user = context.Users.SingleOrDefault(l => l.Username == model.Username && l.Password == model.Password);
                if (user != null)
                {
                    int usid = user.UserId;
                    int id = 0;
                    return RedirectToAction("Home", new { id = id, userId = usid });

                }
                else
                {
                    ViewBag.LoginFailMessage = "Username or Password is incorrect";

                    return View(model);
                }
            }
        }

        public IActionResult Home(int id,int userId)
        {
            using (ProjectContext context = new ProjectContext())
            {
                var questions = context.Questions.Include(q => q.Answers).Where(q => q.ExamId == id).ToList();
                ViewBag.exam = context.Exams.ToList();
                ViewBag.abc = userId;
                ViewBag.examname = context.Exams.Where(e => e.ExamId == id).Select(e => e.ExamName).FirstOrDefault();
             
                return View(questions);
            }
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(User user)
        {
            using (ProjectContext context = new ProjectContext())
            {
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

            }
        }
        [HttpPost]
        public IActionResult Submitted(int questionId, int answerId)
        {
            using (ProjectContext context = new ProjectContext())
            {
                var answer = context.Answers.FirstOrDefault(a => a.AnswerId == answerId);
                if (answer != null && answer.IsCorrect)
                {
                    // Tăng điểm số của người dùng nếu đáp án đúng
                    HttpContext.Session.SetInt32($"score_{questionId}", 1);
                }
                else
                {
                    // Đặt điểm số của người dùng về 0 nếu đáp án sai hoặc không tìm thấy đáp án
                    HttpContext.Session.SetInt32($"score_{questionId}", 0);
                }

                // Tính tổng điểm số của người dùng cho tất cả các câu hỏi
                int totalScore = 0;
                foreach (var question in context.Questions)
                {
                    int? score = HttpContext.Session.GetInt32($"score_{question.QuestionId}");
                    if (score.HasValue)
                    {
                        totalScore += score.Value;
                    }
                }

                return Json(totalScore);
            }
        }
        public IActionResult LogOut()
        {
            return RedirectToAction("Index");
        }
        public IActionResult Information(int id)
        {
            User user = new User();
            using (ProjectContext _context = new ProjectContext())
            {
                 user = _context.Users.SingleOrDefault(u=> u.UserId == id);
                 return View(user);
               
            }
            //return RedirectToAction("Information", new { user = user });
        }

    }
}
