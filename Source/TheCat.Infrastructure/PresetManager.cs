using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using System.Linq;
using System.IO;

namespace Cat.Infrastructure
{
    public class PresetManager
    {
        public PresetManager(string codeFolder)
        {
            CodeFolder = codeFolder;
        }

        public string CodeFolder
        {
            get { return _CodeFolder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _CodeFolder = value;
            }            
        }

        public string MarkerFileName
        {
            get { return CodeFolder + @"\Preset.Done"; }
        }

        public void EnsureInitialized()
        {
            if (!IsInitialized())
                AddPresetFiles();
        }

        public bool IsInitialized()
        {
            return CatEnvironment.FileSystem.DirectoryExists(CodeFolder) && CatEnvironment.FileSystem.FileExists(MarkerFileName);
        }

        public void AddPresetFiles()
        {
            // Create folder
            if (!CatEnvironment.FileSystem.DirectoryExists(CodeFolder))
                CatEnvironment.FileSystem.CreateDirectory(CodeFolder);

            // Copy files
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourcePath = "The" + typeof(PresetManager).Namespace + ".CoreCatLib.";

            foreach (string resourceName in assembly.GetManifestResourceNames().Where(s => s.StartsWith(resourcePath)))
            {
                using (Stream inputStream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader inputReader = new StreamReader(inputStream))
                    {
                        string fileName = resourceName.Substring(resourcePath.Length);
                        using (StreamWriter outputWriter = CatEnvironment.FileSystem.GetStreamWriter(CodeFolder + "\\" + fileName))
                        {
                            outputWriter.Write(inputReader.ReadToEnd());
                        }
                    }
                }
            }

            // Add marker
            using (StreamWriter sw = CatEnvironment.FileSystem.GetStreamWriter(MarkerFileName))
                sw.WriteLine("PresetManager : preset is done");
        }

        private string _CodeFolder;
    }
}
