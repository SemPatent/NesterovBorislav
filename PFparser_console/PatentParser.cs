using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace PFparser_console
{
    class PatentParser
    {
        //string path_in;
        //string path_out;
        //string path_cur;

        private static Database1Entities1 db = new Database1Entities1(); // создаем представление БД

        /// <summary>
        /// Parses EN and RU files (two) into one output file
        /// </summary>
        /// <param name="enPatentFileName"></param>
        /// <param name="ruPatentFileName"></param>
        /// <param name="outputFileName"></param>
        public static void parseTwoInOne(string enPatentFileName, string ruPatentFileName, string outputFileName)
        {
            //Read patent files into patent strings
            string enPatent = System.IO.File.ReadAllText(enPatentFileName);
            string ruPatent = System.IO.File.ReadAllText(ruPatentFileName);

            //Clean patent strings from unwanted characters
            string enPatentCleaned = cleanerEN(enPatent);
            string ruPatentCleaned = cleanerRU(ruPatent);


            #region Form Regular Expression for searching sentences

            string sNamedSentence = @"sent"; // имя в регулярном выражениии (для извлечения информации)

            // Sentence
            string sEndDescription = @"(\. |\.$)"; // Конечный стринг обрамляющий Description (для извлечения информации)

            // Вспомогательные стринги (для извлечения информации)
            //string anyThing = @".*?"; // что-то или что угодно, действует до последующей осмысленной конструкции (например до sStartUrl)
            string Brac = @"(?<"; // открывающая скобка для именованого регулярного выражения
            string ketS = @">.*?)"; // закрывающая скобка для именованого регулярного выражения

            string regExpr = (Brac + sNamedSentence + ketS) + sEndDescription;


            Regex rx = new Regex(regExpr); // создаем экземпляр регулярки

            MatchCollection enMatches = rx.Matches(enPatentCleaned); // извлекаем все строки соответствующие нашему регулярному выражению
            MatchCollection ruMatches = rx.Matches(ruPatentCleaned); // извлекаем все строки соответствующие нашему регулярному выражению

            #endregion

            #region Collect Sentences

            //Create Lists of sentences
            List<string> enSents = new List<string>();
            List<string> ruSents = new List<string>();

            foreach (Match match in enMatches)
            {
                if ( isSentence(match.ToString()) )
                {
                    enSents.Add(match.ToString());
                }
            }

            foreach (Match match in ruMatches)
            {
                if (isSentence(match.ToString()))
                {
                    ruSents.Add(match.ToString());
                }
            }
                      

            #endregion


            #region Write sentences to File

            FileInfo outputFile = new FileInfo(outputFileName);
            StreamWriter sw;
            if (!outputFile.Exists)
            {
                sw = outputFile.CreateText();
            }
            else
            {
                sw = outputFile.AppendText();
            }

            for (int i = 0; i < enSents.Count(); ++i)
            {
                sw.WriteLine(enSents[i]);
                sw.WriteLine("-----------");
                sw.WriteLine(ruSents[i]);
                sw.WriteLine("===========");
            }

            sw.WriteLine("////////////////////////////////");

            sw.Close();

            ;

            #endregion

        }

        /// <summary>
        /// Parses all patents from "in" directory into one file from "out" directory
        /// </summary>
        /// <param name="outputFileName"></param>
        public static void parseAllInOne(string in_dirName,string out_dirName, string outputFileName)
        {
            //Get paths
            
            string path_in; //IN folder path
            string path_out; //OUT folder path
            string path_out_file; //path to output file
            string path_cur; //path to app's working directory

            path_cur = Directory.GetCurrentDirectory();

            path_in = path_cur + "\\" + in_dirName;
            path_out = path_cur + "\\" + out_dirName;
            path_out_file = path_out + "\\" + outputFileName;
            
            
            // Parse files from "IN" directory

            string[] files_IN;
            files_IN = Directory.GetFiles(path_in);

            string pat_label;
            string pat_code;
            string enPatentFileName;
            string ruPatentFileName;
            foreach (string pat_path in files_IN)
            {
                pat_label = pat_path.Substring(pat_path.Length - 6, 2);
                if (pat_label == "EN")
                {
                    enPatentFileName = pat_path; // got Engliush patent

                    //Find Russian "brother" for English patent
                    //pat_code = pat_path.Substring(pat_path.Length - 19, 12);
                    ruPatentFileName = pat_path.Substring(0, pat_path.Length - 6) + "RU.txt";

                    parseTwoInOne(enPatentFileName, ruPatentFileName, path_out_file);

                    ;
                }                
            }


            ;

        }



        

        public static void parseTwoInDB(string enPatentFileName, string ruPatentFileName)
        {
            //Read patent files into patent strings
            string enPatent = System.IO.File.ReadAllText(enPatentFileName);
            string ruPatent = System.IO.File.ReadAllText(ruPatentFileName, Encoding.UTF8);

            //Clean patent strings from unwanted characters
            string enPatentCleaned = cleanerEN(enPatent);
            string ruPatentCleaned = cleanerRU(ruPatent);


            #region Form Regular Expression for searching sentences

            string sNamedSentence = @"sent"; // имя в регулярном выражениии (для извлечения информации)

            // Sentence
            string sEndDescription = @"(\. |\.$)"; // Конечный стринг обрамляющий Description (для извлечения информации)

            // Вспомогательные стринги (для извлечения информации)
            //string anyThing = @".*?"; // что-то или что угодно, действует до последующей осмысленной конструкции (например до sStartUrl)
            string Brac = @"(?<"; // открывающая скобка для именованого регулярного выражения
            string ketS = @">.*?)"; // закрывающая скобка для именованого регулярного выражения

            string regExpr = (Brac + sNamedSentence + ketS) + sEndDescription;


            Regex rx = new Regex(regExpr); // создаем экземпляр регулярки

            MatchCollection enMatches = rx.Matches(enPatentCleaned); // извлекаем все строки соответствующие нашему регулярному выражению
            MatchCollection ruMatches = rx.Matches(ruPatentCleaned); // извлекаем все строки соответствующие нашему регулярному выражению

            #endregion

            #region Collect Sentences

            //Create Lists of sentences
            List<string> enSents = new List<string>();
            List<string> ruSents = new List<string>();

            foreach (Match match in enMatches)
            {
                if (isSentence(match.ToString()))
                {
                    enSents.Add(match.ToString());
                }
            }

            foreach (Match match in ruMatches)
            {
                if (isSentence(match.ToString()))
                {
                    ruSents.Add(match.ToString());
                }
            }


            #endregion

            #region Save Sentences in DB

           
            var SentencesList = (from Pair in db.Sentences select Pair).ToList(); // считываем список пар предложений в БД



            Sentences newPair;// Создаем заготовку для вносимой пары


            for (int i = 0; i < enSents.Count(); ++i)
            {
                newPair = db.Sentences.Create(); // Создаем заготовку для вносимой пары

                // Заполняем поля заготовки для нового патента :
                newPair.Id = SentencesList.Count() + i + 1;
                newPair.ENSentence = enSents[i];
                newPair.RUSentence = ruSents[i];


                db.Sentences.Add(newPair); // добавляем новый патент в БД
                db.SaveChanges(); // Сохраняем внесенне изменения БД
            }



            #endregion
        }

        /// <summary>
        /// Parses all patents from "in" directory into DB
        /// </summary>
        public static void parseAllInDB(string in_dirName)
        {
            //Get paths

            string path_in; //IN folder path
            string path_cur; //path to app's working directory

            path_cur = Directory.GetCurrentDirectory();

            path_in = path_cur + "\\" + in_dirName;
           


            // Parse files from "IN" directory

            string[] files_IN;
            files_IN = Directory.GetFiles(path_in);

            string pat_label;
            string pat_code;
            string enPatentFileName;
            string ruPatentFileName;
            foreach (string pat_path in files_IN)
            {
                pat_label = pat_path.Substring(pat_path.Length - 6, 2);
                if (pat_label == "EN")
                {
                    enPatentFileName = pat_path; // got Engliush patent

                    //Find Russian "brother" for English patent
                    //pat_code = pat_path.Substring(pat_path.Length - 19, 12);
                    ruPatentFileName = pat_path.Substring(0, pat_path.Length - 6) + "RU.txt";

                    parseTwoInDB(enPatentFileName, ruPatentFileName);

                    ;
                }
            }


            ;

        }



        #region Cleaning

        /// <summary>
        /// Clean EN patent string from unwanted characters
        /// </summary>
        /// <param name="enPatent"></param>
        /// <returns></returns>
        static string cleanerEN(string enPatent)
        {
            string StartingTabs = @"[^A-Za-z0-9\(\)\.\,\;\: ]";

            string enPatentCleaned = Regex.Replace(enPatent, StartingTabs, " ");

            enPatentCleaned = cleanStartingTabs(enPatentCleaned);
            enPatentCleaned = cleanInternalTabs(enPatentCleaned);
            enPatentCleaned = cleanEndTabs(enPatentCleaned);
            enPatentCleaned = cleanAbbreviationBlanks(enPatentCleaned);

            return enPatentCleaned;
        }

        /// <summary>
        /// Clean RU patent string from unwanted characters
        /// </summary>
        /// <param name="ruPatent"></param>
        /// <returns></returns>
        static string cleanerRU(string ruPatent)
        {
            string StartingTabs = @"[^А-Яа-яA-Za-z0-9\(\)\.\,\;\: ]";

            string ruPatentCleaned = Regex.Replace(ruPatent, StartingTabs, " ");

            ruPatentCleaned = cleanStartingTabs(ruPatentCleaned);
            ruPatentCleaned = cleanInternalTabs(ruPatentCleaned);
            ruPatentCleaned = cleanEndTabs(ruPatentCleaned);
            ruPatentCleaned = cleanAbbreviationBlanks(ruPatentCleaned);

            return ruPatentCleaned;
        }

        // На входе :
        // текст
        // На выходе 
        // текст очищенный от начальных знаков табуляции (" ", \t, \n ...)
        public static string cleanStartingTabs(string inText)
        {
            string StartingTabs = @"^(\s*(?=\S))";

            string outText = Regex.Replace(inText, StartingTabs, "");

            return outText;
        }

        // На входе :
        // текст
        // На выходе 
        // текст очищенный от конечных знаков табуляции (" ", \t, \n ...)
        public static string cleanEndTabs(string inText)
        {
            string EndTabs = @"((?<=\S)\s*)$";

            string outText = Regex.Replace(inText, EndTabs, "");

            return outText;
        }

        // На входе :
        // html текст
        // На выходе 
        // текст очищенный от внутрених знаков табуляции (" ", \t, \n ...)
        public static string cleanInternalTabs(string inText)
        {
            string InternalTabs = @"(?<=\S)" + @"\s+?" + @"(?=\S)";

            string outText = Regex.Replace(inText, InternalTabs, " ");

            return outText;
        }





        public static string cleanAbbreviationBlanks(string inText)
        {
            List<string> abbreviations = new List<string>();
            List<string> abbrReplacements = new List<string>();            
            
            abbreviations.Add(@" п\. " + @"(?=\d)");
            abbrReplacements.Add(@" п.");
            abbreviations.Add(@" фиг\. " + @"(?=\d)");
            abbrReplacements.Add(@" фиг.");
            abbreviations.Add(@"см\. ");
            abbrReplacements.Add(@"см.");

            abbreviations.Add(@"c\. f\. " + @"(?=\S)");
            abbrReplacements.Add(@"cf.");
            abbreviations.Add(@"cf\. " + @"(?=\S)");
            abbrReplacements.Add(@"cf.");
            abbreviations.Add(@"deg\. ");
            abbrReplacements.Add(@"deg.");
            abbreviations.Add(@"C\. ");
            abbrReplacements.Add(@"C.");
            abbreviations.Add(@"RMS\. ");
            abbrReplacements.Add(@"RMS.");
            abbreviations.Add(@"RMS\. ");
            abbrReplacements.Add(@"RMS.");


            string outText = Regex.Replace(inText, abbreviations[0], abbrReplacements[0]);

            for (int i = 1; i < abbreviations.Count(); i++)
            {
                outText = Regex.Replace(outText, abbreviations[i], abbrReplacements[i]);
            }




            string sNamedSentence = @"cfig"; // имя в регулярном выражениии (для извлечения информации)
            // Sentence
            string sStartDescription = @"\. \(";
            string sEndDescription = @"\) "; // Конечный стринг обрамляющий Description (для извлечения информации)

            // Вспомогательные стринги (для извлечения информации)
            //string anyThing = @".*?"; // что-то или что угодно, действует до последующей осмысленной конструкции (например до sStartUrl)
            
            string Brac = @"(?<"; // открывающая скобка для именованого регулярного выражения
            string ketS = @">.*?)"; // закрывающая скобка для именованого регулярного выражения

            string regExprString = sStartDescription + (Brac + sNamedSentence + ketS) + sEndDescription;


            Regex rxf = new Regex(regExprString); // создаем экземпляр регулярки

            string patBuf = @"";
            string replBuf = @"";
            foreach (Match match in rxf.Matches(outText))
            {
                //Console.WriteLine(match.Groups["cfig"]);

                patBuf =  sStartDescription + match.Groups["cfig"] + sEndDescription;
                replBuf =  @" (" + match.Groups["cfig"] + @"). ";


                outText = Regex.Replace(outText, patBuf, replBuf);

            }

           

           


            return outText;
        }

        #endregion 

        static bool isSentence(string str)
        {
            string regExpr = @"[А-Яа-яA-Za-z]";

            Regex rx = new Regex(regExpr); // создаем экземпляр регулярки

            MatchCollection strMatches = rx.Matches(str);

            if (strMatches.Count != 0)
            {
                return true;
            }
            else
                return false;
        }

        
        
    }
}
