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
using TheCat.Infrastructure.VirtualFileSystem;
using TheCat.Infrastructure.VirtualFileSystem.Views;
using TheCat.Infrastructure.Events;
using TheCat.Infrastructure.Sessions.Views;
using TheCat.Infrastructure.Sessions;

namespace TheCat.Infrastructure
{
    public static class LocatorConfigurer
    {
        public static void Configure()
        {
            Locator.AddService<IVirtualFileSystemRepository>((p) => new VirtualFileSystemRepository(Locator.Get<IExtendedFileSystemProvider>()));
            Locator.AddService<ISessionDefinitionRepository>((p) => new SessionDefinitionRepository(Locator.Get<IExtendedFileSystemProvider>()));
            Locator.AddService<EventManager>((p) => new EventManager());

            Locator.AddService<BaseFileDescriptorViewModel>(StringKeys.CreateFile, (p) => new CreateFileViewModel(Locator.Get<IVirtualFileSystemRepository>(), p[StringKeys.CurrentFolderName]), true);
            Locator.AddService<BaseFileDescriptorViewModel>(StringKeys.CreateFolder, (p) => new CreateFolderViewModel(Locator.Get<IVirtualFileSystemRepository>(), p[StringKeys.CurrentFolderName]), true);
            Locator.AddService<EditFileViewModel>(StringKeys.EditFile, (p) => new EditFileViewModel(Locator.Get<IVirtualFileSystemRepository>(), p[StringKeys.FileName]), true);
            Locator.AddService<BaseFileDescriptorViewModel>(StringKeys.ModifyFile, (p) => new ChangeFileViewModel(Locator.Get<IVirtualFileSystemRepository>(), p[StringKeys.FileName]), true);
            Locator.AddService<BaseFileDescriptorViewModel>(StringKeys.ModifyFolder, (p) => new ChangeFolderViewModel(Locator.Get<IVirtualFileSystemRepository>(), p[StringKeys.FolderName]), true);

            // TODO - weak references
            Locator.AddService<FilesViewModel>(StringKeys.ViewFiles, (p) => new FilesViewModel(Locator.Get<IVirtualFileSystemRepository>(), p == null ? null : p[StringKeys.CurrentFolderName]), false);

            Locator.AddService<SessionDefinitionsViewModel>(StringKeys.ViewSessions, (p) => new SessionDefinitionsViewModel(Locator.Get<ISessionDefinitionRepository>()), false);
            Locator.AddService<SessionDefinitionsEditModel>(StringKeys.CreateSession, (p) => new SessionDefinitionsEditModel(Locator.Get<ISessionDefinitionRepository>()), true);
            Locator.AddService<SessionDefinitionsEditModel>(StringKeys.EditSession, (p) => new SessionDefinitionsEditModel(Locator.Get<ISessionDefinitionRepository>(), p[StringKeys.SessionName]), false);
        }
    }
}
