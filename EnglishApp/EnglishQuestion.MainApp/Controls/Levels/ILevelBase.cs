using System;

namespace EnglishQuestion.MainApp.Controls.Levels
{
    public interface ILevelBase : IControlBase
    {
        DateTime Created { get; }
    }
}
