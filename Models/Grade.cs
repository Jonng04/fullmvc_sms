using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Grade
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        [StringLength(20)]
        public string? Score { get; set; }
    }
}
