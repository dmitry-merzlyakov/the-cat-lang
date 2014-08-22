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
using System.Collections.ObjectModel;
using Cat.Abstract;
using System.Threading;
using System.ComponentModel;

namespace TheCat.Infrastructure.Sessions.Views
{
    public sealed class ConsoleViewModel : BaseViewModel
    {
        public ConsoleViewModel(IFileSystemProvider fileSystemProvider, IGraphicConsole graphicConsole)
        {
            Output.SetCallBack(s => { Deployment.Current.Dispatcher.BeginInvoke(() => OutputText.Add(s)); });

            // TODO  - it should be moved to more appropriate place
            CatEnvironment.gsDataFolder = String.Empty;
            CatEnvironment.FileSystem = fileSystemProvider;
            CatEnvironment.GraphicConsole = graphicConsole;

            ProcessInputLine = new Command(() => DoProcessInputLine());
            StopExecution = new Command(() => DoStopExecution()) { CanExecute = false };
            RestartSession = new Command(() => DoRestartSession());
            ClearOutput = new Command(() => DoClearOutput());

            BackgroundWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };
            BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
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

        public Session Session { get; private set; }

        public ObservableCollection<string> OutputText
        {
            get { return _OutputText; }
        }

        public bool IsRunning 
        {
            get { return _IsRunning; }
            private set
            {
                _IsRunning = value;

                ((Command)ProcessInputLine).CanExecute = !IsRunning;
                ((Command)StopExecution).CanExecute = IsRunning;
                ((Command)RestartSession).CanExecute = !IsRunning;
                ((Command)ClearOutput).CanExecute = !IsRunning;
            }
        }

        public ICommand ProcessInputLine { get; private set; }
        public ICommand StopExecution { get; private set; }
        public ICommand RestartSession { get; private set; }
        public ICommand ClearOutput { get; private set; }

        public string InputLine
        {
            get { return _InputLine; }
            set
            {
                _InputLine = value;
                OnPropertyChanged("InputLine");
            }
        }

        public ObservableCollection<string> InputLineHistory
        {
            get { return _InputLineHistory; }
        }

        public void Initialize(SessionDefinition sessionDefinition)
        {
            SessionDefinition = sessionDefinition;
            DoRestartSession();
        }

        private void DoRestartSession()
        {
            DoClearOutput();
            Run(() => Session = new Session(SessionDefinition));
        }

        private void DoProcessInputLine()
        {
            if (!InputLineHistory.Contains(InputLine))
                InputLineHistory.Add(InputLine);

            if (InputLine.IsExitCommand())
                Locator.Get<INavigationManager>().GoBack();

            Run(() => Session.ProcessInputLine(InputLine));
        }

        private void DoClearOutput()
        {
            OutputText.Clear();
            if (CatEnvironment.GraphicConsole != null)
                CatEnvironment.GraphicConsole.ClearWindow();
        }

        private void Run(Action action)
        {
            BackgroundWorker.RunWorkerAsync(action);
        }

        private void DoStopExecution()
        {
            CatEnvironment.CancelExecution();
        }

        void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Action action = (Action)e.Argument;

            lock (this)
            {
                IsRunning = true;
                try
                {
                    action();
                }
                finally
                {
                    IsRunning = false;
                }
            }
        }

        private SessionDefinition _SessionDefinition;
        private ObservableCollection<string> _OutputText = new ObservableCollection<string>();
        private ObservableCollection<string> _InputLineHistory = new ObservableCollection<string>();
        private string _InputLine = String.Empty;
        private bool _IsRunning;
        private readonly BackgroundWorker BackgroundWorker;
    }
}
