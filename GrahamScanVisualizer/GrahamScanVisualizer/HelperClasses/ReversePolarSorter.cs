using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrahamScanVisualizer.HelperClasses
{
    public class ReversePolarSorter : Comparer<MyPoint>
    {
        const int precision = 6;
        double m_baseX;
        double m_baseY;
        public ReversePolarSorter(MyPoint basePoint)
        {
            m_baseX = basePoint.X;
            m_baseY = basePoint.Y;
        }

        double GetAngle(MyPoint p)
        {
            return Math.Round(Math.Atan2(p.Y - m_baseY, p.X - m_baseX), precision);
        }
        public override int Compare(MyPoint a, MyPoint b)
        {
            return GetAngle(a).CompareTo(GetAngle(b));
        }
    }
}
