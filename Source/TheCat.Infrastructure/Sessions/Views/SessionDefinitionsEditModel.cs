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
using TheCat.Infrastructure.Validation;
using System.Text;

namespace TheCat.Infrastructure.Sessions.Views
{
    public class SessionDefinitionsEditModel : BaseEditViewModel
    {
        public SessionDefinitionsEditModel(ISessionDefinitionRepository repository, string sessionDefinitionID = null)
        {
            Repository = repository;
            SessionDefinition = String.IsNullOrWhiteSpace(sessionDefinitionID) ? Repository.GetDefaultSessionDefinition() : Repository.Get(sessionDefinitionID);
            ConvertToModel();
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        public bool ShowWelcome
        {
            get { return _ShowWelcome; }
            set
            {
                _ShowWelcome = value;
                OnPropertyChanged("ShowWelcome");
                WelcomeStatusText = ShowWelcome ? "Welcome is shown" : "Welcome is not shown";
            }
        }

        public string InitModules
        {
            get { return _InitModules; }
            set
            {
                _InitModules = value;
                OnPropertyChanged("InitModules");
            }
        }

        public string InitCommands
        {
            get { return _InitCommands; }
            set
            {
                _InitCommands = value;
                OnPropertyChanged("InitCommands");
            }
        }
        
        public bool OutputTimeElapsed
        {
            get { return _OutputTimeElapsed; }
            set
            {
                _OutputTimeElapsed = value;
                OnPropertyChanged("OutputTimeElapsed");
                OutputTimeElapsedStatusText = OutputTimeElapsed ? "Time is shown" : "Time is not shown";
            }
        }

        public bool OutputStack
        {
            get { return _OutputStack; }
            set
            {
                _OutputStack = value;
                OnPropertyChanged("OutputStack");
                OutputStackStatusText = OutputStack ? "Stack is shown" : "Stack is not shown";
            }
        }

        public string WelcomeStatusText
        {
            get { return _WelcomeStatusText; }
            set
            {
                _WelcomeStatusText = value;
                OnPropertyChanged("WelcomeStatusText");
            }
        }

        public string OutputTimeElapsedStatusText
        {
            get { return _OutputTimeElapsedStatusText; }
            set
            {
                _OutputTimeElapsedStatusText = value;
                OnPropertyChanged("OutputTimeElapsedStatusText");
            }
        }

        public string OutputStackStatusText
        {
            get { return _OutputStackStatusText; }
            set
            {
                _OutputStackStatusText = value;
                OnPropertyChanged("OutputStackStatusText");
            }
        }

        public bool IsNotDefaultSessionDefinition
        {
            get { return SessionDefinition.SessionDefinitionID != SessionDefinitionRepository.SessionDefinitionDefaultID; }
        }

        protected ISessionDefinitionRepository Repository
        {
            get { return _Repository; }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _Repository = value;
            }
        }

        protected SessionDefinition SessionDefinition { get; set; }

        protected override void InitializeValidator()
        {
            base.InitializeValidator();

            Validator<SessionDefinitionsEditModel> validator = new Validator<SessionDefinitionsEditModel>();
            validator.AddIsNotEmptyRule(context => context.Name, "Name is required");
            AddValidator(validator);
        }

        protected override bool CommitChanges(ref string messages)
        {
            ConvertToEntity();

            if (String.IsNullOrWhiteSpace(SessionDefinition.SessionDefinitionID))
                SessionDefinition.SessionDefinitionID = Guid.NewGuid().ToString();

            try
            {
                Repository.Update(SessionDefinition);
            }
            catch (Exception ex)
            {
                messages = ex.Message;
                return false;
            }

            return true;
        }

        private void ConvertToModel()
        {
            Name = SessionDefinition.Name;
            Description = SessionDefinition.Description;
            ShowWelcome = SessionDefinition.ShowWelcome;
            InitModules = SessionDefinition.InitModules.CreateStringFromList();
            InitCommands = SessionDefinition.InitCommands.CreateStringFromList();
            OutputTimeElapsed = SessionDefinition.OutputTimeElapsed;
            OutputStack = SessionDefinition.OutputStack;
        }

        private void ConvertToEntity()
        {
            SessionDefinition.Name = Name;
            SessionDefinition.Description = Description;
            SessionDefinition.ShowWelcome = ShowWelcome;
            SessionDefinition.InitModules = InitModules.CreateListFromString();
            SessionDefinition.InitCommands = InitCommands.CreateListFromString();
            SessionDefinition.OutputTimeElapsed = OutputTimeElapsed;
            SessionDefinition.OutputStack = OutputStack;
        }


        private ISessionDefinitionRepository _Repository;

        private string _Name;
        private string _Description;
        private bool _ShowWelcome;
        private string _InitModules;
        private string _InitCommands;
        private bool _OutputTimeElapsed;
        private bool _OutputStack;

        private string _WelcomeStatusText;
        private string _OutputTimeElapsedStatusText;
        private string _OutputStackStatusText;
    }
}
