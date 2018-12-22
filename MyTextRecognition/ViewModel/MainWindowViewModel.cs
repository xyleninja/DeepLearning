using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        public string trainingFilePath { get { return p_trainingFilePath; } set { p_trainingFilePath = value; NotifyPropertyChanged("trainingFilePath"); } }
        private string p_trainingFilePath = Environment.CurrentDirectory + @"\defaultTrainData";

        public string slpFilePath { get; set; } = Environment.CurrentDirectory + @"\defaultSLP";

        private SingleLayerPerceptron singleLayerPerceptron = new SingleLayerPerceptron(16 * 16, alphabet.Length);

        private List<TrainingData> training = new List<TrainingData>();

        public MainWindowViewModel()
        {
            foreach (char c in alphabet)
            {
                bool[] code = new bool[alphabet.Length];
                code[alphabet.IndexOf(c)] = true;
                oneHotEncoding.Add(code, c);
            }

            try
            {
                List<string> lines = new List<string>();
                using (StreamReader streamReader = new StreamReader(File.Open(trainingFilePath, FileMode.OpenOrCreate)))
                {
                    while (!streamReader.EndOfStream)
                    {
                        lines.Add(streamReader.ReadLine());
                    }
                }

                foreach (string line in lines)
                {
                    TrainingData trainingData = new TrainingData();
                    string[] splittedLine = line.Split(' ');
                    string inputString = splittedLine[0];
                    string outputString = splittedLine[1];

                    bool[] input = new bool[inputString.Length];
                    for (int i = 0; i < inputString.Length - 1; i++)
                    {
                        if (inputString[i] == '0')
                        {
                            input[i] = false;
                        }
                        else if (inputString[i] == '1')
                        {
                            input[i] = true;
                        }
                    }
                    trainingData.input = input;

                    //Doublecoding :(
                    bool[] output = new bool[splittedLine[1].Length];
                    for (int i = 0; i < outputString.Length - 1; i++)
                    {
                        if (outputString[i] == '0')
                        {
                            output[i] = false;
                        }
                        else if (outputString[i] == '1')
                        {
                            output[i] = true;
                        }
                    }
                    trainingData.output = output;

                    training.Add(trainingData);
                }

            }
            catch (Exception)
            {
                //Nothing special happens here since we initialized training as new list already.
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

            currentResult = oneHotEncoding.ElementAt(singleLayerPerceptron.predict(input)).Value;
        }

        internal void closing()
        {
            try
            {
                using (FileStream fs = File.Open(slpFilePath,FileMode.OpenOrCreate))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fs, singleLayerPerceptron);
                }
            }
            catch (Exception)
            {
                //Window closes anyway, maybe explain that saving data didn't work.
            }
        }

        internal void train(UIElementCollection children)
        {
            bool[] input = new bool[16 * 16];

            foreach (Rectangle rect in children)
            {
                int top = Convert.ToInt32(rect.GetValue(Canvas.TopProperty));
                int left = Convert.ToInt32(rect.GetValue(Canvas.LeftProperty));

                input[top * 16 + left] = true;
            }

            bool[] output = (from e in oneHotEncoding where e.Value == trainChar select e.Key).First();
            singleLayerPerceptron.train(input,output);
        }
    }
}
