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
        //Берем список файлов из указанной папки, передаем на перемещение. Почти бесполезный метод, что ли. 
		public void GetListOfFiles(){
			DirectoryInfo dir = new DirectoryInfo(dirPath);
			FileInfo[] files = dir.GetFiles("*.*",SearchOption.TopDirectoryOnly);
            string msgAboutFileListLength = (files.Length > 0) ? string.Format("В папке {0} {1} файлов", dir.Name, files.Length) : string.Format("В папке {0} нету файлов", dir.Name);
             Console.Title = msgAboutFileListLength;
             Console.WriteLine(msgAboutFileListLength);
            int i = 0;
                foreach (FileInfo f in files){
                    Console.WriteLine(f.DirectoryName+"\\"+ f.Name);
                    if (i % 9 == 0) { Console.Clear(); }
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
                          
                using (FileStream fs = tempFile.Create()) { }
                if (File.Exists(sPath))
                {
                    tempFile.Delete();
                }

                fileToCopy.MoveTo(sPath);
                Console.WriteLine("Файл {0} перемещен в каталог {1}", fileToCopy.Name, tempFile.DirectoryName);
                WriteToLog(fileToCopy.Name, sPath, fileToCopy.CreationTime);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("\nОшибка:\n{0}", ex);
                WriteToLog(ex.Message);
                //Дописать вывод информации о ошибках во время исполнения
            }
		}

        
        private void WriteToLog(string fileToLog, string sPath, DateTime creationDate)
        {
            using (StreamWriter streamWr = new StreamWriter (Directory.GetCurrentDirectory()+"\\logs.txt", true)) {
                streamWr.WriteLine("{0} || created {1} => {2}",fileToLog, creationDate, sPath);
            }
        }

        private void WriteToLog(string exmsg)
        {
            using (StreamWriter streamWr = new StreamWriter(Directory.GetCurrentDirectory() + "\\errors.txt", true))
            {
                streamWr.WriteLine("Error!||{0} Text next: \n{1}", DateTime.Now, exmsg);
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
