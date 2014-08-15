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
using Cat.Abstract;

namespace Cat
{
    public class CatEnvironment
    {
        public static string gsDataFolder { get; set; }

        public static IFileSystemProvider FileSystem { get; set; }
        public static IGraphicConsole GraphicConsole { get; set; }

        public static char ReadChar()
        {
            throw new NotImplementedException();
        }

        public static void CancelExecution()
        {
            IsCancellationPending = true;
        }

        public static void CheckCancellationPending()
        {
            if (IsCancellationPending)
            {
                IsCancellationPending = false;
                throw new Exception("Thread is aborted");
            }
        }

        private static bool IsCancellationPending { get; set; }
    }
}
