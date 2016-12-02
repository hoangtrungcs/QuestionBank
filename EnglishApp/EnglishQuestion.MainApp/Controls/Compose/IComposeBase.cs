using EnglishQuestion.Common;

namespace EnglishQuestion.MainApp.Controls.Compose
{
    public interface IComposeBase : IControlBase
    {
        LevelSection Section { get; }
    }
}
