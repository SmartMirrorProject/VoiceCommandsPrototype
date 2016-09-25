using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using VoiceCommandPrototype.Models.Commands;

namespace VoiceCommandPrototype.Models
{
    class VoiceProcessor
    {
        private static readonly VoiceProcessor _instance = new VoiceProcessor();
        //Constructor is private because this is class is a singleton.
        private VoiceProcessor(){}
        public static VoiceProcessor Instance => _instance;
        
        private const string TAG_MODULE = "module";
        private const string TAG_TARGET = "target";
        private const string TAG_CMD = "cmd";
        private const string TAG_TIME_FRAME = "timeFrame";

        private static Dictionary<string, IVoiceControlModule> activeModules = new Dictionary<string, IVoiceControlModule>(); 

        private SpeechRecognizer _recognizer;

        /// <summary>
        /// When the program is started, initialize the SpeechRecognizer
        /// </summary>
        public void InitializeSpeechRecognizer()
        {
            // Initialize recognizer and set it's event handlers
            _recognizer = new SpeechRecognizer();
            _recognizer.StateChanged += RecognizerStateChanged;
            _recognizer.ContinuousRecognitionSession.ResultGenerated += RecognizerResultGenerated;
        }

        public async void LoadModulesAndStartProcessor(List<IVoiceControlModule> modules)
        {
            foreach(IVoiceControlModule module in modules)
            {
                await LoadVoiceControlModule(module);
            }
            StartSpeechRecognizer();
        }

        public async Task<bool> LoadVoiceControlModule(IVoiceControlModule module)
        {
            //If the module is not loaded.
            if (!IsModuleLoaded(module))
            {
                //Create the grammar for the passed in module.
                SpeechRecognitionGrammarFileConstraint grammar = await CreateGrammarFromFile(module.GrammarFilePath,
                    module.VoiceControlKey);
                //Add the grammar file from the passed in module to the speech recognizer
                _recognizer.Constraints.Add(grammar);
                //Store the module into the activeModules Dictionary
                activeModules[module.VoiceControlKey] = module;
                //Set the voice control memeber variables of the module.
                module.IsVoiceControlLoaded = true;
                module.IsVoiceControlEnabled = true;
                module.Grammar = grammar;
                return true;
            }
            return false;
        }


        public bool IsModuleLoaded(IVoiceControlModule module)
        {
            return activeModules.ContainsValue(module);
        }

        public async void StartSpeechRecognizer()
        {
            // Compile the loaded GrammarFiles
            SpeechRecognitionCompilationResult compilationResult = await _recognizer.CompileConstraintsAsync();

            // If successful, display the recognition result.
            if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
            {
                Debug.WriteLine("Result: " + compilationResult.ToString());

                SpeechContinuousRecognitionSession session = _recognizer.ContinuousRecognitionSession;
                try
                {
                    await session.StartAsync();
                }
                catch (Exception e)
                {
                    //TODO this needs to report to the user that something failed.
                    //also potentially write to a log somewhere.               
                    Debug.WriteLine(e.Data);
                }

            }
            else
            {
                //TODO this needs to report to the user that something failed.
                //also potentially write to a log somewhere.
                Debug.WriteLine("Status: " + compilationResult.Status);
            }
        }

        //Handle appropriate voice commands
        private void RecognizerResultGenerated(SpeechContinuousRecognitionSession session,
                                                      SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            // Output debug strings
            Debug.WriteLine(args.Result.Status);
            Debug.WriteLine(args.Result.Text);

            Debug.WriteLine(args.Result.Constraint.Tag);

            int count = args.Result.SemanticInterpretation.Properties.Count;
            Debug.WriteLine("Count: " + count);
            Debug.WriteLine("Tag: " + args.Result.Constraint.Tag);            

            IVoiceControlModule commandsModule = activeModules[args.Result.Constraint.Tag];
            commandsModule.ProcessVoiceCommand(args.Result);
        }



        // Debug changes to state of the recognizer for test purposes
        private void RecognizerStateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            Debug.WriteLine("Speech recognizer state: " + args.State.ToString());
        }

        public async Task<SpeechRecognitionGrammarFileConstraint> CreateGrammarFromFile(string file, string key)
        {
            StorageFile grammarContentFile = await Package.Current.InstalledLocation.GetFileAsync(String.Format(file));
            //Create grammar constraint
            return new SpeechRecognitionGrammarFileConstraint(grammarContentFile, key);
        }



        /// <summary>
        /// When the program is terminated, stop the SpeechRecognizer
        /// </summary>
        public async void UnloadSpeechRecognizer()
        {
            //Stop voice recognition
            await _recognizer.ContinuousRecognitionSession.StopAsync();
        }
    }
}
