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
    /// Interaction logic for Add_Client_Window.xaml
    /// </summary>
    public partial class Add_Client_Window : Window
    {
        public Add_Client_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод, который добавляет клиента в базу и программу
        /// </summary>
        private void add_client_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(client_name_textbox.Text) == false &&
                        String.IsNullOrEmpty(client_lastname_textbox.Text) == false &&
                            client_dateofbirth_datepicker.SelectedDate != null &&
                                InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                {
                    MainWindow.Connection.Open();
                    SqlCommand cmd = new SqlCommand("Insert into [Customer] (First_Name, Last_Name, Birth_Date, Telephone) values" +
                        " (@First_Name, @Last_Name, @Birth_Date, @Telephone)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@First_Name", client_name_textbox.Text);
                    cmd.Parameters.AddWithValue("@Last_Name", client_lastname_textbox.Text);
                    cmd.Parameters.AddWithValue("@Birth_Date", client_dateofbirth_datepicker.SelectedDate);
                    cmd.Parameters.AddWithValue("@Telephone", client_telephone_textbox.Text);
                    cmd.ExecuteNonQuery();
                    MainWindow.Connection.Close();

                    DialogResult = true;
                }
                else
                    MessageBox.Show("Check the entered data!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
