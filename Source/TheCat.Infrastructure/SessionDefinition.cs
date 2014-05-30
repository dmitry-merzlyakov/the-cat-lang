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

namespace Cat.Infrastructure
{
    public class SessionDefinition
    {
        public SessionDefinition()
        {
            ShowWelcome = true;
            InitModule = "everything.cat";
            InitCommands = new List<string>();
            OutputTimeElapsed = true;
            OutputStack = true;
        }

        public bool ShowWelcome { get; set; }
        public string InitModule { get; set; }
        public IList<string> InitCommands { get; set; }
        public bool OutputTimeElapsed { get; set; }
        public bool OutputStack { get; set; }
    }
}
