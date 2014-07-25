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
using System.Xml.Serialization;
using System.ComponentModel;

namespace TheCat.Infrastructure.VirtualFileSystem.ExtendedInfo
{
    [XmlRoot(ElementName = "ext-item")]
    public sealed class ExtendedInfoItem
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "is-system")]
        [DefaultValue(false)]
        public bool IsSystem { get; set; }

        [XmlAttribute(AttributeName = "is-hidden")]
        [DefaultValue(false)]
        public bool IsHidden { get; set; }
    }
}
