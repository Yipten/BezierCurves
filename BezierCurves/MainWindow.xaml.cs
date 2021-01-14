using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BezierCurves
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PointCollection points = new PointCollection();
                if ((bool)P1CheckBox.IsChecked)
                    points.Add(new Point(double.Parse(X1TextBox.Text), double.Parse(Y1TextBox.Text)));
                if ((bool)P2CheckBox.IsChecked)
                    points.Add(new Point(double.Parse(X2TextBox.Text), double.Parse(Y2TextBox.Text)));
                if ((bool)P3CheckBox.IsChecked)
                    points.Add(new Point(double.Parse(X3TextBox.Text), double.Parse(Y3TextBox.Text)));
                if ((bool)P4CheckBox.IsChecked)
                    points.Add(new Point(double.Parse(X4TextBox.Text), double.Parse(Y4TextBox.Text)));
                if ((bool)P5CheckBox.IsChecked)
                    points.Add(new Point(double.Parse(X5TextBox.Text), double.Parse(Y5TextBox.Text)));
                Draw(points);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nMake sure everything is entered correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetButton_click(object sender, RoutedEventArgs e)        //resets field and all text boxes
        {
            Reset(true);
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            string helpText =
                "Point 1 - Point 5 -> Enter X and Y coordinates for each point here (can be decimals)\n\n" +
                "Checkboxes -> Use the checkboxes to select which points will be used\n\n" +
                "Draw -> Draws points, lines connecting the points, and the curve on the canvas\n\n" +
                "Reset -> Resets feild and all text boxes\n\n" +
                "Help -> Opens this help message\n\n" +
                "Close -> Closes the program\n\n" +
                "=======================================\n\n" +
                "Created by Andrew Sealing";
            MessageBox.Show(helpText, "Bezier Curves Help", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void CloseButton_click(object sender, RoutedEventArgs e)
        {
            Close();        //closes the window
        }

        private void Reset(bool tb)
        {
            if (tb)
            {
                X1TextBox.Clear();
                Y1TextBox.Clear();
                X2TextBox.Clear();
                Y2TextBox.Clear();
                X3TextBox.Clear();
                Y3TextBox.Clear();
                X4TextBox.Clear();
                Y4TextBox.Clear();
                X5TextBox.Clear();
                Y5TextBox.Clear();
                P1CheckBox.IsChecked = true;
                P2CheckBox.IsChecked = true;
                P3CheckBox.IsChecked = true;
                P4CheckBox.IsChecked = false;
                P5CheckBox.IsChecked = false;
            }
            Point1.Visibility = Visibility.Hidden;
            Point2.Visibility = Visibility.Hidden;
            Point3.Visibility = Visibility.Hidden;
            Point4.Visibility = Visibility.Hidden;
            Point5.Visibility = Visibility.Hidden;
            Line1.Visibility = Visibility.Hidden;
            Line2.Visibility = Visibility.Hidden;
            Line3.Visibility = Visibility.Hidden;
            Line4.Visibility = Visibility.Hidden;
            Curve.Visibility = Visibility.Hidden;
        }

        private void Draw(PointCollection points)
        {
            Reset(false);
            Polygon[] DrawingPoints = { Point1, Point2, Point3, Point4, Point5 };
            for (int i = 0; i < points.Count; i++)
                DrawPoint(DrawingPoints[i], points[i]);
            Line[] DrawingLines = { Line1, Line2, Line3, Line4};
            for (int i = 0; i < points.Count - 1; i++)
                DrawLine(DrawingLines[i], points[i], points[i + 1]);
            DrawBezierCurve(Curve, points);
        }

        private void DrawPoint(Polygon polygon, Point point)
        {
            Point       //draws a small sqare to make the point more easily seen than a single pixel
                p1 = new Point(point.X - 2, point.Y - 2),
                p2 = new Point(point.X + 2, point.Y - 2),
                p3 = new Point(point.X + 2, point.Y + 2),
                p4 = new Point(point.X - 2, point.Y + 2);
            PointCollection points = new PointCollection(4) { p1, p2, p3, p4 };
            polygon.Points = points;
            polygon.Visibility = Visibility.Visible;
        }

        private void DrawLine(Line line, Point point1, Point point2)
        {
            line.X1 = point1.X;
            line.Y1 = point1.Y;
            line.X2 = point2.X;
            line.Y2 = point2.Y;
            line.Visibility = Visibility.Visible;
        }

        private void DrawBezierCurve(Polyline polyline, PointCollection enteredPoints)
        {
            PointCollection points = new PointCollection();
            for (double i = 0; i <= 1; i += 0.01)
            {
                points.Add(GeneratePoints(i, enteredPoints));
            }
            polyline.Points = points;
            polyline.Visibility = Visibility.Visible;
        }

        private Point GeneratePoints(double t, PointCollection points)
        {
            PointCollection generatedPoints = new PointCollection();
            for (int i = 0; i < points.Count - 1; i++)
            {
                double x = (1 - t) * points[i].X + t * points[i + 1].X;
                double y = (1 - t) * points[i].Y + t * points[i + 1].Y;
                generatedPoints.Add(new Point(x, y));
            }
            if (generatedPoints.Count > 1)
                return GeneratePoints(t, generatedPoints);
            return generatedPoints[0];
        }
    }
}
