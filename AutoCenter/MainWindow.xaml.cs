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
        private SqlConnection connection = new SqlConnection("Data Source=ROMAPC\\SQLEXPRESS;Initial Catalog=Car_Center;Integrated Security=True");

        public static SqlConnection Connection { get; set; }

        Add_Client_Window client_window;

        SqlCommand cmd;


        public MainWindow()
        {
            InitializeComponent();
            Connection = connection;

            GetClients(Connection, client_listbox);
            GetEmps(Connection, emps_listbox);
        }

        /// <summary>
        /// Метод, который добавляет клиента в базу и программу
        /// </summary>
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
                        Connection.Open();
                        cmd = new SqlCommand("Insert into [Customer] (First_Name, Last_Name, Birth_Date, Telephone) values" +
                            " (@First_Name, @Last_Name, @Birth_Date, @Telephone)", Connection);
                        cmd.Parameters.AddWithValue("@First_Name", client_window.client_name_textbox.Text);
                        cmd.Parameters.AddWithValue("@Last_Name", client_window.client_lastname_textbox.Text);
                        cmd.Parameters.AddWithValue("@Birth_Date", client_window.client_dateofbirth_datepicker.SelectedDate);
                        cmd.Parameters.AddWithValue("@Telephone", client_window.client_telephone_textbox.Text);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetClients(Connection, client_listbox);
                    }
                    else
                        MessageBox.Show("Check the entered data!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
        }

        // Метод работает, но не удаляет из-за связей в Rental_Contract
        private void delete_client_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client_listbox.SelectedItem != null)
                {
                    int client_id = GetClientId(Connection, client_listbox);
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
                Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
        }

        /// <summary>
        /// Метод, который получает Id клиента по номеру его телефона
        /// </summary>
        private int GetClientId(SqlConnection connection, ListBox listbox)
        {
            int id = 0;
            try
            {
                connection.Open();
                var id_query = new SqlCommand(("Select Customer_Id from [Customer] where Telephone like '" +
                    listbox.SelectedItem.ToString().Split(':')[1]) + "'", connection);
                SqlDataReader reader = id_query.ExecuteReader();
                while (reader.Read())
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

        /// <summary>
        /// Метод, который получает клиентов из базы и добавляет в программу
        /// </summary>
        private void GetClients(SqlConnection connection, ListBox listbox)
        {
            try
            {
                listbox.Items.Clear();
                connection.Open();
                var clients = new SqlCommand(("select First_Name, Last_Name, Birth_Date, Telephone from Customer"), connection);
                SqlDataReader reader = clients.ExecuteReader();
                while (reader.Read())
                    listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " (" + reader.GetDateTime(2).ToShortDateString() + ")" + " :" +
                        reader.GetString(3));

                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
        }

        /// <summary>
        /// Метод, который получает сотрудников центров из базы и добавляет их в программу
        /// </summary>
        private void GetEmps(SqlConnection connection, ListBox listbox)
        {
            try
            {
                listbox.Items.Clear();
                connection.Open();
                var emps = new SqlCommand("select First_Name, Last_Name, Birth_Date, e.Telephone " +  /*Name as Town*/ "from Employee e " +
                                          "inner join Center c on e.Center_Id = c.Center_Id " +
                                          "inner join Town t on c.Town_Id = t.Town_Id", connection);

                SqlDataReader reader = emps.ExecuteReader();
                while (reader.Read())
                    listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " (" + reader.GetDateTime(2).ToShortDateString() + ") :" +
                        reader.GetString(3));
                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
        }

        /// <summary>
        /// Метод, который получает Id сотрудника центра по его номеру телефона
        /// </summary>
        private int GetEmpId(SqlConnection connection, ListBox listbox)
        {
            int emp_id = 0;

            try
            {
                connection.Open();
                var id_query = new SqlCommand(("Select Employee_Id from [Employee] where Telephone like '" +
                    listbox.SelectedItem.ToString().Split(':')[1]) + "'", connection);

                SqlDataReader reader = id_query.ExecuteReader();
                while (reader.Read())
                    emp_id = reader.GetInt32(0);
                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }

            return emp_id;
        }

        /// <summary>
        /// Метод, который добалвляет новых сотрудников в базу и систему
        /// </summary>
        private void add_emp_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_Employee_Window emp_window = new Add_Employee_Window();
                GetCenters(Connection, emp_window.emp_center_combobox);
                bool? result = emp_window.ShowDialog();
                if (result.Value == true)
                {
                    if (String.IsNullOrEmpty(emp_window.emp_name_textbox.Text) == false &&
                            String.IsNullOrEmpty(emp_window.emp_lastname_textbox.Text) == false &&
                                String.IsNullOrEmpty(emp_window.emp_phone_textbox.Text) == false &&
                                    emp_window.emp_datepicker.SelectedDate != null &&
                                        emp_window.emp_center_combobox.SelectedItem != null &&
                                            InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                    {
                        Connection.Open();
                        cmd = new SqlCommand("insert into [Employee] (First_Name, Last_Name, Birth_Date, Center_Id, Telephone) values " +
                            "(@First_Name, @Last_Name, @Birth_Date, @Center_Id, @Telephone)", Connection);
                        cmd.Parameters.AddWithValue("@First_Name", emp_window.emp_name_textbox.Text);
                        cmd.Parameters.AddWithValue("@Last_Name", emp_window.emp_lastname_textbox.Text);
                        cmd.Parameters.AddWithValue("@Birth_Date", emp_window.emp_datepicker.SelectedDate);
                        cmd.Parameters.AddWithValue("@Center_Id", emp_window.emp_center_combobox.SelectedItem);
                        cmd.Parameters.AddWithValue("@Telephone", emp_window.emp_phone_textbox.Text);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetEmps(Connection, emps_listbox);
                    }
                    else
                        MessageBox.Show("Check the entered data!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
        }

        /// <summary>
        /// Метод, который получает центры из базы и добавляет в систему
        /// </summary>
        private void GetCenters(SqlConnection connection, ComboBox combobox)
        {
            try
            {
                combobox.Items.Clear();
                connection.Open();
                var centers = new SqlCommand("select * from [Center]", connection);

                SqlDataReader reader = centers.ExecuteReader();
                while (reader.Read())
                    combobox.Items.Add(reader.GetInt32(0));
                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
        }

        /// <summary>
        /// Метод, который удаляет сотрудника из системы и базы
        /// </summary>
        private void delete_emp_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (emps_listbox.SelectedItem != null)
                {
                    int emp_id = GetEmpId(Connection, emps_listbox);
                    Connection.Open();
                    cmd = new SqlCommand(@"delete from [Employee] where Employee_Id like " + emp_id, Connection);
                    cmd.ExecuteNonQuery();
                    Connection.Close();

                    GetEmps(Connection, emps_listbox);
                }
                else
                    MessageBox.Show("You can't delete nothing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


                /*if (client_listbox.SelectedItem != null)
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
                    MessageBox.Show("You can't delete nothing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error); */
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
        }
    }
}
