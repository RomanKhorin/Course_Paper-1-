using System;
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
    /// Interaction logic for New_Rental_Contract_Window.xaml
    /// </summary>
    public partial class New_Rental_Contract_Window : Window
    {
        public New_Rental_Contract_Window()
        {
            InitializeComponent();

            car_number_textbox.Text = MainWindow.CurrentRentalCarNumber;
            client_id_textbox.Text = MainWindow.CurrentClientId.ToString();
            employee_id_textbox.Text = MainWindow.CurrentEmpId.ToString();
            date_of_begin_datepicker.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Метод, который заключает новый контракт аренды и добавляет его в базу и систему
        /// </summary>
        private void add_rental_contract_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal checker;
                if (date_of_begin_datepicker.SelectedDate != null &&
                        date_of_end_datepicker.SelectedDate != null &&
                            String.IsNullOrEmpty(price_per_day_textbox.Text) == false &&
                                decimal.TryParse(price_per_day_textbox.Text, out checker))
                {
                    MainWindow.Connection.Open();
                    SqlCommand cmd = new SqlCommand("insert into [Rental_Contract] (Car_Number, Customer_Id, Employee_Id, Date_Of_Begin, Date_Of_End, Price_Per_Day) " +
                                                    "values (@Car_Number, @Customer_Id, @Employee_Id, @Date_Of_Begin, @Date_Of_End, @Price_Per_Day)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@Car_Number", car_number_textbox.Text);
                    cmd.Parameters.AddWithValue("@Customer_Id", int.Parse(client_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Employee_Id", int.Parse(employee_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Date_Of_Begin", date_of_begin_datepicker.SelectedDate);
                    cmd.Parameters.AddWithValue("@Date_Of_End", date_of_end_datepicker.SelectedDate);
                    cmd.Parameters.AddWithValue("@Price_Per_Day", decimal.Parse(price_per_day_textbox.Text));
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
