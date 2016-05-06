﻿using System;
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

namespace AutoCenter
{
    /// <summary>
    /// Interaction logic for Sales_Contracts_Window.xaml
    /// </summary>
    public partial class Sales_Contracts_Window : Window
    {
        public Sales_Contracts_Window()
        {
            InitializeComponent();
            MainWindow.GetSalesContracts(MainWindow.Connection, sales_contracts_listbox, MainWindow.reader);
        }
    }
}
