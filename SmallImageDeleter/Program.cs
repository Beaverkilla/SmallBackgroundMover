using System;
using System.IO;
using System.Drawing;
using ImageDimensions;

namespace SmallImageDeleter
{
    static class Program
    {
        static void Main(string[] args)
        {
            String filepath = @"F:\Images\";
            var deleteFolderPath = @"F:\DeleteImages\";
            var logFile = new StreamWriter(filepath + "results.log");
            DirectoryInfo di = new DirectoryInfo(filepath);
            double i = 0;
            using (ProgressBar pb = new ProgressBar())
            {
                double files = di.GetFiles().Length;

                Console.Write("Counting files in folder :");
                foreach (var fileInfo in di.GetFiles())
                {
                    pb.Report((i/files));
                    if ((i/files) < 0.95)
                    {
                        i++;
                        continue;
                    }

                    if (fileInfo.Extension != ".jpg" && fileInfo.Extension != ".jpeg" && fileInfo.Extension != ".bmp" && fileInfo.Extension != ".png")
                    {
                        i++;
                        continue;
                    }

                    var bitmap = Image.FromFile(fileInfo.FullName);
                    var a = bitmap.Size;//ImageHelper.GetDimensions(fileInfo.FullName);
                    bitmap.Dispose();

                    try
                    {
                        if (a.Height > 2000 && a.Width < 2000)
                        {
                            File.Move(fileInfo.FullName, deleteFolderPath + fileInfo.Name);
                        }
                        else if (a.Height < 2000 && a.Width < 2000)
                        {
                            File.Move(fileInfo.FullName, deleteFolderPath + fileInfo.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        logFile.WriteLineAsync("Error");
                    }

                    i++;
                }
            }

            logFile.Close();
            Console.WriteLine();
            Console.WriteLine("Task Completed.");
            var throwaway = Console.ReadLine();
        }
    }
}
