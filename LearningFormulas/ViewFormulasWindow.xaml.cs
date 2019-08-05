using LearningFormulas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ViewFormulasWindow.xaml
    /// </summary>
    public partial class ViewFormulasWindow : Window
    {
        private List<FormulaModel> formulas;

        public ViewFormulasWindow()
        {
            InitializeComponent();
            formulas = new List<FormulaModel>();
            InitializeFormulas();
            WireupGrid(formulas);
            FormulasGrid.SelectedCellsChanged += FormulasGrid_SelectedCellsChanged;
        }

        /// <summary>
        /// Check if selected row is empty, disable the DeleteButton in this case. Enable the button if selected row is not empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormulasGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if ((FormulasGrid.SelectedItem as FormulaModel) == null)
            {
                DeleteButton.IsEnabled = false;
                DeleteButton.Foreground = Brushes.Gray;
                DeleteButton.BorderBrush = Brushes.Gray;
            } 
            else
            {
                DeleteButton.IsEnabled = true;
                DeleteButton.Foreground = Brushes.White;
                DeleteButton.BorderBrush = Brushes.White;
            }
        }

        /// <summary>
        /// Refresh the DataGrid.
        /// </summary>
        /// <param name="collection"></param>
        private void WireupGrid(IEnumerable<FormulaModel> collection)
        {
            FormulasGrid.ItemsSource = null;
            FormulasGrid.ItemsSource = collection;
        }

        /// <summary>
        /// Initialize the list of FormulaModel, close the window if list is empty.
        /// </summary>
        private void InitializeFormulas()
        {
            var lines = File.ReadAllLines(GlobalConfig.FormulasFile);
            foreach (string line in lines)
            {
                var lineParts = line.Split('|');
                FormulaModel newFormula = new FormulaModel(lineParts[0].Replace('`', '|'), lineParts[1], lineParts[2]);
                formulas.Add(newFormula);
            }
        }

        /// <summary>
        /// Open PictureWindow, with the given picture.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureLink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = e.OriginalSource as Hyperlink;
            PictureWindow pw = new PictureWindow(link.NavigateUri.OriginalString);
            pw.Show();
        }

        /// <summary>
        /// Reset the whole window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetLink_Click(object sender, RoutedEventArgs e)
        {
            TitleValue.Text = "";
            BookValue.SelectedIndex = 0;
            WireupGrid(formulas);
        }

        /// <summary>
        /// Filter the DataGrid according to the given request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            List<FormulaModel> filteredList = new List<FormulaModel>();

            foreach (var formula in formulas)
            {
                if (Regex.IsMatch(formula.FormulaTitle, ".*" + TitleValue.Text + ".*", RegexOptions.IgnoreCase))
                {
                    if ((BookValue.SelectedIndex != 0 && formula.Book == ((ListBoxItem)BookValue.SelectedValue).Content.ToString()) || BookValue.SelectedIndex == 0)
                    {
                        filteredList.Add(formula);
                    }
                }
            }
            WireupGrid(filteredList);
        }

        /// <summary>
        /// Delete the all the related information of the selected formula.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you shure that you want to delete this formula?", "Deletion alert!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                var currentFormula = ((FormulaModel)FormulasGrid.SelectedItem);
                List<string> changedFile = new List<string>();
                using (StreamReader sr = File.OpenText(GlobalConfig.FormulasFile))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineSplited = line.Split('|');
                        if (!currentFormula.EqualsToFormula(lineSplited[0], lineSplited[1], lineSplited[2]))
                        {
                            changedFile.Add(line);
                        }
                        else
                        {
                            File.Delete(GlobalConfig.PicturesFolder + @"\" + lineSplited[1]);
                        }
                    }
                }

                File.Delete(GlobalConfig.FormulasFile);

                using (StreamWriter sw = File.AppendText(GlobalConfig.FormulasFile))
                {
                    foreach (var line in changedFile)
                    {
                        sw.WriteLine(line);
                    }

                    sw.Close();
                }

                formulas.Remove(currentFormula);

                WireupGrid(formulas);
            }
        }

    }
}
