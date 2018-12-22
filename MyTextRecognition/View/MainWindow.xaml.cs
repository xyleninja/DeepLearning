using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
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
            (DataContext as MainWindowViewModel).prevTrainChar();
        }

        private void b_nextChar_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).nextTrainChar();
        }

        private void b_predictTrain_Click(object sender, RoutedEventArgs e)
        {
            if (chx_training.IsChecked == true)
            {
            }
            else if (chx_training.IsChecked == false)
            {
                (DataContext as MainWindowViewModel).predict(can_input.Children);
            }
        }

        private void b_newSLP_Click(object sender, RoutedEventArgs e)
        {

        }

        private void b_saveSLP_Click(object sender, RoutedEventArgs e)
        {

        }

        private void b_loadSLP_Click(object sender, RoutedEventArgs e)
        {

        }

        private void b_openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
                (DataContext as MainWindowViewModel).filePath = openFileDialog.FileName;
        }

        private void tbx_output_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!chx_training.IsEnabled)
                return;

            MainWindowViewModel mainWindowViewModel = (DataContext as MainWindowViewModel);

            if (MainWindowViewModel.alphabet.IndexOf(e.Text) < 0)
                e.Handled = true;
        }

        private void tbx_output_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
