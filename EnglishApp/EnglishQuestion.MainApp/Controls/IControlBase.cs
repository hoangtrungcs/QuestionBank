using EnglishQuestion.Common;

namespace EnglishQuestion.MainApp.Controls
{
    public interface IControlBase
    {
        TestLevel Level { get; }
        void Insert();
        void Save();
        void Delete();

        void Cancel();
    }
}
