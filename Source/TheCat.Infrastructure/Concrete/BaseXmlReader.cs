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
    public class BaseXmlReader<T>
    {
        public List<T> Read(IExtendedFileSystemProvider provider, string fileName)
        {
            if (provider.FileExists(fileName))
            {
                try
                {
                    using (Stream stream = provider.OpenStream(fileName))
                        return XmlSerializer.Deserialize(stream) as List<T>;
                }
                catch //(Exception ex)
                {
                    //ex = null;
                    /* Ignore deserialization errors.. TODO - log errors! */ 
                }
            }

            return null;
        }

        private readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(List<T>));
    }
}
