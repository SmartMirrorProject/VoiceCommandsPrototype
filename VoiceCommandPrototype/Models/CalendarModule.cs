using System.Collections.Generic;
using System.Diagnostics;
using Windows.Media.SpeechRecognition;
using VoiceCommandPrototype.Models.Commands;

namespace VoiceCommandPrototype.Models
{
    //TODO add example for calendar of the ability to include a set of commands
    //that are optional commands, enableable and disableable.
    class CalendarModule : IVoiceControlModule
    {
        public bool IsVoiceControlLoaded { get; set; }
        public bool IsVoiceControlEnabled { get; set; }
        public string VoiceControlKey { get; }
        public string GrammarFilePath { get; }
        public SpeechRecognitionGrammarFileConstraint Grammar { get; set; }

        public CalendarModule(string grammarFilePath)
        {
            IsVoiceControlLoaded = false;
            IsVoiceControlEnabled = false;
            VoiceControlKey = "calendar";
            GrammarFilePath = grammarFilePath;
        }

        /// <summary>
        /// Receive a voice recognition result and handle logic based on command.
        /// </summary>
        public void ProcessVoiceCommand(SpeechRecognitionResult command)
        {
            IReadOnlyDictionary<string, IReadOnlyList<string>> tags = command.SemanticInterpretation.Properties;
            string cmd = tags.ContainsKey(CalendarCommands.TAG_CMD) ? tags[CalendarCommands.TAG_CMD][0] : "";
            string timeFrame = tags.ContainsKey(CalendarCommands.TAG_TIME) ? tags[CalendarCommands.TAG_TIME][0] : "";

            switch (cmd)
            {
                case WeatherCommands.CMD_SHOW:
                    Debug.WriteLine("CMD_SHOW: " + cmd);
                    break;
                case WeatherCommands.CMD_HIDE:
                    Debug.WriteLine("CMD_HIDE: " + cmd);
                    break;
                default:
                    Debug.WriteLine("An error occured, unhandled voice command: " + command.Text);
                    break;
            }
            switch (timeFrame)
            {
                case WeatherCommands.TIME_TODAY:
                    Debug.WriteLine("TIME_TODAY: " + timeFrame);
                    break;
                case WeatherCommands.TIME_TOMORROW:
                    Debug.WriteLine("TIME_TOMORROW: " + timeFrame);
                    break;
                case WeatherCommands.TIME_WEEK:
                    Debug.WriteLine("TIME_WEEK: " + timeFrame);
                    break;
                case CalendarCommands.TIME_MONTH:
                    Debug.WriteLine("TIME_MONTH: " + timeFrame);
                    break;
            }
        }
    }
}
