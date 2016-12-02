using System.Data.Entity.Migrations;
using EnglishQuestion.Entity;

namespace EnglishQuestion.Service.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EnglishQuestionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EnglishQuestionContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Paragraphs.Add(new Paragraph()
            {
                Status = "Dummy"
            });
        }
    }
}
