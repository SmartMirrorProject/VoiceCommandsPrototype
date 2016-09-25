using Windows.Media.SpeechRecognition;
using VoiceCommandPrototype.Models.Commands;

namespace VoiceCommandPrototype.Models
{
    interface IVoiceControlModule
    {
        //Interface Properties
        bool IsVoiceControlLoaded { get; set; }
        bool IsVoiceControlEnabled { get; set; }
        string VoiceControlKey { get; }
        string GrammarFilePath { get; }
        SpeechRecognitionGrammarFileConstraint Grammar { get; set; }


        //Interface Methods
        void ProcessVoiceCommand(SpeechRecognitionResult command);
    }
}
