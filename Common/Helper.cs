using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Drawing;
using System.Web;
using System.Windows.Forms;

namespace Common
{
    public class Helper
    {
        #region Windows
        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        /// <summary>
        /// Gets app setting from .config file.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when parameter is null or empty.</exception>
        public static string GetAppSetting(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Value must be non-empty string.", "key");

            string result;
            try
            {
                result = ConfigurationSettings.AppSettings[key];
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Opens file with associated external application.
        /// </summary>
        /// <param name="filePath">Full path with filename of file to open.</param>
        /// <exception cref="ArgumentException">Thrown when parameter is null or empty.</exception>
        public static void OpenFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value must be non-empty string.", "filePath");
      
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = filePath;
            process.Start();
        }

        public static bool IsWindowOpen(string windowTitle)
        {
            if (string.IsNullOrEmpty(windowTitle)) throw new ArgumentException("Value must be non-empty string.", "windowTitle");

            IntPtr handle = FindWindowByCaption(IntPtr.Zero, windowTitle);
            return !handle.Equals(IntPtr.Zero);
        }

        public static bool CloseWindow(string windowTitle)
        {
            if (string.IsNullOrEmpty(windowTitle)) throw new ArgumentException("Value must be non-empty string.", "windowTitle");


            IntPtr handle = FindWindowByCaption(IntPtr.Zero, windowTitle);
            if (!handle.Equals(IntPtr.Zero))
            {
                SendMessage(handle.ToInt32(), WM_SYSCOMMAND, SC_CLOSE, 0);
                return true;
            }
            return false;
        }
        #endregion Windows

        #region streams
        /// <summary>
        /// Writes stream content to console screen. If stream is null or empty writes 'NULL' or 'EMPTY'.
        /// </summary>
        /// <param name="theStream"></param>
        static void DumpStream(Stream theStream)
        {
            if (theStream == null)
            {
                Console.WriteLine("NULL");
            }
            else
            {
                if (theStream.Length == 0)
                {
                    Console.WriteLine("EMPTY");
                }
                else
                {
                    // Move the stream's position to the beginning
                    theStream.Position = 0;
                    // Go through entire stream and show the contents
                    while (theStream.Position != theStream.Length)
                    {
                        Console.WriteLine("{0:x2}", theStream.ReadByte());
                    }
                }
            }
        }

        /// <summary>
        /// Appends bytes to stream.
        /// </summary>
        /// <param name="theStream"></param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException">Thrown when one of parameters is null.</exception>
        static void AppendToStream(Stream theStream, byte[] data)
        {
            if (theStream == null) throw new ArgumentNullException("theStream");
            if (data == null) throw new ArgumentNullException("data");

            // Move the Position to the end
            theStream.Position = theStream.Length;
            // Append some bytes
            theStream.Write(data, 0, data.Length);
        }
        #endregion streams

        #region File system

        /// <summary>
        /// Tries to obtain web app folder from httpContext. If fails that means that web app is not running
        /// but form app or console. Gets exePath then. Examples: 
        /// c:\inetpub\wwwroot\myApp\,
        /// C:\Programs\myApp\
        /// </summary>
        /// <returns></returns>
        public static string GetWorkingFolder()
        {
            string workingFolder = null;
            try
            {
                workingFolder = HttpContext.Current.Request.PhysicalApplicationPath;
            }
            catch(Exception)
            {
                workingFolder = Path.GetDirectoryName(Application.ExecutablePath)+@"\";
            }

            return workingFolder;
        }

        public static bool IsFolder(string fileOrFolderPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Examples: for 'C:\aaa\bbb' returns 'C:\aaa'; for 'C:\aaa\bbb.txt' (which can be file or folder) returns 'C:\aaa' .
        /// </summary>
        /// <param name="fullCurrentFolderPath"></param>
        /// <returns></returns>
        public static string GetParentFolderPath(string fullCurrentFolderPath)
        {
            string parentFolderPath = null;

            parentFolderPath = Path.GetDirectoryName(fullCurrentFolderPath);

            //if(!string.IsNullOrEmpty(fullCurrentFolderPath))
            //{
            //    DirectoryInfo currentDirectoryInfo = new DirectoryInfo(fullCurrentFolderPath);
            //    parentFolderPath = currentDirectoryInfo.Parent.FullName;
                
            //}
            return parentFolderPath;
        }

        /// <summary>
        /// Gets list of strings where each is full path to file including filename (for example: <example>c:\dir\filename.ext</example>.
        /// </summary>
        /// <param name="folder">Full path of folder that should be searched. For example: <example>c:\dir</example>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when parameter is null or empty.</exception>
        public static List<string> GetFilePaths(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentException("Value must be non-empty string.", "folderPath");

            List<string> filePaths = new List<string>();
            string[] filePathStrings = Directory.GetFiles(folderPath);
            if (filePathStrings != null)
            {
                filePaths.AddRange(filePathStrings);
            }

            return filePaths;
        }

        /// <summary>
        /// Gets list of strings where each is full path to folder (for example FolderA) including foldername (for example: <example>c:\dir\FolderA</example>.
        /// </summary>
        /// <param name="folder">Full path of folder that should be searched. For example: <example>c:\dir</example>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when parameter is null or empty.</exception>
        public static List<string> GetFolderPaths(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentException("Value must be non-empty string.", "folderPath");

            List<string> folderPaths = new List<string>();
            string[] folderPathStrings = Directory.GetDirectories(folderPath);
            if (folderPathStrings != null)
            {
                folderPaths.AddRange(folderPathStrings);
            }

            return folderPaths;
        }

//        public static byte[] ReadBytesFromFile(string filePath)
//        {
//            StreamReader rdr = File.OpenText(@"C:\boot.ini");rdr.CurrentEncoding
//            Console.Write(rdr.Re.ReadToEnd());
//rdr.Close()
//        }


//        public static string ReadTextFromFile(string filePath)
//        {
//            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentException("Value must be non-empty string.", "filePath");
            
//            File.ReadAllText(
//        }

        /// <summary>
        /// Writes provided sequence of bytes to file specified by filepath.
        /// </summary>
        /// <param name="filePath">Full path to file. Example: <example>c:\subdir1\file1.bin</example> </param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentException">Thrown when parameter <paramref>filePath</paramref> is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when parameter <paramref>data</paramref> is null.</exception>
        public static void WriteBytesToFile(string filePath , byte[] data)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value must be non-empty string.", "filePath");
            if (data==null) throw new ArgumentNullException("data");


            FileStream fileStream = new FileStream(filePath, FileMode.CreateNew);
            BinaryWriter writer = new BinaryWriter(fileStream);

            try
            {
                fileStream.Write(data, 0, data.Length);
            }
           
            finally
            {
                try
                {
                    writer.Close();
                    fileStream.Close();
                }
                catch { }
            }

        }

        /// <summary>
        /// Reads all bytes from file specified by parameter.
        /// </summary>
        /// <param name="filePath">Full path to file from which data should be read. Example: <example>c:\subdir1\file1.bin</example></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when parameter <paramref>filePath</paramref> is null or empty.</exception>
        public static byte[] ReadBytesFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value must be non-empty string.", "filePath");

            byte[] data = null;

            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            BinaryWriter writer = new BinaryWriter(fileStream);

            try 
            {
                byte[] buffer = new byte[32768];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read = 0;
                    do
                    {
                        read = fileStream.Read(buffer, 0, buffer.Length);
                        if (read > 0) ms.Write(buffer, 0, read);
                    } while (read > 0);

                    data = ms.ToArray();
                }
            }
            finally
            {
                try
                {
                    writer.Close();
                    fileStream.Close();
                }
                catch { }
            }

