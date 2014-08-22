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
using TheCat.Infrastructure.Concrete;
using TheCat.Infrastructure.VirtualFileSystem;
using System.Linq;

namespace TheCat.Infrastructure.Sessions
{
    public class SessionDefinitionRepository : BaseXmlRepository<SessionDefinition, string>, ISessionDefinitionRepository
    {
        public SessionDefinitionRepository(IExtendedFileSystemProvider provider)
            : base(provider, SessionDefinitionsFileName, sd => sd.SessionDefinitionID)
        { }

        public override IQueryable<SessionDefinition> GetAll()
        {
            return base.GetAll().Where(sd => sd.SessionDefinitionID != SessionDefinitionDefaultID).AsQueryable();
        }

        public SessionDefinition GetDefaultSessionDefinition()
        {
            return base.GetAll().FirstOrDefault(sd => sd.SessionDefinitionID == SessionDefinitionDefaultID) ?? CreateDefaultSessionDefinition();
        }

        public override SessionDefinition Get(string id)
        {
            return id == SessionDefinitionDefaultID ? GetDefaultSessionDefinition() : base.Get(id);
        }


        public void SaveDefaultSessionDefinition(SessionDefinition sessionDefinition)
        {
            if (sessionDefinition == null)
                throw new ArgumentNullException("sessionDefinition");

            sessionDefinition.SessionDefinitionID = SessionDefinitionDefaultID;

            this.Update(sessionDefinition);
        }

        private SessionDefinition CreateDefaultSessionDefinition()
        {
            SessionDefinition sessionDefinition = new SessionDefinition()
            {
                SessionDefinitionID = SessionDefinitionDefaultID,
                Name = "Default Session"
            };
            sessionDefinition.InitModules.Add("everything.cat");
            return sessionDefinition;
        }

        public const string SessionDefinitionsFileName = "SessionDefinitions.xml";
        public const string SessionDefinitionDefaultID = "def@id";
    }
}
