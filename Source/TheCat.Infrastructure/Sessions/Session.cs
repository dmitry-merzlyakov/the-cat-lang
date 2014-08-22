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
using Cat;

namespace TheCat.Infrastructure.Sessions
{
    public class Session
    {
        public Session(SessionDefinition sessionDefinition)
        {
            SessionDefinition = sessionDefinition;
            Executor = new Executor();
            StartSession();
        }

        public SessionDefinition SessionDefinition 
        {
            get { return _SessionDefinition; }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _SessionDefinition = value;
            }
        }

        public Executor Executor { get; private set; }

        public void ProcessInputLine(string inputLine)
        {
            Output.LogLine(inputLine);

            if (!String.IsNullOrWhiteSpace(inputLine))
            {
                DateTime begin = DateTime.Now;
                try
                {
                    Executor.Execute(inputLine + '\n');
                    TimeSpan elapsed = DateTime.Now - begin;
                    if (SessionDefinition.OutputTimeElapsed)
                        Output.WriteLine("Time elapsed in msec " + elapsed.TotalMilliseconds.ToString("F"));
                    // Politely ask graphics window to redraw if needed
                }
                catch (Exception e)
                {
                    Output.WriteLine("exception occurred: " + e.Message);
                }
                if (SessionDefinition.OutputStack)
                    Executor.OutputStack();
            }
        }

        private void StartSession()
        {
            ShowWelcome();
            LoadInitModules();
        }

        private void ShowWelcome()
        {
            if (SessionDefinition.ShowWelcome)
            {
                Output.WriteLine("Welcome to the Cat programming language =^,,^=");
                Output.WriteLine("version " + Config.gsVersion);
                Output.WriteLine("by Christopher Diggins");
                Output.WriteLine("licensed under the MIT License 1.0");
                Output.WriteLine("http://www.cat-language.com");
                Output.WriteLine("");
                Output.WriteLine("for help, type in #help followed by the enter key");
                Output.WriteLine("to exit, type in #exit followed by the enter key");
                Output.WriteLine("");
            }
        }

        private void LoadInitModules()
        {
            foreach (string s in SessionDefinition.InitModules)
                Executor.LoadModule(s);

            foreach (string s in SessionDefinition.InitCommands)
                ProcessInputLine(s);
        }

        private SessionDefinition _SessionDefinition;
    }
}
