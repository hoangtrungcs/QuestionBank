using System.Collections.Generic;

namespace EnglishQuestion.Entity.MetaData
{
    public class SubTestMeta
    {
        public Dictionary<string, ParagraphMeta> WritingParagraphMetas { get; set; }
        public Dictionary<string, ParagraphMeta> ListeningParagraphMetas { get; set; }

        public SubTestMeta()
        {
            WritingParagraphMetas = new Dictionary<string, ParagraphMeta>();
            ListeningParagraphMetas = new Dictionary<string, ParagraphMeta>();
        }
    }
}
