using System.Collections.Generic;
using System.Windows.Controls;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;

namespace EnglishQuestion.MainApp.Controls.Generate
{
    public class GenerateBasePage : UserControl
    {
        public GenerateBasePage CreateGenerateLevel(TestLevel level, GenerateBaseVM pageViewModel)
        {
            GenerateBasePage result = null;
            switch (level)
            {
                case TestLevel.GLevelA:
                case TestLevel.GcLevelA:
                    result = new GenerateLevelA(pageViewModel);
                    break;
                case TestLevel.GLevelB:
                case TestLevel.GcLevelB:
                    result = new GenerateLevelB(pageViewModel);
                    break;
                case TestLevel.GLevelC:
                case TestLevel.GcLevelC:
                    result = new GenerateLevelC(pageViewModel);
                    break;
                case TestLevel.GLevelB1:
                case TestLevel.GcLevelB1:
                    result = new GenerateLevelB1(pageViewModel);
                    break;
                case TestLevel.GLevelB2:
                case TestLevel.GcLevelB2:
                    result = new GenerateLevelB2(pageViewModel);
                    break;
            }

            return result;
        }

        public IEnumerable<KeyValueDisplay> B1B2Configs
        {
            get { return DbHelper.Instance.GetB1B2Configs(); }
        }
    }
}
