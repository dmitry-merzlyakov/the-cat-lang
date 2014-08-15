using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TheCat.Infrastructure.Sessions.Views;
using TheCat.Infrastructure.VirtualFileSystem;
using TheCat.Infrastructure;
using TheCat.WindowsPhone.Concrete;
using TheCat.Infrastructure.Sessions;

namespace TheCat.WindowsPhone
{
    public partial class ConsolePage : PhoneApplicationPage
    {
        public ConsolePage()
        {
            InitializeComponent();

            // INavigationManager navigationManager = Locator.Get<INavigationManager>();
            // DataContext = FileDescriptorViewModel = Locator.Get<BaseFileDescriptorViewModel>(navigationManager.PopTarget(), navigationManager.PopParams());

            DataContext = ConsoleViewModel = new ConsoleViewModel(Locator.Get<IExtendedFileSystemProvider>(), 
                new WindowConsole(this.GraphicWindowCanvas) { DoEnsureVisible = DoEnsureVisible });
        }

        public void DoEnsureVisible()
        {
            MainPivot.SelectedItem = GraphicPivotItem;
        }

        public ConsoleViewModel ConsoleViewModel { get; set; }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SessionDefinition sessionDefinition = new SessionDefinition();

            ConsoleViewModel.Initialize(sessionDefinition);
        }

        // This code positions the scroll viewer to the last row if the nu,ber of items was changed. TODO - refactor
        private void TextScrollViewer_LayoutUpdated(object sender, EventArgs e)
        {
            if (ConsoleViewModel.OutputText.Count() != OutputTextCount)
            {
                TextScrollViewer.ScrollToVerticalOffset(double.MaxValue);
                OutputTextCount = ConsoleViewModel.OutputText.Count();
            }
        }
        private int OutputTextCount = int.MinValue;

        private void GraphicWindowCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // This code positions the graphic output in the center of the canvas. 
            this.GraphicWindowCanvas.RenderTransform = new TranslateTransform()
            {
                X = this.GraphicWindowCanvas.ActualWidth / 2,
                Y = this.GraphicWindowCanvas.ActualHeight / 2
            };
        }

    }
}