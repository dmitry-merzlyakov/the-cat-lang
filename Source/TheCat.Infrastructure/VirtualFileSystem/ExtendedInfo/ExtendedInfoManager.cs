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

namespace TheCat.Infrastructure.VirtualFileSystem.ExtendedInfo
{
    public sealed class ExtendedInfoManager
    {
        public static ExtendedInfoManager Current
        {
            get { return Lazy<ExtendedInfoManager>.Value; }
        }

        public ExtendedInfoManager()
        {
            Provider = Locator.Get<IExtendedFileSystemProvider>();
            Reader = new ExtendedInfoReader(Provider);
            Writer = new ExtendedInfoWriter(Provider);
        }

        public IExtendedFileSystemProvider Provider
        {
            get { return _Provider; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _Provider = value;
            }
        }

        public ExtendedInfoItem Get(string fullName)
        {
            string folderName = fullName.GetFolderName();
            string shortName = fullName.GetFileName();
            return GetCollection(folderName).Get(shortName);
        }

        public void SaveOrUpdate(string folderName, ExtendedInfoItem item)
        {
            ExtendedInfoCollection collection = GetCollection(folderName);
            collection.Update(item);
            Writer.Write(collection, folderName, FileName);
        }

        public void Delete(string fullName)
        {
            string folderName = fullName.GetFolderName();
            string shortName = fullName.GetFileName();
            ExtendedInfoCollection collection = GetCollection(folderName);
            collection.Remove(shortName);
            Writer.Write(collection, folderName, FileName);
        }

        public void ClearCache()
        {
            Cache.Clear();
        }

        private ExtendedInfoCollection GetCollection(string folderName)
        {
            if (String.IsNullOrWhiteSpace(folderName))
                throw new ArgumentNullException("folderName");

            ExtendedInfoCollection collection = Cache.FirstOrDefault(c => c.FolderName == folderName);
            if (collection == null)
            {
                collection = Reader.Read(folderName, FileName) ?? new ExtendedInfoCollection(folderName);
                Cache.Add(collection);
            }

            return collection;
        }

        private const string FileName = "Descript.ion";
        private IExtendedFileSystemProvider _Provider;
        private readonly ExtendedInfoReader Reader;
        private readonly ExtendedInfoWriter Writer;
        private readonly IList<ExtendedInfoCollection> Cache = new List<ExtendedInfoCollection>();
    }
}
