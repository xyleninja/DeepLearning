using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MyTextRecognition
{
    public class MainWindowViewModel : BaseViewModel
    {
        public const string alphabet = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ";
        public Dictionary<bool[], char> oneHotEncoding = new Dictionary<bool[], char>(new BoolEqualityComparer());

        public char currentResult { get { return p_currentResult; } set { p_currentResult = value; NotifyPropertyChanged("currentResult"); } }
        private char p_currentResult = ' ';

        public char trainChar { get { return p_trainChar; } set {p_trainChar = value; NotifyPropertyChanged("trainChar"); } }
        private char p_trainChar = 'a';

        public string filePath { get { return p_filePath; } set { p_filePath = value; NotifyPropertyChanged("filePath"); } }
        private string p_filePath;

        private SingleLayerPerceptron singleLayerPerceptron = new SingleLayerPerceptron(16 * 16, alphabet.Length);

        public MainWindowViewModel()
        {
            foreach (char c in alphabet)
            {
                bool[] code = new bool[alphabet.Length];
                code[alphabet.IndexOf(c)] = true;
                oneHotEncoding.Add(code, c);
            }
        }

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

        internal void predict(UIElementCollection children)
        {
            currentResult = ' ';

            bool[] input = new bool[16 * 16];

            foreach (Rectangle rect in children)
            {
                int top = Convert.ToInt32(rect.GetValue(Canvas.TopProperty));
                int left = Convert.ToInt32(rect.GetValue(Canvas.LeftProperty));

                input[top * 16 + left] = true;
            }

            bool[] result = singleLayerPerceptron.predict(input);
            List<char> possibleChars = new List<char>();

            for (int i = 0; i < result.Length - 1; i++)
            {
                if (result[i])
                {
                    bool[] charCode = new bool[result.Length];
                    charCode[i] = true;
                    possibleChars.Add(oneHotEncoding[charCode]);
                }
            }

            if (possibleChars.Count>0)
            {
                Random random = new Random();
                currentResult = possibleChars[random.Next(possibleChars.Count)];
            }
        }
    }
}
