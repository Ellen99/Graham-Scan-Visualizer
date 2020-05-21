using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrahamScanVisualizer.HelperClasses
{
    public class MyPoint : IComparable<MyPoint>
    {
        private int x, y;

        public MyPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public int CompareTo(MyPoint other)
        {
            return x.CompareTo(other.x);
        }
        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }

    }
}
