﻿using SCEEC.MI.TZ3310;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCEEC.TTM
{

    /// <summary>
    /// TestingInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestingInfoWindow : Window
    {
        public JobInformation Information;

        public int state = 0;

        public TestingInfoWindow(TestingWorkerSender sender)
        {
            InitializeComponent();
            IsUsb = false;
            MeasurementItemsListBox.ItemsSource = sender.GetList();
            testingTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        }

        private List<string> Gettestme(List<MeasurementItemStruct> job)
        {
            if (job != null)
            {
                List<string> mess = new List<string>();
                foreach (var a in job)
                {
                    mess.Add(a.Description);
                }
                return mess;
            }
            else
                return null;

        }
        public bool IsUsb { get; set; } = false;
        public TestingInfoWindow(List<MeasurementItemStruct> job)
        {
            InitializeComponent();
            MeasurementItemsListBox.ItemsSource = Gettestme(job);
            testingTime.Text = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            IsUsb = true;
        }

        private void TextBlock_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!Microsoft.VisualBasic.Information.IsNumeric((OilTemperature.Text)))
            {
                MessageBox.Show("油温为非数值参数!", "试验参数管理器");
                OilTemperature.Focus();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            //if (!DateTime.TryParse(testingTime.Text, out Information.testingTime))
            //{
            //    MessageBox.Show("试验日期信息错误");
            //    return;
            //}
            if (!double.TryParse(OilTemperature.Text, out Information.oilTemperature))
            {
                MessageBox.Show("油温必须为纯数值");
                return;
            }

            // (testingTime.Text, out Information.testingTime
            Information.testingTime = Convert.ToDateTime(testingTime.Text); // testingTime.Text.ToString("yyyy-MM-dd HH:mm:ss");
            Information.testingName = testingName.Text;
            Information.tester = tester.Text;
            Information.testingAgency = testingAgency.Text;
            Information.auditor = auditor.Text;
            Information.approver = approver.Text;
            Information.weather = weather.Text;
            Information.temperature = temperature.Text;
            Information.humidity = humidity.Text;
            Information.principal = principal.Text;


            state = 1;
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            state = 2;
            this.DialogResult = false;
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState != WindowState.Normal)
            {
                this.WindowState = WindowState.Normal;
                this.Top = 0;
            }
            this.DragMove();
        }

        private void UpanButton_Click(object sender, RoutedEventArgs e)
        {
            if (!DateTime.TryParse(testingTime.Text, out Information.testingTime))
            {
                MessageBox.Show("试验日期信息错误");
                return;
            }
            if (!double.TryParse(OilTemperature.Text, out Information.oilTemperature))
            {
                MessageBox.Show("油温必须为纯数值");
                return;
            }
            Information.testingName = testingName.Text;
            Information.tester = tester.Text;
            Information.testingAgency = testingAgency.Text;
            Information.auditor = auditor.Text;
            Information.approver = approver.Text;
            Information.weather = weather.Text;
            Information.temperature = temperature.Text;
            Information.humidity = humidity.Text;
            Information.principal = principal.Text;
            state = 1;
            this.DialogResult = true;
            this.Close();
        }
    }
}
