﻿using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Dtos;
using System.ComponentModel;
using Newtonsoft.Json;
using MathNet.Numerics.Distributions;

namespace ChartsWave
{
    /// <summary>
    /// ChartWave.xaml 的交互逻辑
    /// </summary>
    public partial class ChartWave : UserControl, INotifyPropertyChanged
    {
        public ChartWave()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private GearedValues<ObservablePoint> cutChart;

        public GearedValues<ObservablePoint> cutWaveChart
        {
            get { return cutChart; }
            set
            {
                GearedValues<ObservablePoint> data = new GearedValues<ObservablePoint>();
                var temp = value.ToArray();
                for (int i = 0; i < value.Count(); i++)
                {
                    ObservablePoint p = new ObservablePoint { X = temp[i].X, Y = Math.Round(temp[i].Y, 2) };
                    data.Add(p);
                }
                DrawWave(data);
                cutChart = value;
            }
        }

        private GearedValues<ObservablePoint> sumChart;
        public GearedValues<ObservablePoint> shortWave
        {
            get
            {
                var data = (GearedValues<ObservablePoint>)GetValue(shortWaveProperty);
                sumChart = data;
                return data;
            }
            set
            {
                if (value != null && value.Count() == 6000)
                {
                    sumChart = value;
                    cutWaveChart = CutWave(value);
                }
                SetValue(shortWaveProperty, value);
            }
        }

        public static readonly DependencyProperty shortWaveProperty =
            DependencyProperty.Register("shortWave", typeof(GearedValues<ObservablePoint>), typeof(ChartWave), new PropertyMetadata(null, null, coreValueCallback));

        private static object coreValueCallback(DependencyObject d, object baseValue)
        {
            return (GearedValues<ObservablePoint>)baseValue;
        }

