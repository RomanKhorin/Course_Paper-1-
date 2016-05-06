﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for New_Sales_Contract_Window.xaml
    /// </summary>
    public partial class New_Sales_Contract_Window : Window
    {
        public New_Sales_Contract_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод, который заключает контракт продажи машины и добавляет его в базу
        /// </summary>
        private void add_sales_contract_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(price_textbox.Text) == false &&
                        date_datepicker.SelectedDate != null)
                {
                    MainWindow.Connection.Open();
                    SqlCommand cmd = new SqlCommand("insert into [Sales_Contract] (Car_Id, Employee_Id, Customer_Id, Date, Price) " +
                                                    "values (@Car_Id, @Employee_Id, @Customer_Id, @Date, @Price)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@Car_Id", int.Parse(car_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Employee_Id", int.Parse(employee_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Customer_Id", int.Parse(client_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Date", date_datepicker.SelectedDate);
                    cmd.Parameters.AddWithValue("@Price", decimal.Parse(price_textbox.Text));
                    cmd.ExecuteNonQuery();
                    MainWindow.Connection.Close();

                    DialogResult = true;
                }
                else
                    MessageBox.Show("Check the entered data!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
        }
    }
}