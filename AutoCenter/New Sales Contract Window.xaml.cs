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
    /// Interaction logic for New_Sales_Contract_Window.xaml
    /// </summary>
    public partial class New_Sales_Contract_Window : Window
    {
        public New_Sales_Contract_Window()
        {
            InitializeComponent();

            car_id_textbox.Text = MainWindow.CurrentSalesCarId.ToString();
            employee_id_textbox.Text = MainWindow.CurrentEmpId.ToString();
            client_id_textbox.Text = MainWindow.CurrentClientId.ToString();
            date_datepicker.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Метод, который заключает контракт продажи машины и добавляет его в базу данных и систему
        /// после нажатия на соответствующую кнопку в окне
        /// </summary>
        private void add_sales_contract_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal checker;
                if (String.IsNullOrEmpty(price_textbox.Text) == false &&
                        date_datepicker.SelectedDate != null &&
                            decimal.TryParse(price_textbox.Text, out checker))
                {
                    MainWindow.Connection.Open();
                    SqlCommand cmd = new SqlCommand("insert into [Sales_Contract] (Car_Id, Employee_Id, Customer_Id, Date, Price) " +
                                                    "values (@Car_Id, @Employee_Id, @Customer_Id, @Date, @Price)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@Car_Id", int.Parse(car_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Employee_Id", int.Parse(employee_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Customer_Id", int.Parse(client_id_textbox.Text));
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
