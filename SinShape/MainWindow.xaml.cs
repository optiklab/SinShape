using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace SinShape
{
    public struct Borders
    {
        public int x1;
        public int x2;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Timer
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            _borders.x1 = 1;
            _borders.x2 = 301;

            Amplify = 50.0;
            Freq = 0.03;
        }

        #endregion

        #region Event handlers
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs args)
        {
            if (_isAnimationStarted)
            {
                int speed = _CalculateSpeed(SuperSlider.Value);

                if (_borders.x1 < MONITOR_RIGHT*2 + 20)
                {
                    _borders.x1 += speed;
                    _borders.x2 += speed;
                }
                else
                {
                    _borders.x1 = MONITOR_LEFT;
                    _borders.x2 = MONITOR_RIGHT;
                }
            }

            _DrawShape();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _isAnimationStarted = true;

            if (SuperSlider.Value == 100)
            {
                Signal12.Visibility = Visibility.Visible;
                Signal22.Visibility = Visibility.Visible;
                Signal32.Visibility = Visibility.Visible;
                Signal42.Visibility = Visibility.Visible;
                Signal52.Visibility = Visibility.Visible;
            }
            else if (SuperSlider.Value > 75)
            {
                Signal12.Visibility = Visibility.Visible;
                Signal22.Visibility = Visibility.Visible;
                Signal32.Visibility = Visibility.Visible;
                Signal42.Visibility = Visibility.Visible;
                Signal52.Visibility = Visibility.Hidden;
            }
            else if (SuperSlider.Value > 50)
            {
                Signal12.Visibility = Visibility.Visible;
                Signal22.Visibility = Visibility.Visible;
                Signal32.Visibility = Visibility.Visible;
                Signal42.Visibility = Visibility.Hidden;
                Signal52.Visibility = Visibility.Hidden;
            }
            else if (SuperSlider.Value > 25)
            {
                Signal12.Visibility = Visibility.Visible;
                Signal22.Visibility = Visibility.Visible;
                Signal32.Visibility = Visibility.Hidden;
                Signal42.Visibility = Visibility.Hidden;
                Signal52.Visibility = Visibility.Hidden;
            }
            else if (SuperSlider.Value > 0)
            {
                Signal12.Visibility = Visibility.Visible;
                Signal22.Visibility = Visibility.Hidden;
                Signal32.Visibility = Visibility.Hidden;
                Signal42.Visibility = Visibility.Hidden;
                Signal52.Visibility = Visibility.Hidden;
            }
            else
            {
                Signal12.Visibility = Visibility.Hidden;
                Signal22.Visibility = Visibility.Hidden;
                Signal32.Visibility = Visibility.Hidden;
                Signal42.Visibility = Visibility.Hidden;
                Signal52.Visibility = Visibility.Hidden;
                _isAnimationStarted = false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Checked(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Start();            
            NoSignalLabel.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Unchecked(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
            NoSignalLabel.Visibility = Visibility.Visible;
            DrawingSpace.DeleteVisual(_visual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Checked(object sender, RoutedEventArgs e)
        {
            using (DrawingContext dc = _grid.RenderOpen())
            {
                // vertical.
                dc.DrawLine(_gridPen, new Point(0, 40), new Point(301, 40));
                dc.DrawLine(_gridPen, new Point(0, 80), new Point(301, 80));
                dc.DrawLine(_gridPen, new Point(0, 120), new Point(301, 120));
                dc.DrawLine(_gridPen, new Point(0, 160), new Point(301, 160));

                // horizontal.
                dc.DrawLine(_gridPen, new Point(60, 0), new Point(60, 201));
                dc.DrawLine(_gridPen, new Point(120, 0), new Point(120, 201));
                dc.DrawLine(_gridPen, new Point(180, 0), new Point(180, 201));
                dc.DrawLine(_gridPen, new Point(240, 0), new Point(240, 201));

                DrawingSpace.AddVisual(_grid);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Unchecked(object sender, RoutedEventArgs e)
        {
            DrawingSpace.DeleteVisual(_grid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Checked(object sender, RoutedEventArgs e)
        {
            SpeedRectangle.Visibility = Visibility.Hidden;
            AmplitudeRectangle.Visibility = Visibility.Hidden;
            PhaseRectangle.Visibility = Visibility.Hidden;
            SuperSlider.IsEnabled = true;
            MinusAmpButton.IsEnabled = true;
            MinusFreqButton.IsEnabled = true;
            MinusSpeedButton.IsEnabled = true;
            PlusAmpButton.IsEnabled = true;
            PlusFreqButton.IsEnabled = true;
            PlusSpeedButton.IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Unchecked(object sender, RoutedEventArgs e)
        {
            SpeedRectangle.Visibility = Visibility.Visible;
            AmplitudeRectangle.Visibility = Visibility.Visible;
            PhaseRectangle.Visibility = Visibility.Visible;
            SuperSlider.IsEnabled = false;
            SuperSlider.IsEnabled = false;
            MinusAmpButton.IsEnabled = false;
            MinusFreqButton.IsEnabled = false;
            MinusSpeedButton.IsEnabled = false;
            PlusAmpButton.IsEnabled = false;
            PlusFreqButton.IsEnabled = false;
            PlusSpeedButton.IsEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            Amplify = 50.0; // 1 ... 98
            Freq = 0.03; // 0 ... 0.1
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlusFreqButton_Click(object sender, RoutedEventArgs e)
        {
            Freq += 0.01;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinusFreqButton_Click(object sender, RoutedEventArgs e)
        {
            Freq -= 0.01;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlusAmpButton_Click(object sender, RoutedEventArgs e)
        {
            Amplify += 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinusAmpButton_Click(object sender, RoutedEventArgs e)
        {
            Amplify -= 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlusSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            SuperSlider.Value += 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinusSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            SuperSlider.Value -= 1;
        }

        #endregion

        #region Amplify property

        ///<summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AmplifyProperty =
            DependencyProperty.Register("Amplify", typeof(double), typeof(MainWindow));
                                        //new FrameworkPropertyMetadata(OnAmplifyChanged));
                                        //new ValidateValueCallback(IsAmplitudeValid));

        /// <summary>
        /// 
        /// </summary>
        public double Amplify
        {
            get 
            {
                double temp = GetValidAmplitude(GetValue(AmplifyProperty));
                return temp; 
            }
            set 
            {
                double temp = GetValidAmplitude(value);
                SetValue(AmplifyProperty, temp); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double GetValidAmplitude(object value)
        {
            double temp = (double)value;
            if (temp > 98)
                temp = 98;
            else if (temp < 1)
                temp = 1;

            return temp;
        }

        #endregion

        #region Radial frequency property

        ///<summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty FreqProperty =
            DependencyProperty.Register("Freq", typeof(double), typeof(MainWindow));

        /// <summary>
        /// 
        /// </summary>
        public double Freq
        {
            get 
            {
                double temp = GetValidFreq(GetValue(FreqProperty));
                return temp; 
            }
            set 
            {
                double temp = GetValidFreq(value);
                SetValue(FreqProperty, temp); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double GetValidFreq(object value)
        {
            double temp = (double)value;
            if (temp > 1)
                temp = 1;
            else if (temp < 0)
                temp = 0;

            return temp;
        }

        #endregion
        
        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        private int _CalculateSpeed(double sensor)
        {
            int temp = (int)(sensor/5);
            int speed = 1;
            if (temp != 0)
                speed = temp;

            return speed;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _DrawShape()
        {
            List<Point> points = _CalculatePoints();

            DrawingVisual visual = new DrawingVisual();            

            using (DrawingContext dc = visual.RenderOpen())
            {
                if (points.Count > 0)
                {
                    Point start = points[0];
                    foreach (Point p in points)
                    {
                        dc.DrawLine(_pen, start, p);
                        start = p;
                    }
                }
 
                DrawingSpace.DeleteVisual(_visual);
                _visual = visual;
                DrawingSpace.AddVisual(_visual);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<Point> _CalculatePoints()
        {
            List<Point> points = new List<Point>();
            int step = 3;
            int pointX = _borders.x1;
            int pointsCounter = MONITOR_LEFT;
            while ((pointX >= _borders.x1 && pointX < _borders.x2) &&
                    (pointsCounter >= MONITOR_LEFT && pointsCounter < MONITOR_RIGHT))
            {
                // A sin(wx)
                double result = Amplify * Math.Sin(pointX * Freq) + ABSCISS;

                points.Add(new Point(pointsCounter, result));

                pointX += step;
                pointsCounter += step;
            }

            return points;
        }

        #endregion

        #region Private constants

        private int MONITOR_LEFT = 1;

        private int MONITOR_RIGHT = 301;

        private int ABSCISS = 100;

        #endregion

        #region Private fields
        
        private DrawingVisual _visual = new DrawingVisual();

        private DrawingVisual _grid = new DrawingVisual();

        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        private bool _isAnimationStarted = false;

        private Borders _borders;

        private Pen _pen = (Pen)App.Current.FindResource("DrawingPen");

        private Pen _gridPen = (Pen)App.Current.FindResource("GridPen");
        
        #endregion
    }
}
