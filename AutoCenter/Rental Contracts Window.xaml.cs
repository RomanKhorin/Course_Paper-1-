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
    /// Interaction logic for Rental_Contracts_Window.xaml
    /// </summary>
    public partial class Rental_Contracts_Window : Window
    {
        public Rental_Contracts_Window()
        {
            InitializeComponent();
            MainWindow.GetRentalContracts(MainWindow.Connection, rental_contracts_listbox, MainWindow.reader);
        }


        /// НЕ ЗАБУДЬ, ЧТО У ТЕБЯ ЗАКОМЕНЧЕНА ЭТА КНОПКА В XAML
        /// <summary>
        /// Метод, который удаляет выбранный контракт аренды после нажатия на определенную кнопку
        /// </summary>
        private void delete_rental_contract_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rental_contracts_listbox.SelectedItem != null)
                {
                    MainWindow.Connection.Open();
                    var cmd = new SqlCommand(@"delete from [Rental_Contract] where Number_Of_Days like " +
                                             int.Parse(rental_contracts_listbox.SelectedItem.ToString().Split('/')[4]) +
                                             " and Total_Price like " +
                                             decimal.Parse(rental_contracts_listbox.SelectedItem.ToString().Split('/')[6]), MainWindow.Connection);
                    cmd.ExecuteNonQuery();
                    MainWindow.Connection.Close();

                    MainWindow.GetRentalContracts(MainWindow.Connection, rental_contracts_listbox, MainWindow.reader);
                }
                else
                    MessageBox.Show("You can't delete nothing!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
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
