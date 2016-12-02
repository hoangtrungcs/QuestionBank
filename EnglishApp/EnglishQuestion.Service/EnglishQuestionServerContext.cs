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
    public class EnglishQuestionServerContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<SubTest> SubTests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