            return data;
        }

        #endregion File system

        #region XML

        /// <summary>
        /// Gets element inner text.
        /// </summary>
        /// <param name="path">List of element names (including target element) that build path to target element.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when parameter <paramref>filePath</paramref> is null or empty.</exception>
        //public static string GetXMLElementValue(Stream stream,List<string,int> pathSteps)
        //{
        //    if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value must be non-empty string.", "filePath");

        //    XmlTextReader xmlTextReader = new XmlTextReader(stream);
            
        //    int i=0;
        //    while(i<pathSteps.Count && pathSteps[i]!=xmlTextReader.LocalName)
        //    {

        //        i++;
        //    }

        //    foreach(

        //    return null;
        //}

        public static object Deserialize(Type type, string fileName)
        {
            object returnValue = null;
            FileStream fileStream = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                fileStream = new FileStream(fileName, FileMode.Open);
                returnValue = xmlSerializer.Deserialize(fileStream);     
            }
            finally
            {
                try
                {
                    fileStream.Close();
                }
                catch { }
            }

            return returnValue;
        }

        public static object Deserialize(string fileName, bool useSoapFormatter)
        {
            object returnValue = null;
            FileStream fileStream = null;
            try
            {                
                fileStream = new FileStream(fileName, FileMode.Open);
                if (useSoapFormatter)
                {
                    SoapFormatter soapFormatter = new SoapFormatter();
                    returnValue = soapFormatter.Deserialize(fileStream);
                }
                else
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    returnValue = binaryFormatter.Deserialize(fileStream);
                }
            }
            finally
            {
                try
                {
                    fileStream.Close();
                }
                catch { }
            }

            return returnValue;
        }

        /// <summary>
        /// Serializes provided object to XML format to specified file.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="myObject"></param>
        /// <param name="fileName"></param>
        public static void Serialize(object myObject, string fileName)
        {
            StreamWriter streamWritter = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(myObject.GetType());
                streamWritter = new StreamWriter(fileName);
                xmlSerializer.Serialize(streamWritter, myObject);                
            }
            finally
            {
                try
                {
                    streamWritter.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// Serializes provided object to specified file. 
        /// Object serialized to binary format can be deserialized by only .NET application.
        /// Soap format consume more disk space and time comparing to binary format.
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="fileName"></param>
        /// <param name="useSoapFormatter"></param>
        public static void Serialize(object myObject, string fileName, bool useSoapFormatter)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(fileName, FileMode.Create);
                if (useSoapFormatter)
                {
                    SoapFormatter soapFormatter = new SoapFormatter();
                    soapFormatter.Serialize(fileStream, myObject);
                }
                else
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, myObject);
                }
            }
            finally
            {
                try
                {
                    fileStream.Close();
                }
                catch { }
            }
        }

        #endregion XML

        #region Graphics

        public static Size ResizeToFit(Size oldDimmensions, Size constraintDimensions, bool allowEnlarging)
        {
            Size newDimensions = new Size(0,0);

            if (allowEnlarging)
            {
                throw new NotImplementedException();
            }
            else
            {
                newDimensions = new Size(0,0);
                
                if (oldDimmensions.Width <= constraintDimensions.Width && oldDimmensions.Height <= constraintDimensions.Height)
                {
                    newDimensions.Width = oldDimmensions.Width;
                    newDimensions.Height = oldDimmensions.Height;
                }
                else
                {
                    newDimensions.Width = Convert.ToInt32(oldDimmensions.Width * constraintDimensions.Height / oldDimmensions.Height);
                    newDimensions.Height = constraintDimensions.Height;

                    if (newDimensions.Width > constraintDimensions.Width)
                    {
                        newDimensions.Width = constraintDimensions.Width;
                        newDimensions.Height = Convert.ToInt32(constraintDimensions.Width * oldDimmensions.Height / oldDimmensions.Width);
                    }
                }
            }

            return newDimensions;
        }

        #endregion Graphics

        #region Other
        #endregion Other
    }
}

