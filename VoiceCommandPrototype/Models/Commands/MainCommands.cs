using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;

namespace VoiceCommandPrototype.Models.Commands
{
    class MainCommands
    {
        public const string TAG_CMD = "cmd";

        public const string TAG_MODULE = "module";

        //There is no target for the main module
        public const string TARGET = "";

        public const string MIRROR = "MIRROR";

        public const string CMD_ON = "ON";

        public const string CMD_OFF = "OFF";

        public const string CMD_ENABLE = "ENABLE";

        public const string CMD_DISABLE = "DISABLE";

        public const string CMD_SHOW = "SHOW";

        public const string CMD_HIDE = "HIDE";

        public const string CMD_FOCUS = "FOCUS";

        //The key for this dictionary is the Module's 
        public static Dictionary<string, IVoiceControlModule> Modules;
    }
}
