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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TheCat.Infrastructure.VirtualFileSystem.ExtendedInfo
{
    public class ExtendedInfoCollection
    {
        public ExtendedInfoCollection(string folderName) 
            : this()
        {
            FolderName = folderName;
        }

        public ExtendedInfoCollection()
        {
            Items = new List<ExtendedInfoItem>();
        }

        [XmlAttribute(AttributeName="folder")]
        public string FolderName { get; set; }
        public List<ExtendedInfoItem> Items { get; set; }

        public void Update(ExtendedInfoItem item)
        {
            Items.Remove(Get(item.Name));
            Items.Add(item);
        }

        public void Remove(string name)
        {
            Items.Remove(Get(name));
        }

        public ExtendedInfoItem Get(string name)
        {
            return Items.FirstOrDefault(i => i.Name == name);
        }
    }
}
