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
using Cat;
using Cat.Abstract;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace TheCat.WindowsPhone.Concrete
{
    public class WindowConsole : IGraphicConsole
    {
        public WindowConsole(Canvas canvas)
        {
            Canvas = canvas;
        }

        private readonly object SyncRoot = new object();
        private readonly IList<GraphicCommand> Commands = new List<GraphicCommand>();
        public Canvas Canvas { get; private set; }

        public void EnsureVisible()
        {
            // TODO
        }

        public void AddCommand(GraphicCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            lock (SyncRoot)
                Commands.Add(cmd);

            // draw command
            cmd.Invoke(this, this.GetType());
        }

        /* ****************************** */

        public void Render(GraphicCommand cmd)
        {
            EnsureVisible();
            AddCommand(cmd);
        }

        private double CurrentRotate = 0;
        public void Rotate(double x)
        {
            // TODO ???
            //Canvas.RenderTransform = new RotateTransform()
            //{
            //    Angle = x
            //};
            ////////GetCompositeTransform().Rotation = x;
            CurrentRotate += x;
            if (CurrentRotate > 360)
                CurrentRotate -= 360;
            if (CurrentRotate < -360)
                CurrentRotate += 360;
        }

 //       private int testcounter = 0;
        public void Line(double x1, double y1, double x2, double y2)
        {
//            testcounter++;
//            if (testcounter >= 80)
//                return;

            // Special limitation - compatibility
            if (x1 < 0 || x2 < 0 || y1 < 0 || y2 < 0)
                return;

            // TODO
	        Line line = new Line() { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.RenderTransform = new CompositeTransform()
            {
                Rotation = CurrentRotate,
                TranslateX = CurrentSlideX,
                TranslateY = CurrentSlideY,
            };
            Canvas.Children.Add(line);
        }

        private double CurrentSlideX = 0;
        private double CurrentSlideY = 0;
        public void Slide(double x, double y)
        {
            // TODO
            //Canvas.RenderTransform = new TranslateTransform() { X = x, Y = y };     
            /////////GetCompositeTransform().TranslateX = x;
            /////////GetCompositeTransform().TranslateY = y;

            double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double f = ((CurrentRotate * Math.PI) / 180) + (Math.PI / 2);

            CurrentSlideX += Math.Round(r * Math.Cos(f));
            CurrentSlideY += Math.Round(r * Math.Sin(f));
        }

        /*
        private CompositeTransform GetCompositeTransform()
        {
            if (Canvas.RenderTransform == null || Canvas.RenderTransform.GetType() != typeof(CompositeTransform))
                Canvas.RenderTransform = new CompositeTransform();

            return (CompositeTransform)Canvas.RenderTransform;
        }
         */


        public void Scale(double x, double y)
        {
            throw new NotImplementedException();
        }

        public void SetPenUp(bool b)
        {
            throw new NotImplementedException();
        }

        public bool IsPenUp()
        {
            throw new NotImplementedException();
        }

        public void Ellipse(double x, double y, double w, double h)
        {
            throw new NotImplementedException();
        }

        public void DrawTurtleFoot(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SetTurtleVisibility(bool b)
        {
            throw new NotImplementedException();
        }

        public bool GetTurtleVisibility()
        {
            throw new NotImplementedException();
        }

        private Image TurtleImage = null;
        public void DrawTurtle()
        {
            if (TurtleImage == null)
            {
                TurtleImage = new Image();
                TurtleImage.Source = new BitmapImage(new Uri("/TheCatWF;component/Images/turtle.png", UriKind.Relative));
                Canvas.Children.Add(TurtleImage);
            }
            TurtleImage.RenderTransform = new CompositeTransform()
            {
                Rotation = CurrentRotate + 90,
                TranslateX = CurrentSlideX,
                TranslateY = CurrentSlideY,
                ScaleX = 0.5,
                ScaleY = 0.5
            };
            //Canvas.SetTop(TurtleImage, 0);
            //Canvas.SetLeft(TurtleImage, 0);
        }

        public void SetPenColor(Color x)
        {
            throw new NotImplementedException();
        }

        public void SetPenWidth(int x)
        {
            throw new NotImplementedException();
        }

        public void SetSolidFill(Color x)
        {
            throw new NotImplementedException();
        }

        public void ClearFill()
        {
            throw new NotImplementedException();
        }

        public Color Rgb(int r, int g, int b)
        {
            throw new NotImplementedException();
        }

        public void OpenWindow()
        {
            throw new NotImplementedException();
        }

        public void NullifyWindow()
        {
            CurrentRotate = 0;
            CurrentSlideX = 0;
            CurrentSlideY = 0;
        }

        public void CloseWindow()
        {
            throw new NotImplementedException();
        }

        public void ClearWindow()
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string s)
        {
            throw new NotImplementedException();
        }
    }
}
