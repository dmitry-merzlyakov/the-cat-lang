using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TheCat.Infrastructure.VirtualFileSystem;
using System.Reflection;
using System.IO;

namespace TheCat.Infrastructure.EnvironmentManagement
{
    public class EnvironmentManager : IEnvironmentManager
    {
        public EnvironmentManager(IExtendedFileSystemProvider provider, string rootFolderName)
        {
            Provider = provider;

            if (String.IsNullOrWhiteSpace(rootFolderName))
                rootFolderName = @"\";

            RootFolderName = rootFolderName;
        }

        public string RootFolderName
        {
            get { return _RootFolderName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _RootFolderName = value;
            }
        }

        public bool IsEnvironmentInitialized
        {
            get { return Provider.GetRegistryValue("IsEnvironmentInitialized") != null; }
            private set { Provider.SetRegistryValue("IsEnvironmentInitialized", true); }
        }

        public void InitializeEnvironment()
        {
            // Create folder
            if (!Provider.DirectoryExists(RootFolderName))
                Provider.CreateDirectory(RootFolderName);

            // Copy files
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourcePath = typeof(IEnvironmentManager).Namespace + ".CoreCatLib.";

            foreach (string resourceName in assembly.GetManifestResourceNames().Where(s => s.StartsWith(resourcePath)))
            {
                using (Stream inputStream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader inputReader = new StreamReader(inputStream))
                    {
                        string fileName = resourceName.Substring(resourcePath.Length);
                        using (StreamWriter outputWriter = Provider.GetStreamWriter(System.IO.Path.Combine(RootFolderName, fileName)))
                        {
                            outputWriter.Write(inputReader.ReadToEnd());
                        }
                    }
                }
            }

            // Add marker
            IsEnvironmentInitialized = true;
        }

        private IExtendedFileSystemProvider Provider
        {
            get { return _Provider; }
            set
            {
                if (value == null)
                    throw new ArgumentException("value");

                _Provider = value;
            }
        }

        private IExtendedFileSystemProvider _Provider;
        private string _RootFolderName;
    }
}
