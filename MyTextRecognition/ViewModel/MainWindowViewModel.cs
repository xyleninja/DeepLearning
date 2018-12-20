using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTextRecognition
{
    public class MainWindowViewModel : BaseViewModel
    {
        public const string alphabet = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ";

        public char currentResult { get { return p_currentResult; } set { p_currentResult = value; NotifyPropertyChanged("currentResult"); } }
        private char p_currentResult = ' ';

        public char trainChar { get { return p_trainChar; } set {p_trainChar = value; NotifyPropertyChanged("trainChar"); } }
        private char p_trainChar = 'a';

        public string filePath { get { return p_filePath; } set { p_filePath = value; NotifyPropertyChanged("filePath"); } }
        private string p_filePath;

        internal void prevTrainChar()
        {
            int currentTrainIndex = alphabet.IndexOf(trainChar);
            trainChar = alphabet[(alphabet.Length + currentTrainIndex - 1) % alphabet.Length];
        }

        internal void nextTrainChar()
        {
            int currentTrainIndex = alphabet.IndexOf(trainChar);
            trainChar = alphabet[(alphabet.Length + currentTrainIndex + 1) % alphabet.Length];
        }
    }
}
