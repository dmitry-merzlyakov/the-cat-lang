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
using TheCat.Infrastructure.VirtualFileSystem;

namespace TheCat.Infrastructure.Concrete
{
    public class BaseXmlWriter<T>
    {
        public void Write(IExtendedFileSystemProvider provider, List<T> collection, string fileName)
        {
            string bakFileName = fileName + ".bak";

            if (provider.FileExists(bakFileName))
                provider.DeleteFile(bakFileName);

            if (provider.FileExists(fileName))
                provider.MoveFile(fileName, bakFileName);

            using (Stream stream = provider.CreateStream(fileName))
                XmlSerializer.Serialize(stream, collection);
        }

        private readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(List<T>));
    }
}
