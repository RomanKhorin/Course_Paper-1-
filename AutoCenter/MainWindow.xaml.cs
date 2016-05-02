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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=" + System.IO.Path.GetFullPath("../../DB.mdf") + ";Integrated Security=True;Connect Timeout=30");
        private SqlConnection connection = new SqlConnection("Data Source=ROMAPC\\SQLEXPRESS;Initial Catalog=Car_Center;Integrated Security=True");

        public static SqlConnection Connection { get; set; }

        Add_Client_Window client_window;

        SqlCommand cmd;

        
        public MainWindow()
        {
            InitializeComponent();
            Connection = connection;

            GetClients(Connection, client_listbox);
        }

        private void add_client_button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client_window = new Add_Client_Window();
                bool? result = client_window.ShowDialog();

                if (result.Value == true)
                {
                    if (String.IsNullOrEmpty(client_window.client_name_textbox.Text) == false &&
                            String.IsNullOrEmpty(client_window.client_lastname_textbox.Text) == false &&
                                client_window.client_dateofbirth_datepicker.SelectedDate != null &&
                                    InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                    {
                        MainWindow.Connection.Open();
                        cmd = new SqlCommand("Insert into [Customer] (First_Name, Last_Name, Birth_Date) values" +
                            " (@First_Name, @Last_Name, @Birth_Date)", Connection);
                        cmd.Parameters.AddWithValue("@First_Name", client_window.client_name_textbox.Text);
                        cmd.Parameters.AddWithValue("@Last_Name", client_window.client_lastname_textbox.Text);
                        cmd.Parameters.AddWithValue("@Birth_Date", client_window.client_dateofbirth_datepicker.SelectedDate);
                        cmd.ExecuteNonQuery();
                        MainWindow.Connection.Close();

                        GetClients(Connection, client_listbox);
                    }
                    else
                        MessageBox.Show("Check the entered data!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

       // Метод работает, но не удаляет из-за связей в Rental_Contract
        private void delete_client_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client_listbox.SelectedItem != null)
                {
                    int client_id = GetClientId(Connection, client_listbox.SelectedItem.ToString().Split(' ')[0],
                        client_listbox.SelectedItem.ToString().Split(' ')[1]);
                    Connection.Open();
                    cmd = new SqlCommand(@"delete from [Customer] where Customer_Id like " + client_id, Connection);
                    cmd.ExecuteNonQuery();
                    Connection.Close();

                    GetClients(Connection, client_listbox);
                }
                else
                    MessageBox.Show("You can't delete nothing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private int GetClientId(SqlConnection connection, string name, string last_name)
        {
            int id = 0;
            try
            {
                connection.Open();
                var id_query = new SqlCommand(("Select Customer_Id from [Customer] where First_Name like '"+
                    client_listbox.SelectedItem.ToString().Split(' ')[0]+"' and Last_Name like '"+
                    client_listbox.SelectedItem.ToString().Split(' ')[1]+"'"), connection);
                
                SqlDataReader reader = id_query.ExecuteReader();

                while(reader.Read())
                    id = reader.GetInt32(0);
                
                connection.Close();
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

            return id;
        }

        private void GetClients(SqlConnection connection, ListBox listbox)
        {
            listbox.Items.Clear();
            connection.Open();
            var clients = new SqlCommand(("select First_Name, Last_Name, Birth_Date from Customer"), connection);
            SqlDataReader reader = clients.ExecuteReader();
            while (reader.Read())
                listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " (" + reader.GetDateTime(2).ToShortDateString() + ")");

            connection.Close();
        }
    }
}
