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

namespace TheCat.Infrastructure.Sessions
{
    public class SessionDefinitionRepository : BaseXmlRepository<SessionDefinition, string>, ISessionDefinitionRepository
    {
        public SessionDefinitionRepository(IExtendedFileSystemProvider provider)
            : base(provider, SessionDefinitionsFileName, sd => sd.SessionDefinitionID)
        { }

        public const string SessionDefinitionsFileName = "SessionDefinitions.xml";
    }
}
