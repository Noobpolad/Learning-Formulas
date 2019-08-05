using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace LearningFormulas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static RoutedUICommand viewFormula; 
        private bool veiwFormulasCommandAvailable = true;
        private static RoutedUICommand addFormulas;
        private bool addFormulasCommandAvailable = true;
        private static RoutedUICommand splitQuestions;
        private bool splitQuestionsCommandAvailable = true;

        public static RoutedUICommand ViewFormulas
        {
            get { return viewFormula; }
        }
        public static RoutedUICommand AddFormulas
        {
            get { return addFormulas; }
        }
        public static RoutedUICommand SplitQuestions
        {
            get { return splitQuestions; }
        }

        public MainWindow()
        {
            CheckFileAndFolderExists();
            InitializeAllCommands();
            InitializeComponent();
        }

        private void InitializeAllCommands()
        {
            viewFormula = new RoutedUICommand("View formulas", "View formulas", typeof(MainWindow));
            addFormulas = new RoutedUICommand("Add formulas", "Add formulas", typeof(MainWindow));
            splitQuestions = new RoutedUICommand("Split questions", "Split questions", typeof(MainWindow));
        }

        /// <summary>
        /// Check if folder and file exists, if not, create them.
        /// </summary>
        private void CheckFileAndFolderExists()
        {
            if (!File.Exists(GlobalConfig.FormulasFile))
            {

                using (FileStream fs = File.Create(GlobalConfig.FormulasFile)){}
                File.SetAttributes(GlobalConfig.FormulasFile,FileAttributes.Hidden);
                
            }
            if (!Directory.Exists(GlobalConfig.PicturesFolder))
            {
                Directory.CreateDirectory(GlobalConfig.PicturesFolder);
                var dir = new DirectoryInfo(GlobalConfig.PicturesFolder);
                dir.Attributes = FileAttributes.Hidden;
            }
        }

        private void ViewFormulas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            veiwFormulasCommandAvailable = !veiwFormulasCommandAvailable;
            ViewFormulasWindow vfw = new ViewFormulasWindow();
            vfw.Closed += Vfw_Closed;
            vfw.Show();
        }

        private void ViewFormulas_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = veiwFormulasCommandAvailable;
        }

        private void Vfw_Closed(object sender, EventArgs e)
        {
            veiwFormulasCommandAvailable = !veiwFormulasCommandAvailable;
            CommandManager.InvalidateRequerySuggested();
        }

        private void AddFormulas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            addFormulasCommandAvailable = !addFormulasCommandAvailable;
            AddFormulasWindow afw = new AddFormulasWindow();
            afw.Closed += Afw_Closed;
            afw.Show();
        }

        private void Afw_Closed(object sender, EventArgs e)
        {
            addFormulasCommandAvailable = !addFormulasCommandAvailable;
            CommandManager.InvalidateRequerySuggested();
        }

        private void AddFormulas_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = addFormulasCommandAvailable;
        }

        private void SplitQuestions_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            splitQuestionsCommandAvailable = !splitQuestionsCommandAvailable;
            SplitQuestionsWindow sqw = new SplitQuestionsWindow();
            sqw.Closed += Sqw_Closed;
            sqw.Show();
        }

        private void Sqw_Closed(object sender, EventArgs e)
        {
            splitQuestionsCommandAvailable = !splitQuestionsCommandAvailable;
            CommandManager.InvalidateRequerySuggested();
        }

        private void SplitQuestions_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = splitQuestionsCommandAvailable;
        }

    }
}