        //private int cutSkip = 0;
        //private int cutTake = 6000;
        public int cutTake
        {
            get { return (int)GetValue(cutTakeProperty); }
            set { SetValue(cutTakeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for cutTake.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cutTakeProperty =
            DependencyProperty.Register("cutTake", typeof(int), typeof(ChartWave), new PropertyMetadata(0, null, takeBack));

        private static object takeBack(DependencyObject d, object baseValue)
        {
            return (int)baseValue;

        }

        public int cutSkip
        {
            get { return (int)GetValue(cutSkipProperty); }
            set { SetValue(cutSkipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for cutSkip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cutSkipProperty =
            DependencyProperty.Register("cutSkip", typeof(int), typeof(ChartWave), new PropertyMetadata(0, null, cutBack));

        private static object cutBack(DependencyObject d, object baseValue)
        {
            return (int)baseValue;
        }

        private GearedValues<ObservablePoint> CutWave(GearedValues<ObservablePoint> wave, int cutHeaderLever = 400)
        {
            GearedValues<ObservablePoint> data = new GearedValues<ObservablePoint>();
            data.AddRange(wave.Skip(cutSkip).Take(cutTake));
            return data;
        }


        private void DrawFourLine(GearedValues<ObservablePoint> schart, GearedValues<ObservablePoint> sundata)
        {
            double needSelectData = Math.Round((sundata.Select(x => x.Y).Max() + sundata.Select(x => x.Y).Min()) / 2, 3);
            ObservablePoint[] twopoint = new ObservablePoint[4];
            double[] tempD = sundata.Select(x => Math.Round(x.Y, 3)).ToArray();
            for (int i = 1; i < 6000; i++)
            {
                if ((tempD[i - 1] >= needSelectData && tempD[i] <= needSelectData) || (tempD[i] >= needSelectData && tempD[i - 1] <= needSelectData))
                {
                    twopoint[0] = sundata[i];
                    break;
                }
            }
            for (int i = 5999; i > 1; i--)
            {
                if ((tempD[i - 1] >= needSelectData && tempD[i] <= needSelectData) || (tempD[i] >= needSelectData && tempD[i - 1] <= needSelectData))
                {
                    twopoint[3] = sundata[i];
                    break;
                }
            }


            double cuttime = cutTake * 0.05;
            double skiptime = cutSkip * 0.05;

            //绘制t1
            Hover1.X1 = (twopoint[0].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            Hover1.X2 = (twopoint[0].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            t1.Text = "t₁";
            Canvas.SetLeft(t1, Hover1.X1 - 5);
            Canvas.SetBottom(t1, 5);

            Hover4.X1 = (twopoint[3].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            Hover4.X2 = (twopoint[3].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            t4.Text = "t₄";
            Canvas.SetLeft(t4, Hover4.X1 - 5);
            Canvas.SetBottom(t4, 5);


            int oneStart = (int)((twopoint[0].X) / 300 * sumChart.Count());
            int fourStart = (int)((twopoint[3].X) / 300 * sumChart.Count());

            var centerPoints = schart.Skip(oneStart + (fourStart - oneStart) / 4).Take((fourStart - oneStart) / 2);

            int startI = oneStart + (fourStart - oneStart) / 4;

            var centerValue = (centerPoints.Select(x => x.Y).Max() + centerPoints.Select(x => x.Y).Min()) / 2;

            double[] centDoble = centerPoints.Select(x => Math.Round(x.Y, 3)).ToArray();
            for (int i = 1; i < centDoble.Length; i++)
            {
                if ((centDoble[i - 1] >= centerValue && centDoble[i] <= centerValue) || (centDoble[i] >= centerValue && centDoble[i - 1] <= centerValue))
                {
                    twopoint[1] = sundata[i + startI];
                    break;
                }
            }
            for (int i = centDoble.Length - 1; i > 1; i--)
            {
                if ((centDoble[i - 1] >= centerValue && centDoble[i] <= centerValue) || (centDoble[i] >= centerValue && centDoble[i - 1] <= centerValue))
                {
                    twopoint[2] = sundata[i + startI];
                    break;
                }
            }



            Hover2.X1 = (twopoint[1].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            Hover2.X2 = (twopoint[1].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            t2.Text = "t₂";
            Canvas.SetLeft(t2, Hover2.X1 - 5);
            Canvas.SetBottom(t2, 5);

            Hover3.X1 = (twopoint[2].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            Hover3.X2 = (twopoint[2].X - skiptime) / cuttime * chartCore.Width + chartCore.Left + 5;
            t3.Text = "t₃";
            Canvas.SetLeft(t3, Hover3.X1 - 5);
            Canvas.SetBottom(t3, 5);

            initLine();

            getResult(twopoint, sundata);
        }


        private void getResult(ObservablePoint[] ret, GearedValues<ObservablePoint> shortWave)
        {
            if (ret.Length == 4)
            {
                t1Ret = (ret[1].X - ret[0].X).ToString("N2") + " ms";
                t2Ret = (ret[2].X - ret[1].X).ToString("N2") + " ms";
                t3Ret = (ret[3].X - ret[2].X).ToString("N2") + " ms";
                t4Ret = (ret[3].X - ret[0].X).ToString("N2") + " ms";

                var arr1 = shortWave.Skip((int)(ret[0].X / 300 * 6000)).Take((int)((ret[1].X - ret[0].X) / 300 * 6000)).Select(x => Math.Round(x.Y, 2));
                var arr2 = shortWave.Skip((int)(ret[2].X / 300 * 6000)).Take((int)((ret[3].X - ret[2].X) / 300 * 6000)).Select(x => Math.Round(x.Y, 2));
                var arr3 = shortWave.Skip((int)(ret[1].X / 300 * 6000)).Take((int)((ret[2].X - ret[1].X) / 300 * 6000)).Select(x => Math.Round(x.Y, 2));

                //double r1 = LogNormal.Estimate(arr1).Mean;
                //double r2 = LogNormal.Estimate(arr2).Mean;
                //double r3 = LogNormal.Estimate(arr3).Mean;

                double r1 = GetElevationMode(arr1.ToList());
                double r2 = GetElevationMode(arr2.ToList());
                //double r3 = GetElevationMode(arr3.ToList());
                double r3 = LogNormal.Estimate(arr3).Mean;

                //var sss = new MathNet.Numerics.Differentiation.NumericalDerivative(20, 2);
                R1Ret = r1.ToString("N2") + " Ω";
                R2Ret = r2.ToString("N2") + " Ω";

                R1AndR2Ret = r3.ToString("N2") + " Ω"; ;

            }
        }

        private CoreRectangle chartCore;

        private void initLine()
        {
            Hover1.Y1 = 13;
            Hover1.Y2 = ActualHeight - 25;
            Hover2.Y1 = 13;
            Hover2.Y2 = ActualHeight - 25;
            Hover3.Y1 = 13;
            Hover3.Y2 = ActualHeight - 25;
            Hover4.Y1 = 13;
            Hover4.Y2 = ActualHeight - 25;
        }


        private static double GetElevationMode(List<double> elevationList)
        {
            try
            {
                int count;
                bool flag = false;
                Dictionary<double, int> dictionary = new Dictionary<double, int>();
                for (int i = 0; i < elevationList.Count; i++)
                {
                    if (dictionary.TryGetValue(elevationList[i], out count))
                    {
                        flag = true;
                        dictionary[elevationList[i]]++;
                    }
                    else
                        dictionary.Add(elevationList[i], 1);
                }
                if (!flag)
                    return 0;
                int max = 0;
                int position = 0;
                double[] modeArray = new double[elevationList.Count];//众数数组
                foreach (KeyValuePair<double, int> myKey in dictionary)
                {
                    if (myKey.Value > max)
                    {
                        max = myKey.Value;
                        position = 0;
                        modeArray[0] = myKey.Key;
                    }
                    else if (myKey.Value == max)
                        modeArray[++position] = myKey.Key;
                }
                Array.Resize(ref modeArray, position + 1);
                double mode = 0;
                if (modeArray.Length > 1)
                {
                    for (int i = 0; i < modeArray.Length; i++)
                    {
                        mode += modeArray[i];
                    }
                    double elevationMode = mode / modeArray.Length;
                    return elevationMode;
                }
                else
                {
                    mode = modeArray[0];
                }
                return mode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        private double maxVal;
        private double minVal;

        private void DrawWave(GearedValues<ObservablePoint> data)
        {
            var maxf =data.Select(x => x.Y).Max();
            var minf =data.Select(x => x.Y).Min();
            GearedValues<ObservablePoint> tempdata = new GearedValues<ObservablePoint>();
            tempdata.AddRange(data.Select(x =>
            {
                x.Y = maxf - Math.Abs(x.Y - minf);
                return x;
            }));

            var max = Math.Round(tempdata.Select(x => x.Y).Max(), 2);
            var min =Math.Round(tempdata.Select(x => x.Y).Min(), 2);
            var avg = Math.Round((Math.Abs(max) + Math.Abs(min)) / 5, 2);
            var step =Math.Ceiling(Math.Abs(min) / avg);
            var stepmax =Math.Ceiling( Math.Abs(max) / avg);

            min = min >= 0 ? (avg * step) : (-1 * avg * step);
            max = max >= 0 ? (avg * stepmax) : (-1 * avg * stepmax);
            maxVal = max;
            minVal = min;
            Yaxis.MaxValue = max;
            Yaxis.MinValue = min;
            XFormatter = val => (val).ToString() + "ms";
            YFormatter = val =>
            {
                return Math.Round((max - Math.Abs(val - min)), 1).ToString() + "Ω";
            };

            YaxisSpe.Step = Math.Abs(avg);
            series = new SeriesCollection();
            series.Add(new GLineSeries
            {
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(28, 142, 196)),
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 10,//0为折现样式
                PointGeometrySize = 0,
                PointForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49)),
                Values = tempdata

            });
            chart_wave.Series = series;

            //Yaxis.Sections = new SectionsCollection {
            //     new AxisSection
            //        {
            //            Value = maxVal-Math.Abs(minVal),
            //             Stroke = Brushes.Red,
            //            StrokeThickness = 1.5,
            //        },

            //};
        }
        public SeriesCollection series { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private double chartHeight;
        private double chartWidth;

        public event PropertyChangedEventHandler PropertyChanged;

        public string t1Ret { get; set; }
        public string t2Ret { get; set; }
        public string t3Ret { get; set; }
        public string t4Ret { get; set; }
        public string R1Ret { get; set; }
        public string R2Ret { get; set; }
        public string R1AndR2Ret { get; set; }



        private void chart_wave_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var chart = sender as CartesianChart;
            chartHeight = chart.ActualHeight;
            chartWidth = chart.ActualWidth;
        }

        private void chart_wave_DataHover(object sender, ChartPoint chartPoint)
        {
            HoverXLine.Visibility = System.Windows.Visibility.Visible;
            HoverYLine.Visibility = System.Windows.Visibility.Visible;
            var mouselocation = Mouse.GetPosition(chart_wave);
            HoverXLine.X1 = mouselocation.X;
            HoverXLine.X2 = mouselocation.X;
            HoverXLine.Y1 = 10;
            HoverXLine.Y2 = ActualHeight - 25;

            HoverYLine.X1 = 35;
            HoverYLine.X2 = ActualWidth * 0.83;
            HoverYLine.Y1 = mouselocation.Y;
            HoverYLine.Y2 = mouselocation.Y;

            //    var asPixels = chart_wave.ConvertToPixels(chartPoint.AsPoint());
        }

        private void chart_wave_UpdaterTick(object sender)
        {
            var p = (sender as CartesianChart);
            chartCore = p.Model.DrawMargin;
            if (cutChart != null && sumChart != null)
            {
                DrawFourLine(cutChart, sumChart);
            }
        }

        private void chart_wave_MouseLeave(object sender, MouseEventArgs e)
        {
            HoverXLine.Visibility = System.Windows.Visibility.Collapsed;
            HoverYLine.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void chart_wave_Loaded(object sender, RoutedEventArgs e)
        {
            var p = chart_wave.AxisY.Max();
        }
    }

    //移动游标类
    public partial class ChartWave
    {
        //选中控件的鼠标位置偏移量
        Point targetPoint;

        //选中控件
        UIElement targetElement;

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //确定选中控件，然后设置选中控件
            targetElement = Mouse.DirectlyOver as UIElement;
            if (targetElement != null)
                targetPoint = e.GetPosition(targetElement);
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //鼠标松开后，将选中控件设置成null
            targetElement = null;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //确定鼠标左键处于按下状态并且有元素被选中
            if (e.LeftButton == MouseButtonState.Pressed && targetElement != null)
            {
                var pCanvas = e.GetPosition(drawCv);
                //设置最终位置
                Canvas.SetLeft(targetElement, pCanvas.X - targetPoint.X);
                Canvas.SetTop(targetElement, pCanvas.Y - targetPoint.Y);
            }
        }
    }
}
