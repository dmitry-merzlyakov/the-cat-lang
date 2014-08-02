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
using Cat;
using TheCat.WindowsPhone.Concrete;
using TheCat.Infrastructure.Sessions;

namespace TheCat.WindowsPhone
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private Session Session { get; set; }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Output.SetCallBack(s =>
            {
                this.textBlock1.Text += s;
                this.textBlock1.InvalidateArrange();
            });

            CatEnvironment.gsDataFolder = String.Empty;
            CatEnvironment.FileSystem = new IsolatedStorageFileSystem();
            CatEnvironment.GraphicConsole = new WindowConsole(this.canvas1);

            //new PresetManager(CatEnvironment.gsDataFolder).EnsureInitialized();

            SessionDefinition sessionDefinition = new SessionDefinition();
            sessionDefinition.InitCommands.Add("\"demo-graphics.cat\" #load");
            Session = new Session(sessionDefinition);


            this.canvas1.RenderTransform = new TranslateTransform()
            {
                X = this.canvas1.Width / 2,
                Y = this.canvas1.Height / 2
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Session.ProcessInputLine(this.textBox1.Text);

            // TODO
            //WindowGDI.CurrentConsole.DrawTurtle();
            this.canvas1.InvalidateArrange();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            textBlock1.Text = "";
            canvas1.Children.Clear();
            CatEnvironment.GraphicConsole.NullifyWindow();
        }
    }
}