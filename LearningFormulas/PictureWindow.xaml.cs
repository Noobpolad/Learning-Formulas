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
    /// Interaction logic for PictureWindow.xaml
    /// </summary>
    public partial class PictureWindow : Window
    {
        public PictureWindow(string uri)
        {
            InitializeComponent();
            InitializeImage(uri);
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        /// <summary>
        /// Check if ESC is pressed, close the window in this case.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        /// <summary>
        /// Initialize image from the given URI.
        /// </summary>
        /// <param name="uri"></param>
        private void InitializeImage(string uri)
        {
            FormulaPicture.Source = new BitmapImage(new Uri(uri));
        }

    }
}
