using System.Collections.Generic;
using EnglishQuestion.Entity;

namespace EnglishQuestion.Service
{
    public class DbServerHelper
    {
        private static volatile DbServerHelper m_instance;
        private static object m_syncRoot = new object();

        public static DbServerHelper Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_syncRoot)
                    {
                        if (m_instance == null)
                            m_instance = new DbServerHelper();
                    }
                }

                return m_instance;
            }
        }

        public bool SynchronizeTest(IEnumerable<Test> tests)
        {
            using (var context = new EnglishQuestionServerContext())
            {
                context.Tests.AddRange(tests);
                context.SaveChanges();
            }

            return true;
        }
    }
}
