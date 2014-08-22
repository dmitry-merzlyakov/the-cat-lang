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
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TheCat.Infrastructure.Sessions
{
    [XmlRoot(ElementName = "session-def")]
    public class SessionDefinition
    {
        public SessionDefinition()
        {
            ShowWelcome = true;
            InitModules = new List<string>();
            InitCommands = new List<string>();
            OutputTimeElapsed = true;
            OutputStack = true;
        }

        [XmlAttribute(AttributeName = "id")]
        public string SessionDefinitionID { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "show-welcome")]
        [DefaultValue(true)]
        public bool ShowWelcome { get; set; }

        public List<string> InitModules { get; set; }

        public List<string> InitCommands { get; set; }

        [XmlAttribute(AttributeName = "output-time-elapsed")]
        [DefaultValue(true)]
        public bool OutputTimeElapsed { get; set; }

        [XmlAttribute(AttributeName = "output-stack")]
        [DefaultValue(true)]
        public bool OutputStack { get; set; }
    }
}
