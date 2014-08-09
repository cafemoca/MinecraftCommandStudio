using System;
using System.Diagnostics;
using System.IO;

namespace Cafemoca.McSlimUtils.Models
{
    public static class FileManager
    {
        public static string LoadTextFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }
            try
            {
                File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return string.Empty;
        }

        public static void SaveTextFile(string filePath, string text)
        {
            try
            {
                File.WriteAllText(filePath, text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
