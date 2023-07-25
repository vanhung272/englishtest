using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public partial class Result
    {
        public int ResultId { get; set; }
        public int? UserId { get; set; }
        public int? ExamId { get; set; }
        public double TotalScore { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User User { get; set; }
    }
}
