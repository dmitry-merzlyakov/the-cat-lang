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

namespace TheCat.Infrastructure.VirtualFileSystem
{
    public static class Extensions
    {
        public static string GenerateUniqueName(this IVirtualFileSystemRepository repository, string currentFolderName, string template)
        {
            IEnumerable<int> indexes = repository.GetFolderContent(currentFolderName).
                Where(fs => fs.Name.StartsWith(template, StringComparison.CurrentCultureIgnoreCase)).
                Select(fs => fs.Name.Substring(template.Length)).
                Select(s => { int index; Int32.TryParse(s, out index); return index; });
            int maxIndex = indexes.Any() ? indexes.Max() : 0;
            return template + (maxIndex + 1);
        }

        public static void ShowFileSystemError(this FileSystemResult fileSystemResult)
        {
            if (fileSystemResult.ErrorCode != FileSystemErrorCode.OK)
                MessageBox.Show(fileSystemResult.ErrorMessage);
        }

    }
}
