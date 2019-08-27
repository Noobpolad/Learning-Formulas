using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddFormulasWindow.xaml
    /// </summary>
    public partial class AddFormulasWindow : Window
    {
        private static RoutedUICommand chooseThePicture;
        private bool chooseThePictureIsAvailable = true;

        public static RoutedUICommand ChooseThePicture
        {
            get { return chooseThePicture; }
        }

        private string thePictureFileName;

        public AddFormulasWindow()
        {
            InitializeCommands();
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the custom commands.
        /// </summary>
        private void InitializeCommands()
        {
            chooseThePicture = new RoutedUICommand("Choose the picture", "Choose the picture", typeof(AddFormulasWindow));
        }

        /// <summary>
        /// Open the file dialog, save the chosen picture path and name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseThePictureCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath((Environment.SpecialFolder.Desktop)).ToString();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png, *.bmp";

            chooseThePictureIsAvailable = !chooseThePictureIsAvailable;
            if (ofd.ShowDialog() == true)
            {
                if (!File.Exists(GlobalConfig.PicturesFolder + @"\" + ofd.SafeFileName))
                {
                    thePictureFileName = ofd.FileName;
                    ImageSelected.Text = $"Image selected: {ofd.SafeFileName}";
                }
                else
                {
                    MessageBox.Show("The image with this name, already exist in the folder! Change the image name and continue.","Selecte exist.",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
            chooseThePictureIsAvailable = !chooseThePictureIsAvailable;
        }

        private void ChooseThePictureCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = chooseThePictureIsAvailable;
        }

        /// <summary>
        /// Check if formula is ready, move the selected formula picture to the special folder and write the formula to the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (FormulaIsReady())
            {
                File.Move(thePictureFileName, GlobalConfig.PicturesFolder + @"\" + thePictureFileName.Split('\\').Last());
                string line = FormTheLine();
                File.AppendAllText(GlobalConfig.FormulasFile, line);
                UpdateTheWindow();
            }
            else
            {
                MessageBox.Show("Fill all the fields!", "Empty fields",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Form the line with formula info.
        /// </summary>
        /// <returns></returns>
        private string FormTheLine()
        {
            string output = "";

            if (FormulaTitle.Text.Contains('|')) FormulaTitle.Text = FormulaTitle.Text.Replace('|','`');
            output += FormulaTitle.Text + "|";
            output += thePictureFileName.Split('\\').Last() + "|";
            output += ((ListBoxItem)BooksList.SelectedValue).Content + Environment.NewLine;

            return output;
        }

        /// <summary>
        /// Reset the window
        /// </summary>
        private void UpdateTheWindow()
        {
            FormulaTitle.Text = "";
            thePictureFileName = "";
            ImageSelected.Text = "No file selected";
            BooksList.SelectedIndex = 0;
        }

        /// <summary>
        /// Check if all important fields are filled up.
        /// </summary>
        /// <returns></returns>
        private bool FormulaIsReady()
        {
            if (FormulaTitle.Text.Length == 0) return false;

            if (thePictureFileName.Length == 0) return false;

            return true;
        }
    }
}
