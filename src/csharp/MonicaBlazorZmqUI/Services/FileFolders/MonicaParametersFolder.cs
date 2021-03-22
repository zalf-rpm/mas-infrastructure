using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MonicaBlazorZmqUI.Services.FileFolders
{
    public class MonicaParametersFolder
    {

        public void GetSubDirectories()

        {
            string root = @"Data/monica-parameters/";
            // Get all subdirectories
            string[] subdirectoryEntries = Directory.GetDirectories(root);

            // Loop through them to see if they have any other subdirectories
            foreach (string subdirectory in subdirectoryEntries)
                LoadSubDirs(subdirectory);
        }


        private void LoadSubDirs(string dir)
        {
            Console.WriteLine(dir);
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            foreach (string subdirectory in subdirectoryEntries)
            {
                LoadSubDirs(subdirectory);
            }

        }

    }
    
}
