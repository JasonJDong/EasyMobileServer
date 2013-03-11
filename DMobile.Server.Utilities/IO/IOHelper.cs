using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DMobile.Server.Utilities.IO
{
    public class IOHelper
    {
        /// <summary>
        /// Gets the files in directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static FileInfo[] GetFilesInDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("Directory supplied is either null or empty");

            var dirInfo = new DirectoryInfo(directory);

            if (!dirInfo.Exists)
                throw new ArgumentException("Directory '" + directory + "' does not exist.");

            return dirInfo.GetFiles();
        }

        public static string[] GetFiles(string sourceFolder, string filters)
        {
            return GetFiles(sourceFolder, filters, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetFiles(string sourceFolder, string filters, SearchOption searchOption)
        {
            return
                filters.Split('|').SelectMany(filter => Directory.GetFiles(sourceFolder, filter, searchOption)).ToArray();
        }

        public static void WriteSimpleLog(string filePath, string msg)
        {
            if (!File.Exists(filePath))
            {
                FileStream fs = File.Create(filePath);
                fs.Close();
            }
            using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                using (TextWriter tw = new StreamWriter(fs))
                {
                    tw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture) + Environment.NewLine + msg);
                }
            }
        }

        /// <summary>
        /// 返回指定程序集所在文件位置
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyDirectoryPath(Assembly assembly)
        {
            if (assembly != null)
            {
                string codeBase = assembly.CodeBase;
                return codeBase.Replace("file:///", "");
            }
            return string.Empty;
        }
    }
}