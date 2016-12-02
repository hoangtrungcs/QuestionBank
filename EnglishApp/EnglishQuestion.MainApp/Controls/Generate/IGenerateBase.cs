using System;

namespace EnglishQuestion.MainApp.Controls.Generate
{
    public interface IGenerateBase : IControlBase
    {
        DateTime Created { get; }
    }
}
