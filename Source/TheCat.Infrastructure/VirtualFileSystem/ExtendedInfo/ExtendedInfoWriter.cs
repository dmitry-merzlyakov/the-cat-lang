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
    public class ExtendedInfoWriter
    {
        public ExtendedInfoWriter(IExtendedFileSystemProvider provider)
        {
            Provider = provider;
        }

        public void Write(ExtendedInfoCollection collection, string folderName, string fileName)
        {
            string fullFileName = System.IO.Path.Combine(folderName, fileName);
            string bakFileName = fullFileName + ".bak";

            if (Provider.FileExists(bakFileName))
                Provider.DeleteFile(bakFileName);

            if (Provider.FileExists(fullFileName))
                Provider.MoveFile(fullFileName, bakFileName);

            using (Stream stream = Provider.CreateStream(fullFileName))
                XmlSerializer.Serialize(stream, collection);
        }

        private readonly IExtendedFileSystemProvider Provider;
        private readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(ExtendedInfoCollection));
    }
}
