using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommandPrototype.Models.Commands
{
    class WeatherCommands
    {
        public const string TAG_CMD = "cmd";

        public const string TAG_TIME = "timeFrame";

        public const string TARGET = "WEATHER";

        public const string CMD_SHOW = "SHOW";

        public const string CMD_HIDE = "HIDE";

        public const string TIME_TODAY = "TODAY";

        public const string TIME_TOMORROW = "TOMORROW";

        public const string TIME_WEEK = "WEEK";
    }
}
