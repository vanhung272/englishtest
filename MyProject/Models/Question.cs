using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public int? ExamId { get; set; }
        public string QuestionContent { get; set; }
        public string Explanation { get; set; }
        public int Difficulty { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
