using System;
using System.Collections.Generic;

namespace MyProject.Models
{
    public partial class User
    {
        public User()
        {
            Results = new HashSet<Result>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Result> Results { get; set; }
    }
}
