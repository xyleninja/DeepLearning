using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyTextRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = viewModel = new MainWindowViewModel();
        }

        private void can_input_DrawRemove(Point notSoPerfectPoint, MouseButtonState leftButton, MouseButtonState rightButton)
        {
            Point perfectPoint = new Point();

            if (notSoPerfectPoint.X > 15)
                perfectPoint.X = 15;
            else if (notSoPerfectPoint.X < 0)
                perfectPoint.X = 0;
            else
                perfectPoint.X = Convert.ToInt32(notSoPerfectPoint.X);

            if (notSoPerfectPoint.Y > 15)
                perfectPoint.Y = 15;
            else if (notSoPerfectPoint.Y < 0)
                perfectPoint.Y = 0;
            else
                perfectPoint.Y = Convert.ToInt32(notSoPerfectPoint.Y);

            Rectangle rectangle = (from r in (from o in can_input.Children.Cast<Object>()
                                              where o.GetType() == typeof(Rectangle)
                                              select o).Cast<Rectangle>()
                                   where Canvas.GetTop(r) == perfectPoint.Y
                                   && Canvas.GetLeft(r) == perfectPoint.X
                                   select r).FirstOrDefault();

            if (leftButton == MouseButtonState.Pressed)
            {
                if (rectangle != null)
                    return;

                rectangle = new Rectangle();
                rectangle.Stroke = Brushes.Black;

                rectangle.SetValue(Canvas.LeftProperty, perfectPoint.X);
                rectangle.SetValue(Canvas.TopProperty, perfectPoint.Y);

                can_input.Children.Add(rectangle);
            }
            else if (rightButton == MouseButtonState.Pressed)
            {
                if (rectangle == null)
                    return;

                can_input.Children.Remove(rectangle);
            }
        }

        private void can_input_MouseDown(object sender, MouseButtonEventArgs e)
        {
            can_input_DrawRemove(e.GetPosition(can_input), e.LeftButton, e.RightButton);
        }

        private void can_input_MouseMove(object sender, MouseEventArgs e)
        {
            can_input_DrawRemove(e.GetPosition(can_input), e.LeftButton, e.RightButton);
        }
        
        private void b_clear_Click(object sender, RoutedEventArgs e)
        {
            can_input.Children.Clear();
        }

        private void b_prevChar_Click(object sender, RoutedEventArgs e)
        {
            viewModel.prevTrainChar();
        }

        private void b_nextChar_Click(object sender, RoutedEventArgs e)
        {
            viewModel.nextTrainChar();
        }

        private void b_predictTrain_Click(object sender, RoutedEventArgs e)
        {
            if (chx_training.IsChecked == true)
            {
                viewModel.train(can_input.Children);
            }
            else if (chx_training.IsChecked == false)
            {
                viewModel.predict(can_input.Children);
            }
        }

        private void b_newSLP_Click(object sender, RoutedEventArgs e)
        {
            viewModel.singleLayerPerceptron = new SingleLayerPerceptron(16 * 16, MainWindowViewModel.alphabet.Length);
        }

        private void b_saveSLP_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (saveFileDialog.ShowDialog() == true)
            {
                using(var fs = File.Open(saveFileDialog.FileName, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fs, viewModel.singleLayerPerceptron);
                }
            }
        }

        private void b_loadSLP_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var fs = File.Open(openFileDialog.FileName, FileMode.Open))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        viewModel.singleLayerPerceptron = (SingleLayerPerceptron)binaryFormatter.Deserialize(fs);
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Couldn't load Single Layer Perceptron. File might be corrupted.");
                }
            }
        }

        private void b_openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
                viewModel.trainingFilePath = openFileDialog.FileName;
        }

        private void tbx_output_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!chx_training.IsEnabled)
                return;

            if (MainWindowViewModel.alphabet.IndexOf(e.Text) < 0)
                e.Handled = true;
        }

        private void tbx_output_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.closing();
        }
    }
}
