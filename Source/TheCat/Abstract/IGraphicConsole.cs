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

namespace Cat.Abstract
{
    public interface IGraphicConsole
    {
        void Render(GraphicCommand c);

        void Rotate(double x);
        void Line(double x1, double y1, double x2, double y2);
        void Slide(double x, double y);
        void Scale(double x, double y);
        void SetPenUp(bool b);
        bool IsPenUp();
        void Ellipse(double x, double y, double w, double h);
        void DrawTurtleFoot(int x, int y);
        void SetTurtleVisibility(bool b);
        bool GetTurtleVisibility();
        void DrawTurtle();
        void SetPenColor(Color x);
        void SetPenWidth(int x);
        void SetSolidFill(Color x);
        void ClearFill();

        Color Rgb(int r, int g, int b);

        void OpenWindow();
        void NullifyWindow();
        void CloseWindow();
        void ClearWindow();
        void SaveToFile(string s);
    }
}
