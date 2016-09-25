using System.Collections.Generic;
using System.Diagnostics;
using Windows.Media.SpeechRecognition;
using VoiceCommandPrototype.Models.Commands;

namespace VoiceCommandPrototype.Models
{
    class MainModule : IVoiceControlModule
    {
        //The main module will have a reference to every other module. This is to perform actions
        //such as loading/unloading module grammars, toggling module displays, and setting focus.
        //The key for this dictionary is the module's VoiceControlKey.
        private Dictionary<string, IVoiceControlModule> _modules = new Dictionary<string, IVoiceControlModule>();

        public bool IsVoiceControlLoaded { get; set; }
        public bool IsVoiceControlEnabled { get; set; }
        public string VoiceControlKey { get; }
        public string GrammarFilePath { get; }
        public SpeechRecognitionGrammarFileConstraint Grammar { get; set; }
       
        public MainModule(string grammarFilePath)
        {
            IsVoiceControlLoaded = false;
            IsVoiceControlEnabled = false;
            VoiceControlKey = "main";
            GrammarFilePath = grammarFilePath;
        }

        /// <summary>
        /// Receive a voice recognition result and handle logic based on command.
        /// </summary>
        public void ProcessVoiceCommand(SpeechRecognitionResult command)
        {
            IReadOnlyDictionary<string, IReadOnlyList<string>> tags = command.SemanticInterpretation.Properties;
            string cmd = tags.ContainsKey(MainCommands.TAG_CMD) ? tags[MainCommands.TAG_CMD][0] : "";
            string module = tags.ContainsKey(MainCommands.TAG_MODULE) ? tags[MainCommands.TAG_MODULE][0] : "";

            switch (cmd)
            {
                case MainCommands.CMD_ON:
                    Debug.WriteLine("CMD_ON: " + command.Text);
                    break;
                case MainCommands.CMD_OFF:
                    Debug.WriteLine("CMD_OFF: " + command.Text);
                    break;
                case MainCommands.CMD_SHOW:
                    Debug.WriteLine("CMD_SHOW: " + command.Text);
                    break;
                case MainCommands.CMD_HIDE:
                    Debug.WriteLine("CMD_HIDE: " + command.Text);
                    break;
                case MainCommands.CMD_FOCUS:
                    Debug.WriteLine("CMD_FOCUS" + command.Text + "    Module: " + module);
                    break;
                case MainCommands.CMD_ENABLE:
                    Debug.WriteLine("CMD_ENABLE" + command.Text + "    Module: " + module);
                    break;
                case MainCommands.CMD_DISABLE:
                    Debug.WriteLine("CMD_DISABLE" + command.Text + "    Module: " + module);
                    break;
                default:
                    Debug.WriteLine("An error occured, unhandled voice command: " + command.Text);
                    break;
            }
        }
    }
}
