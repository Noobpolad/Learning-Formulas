using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningFormulas
{
    public static class GlobalConfig
    {
        public static readonly string FormulasFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Formulas.txt";
        public static readonly string PicturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Pictures";

    }
}
