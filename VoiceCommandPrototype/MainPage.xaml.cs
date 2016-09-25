using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VoiceCommandPrototype.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VoiceCommandPrototype
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly List<IVoiceControlModule> voiceControlModules = new List<IVoiceControlModule>();
        public MainPage()
        {
            this.InitializeComponent();
            //All voice control modules should be loaded into the voice processor on creation.
            VoiceProcessor.Instance.InitializeSpeechRecognizer();
            voiceControlModules.Add(new MainModule("Grammar\\mainGrammar.xml"));
            voiceControlModules.Add(new CalendarModule("Grammar\\weatherGrammar.xml"));
            voiceControlModules.Add(new WeatherModule("Grammar\\calendarGrammar.xml"));
            VoiceProcessor.Instance.LoadModulesAndStartProcessor(voiceControlModules);
        }

        public static void MainPage_Unloaded(object sender, object args)
        {
            VoiceProcessor.Instance.UnloadSpeechRecognizer();
        }
    }
}
