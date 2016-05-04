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


        SqlCommand cmd;
        SqlDataReader reader;

        /// <summary>
        /// Id текущего сотрудника
        /// </summary>
        public int CurrentEmpId { get; set; }

        /// <summary>
        /// Текущий центр продаж
        /// </summary>
        public int CurrentCenter { get; set; }

        /// <summary>
        /// Id текущей машины на продажу
        /// </summary>
        public int CurrentSalesCarId { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Connection = connection;

            GetClients(Connection, client_listbox, reader);
            GetEmps(Connection, emps_listbox, reader);
        }

        /// <summary>
        /// Метод, который добавляет клиента в базу и программу
        /// </summary>
        private void add_client_button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_Client_Window client_window = new Add_Client_Window();
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

                        GetClients(Connection, client_listbox, reader);
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
                    int client_id = GetClientId(Connection, client_listbox, reader);
                    Connection.Open();
                    cmd = new SqlCommand(@"delete from [Customer] where Customer_Id like " + client_id, Connection);
                    cmd.ExecuteNonQuery();
                    Connection.Close();

                    GetClients(Connection, client_listbox, reader);
                }
                else
                    MessageBox.Show("You can't delete nothing!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private int GetClientId(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            int id = 0;
            try
            {
                connection.Open();
                var id_query = new SqlCommand(("Select Customer_Id from [Customer] where Telephone like '" +
                    listbox.SelectedItem.ToString().Split(':')[1]) + "'", connection);
                reader = id_query.ExecuteReader();
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
        private void GetClients(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            try
            {
                listbox.Items.Clear();
                connection.Open();
                var clients = new SqlCommand(("select First_Name, Last_Name, Birth_Date, Telephone from Customer"), connection);
                reader = clients.ExecuteReader();
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
        private void GetEmps(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            try
            {
                listbox.Items.Clear();
                connection.Open();
                var emps = new SqlCommand("select First_Name, Last_Name, Birth_Date, e.Telephone " +  /*Name as Town*/ "from Employee e " +
                                          "inner join Center c on e.Center_Id = c.Center_Id " +
                                          "inner join Town t on c.Town_Id = t.Town_Id", connection);

                reader = emps.ExecuteReader();
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
        private int GetEmpId(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            int emp_id = 0;

            try
            {
                connection.Open();
                var id_query = new SqlCommand(("Select Employee_Id from [Employee] where Telephone like '" +
                    listbox.SelectedItem.ToString().Split(':')[1]) + "'", connection);

                reader = id_query.ExecuteReader();
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
                GetCenters(Connection, emp_window.emp_center_combobox, reader);
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

                        GetEmps(Connection, emps_listbox, reader);
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
        private void GetCenters(SqlConnection connection, ComboBox combobox, SqlDataReader reader)
        {
            try
            {
                combobox.Items.Clear();
                connection.Open();
                var centers = new SqlCommand("select * from [Center]", connection);

                reader = centers.ExecuteReader();
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
                    int emp_id = GetEmpId(Connection, emps_listbox, reader);
                    Connection.Open();
                    cmd = new SqlCommand(@"delete from [Employee] where Employee_Id like " + emp_id, Connection);
                    cmd.ExecuteNonQuery();
                    Connection.Close();

                    GetEmps(Connection, emps_listbox, reader);
                }
                else
                    MessageBox.Show("You can't delete nothing!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Метод, который получает машины для аренды из базы и заносит их в систему
        /// </summary>
        private void GetRentalCars(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            try
            {
                listbox.Items.Clear();
                connection.Open();
                var rental_cars = new SqlCommand("select Car_Number, Firm, Model from [Rental_Car] where Center_Id like " + CurrentCenter, connection);
                reader = rental_cars.ExecuteReader();

                while (reader.Read())
                    listbox.Items.Add(reader.GetString(0) + " (" + reader.GetString(1) + " " + reader.GetString(2) + ")");
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
        /// Метод, который получает машины на продажу из базы и заносит их в систему
        /// </summary>
        private void GetSalesCars(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            try
            {
                listbox.Items.Clear();
                connection.Open();
                var sales_cars = new SqlCommand("select VIN, Firm, Model from [Sales_Car] where Center_Id like " + CurrentCenter, connection);
                reader = sales_cars.ExecuteReader();

                while (reader.Read())
                    listbox.Items.Add(reader.GetString(0) + " (" + reader.GetString(1) + " " + reader.GetString(2) + ")");

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
        /// Метод, который определяет центр продаж и аренды для выбранного сотрудника
        /// </summary>
        private int GetCenter(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            int center = 0;

            try
            {
                connection.Open();
                var center_query = new SqlCommand("select Center_Id from [Employee] where Employee_Id like " + CurrentEmpId, connection);
                reader = center_query.ExecuteReader();
                while (reader.Read())
                    center = reader.GetInt32(0);
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

            return center;
        }

        /// <summary>
        /// Метод, который определяет Id выбранного сотрудника и текущий центр продаж и аренды
        /// </summary>
        private void emps_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (emps_listbox.SelectedItem != null)
            {
                CurrentEmpId = GetEmpId(Connection, emps_listbox, reader);
                CurrentCenter = GetCenter(Connection, emps_listbox, reader);
                GetRentalCars(Connection, rental_cars_listbox, reader);
                GetSalesCars(Connection, sales_cars_listbox, reader);
            }
            else
            {
                CurrentEmpId = 0;
                CurrentCenter = 0;
            }
        }

        /// <summary>
        /// Метод, который добавляет машину на продажу в систему и базу
        /// </summary>
        private void add_salesCar_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_Sales_Car_Window sc_wind = new Add_Sales_Car_Window();

                GetCenters(Connection, sc_wind.center_combobox, reader);

                bool? result = sc_wind.ShowDialog();
                if (result.Value == true)
                {
                    if (String.IsNullOrEmpty(sc_wind.firm_textbox.Text) == false &&
                            String.IsNullOrEmpty(sc_wind.model_textbox.Text) == false &&
                                sc_wind.center_combobox.SelectedItem != null &&
                                    String.IsNullOrEmpty(sc_wind.VIN_textbox.Text) == false &&
                                        InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                    {
                        Connection.Open();
                        cmd = new SqlCommand("insert into [Sales_Car] (VIN, Firm, Model, Colour, Engine, Country, Center_Id) " +
                                "values (@VIN, @Firm, @Model, @Colour, @Engine, @Country, @Center_Id)", Connection);
                        cmd.Parameters.AddWithValue("@VIN", sc_wind.VIN_textbox.Text);
                        cmd.Parameters.AddWithValue("@Firm", sc_wind.firm_textbox.Text);
                        cmd.Parameters.AddWithValue("@Model", sc_wind.model_textbox.Text);
                        cmd.Parameters.AddWithValue("@Colour", sc_wind.color_textbox.Text);
                        cmd.Parameters.AddWithValue("@Engine", sc_wind.engine_textbox.Text);
                        cmd.Parameters.AddWithValue("@Country", sc_wind.country_textbox.Text);
                        cmd.Parameters.AddWithValue("@Center_Id", sc_wind.center_combobox.SelectedItem);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetSalesCars(Connection, sales_cars_listbox, reader);
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
        /// Метод, который определяет Id машины на продажу через его VIN (идентификационный номер машины)
        /// </summary>
        private int GetSalesCarId(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            int car_id = 0;

            try
            {
                connection.Open();
                var id_query = new SqlCommand("select Car_Id from [Sales_Car] where VIN like '" + listbox.SelectedItem.ToString().Split(' ')[0] + "'", connection);
                reader = id_query.ExecuteReader();

                while (reader.Read())
                    car_id = reader.GetInt32(0);
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

            return car_id;
        }

        /// <summary>
        /// Определяет Id выбранного автомобиля на продажу
        /// </summary>
        private void sales_cars_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sales_cars_listbox.SelectedItem != null)
                CurrentSalesCarId = GetSalesCarId(Connection, sales_cars_listbox, reader);
            else
                CurrentSalesCarId = 0;
        }

        /// <summary>
        /// Метод, который удаляет выбранную машину из базы и системы
        /// </summary>
        private void delete_salesCar_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sales_cars_listbox.SelectedItem != null)
                {
                    Connection.Open();
                    cmd = new SqlCommand(@"delete from [Sales_Car] where Car_Id like " + CurrentSalesCarId, Connection);
                    cmd.ExecuteNonQuery();
                    Connection.Close();

                    GetSalesCars(Connection, sales_cars_listbox, reader);
                }
                else
                    MessageBox.Show("You can't delete nothing!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Метод, который добавляет машину аренды в базу и систему
        /// </summary>
        private void add_rentalCar_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Add_Rental_Car_Window rc_wind = new Add_Rental_Car_Window();

                Connection.Open();
                var condition_query = new SqlCommand("select * from [Condition]", Connection);
                reader = condition_query.ExecuteReader();
                while (reader.Read())
                    rc_wind.condition_combobox.Items.Add(reader.GetInt32(0) + " (" + reader.GetString(1) + ")");
                Connection.Close();

                GetCenters(Connection, rc_wind.center_combobox, reader);

                bool? result = rc_wind.ShowDialog();
                if (result.Value == true)
                {
                    if (String.IsNullOrEmpty(rc_wind.car_number_textbox.Text) == false &&
                            String.IsNullOrEmpty(rc_wind.firm_textbox.Text) == false &&
                                String.IsNullOrEmpty(rc_wind.model_textbox.Text) == false &&
                                    rc_wind.condition_combobox.SelectedItem != null &&
                                        rc_wind.center_combobox.SelectedItem != null &&
                                            InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                    {
                        Connection.Open();
                        cmd = new SqlCommand("insert into [Rental_Car] (Car_Number, Firm, Model, Colour, Engine, Country, Condition_Number, Center_Id) " +
                                 "values (@Car_Number, @Firm, @Model, @Colour, @Engine, @Country, @Condition_Number, @Center_Id)", Connection);
                        cmd.Parameters.AddWithValue("@Car_Number", rc_wind.car_number_textbox.Text);
                        cmd.Parameters.AddWithValue("@Firm", rc_wind.firm_textbox.Text);
                        cmd.Parameters.AddWithValue("@Model", rc_wind.model_textbox.Text);
                        cmd.Parameters.AddWithValue("@Colour", rc_wind.color_textbox.Text);
                        cmd.Parameters.AddWithValue("@Engine", rc_wind.engine_textbox.Text);
                        cmd.Parameters.AddWithValue("@Country", rc_wind.country_textbox.Text);
                        cmd.Parameters.AddWithValue("@Condition_Number", (int.Parse(rc_wind.condition_combobox.SelectedItem.ToString().Split(' ')[0])));
                        cmd.Parameters.AddWithValue("@Center_Id", rc_wind.center_combobox.SelectedItem);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetRentalCars(Connection, rental_cars_listbox, reader);
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
        /// Метод, который удаляет машину на аренду из базы и системы
        /// </summary>
        private void delete_rentalCar_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rental_cars_listbox.SelectedItem != null)
                {
                    Connection.Open();
                    cmd = new SqlCommand(@"delete from [Rental_Car] where Car_Number like '" + rental_cars_listbox.SelectedItem.ToString().Split(' ')[0] + "'", connection);
                    cmd.ExecuteNonQuery();
                    Connection.Close();

                    GetRentalCars(Connection, rental_cars_listbox, reader);
                }
                else
                    MessageBox.Show("You can't delete nothing!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
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
