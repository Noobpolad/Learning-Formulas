using LearningFormulas.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace LearningFormulas
{
    /// <summary>
    /// Interaction logic for SplitQuestionsWindow.xaml
    /// </summary>
    public partial class SplitQuestionsWindow : Window
    {

        private List<FormulaModel> formulas;
        private int counter = 0;

        public SplitQuestionsWindow()
        {
            InitializeComponent();
            formulas = new List<FormulaModel>();
            this.Loaded += SplitQuestionsWindow_Loaded;
        }

        /// <summary>
        /// When window is loaded, initialize the formulas, randomize them, and initialize the title for TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitQuestionsWindow_Loaded(object sender, EventArgs e)
        {
            InitializeFormulas();
            RandomizeFormulas();
            InitializeFormulaTitle(counter);
        }

        /// <summary>
        /// Initialize the list of FormulaModel, close the window if list is empty.
        /// </summary>
        private void InitializeFormulas()
        {
            var lines = File.ReadAllLines(GlobalConfig.FormulasFile);

            foreach (var line in lines)
            {
                string[] lineElements = line.Split('|');
                formulas.Add(new FormulaModel(lineElements[0].Replace('`', '|'), lineElements[1], lineElements[2]));
            }

            if (formulas.Count == 0)
            {
                MessageBox.Show("You have no added formulas, add one and then come here!","No formulas found.", MessageBoxButton.OK,MessageBoxImage.Error);
                this.Close();
            }

        }

        private void RandomizeFormulas()
        {
            formulas = formulas.OrderBy(x => Guid.NewGuid()).ToList();
        }

        private void InitializeFormulaTitle(int index)
        {
            if (index < formulas.Count)
            {
                FormulaTitle.Text = formulas.ElementAt(index).FormulaTitle;
            }
        }

        /// <summary>
        /// Show the PictureWindow with the current picture uri, while window is open disable ShowButton and NextButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            PictureWindow pw = new PictureWindow(formulas.ElementAt(counter).FormulaImageFileName);
            pw.Closed += Pw_Closed;
            ShowButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            pw.Show();
        }

        /// <summary>
        /// When PictureWindow is closed, enable ShowButton and NextButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pw_Closed(object sender, EventArgs e)
        {
            ShowButton.IsEnabled = true;
            NextButton.IsEnabled = true;
        }

        /// <summary>
        /// Check if any formula left, select next formula.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextFormulaButton_Click(object sender, RoutedEventArgs e)
        {
            if (counter == formulas.Count - 1)
            {
                MessageBox.Show("No more formulas left!","No more formulas found.",MessageBoxButton.OK,MessageBoxImage.Stop);
                return;
            }
            InitializeFormulaTitle(++counter);
        }
    }
}
