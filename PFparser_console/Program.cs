using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFparser_console
{
    class Program
    {
        static void Main(string[] args)
        {
            #region For two files

            //string enPatentFileName = "claims_en.txt";
            //string ruPatentFileName = "claims_ru.txt";

            //string outputFileName = "output.txt";

            //PatentParser.parseTwoInOne(enPatentFileName, ruPatentFileName, outputFileName);

            #endregion 

            #region For many files (to one file)

            
            
            //string in_dirName = "IN";
            //string out_dirName = "OUT";
            //string outputFileName = "output.txt";


            //PatentParser.parseAllInOne(in_dirName, out_dirName, outputFileName);

            //;

            



            #endregion

            #region For many files (to DB)



            string in_dirName = "IN";
            


            PatentParser.parseAllInDB(in_dirName);

            ;





            #endregion
        }
    }
}
