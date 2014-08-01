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
using TheCat.Infrastructure.Events;
using TheCat.Infrastructure.Concrete;

namespace TheCat.Infrastructure.Sessions.Views
{
    public class SessionDefinitionsViewModel : BaseListViewModel<SessionDefinition>
    {
        public SessionDefinitionsViewModel(ISessionDefinitionRepository repository)
        {
            Repository = repository;
            RefreshItems();

            CreateSession = new Command(() => Locator.Get<INavigationManager>().Navigate(StringKeys.CreateSession));
            EditSession = new Command((d) => Locator.Get<INavigationManager>().Navigate(StringKeys.EditSession, CompositeParams.Create(StringKeys.SessionName, ((SessionDefinition)d).SessionDefinitionID)));
            DeleteSession = new Command((d) => DeleteSessionDefinition((SessionDefinition)d));
            RunSession = new Command(() => MessageBox.Show("Coming soon - run session"));

            Locator.Get<EventManager>().RegisterSubscription<RepositoryItemChangedEvent<SessionDefinition>>(SessionDefinitionChangedHandler);
        }

        public void SessionDefinitionChangedHandler(RepositoryItemChangedEvent<SessionDefinition> evnt)
        {
            RefreshItems();
            SelectedItem = Items.FirstOrDefault(d => d.SessionDefinitionID == evnt.Subject.SessionDefinitionID);
        }

        public ICommand CreateSession { get; private set; }
        public ICommand EditSession { get; private set; }
        public ICommand DeleteSession { get; private set; }
        public ICommand RunSession { get; private set; }

        protected override IEnumerable<SessionDefinition> GetItemsFromRepository()
        {
            return Repository.GetAll();
        }

        private ISessionDefinitionRepository Repository
        {
            get { return _Repository; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _Repository = value;
            }
        }

        private void DeleteSessionDefinition(SessionDefinition sessionDefinition)
        {
            if (sessionDefinition == null)
                throw new ArgumentNullException("sessionDefinition");

            string text = String.Format("Do you really want to delete the session definition '{0}'?", sessionDefinition.Name);

            if (MessageBox.Show(text, "Delete Session Definition", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    Repository.Delete(sessionDefinition);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private ISessionDefinitionRepository _Repository;
    }
}
