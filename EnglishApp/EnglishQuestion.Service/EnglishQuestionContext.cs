/*
=========================================================================================================
  Module      : EnglishQuestion Context (EnglishQuestionContext.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EnglishQuestion.Entity;

namespace EnglishQuestion.Service
{
    public class EnglishQuestionContext : DbContext
    {
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<SubTest> SubTests { get; set; }
        public DbSet<B1B2ConfigValue> B1B2ConfigValues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Paragraph>().HasOptional(x => x.Questions).WithMany().WillCascadeOnDelete();
            //modelBuilder.Entity<Question>().HasOptional(x => x.Answers).WithMany().WillCascadeOnDelete();
        }
    }
}
