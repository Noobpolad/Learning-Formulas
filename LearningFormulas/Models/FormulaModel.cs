using System; 
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningFormulas.Models
{
    public class FormulaModel
    {

        public string FormulaTitle { get; private set; }
        public string FormulaImageFileName { get; private set; }
        public string Book { get; private set; }

        public FormulaModel()
        {

        }

        public FormulaModel(string FT, string FIFN, string book)
        {
            FormulaTitle = FT;
            FormulaImageFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Pictures\" + FIFN;
            Book = book;
        }

        public bool IsEmpty()
        {
            return FormulaTitle.Length == 0 && FormulaImageFileName.Length == 0 && Book.Length == 0;
        }

        /// <summary>
        /// Check is one formula equals to another.
        /// </summary>
        /// <param name="formulaT"></param>
        /// <param name="formulasIFN"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool EqualsToFormula(string formulaT, string formulasIFN, string book)
        {

            if (this.FormulaTitle != formulaT.Replace('`', '|')) return false;

            if (this.FormulaImageFileName != Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Pictures\" + formulasIFN) return false;

            if (this.Book != book) return false;

            return true;
        }

    }
}
