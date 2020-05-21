using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
 
namespace GrahamScanVisualizer.HelperClasses
{
    public static class ExtensionMethods
    {
        private static readonly Action EmptyDelegate = delegate { };
        public static void Refresh(this UIElement uiElement, double speedInSeconds = 0.5)
        {
            Thread.Sleep((int)(speedInSeconds * 1000));
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
        public static bool cool_line_allpoints(List<MyPoint> points)
        {
            for (int i = 1; i < points.Count - 1; i+=2)
            {
                if (!cool_line(points[i - 1], points[i], points[i + 1]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool Ccw(MyPoint a, MyPoint b, MyPoint c)
        {
            return ((b.X - a.X) * (c.Y - a.Y)) > ((b.Y - a.Y) * (c.X - a.X));
        }
        private static bool cool_line(MyPoint p1, MyPoint p2, MyPoint p3)
        {
            if ((p3.Y - p2.Y) * (p2.X - p1.X) == (p2.Y - p1.Y) * (p3.X - p2.X))
                return true;
            else
                return false;
        }
    }
}
