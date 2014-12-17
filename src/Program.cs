using System;
using System.IO;
using System.Configuration;

namespace renamer
{
    static class MainClass
	{
       
		public static void Main (string[] args)
		{
            
            Replacer repl = new Replacer(@"D:\Загрузки");
            Console.WriteLine("Начинаем работу\n");
			repl.GetListOfFiles ();

            Console.WriteLine("\nРабота программы завершена.");
            Console.WriteLine("Отчет о работе {0}\\log\\move_log.txt", Directory.GetCurrentDirectory());
            Console.WriteLine("Отчет о ошибках {0}\\log\\error_log.txt", Directory.GetCurrentDirectory());
   
            Console.Title = "RePlacer:\tРабота программы завершена";

            Console.ReadLine ();
		}
        public static void CreateHTMLLog(string copiedFile, string copiedFilePath)
        {
        
        }
	}
	class Replacer{
		private  string dirPath;
		public Replacer(string directoryPath){
			dirPath = directoryPath;
		}
		public void GetListOfFiles(){
			DirectoryInfo dir = new DirectoryInfo(dirPath);
			FileInfo[] files = dir.GetFiles("*.*",SearchOption.TopDirectoryOnly);
            string msgAboutFileListLength = (files.Length > 0) ? string.Format("В папке {0} {1} файлов", dir.Name, files.Length) : string.Format("В папке {0} нету файлов", dir.Name);
             Console.Title = msgAboutFileListLength;
             Console.WriteLine(msgAboutFileListLength);
            int i = 0;
                foreach (FileInfo f in files){
                   
                    if (i % 10 == 0) { Console.Clear(); }
                    Console.WriteLine("\nсейчас на очереди {0}\nфайл {1}/{2}\nразмер {3} bite", f.Name, i + 1, files.Length, f.Length);
                    i++;
                    Move(f);
                }
		}
        
        private void Move(FileInfo fileToCopy){

            try
            {
            string sPath = string.Format(CreatePath(fileToCopy)+fileToCopy);
           Console.WriteLine(sPath);

            FileInfo tempFile = new FileInfo(sPath);   
                                    
                if (File.Exists(sPath))
                {
                    tempFile.Delete();
                }

                fileToCopy.MoveTo(sPath);

                Console.WriteLine("Файл {0} перемещен в каталог {1}", fileToCopy.Name, tempFile.DirectoryName);
                WriteToLog(fileToCopy.Name, fileToCopy.DirectoryName, fileToCopy.CreationTime, "Ok");
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                WriteToLog(fileToCopy.Name, fileToCopy.DirectoryName, fileToCopy.CreationTime, "Error");
                Console.WriteLine("При обработке файла возникла ошибка. \nСм. {0}\\log\\error_log.txt", Directory.GetCurrentDirectory());
              
            }
		}

        
        public static void WriteToLog(string fileToLog, string sPath, DateTime creationDate, string msg)
        {
            using (StreamWriter streamWr = new StreamWriter(Directory.GetCurrentDirectory()+"\\log\\move_logs.txt", true))
            {
                streamWr.WriteLine("Result:\t{0} || {1}|| created {2}, moved to {3}", msg, fileToLog, creationDate, sPath);
            }
        }

        public static void WriteToLog(Exception e)
        {
            using (StreamWriter streamWr = new StreamWriter(Directory.GetCurrentDirectory()+"\\log\\error_log.txt", true))
            {
                streamWr.WriteLine("{0} \nMessage: \n\t{1}\nStackTrace:\n{2}\nMethod:\n\t{3}\n------------------", DateTime.Now, e.Message, e.StackTrace, e.TargetSite);
            }
        }


        //построение пути к файлу, основываясь на его типе. 
        //Используем конфиг
        //TODO
        //Протестировать и исключить вылеты

        private string CreatePath(FileInfo file)
        {
            string sPath = null;

            try
            {
                sPath = ConfigurationManager.AppSettings[file.Extension];              
            }
            catch (InvalidOperationException ex)
            {
                Replacer.WriteToLog(ex);
            }
            if (sPath == null)
            {
                sPath = ConfigurationManager.AppSettings["default"];

            } return sPath;
        }

	}
}
