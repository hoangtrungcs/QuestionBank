using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace EnglishQuestion.Service
{
    public class BankQuestionContext : DbContext
    {
        public DbSet<LopHocs> Classes { get; set; }
    }

    public class LopHocs
    {
        [Key]
        public int ClassId { get; set; }

        public string ClassNo { get; set; }
        public string ClassName { get; set; }
    }
}
