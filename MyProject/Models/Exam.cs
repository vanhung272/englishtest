using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public partial class Exam
    {
        public Exam()
        {
            Questions = new HashSet<Question>();
            Results = new HashSet<Result>();
        }

        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamTime { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
