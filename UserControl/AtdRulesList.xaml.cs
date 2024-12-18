﻿using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeTrack_Pro.UserControl
{
    /// <summary>
    /// AtdRulesList.xaml 的交互逻辑
    /// </summary>
    public partial class AtdRulesList
    {
        public AtdRulesList()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Excel File|*.xls;*.xlsx";
            if (openFile.ShowDialog().HasValue)
            {
                tbxOrgList.Text = openFile.FileName;
            }
        }
    }
}
