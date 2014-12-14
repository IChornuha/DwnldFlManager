using System;
using System.IO;

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
                    CopyFile(f);

                }
		}
        
        private void CopyFile(FileInfo fileToCopy){

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

        
        private void WriteToLog(string fileToLog, string sPath, DateTime creationDate, string msg)
        {
            using (StreamWriter streamWr = new StreamWriter(Directory.GetCurrentDirectory()+"\\log\\move_logs.txt", true))
            {
                streamWr.WriteLine("Result:\t{0} || {1}|| created {2}, moved to {3}", msg, fileToLog, creationDate, sPath);
            }
        }

        private void WriteToLog(Exception e)
        {
            using (StreamWriter streamWr = new StreamWriter(Directory.GetCurrentDirectory()+"\\log\\error_log.txt", true))
            {
                streamWr.WriteLine("{0} \nMessage: \n\t{1}\nStackTrace:\n{2}\nMethod:\n\t{3}\n------------------", DateTime.Now, e.Message, e.StackTrace, e.TargetSite);
            }
        }


        //Метод определяет конечное местоположения файла в зависимости от расширения.
        //переписать, использовать .config 
        private string CreatePath(FileInfo file)
        {
            string ext = file.Extension;
            string sPath = null;
            switch (ext)
            {
                case ".pdf":
                case ".djvu":
                    sPath = @"D:\Документы\Books\";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    sPath = @"D:\Изображения\";
                    break;
                case ".zip":
                case ".7z":
                case ".tar":
                case ".rar":
                    sPath = @"D:\DownloadArc\";
                    break;
                case ".doc":
                case ".docx":
                case ".rtf":
                case ".ppt":
                case ".pptx":
                case ".xls":
                case ".xlsx":
                    sPath = @"C:\Users\Sovushka\Documents\";
                    break;
                case ".mp3":
                    sPath = @"C:\Users\Public\Music\";
                    break;
                case ".exe":
                case ".msi":
                    sPath = @"D:\Pipe\exe\";
                    break;
                default:
                    sPath = @"D:\Pipe\";
                    break;
            }
            return sPath;
        }

	}
            
//     class Rename{
//        private string spath;
//        private string substring;

//        public string FilePath { get; set; }
//        public string SubString { get; set; }
 
//        public Rename(string filepath, string substring)
//        {
//            FilePath = filepath;
//            SubString = substring;
//        }
//        public static void ChangeFileName()
//        {
//            //потом написать
//            //заложить логику проверки на ненулевой размер файла и переименовать. 
//        }
//        }
}
