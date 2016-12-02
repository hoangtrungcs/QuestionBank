using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.LocalizeResource;

namespace EnglishQuestion.Entity
{
    public class SubTest : BaseEntity
    {
        public int TestId { get { return Get<int>(); } set { Set(value); } }
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string XmlWritingStructure { get { return Get<string>(); } set { Set(value); } }
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string XmlListeningStructure { get { return Get<string>(); } set { Set(value); } }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string WritingTestContent { get { return Get<string>(); } set { Set(value); } }
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string ListeningTestContent { get { return Get<string>(); } set { Set(value); } }

        public string No { get { return Get<string>(); } set { Set(value); } }
        public string Name { get { return Get<string>(); } set { Set(value); } }

        public Test Test { get; set; }

        [NotMapped]
        public ObservableCollection<KeyValueDisplay> SubTestTypes
            => new ObservableCollection<KeyValueDisplay>()
            {
                new KeyValueDisplay() { Key = SubTestType.Writing.ToString(), Value = AppCommonResource.Writing },
                new KeyValueDisplay() { Key = SubTestType.Listening.ToString(), Value = AppCommonResource.Listening }
            };
    }
}
