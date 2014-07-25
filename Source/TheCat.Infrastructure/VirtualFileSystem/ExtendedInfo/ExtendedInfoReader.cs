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
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace TheCat.Infrastructure.VirtualFileSystem.ExtendedInfo
{
    public sealed class ExtendedInfoReader
    {
        public ExtendedInfoReader(IExtendedFileSystemProvider provider)
        {
            Provider = provider;
        }

        public ExtendedInfoCollection Read(string folderName, string fileName)
        {
            string fullFileName = System.IO.Path.Combine(folderName, fileName);

            if (Provider.FileExists(fullFileName))
            {
                try
                {
                    using (Stream stream = Provider.OpenStream(fullFileName))
                        return XmlSerializer.Deserialize(stream) as ExtendedInfoCollection;
                }
                catch
                { /* Ignore deserialization errors.. TODO - log errors! */ }
            }

            return null;
        }

        private readonly IExtendedFileSystemProvider Provider;
        private readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(ExtendedInfoCollection));
    }
}
