using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsAppMySql.FileManaging
{
    public class DirectoryManager
    {
        private string mainDirectoryPath = Path.Combine(@"G:\", "Mój dysk", "baza_danych");
        private string directoryPath;


        private int id;
        private string name;
        private string lastname;

        public DirectoryManager(int id, string name, string lastname)
        {
            this.id = id;
            this.name = name;
            this.lastname = lastname;

            string result = id.ToString();
            if (name != null) result = result + " " + name;
            if (lastname != null) result = result + " " + lastname;

            this.directoryPath = Path.Combine(mainDirectoryPath, result.Replace(" ", "_"));
        }

        public void createDirectory()
        {
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception ex)
                {
                   // MessageBox.Show($"Błąd podczas tworzenia folderu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void openDirectory()
        {
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                try
                {
                    //Process.Start("explorer.exe", directoryPath);
                    MessageBox.Show(directoryPath);
                }
                catch (Exception ex)
                {
                   // MessageBox.Show($"Błąd podczas otwierania folderu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void updateDirectory(string new_first_name, string new_last_name)
        {
            if (Directory.Exists(directoryPath))
            {

                string result = id.ToString();
                if (new_first_name != null) result = result + " " + new_first_name;
                if (new_last_name != null) result = result + " " + new_last_name;

                string newDirectoryPath = Path.Combine(mainDirectoryPath, result.Replace("", "_"));
            

            if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.Move(directoryPath, newDirectoryPath);
                }
            }
        }

        public List<string> loadFilesToList()
        {
            List<string> list = new List<string>();

            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    //list.Add(Path.GetFileName(file));
                    list.Add(file);
                }

                return list;
            }
            else
            {
                //MessageBox.Show("Podany katalog nie istnieje.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return list;
            }
        }

        public void openFile(string fileName)
        {
            Process.Start("explorer.exe", Path.Combine(directoryPath, fileName));

        }

        public void copyFileToFolder(string filePath)
        {
            if (File.Exists(filePath))
            {
                string fileName = Path.GetFileName(filePath);

                string destinationPath = Path.Combine(directoryPath, fileName);

                try
                {
                    File.Copy(filePath, destinationPath);
                    //Console.WriteLine($"Plik {fileName} został skopiowany do folderu {destinationFolder}.");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Wystąpił błąd podczas kopiowania pliku: {ex.Message}");
                }
            }
            else
            {
                //Console.WriteLine($"Plik o ścieżce {filePath} nie istnieje.");
            }
        }
    }
}
