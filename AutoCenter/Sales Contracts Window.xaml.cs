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
    /// Interaction logic for Sales_Contracts_Window.xaml
    /// </summary>
    public partial class Sales_Contracts_Window : Window
    {
        public Sales_Contracts_Window()
        {
            InitializeComponent();
            MainWindow.GetSalesContracts(MainWindow.Connection, sales_contracts_listbox, MainWindow.reader);
        }

        // ПРИДУМАТЬ ЛОГИКУ УДАЛЕНИЯ КОНТРАКТОВ ПРОДАЖИ
        private void delete_sales_contract_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sales_contracts_listbox.SelectedItem != null)
                {
                    if (MessageBox.Show("Are you sure?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        MainWindow.Connection.Open();
                        var cmd = new SqlCommand(@"delete from [Sales_Contract] where Date = '" +
                                                 sales_contracts_listbox.SelectedItem.ToString().Split('/')[2] + "'", MainWindow.Connection);
                        cmd.ExecuteNonQuery();
                        MainWindow.Connection.Close();

                        MainWindow.GetRentalContracts(MainWindow.Connection, sales_contracts_listbox, MainWindow.reader);
                    }
                    else
                        return;
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
