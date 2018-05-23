using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.Enum
{
    public enum RunningState
    {
        Stopped,
        Running,
        Starting
    }

    public enum SettingType
    {
        General,
        Support,
        Data,
        DisplayStyleSetting
    }

    public enum TextPositionState: int
    {
        Left = 0,
        Center = 1,
        Right = 2,
        None = 3,
    }
}
