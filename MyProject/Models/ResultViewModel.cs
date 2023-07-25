using MyProject.Models; 
using System.Collections.Generic;
namespace MyProject.Models
{
 
    public class ResultViewModel
    {
        public int TotalScore { get; set; }
        public List<Question> Questions { get; set; }
        public Dictionary<int, int> SelectedAnswers { get; set; } // Store the user-selected answers' correctness status
    }
}
