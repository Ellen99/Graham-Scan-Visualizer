using GrahamScanVisualizer.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GrahamScanVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private members
        private List<MyPoint> points = new List<MyPoint>();
        private List<Ellipse> pointsInCanvas = new List<Ellipse>();
        private MyPoint p;
        private List<Line> sideLines = new List<Line>();
        private readonly Color RED = Color.FromRgb(237, 101, 113);
        private readonly Color GREEN = Color.FromRgb(32, 63, 70);
        private readonly Color YELLOW = Color.FromRgb(255, 208, 134);
        private readonly Color BLUE = Color.FromRgb(26, 140, 184);
        private readonly Color GRAYBLUE = Color.FromRgb(80, 124, 137);
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
        }

        public ReversePolarSorter ReversePolarSorter
        {
            get => default(ReversePolarSorter);
            set
            {
            }
        }

        public MyPoint MyPoint
        {
            get => default(MyPoint);
            set
            {
            }
        }
        #endregion

        #region Private methods
        //Algorithm implementation
        private List<MyPoint> ConvexHull_(List<MyPoint> points)
        {
            if (points.Count < 3)
            {
                MessageBox.Show("The points count is less than 3, please add more points");
                return new List<MyPoint>();
            }
            if (ExtensionMethods.cool_line_allpoints(points))
            {
                MessageBox.Show("The given points are in the same line, cannot make a polygon");
                return new List<MyPoint>();
            }

            points.Sort();
            MyPoint leftMostPoint = points[0];
            points.RemoveAt(0);
            IComparer<MyPoint> sorter = new ReversePolarSorter(leftMostPoint);
            points.Sort(0, points.Count, sorter);
            points.Reverse();
            points.Insert(0, leftMostPoint);
            List<MyPoint> h = new List<MyPoint>();

            // lower hull
            foreach (var pt in points)
            {
                //paint taken point yellow
                drawPoint(pt, YELLOW, 0.2);

                if (h.Count == 1)
                {
                    connectPoints(h[0], pt, Color.FromRgb(237, 101, 113));
                }
                if (h.Count >= 2)
                {
                    connectPoints(h[h.Count - 1], pt, Color.FromRgb(237, 101, 113));
                }
                while (h.Count >= 2 && ExtensionMethods.Ccw(h[h.Count - 2], h[h.Count - 1], pt))
                {

                    drawPoint(h[h.Count - 1], Colors.Red);

                    connectPoints(h[h.Count - 2], h[h.Count - 1], YELLOW);
                    connectPoints(h[h.Count - 1], pt, YELLOW);

                    //removefrom convex hull, draw it blue
                    drawPoint(h[h.Count - 1], RED);
                    h.RemoveAt(h.Count - 1);
                    //connectPoints(h[h.Count - 2], h[h.Count - 1], Colors.Green);
                }


                h.Add(pt);
                drawPoint(h.Last(), GREEN);
                if (h.Count >= 2)
                {
                    connectPoints(h[h.Count - 2], pt, GREEN);
                }
            }
            connectPoints(h[h.Count - 1], h[0], GREEN);

            return h;
        }
        #endregion

        #region Event Methods
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RunButton.IsEnabled == false)
            {
                return;
            }
            //IGraph<string, int> graph = new IGraph<string, int>();
            p = new MyPoint((int)e.GetPosition(myGrid).X, (int)e.GetPosition(myGrid).Y);
            if (p.X > 1000 || p.Y > 500)
            {
                return;
            }
            foreach (var item in points)
            {
                if (item.X == p.X && item.Y == p.Y)
                {
                    MessageBox.Show("you`ve already added point with the same coordinates");
                    return;
                }
            }

            //if (points.Contains(p))
            //{
            //    MessageBox.Show("you`ve already added point with the same coordinates");
            //    return;
            //}
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 8;
            ellipse.Height = 8;
            ellipse.Margin = new Thickness(p.X, p.Y, 0, 0);
            //ellipse.Margin = new Thickness(p.X - 5, p.Y - 5, 0, 0);
            ellipse.Fill = new SolidColorBrush(GRAYBLUE);
            pointsInCanvas.Add(ellipse);
            textBox.Text += '(' + (p.X).ToString() + ',' + (p.Y).ToString() + ')' + "\n";
            canvas.Children.Add(ellipse);
            points.Add(p);
            ErasePointsButton.Visibility = Visibility.Visible;

        }
        //private void HowToAddPoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    switch ((HowToAddPoints.SelectedItem as ComboBoxItem).Content)
        //    {
        //        case "Add points with mouse click":
        //            AddPointWithCoordinatesButton.Visibility = Visibility.Hidden;
        //            XtextBox.Visibility = Visibility.Hidden;
        //            YtextBox.Visibility = Visibility.Hidden;
        //            break;
        //        case "Add points inputting coordinates":
        //            AddPointsButton.Visibility = Visibility.Hidden;
        //            AddPointWithCoordinatesButton.Visibility = Visibility.Visible;
        //            XtextBox.Visibility = Visibility.Visible;
        //            YtextBox.Visibility = Visibility.Visible;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (points.Count < 3)
            {
                MessageBox.Show("The points count is less than 3, please add more points");
                return ;
            }
            resultTextBox.Text = "";
            RunButton.IsEnabled = false;
            Add_Random_Points.IsEnabled = false;
            AddPointWithCoordinatesButton.IsEnabled = false;
            List<MyPoint> convexHullPoints = new List<MyPoint>();
            convexHullPoints = ConvexHull_(points);

            foreach (var item in convexHullPoints)
            {
                resultTextBox.Text += '(' + (item.X).ToString() + ',' + (item.Y).ToString() + ')' + "\n";
            }

            ExtensionMethods.Refresh(canvas,1- speed.Value);
            AddPointsButton.Visibility = Visibility.Visible;

        }
        private void AddPointsButton_Click(object sender, RoutedEventArgs e)
        {
            eraseColors();
            resultTextBox.Text = "";
            foreach (var item in sideLines)
            {
                canvas.Children.Remove(item);
            }
            sideLines = new List<Line>();
            ExtensionMethods.Refresh(canvas, 0);
            RunButton.IsEnabled = true;
            Add_Random_Points.IsEnabled = true;
            AddPointWithCoordinatesButton.IsEnabled = true;
        }
        private void ErasePointsButton_Click(object sender, RoutedEventArgs e)
        {
            points = new List<MyPoint>();
            pointsInCanvas = new List<Ellipse>();
            p = null;
            sideLines = new List<Line>();
            canvas.Children.RemoveRange(0, canvas.Children.Count);

            textBox.Text = "";
            resultTextBox.Text = "";
            ExtensionMethods.Refresh(canvas, 0);
            RunButton.IsEnabled = true;
            AddPointWithCoordinatesButton.IsEnabled = true;
            Add_Random_Points.IsEnabled = true;
        }
        private void AddPointWithCoordinatesButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(XtextBox.Text) || string.IsNullOrEmpty(YtextBox.Text))
            {
                MessageBox.Show("Please input X and Y values to add the point");
                return;
            }
            p = new MyPoint(Int32.Parse(XtextBox.Text), Int32.Parse(YtextBox.Text));
            if (p.X > 1000 || p.Y > 500)
            {
                return;
            }
            foreach (var item in points)
            {
                if (item.X == p.X && item.Y == p.Y)
                {
                    MessageBox.Show("you`ve already added point with the same coordinates");
                    return;
                }
            }
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 8;
            ellipse.Height = 8;
            ellipse.Margin = new Thickness(p.X, p.Y, 0, 0);
            ellipse.Fill = new SolidColorBrush(GRAYBLUE);
            pointsInCanvas.Add(ellipse);
            textBox.Text += '(' + (p.X).ToString() + ',' + (p.Y).ToString() + ')' + "\n";
            canvas.Children.Add(ellipse);
            points.Add(p);
            ErasePointsButton.Visibility = Visibility.Visible;
            randomPointsCount.Text = "";
        }
        private void XtextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            int num;
            if (!Int32.TryParse(e.Text, out num))
            {
                e.Handled = true;
                return;
            }
            if (Int32.TryParse(XtextBox.Text + e.Text, out num))
            {

                if ((!string.IsNullOrEmpty(XtextBox.Text) && num > 1000) || regex.IsMatch(e.Text))
                {
                    e.Handled = true;
                }
                return;
            }
        }
        private void YtextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            int num;
            if(!Int32.TryParse(e.Text,out num))
            {
                e.Handled = true;
                return;
            }
            if (Int32.TryParse(YtextBox.Text + e.Text, out num))
            {
                if ((!string.IsNullOrEmpty(YtextBox.Text) && num > 500) || regex.IsMatch(e.Text))
                {
                    e.Handled = true;
                }
                return;
            }
        }
        private void RandomPointsCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            int num;
            if (Int32.TryParse(randomPointsCount.Text + e.Text, out num))
            {
                return;
            }
            if ((!string.IsNullOrEmpty(randomPointsCount.Text) && num > 1000) || regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void Add_Random_Points_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(randomPointsCount.Text))
            {
                MessageBox.Show("Please input how many random points you want to add");
                return;
            }
            int count = Int32.Parse(randomPointsCount.Text);
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {

                p = new MyPoint(rnd.Next(0, 1000), rnd.Next(0, 500));
                foreach (var item in points)
                {
                    if (item.X == p.X && item.Y == p.Y)
                    {
                        i--;
                        continue;
                    }
                }
                Ellipse ellipse = new Ellipse();
                ellipse.Width = 8;
                ellipse.Height = 8;
                ellipse.Fill = new SolidColorBrush(GRAYBLUE);
                ellipse.Margin = new Thickness(p.X, p.Y, 0, 0);
                pointsInCanvas.Add(ellipse);
                textBox.Text += '(' + (p.X).ToString() + ',' + (p.Y).ToString() + ')' + "\n";
                canvas.Children.Add(ellipse);
                points.Add(p);

            }
            randomPointsCount.Text = "";

            ExtensionMethods.Refresh(canvas);
        }
        #endregion

        #region Helper methods     
        private void connectPoints(MyPoint point1, MyPoint point2, Color color)
        {
            Line line = new Line
            {
                X1 = point1.X + 5,
                Y1 = point1.Y + 5,
                X2 = point2.X + 5,
                Y2 = point2.Y + 5,
                Stroke = new SolidColorBrush(color)
            };
            canvas.Children.Add(line);
            sideLines.Add(line);
            ExtensionMethods.Refresh(canvas, 0.2);
        }
        private void eraseColors()
        {
            foreach (var point in points)
            {
                foreach (var item in pointsInCanvas)
                {
                    //if (item.Margin == new Thickness(point.X - 5, point.Y - 5, 0, 0))
                    if (item.Margin == new Thickness(point.X, point.Y, 0, 0))
                    {
                        item.Fill = new SolidColorBrush(GRAYBLUE);
                    }
                }
            }
        }
        private void drawPoint(MyPoint point, Color color, double speedInSeconds = 0.5)
        {
            foreach (var item in pointsInCanvas)
            {
                //if (item.Margin == new Thickness(point.X - 5, point.Y - 5, 0, 0))
                if (item.Margin == new Thickness(point.X, point.Y, 0, 0))
                {
                    item.Fill = new SolidColorBrush(color);
                    ExtensionMethods.Refresh(canvas,1- speed.Value);
                }
            }
        }
        #endregion

    }
}
