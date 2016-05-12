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

        /// <summary>
        /// Строка соединения к базе данных
        /// </summary>
        public static SqlConnection Connection { get; set; }

        SqlCommand cmd;
        public static SqlDataReader reader;

        Rental_Contracts_Window rc_window;
        Sales_Contracts_Window scr_window;

        /// <summary>
        /// Id текущего сотрудника
        /// </summary>
        public static int CurrentEmpId { get; set; }

        /// <summary>
        /// Id текущего клиента
        /// </summary>
        public static int CurrentClientId { get; set; }

        /// <summary>
        /// Текущий центр продаж
        /// </summary>
        public static int CurrentCenter { get; set; }

        /// <summary>
        /// Id текущей машины на продажу
        /// </summary>
        public static int CurrentSalesCarId { get; set; }

        /// <summary>
        /// Номер текущей машины аренды
        /// </summary>
        public static string CurrentRentalCarNumber { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Connection = connection;

            new_rental_contract_button.IsEnabled = false;
            new_sales_contract_button.IsEnabled = false;

            GetClients(Connection, client_listbox, reader);
            GetEmps(Connection, emps_listbox, reader);
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
                    listbox.SelectedItem.ToString().Split(' ')[4]) + "'", connection);
                reader = id_query.ExecuteReader();
                while (reader.Read())
                    id = reader.GetInt32(0);
                reader.Close();
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
        /// Метод, который получает клиентов из базы данных и добавляет в систему
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
                    listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " (" + reader.GetDateTime(2).ToShortDateString() + ") : " +
                        reader.GetString(3));
                reader.Close();

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
        /// Метод, который открывает окно добавления клиента после
        /// нажатия на соответствующую кнопку
        /// </summary>
        private void add_client_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Client_Window client_window = new Add_Client_Window();
            bool? result = client_window.ShowDialog();

            if (result.Value == true)
                GetClients(Connection, client_listbox, reader);
        }

        /// <summary>
        /// Метод, который получает Id выбранного клиента из списка
        /// </summary>
        private void client_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (client_listbox.SelectedItem != null)
                CurrentClientId = GetClientId(Connection, client_listbox, reader);
            else
                CurrentClientId = 0;
        }

        /// <summary>
        /// Метод, который удаляет выбранного клиента из базы данных и системы
        /// после нажатия на соответствующую кнопку в окне
        /// </summary>
        private void delete_client_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client_listbox.SelectedItem != null)
                {
                    if (MessageBox.Show("You are going to delete a client. All the information about rental and sales contracts connected with this client will be deleted as well. Do you want to delete the client?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Connection.Open();
                        cmd = new SqlCommand(@"delete from [Customer] where Customer_Id like " + CurrentClientId, Connection);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetClients(Connection, client_listbox, reader);
                    }
                    else
                        return;
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
        /// Метод, который получает сотрудников центров из базы данных и добавляет их в систему
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
                    listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " (" + reader.GetDateTime(2).ToShortDateString() + ") : " +
                        reader.GetString(3));
                reader.Close();
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
                    listbox.SelectedItem.ToString().Split(' ')[4]) + "'", connection);

                reader = id_query.ExecuteReader();
                while (reader.Read())
                    emp_id = reader.GetInt32(0);
                reader.Close();
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
        /// Метод, который открывает окно добавления нового сотрудника
        /// после нажатия на соответствующую кнопку в окне
        /// </summary>
        private void add_emp_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Employee_Window emp_window = new Add_Employee_Window();

            bool? result = emp_window.ShowDialog();
            if (result.Value == true)
                GetEmps(Connection, emps_listbox, reader);
        }

        /// <summary>
        /// Метод, который удаляет сотрудника из системы и базы данных
        /// после нажатия на соответствующую кнопку в окне
        /// </summary>
        private void delete_emp_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (emps_listbox.SelectedItem != null)
                {
                    if (MessageBox.Show("You are going to delete an employee. All the information about rental and sales contracts connected with this employee will be deleted as well. Do you want to delete the employee?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Connection.Open();
                        cmd = new SqlCommand(@"delete from [Employee] where Employee_Id like " + CurrentEmpId, Connection);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetEmps(Connection, emps_listbox, reader);
                    }
                    else
                        return;
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
        /// Метод, который получает центры из базы данных и добавляет в систему
        /// </summary>
        public static void GetCenters(SqlConnection connection, ComboBox combobox, SqlDataReader reader)
        {
            try
            {
                combobox.Items.Clear();
                connection.Open();
                var centers = new SqlCommand("select * from [Center]", connection);

                reader = centers.ExecuteReader();
                while (reader.Read())
                    combobox.Items.Add(reader.GetInt32(0));
                reader.Close();
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
        /// Метод, который получает машины на продажу из базы данных и заносит их в систему
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
                    listbox.Items.Add(reader.GetString(1) + " " + reader.GetString(2) + " : " + reader.GetString(0));
                reader.Close();
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
        /// Метод, который определяет Id машины на продажу через его VIN (идентификационный номер машины)
        /// </summary>
        private int GetSalesCarId(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            int car_id = 0;

            try
            {
                connection.Open();
                var id_query = new SqlCommand("select Car_Id from [Sales_Car] where VIN like '" + listbox.SelectedItem.ToString().Split(' ')[3] + "'", connection);
                reader = id_query.ExecuteReader();

                while (reader.Read())
                    car_id = reader.GetInt32(0);
                reader.Close();
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
        /// Метод, который определяет центр продаж и аренды для выбранного сотрудника
        /// </summary>
        private int GetCenterId(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            int center = 0;

            try
            {
                connection.Open();
                var center_query = new SqlCommand("select Center_Id from [Employee] where Employee_Id like " + CurrentEmpId, connection);
                reader = center_query.ExecuteReader();
                while (reader.Read())
                    center = reader.GetInt32(0);
                reader.Close();
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
                CurrentCenter = GetCenterId(Connection, emps_listbox, reader);
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
        /// Метод, который открывает окно добавления машины на продажу
        /// после нажатия на соответствующую кнопку в окне
        /// </summary>
        private void add_salesCar_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Sales_Car_Window sc_wind = new Add_Sales_Car_Window();

            bool? result = sc_wind.ShowDialog();
            if (result.Value == true)
                GetSalesCars(Connection, sales_cars_listbox, reader);
        }

        /// <summary>
        /// Определяет Id выбранного автомобиля на продажу
        /// </summary>
        private void sales_cars_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sales_cars_listbox.SelectedItem != null)
            {
                CurrentSalesCarId = GetSalesCarId(Connection, sales_cars_listbox, reader);

                if (client_listbox.SelectedItem != null)
                    new_sales_contract_button.IsEnabled = true;
            }
            else
            {
                new_sales_contract_button.IsEnabled = false;
                CurrentSalesCarId = 0;
            }
        }

        /// <summary>
        /// Метод, который удаляет выбранную машину из базы данных и системы
        /// </summary>
        private void delete_salesCar_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sales_cars_listbox.SelectedItem != null)
                {
                    if (MessageBox.Show("You are going to delete a sales car. All the information about sales contracts connected with this car will be deleted as well. Do you want to delete the car?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Connection.Open();
                        cmd = new SqlCommand(@"delete from [Sales_Car] where Car_Id like " + CurrentSalesCarId, Connection);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetSalesCars(Connection, sales_cars_listbox, reader);
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
                Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
        }

        /// <summary>
        /// Метод, который получает машины для аренды из базы данных и заносит их в систему
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
                    listbox.Items.Add(reader.GetString(1) + " " + reader.GetString(2) + " : " + reader.GetString(0));
                reader.Close();
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
        /// Метод, который открывает окно добавления машины аренды после
        /// нажатия на соответствующую кнопку в окне
        /// </summary>
        private void add_rentalCar_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Rental_Car_Window rc_wind = new Add_Rental_Car_Window();

            bool? result = rc_wind.ShowDialog();
            if (result.Value == true)
                GetRentalCars(Connection, rental_cars_listbox, reader);
        }

        /// <summary>
        /// Метод, который удаляет машину на аренду из базы данных и системы
        /// </summary>
        private void delete_rentalCar_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rental_cars_listbox.SelectedItem != null)
                {
                    if (MessageBox.Show("You are going to delete a rental car. All the information about rental contracts connected with this car will be deleted as well. Do you want to delete the car?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Connection.Open();
                        cmd = new SqlCommand(@"delete from [Rental_Car] where Car_Number like '" + rental_cars_listbox.SelectedItem.ToString().Split(' ')[3] + "'", connection);
                        cmd.ExecuteNonQuery();
                        Connection.Close();

                        GetRentalCars(Connection, rental_cars_listbox, reader);
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
                Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connection.Close();
            }
        }

        /// <summary>
        ///   Метод, который получает контракты продажи из базы данных и заносит их в систему
        /// </summary>
        public static void GetSalesContracts(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            try
            {
                listbox.Items.Clear();

                connection.Open();
                var sales_contracts = new SqlCommand("select Customer.First_Name, Customer.Last_Name, " +
                                                     "Sales_Car.Firm, Sales_Car.Model, " +
                                                     "Date, Price from [Sales_Contract] sc " +
                                                     "inner join [Sales_Car] on sc.Car_Id = Sales_car.Car_Id " +
                                                     "inner join [Customer] on sc.Customer_Id = Customer.Customer_Id", connection);

                reader = sales_contracts.ExecuteReader();

                while (reader.Read())
                    listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + "/" + reader.GetString(2) + " " + reader.GetString(3) + "/" +
                        reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss") + "/" + reader.GetDecimal(5));

                reader.Close();
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
        /// Метод, который забирает контракты аренды машин из базы данных и заносит их в систему
        /// </summary>
        public static void GetRentalContracts(SqlConnection connection, ListBox listbox, SqlDataReader reader)
        {
            try
            {
                listbox.Items.Clear();

                connection.Open();
                var rental_contracts = new SqlCommand("select Customer.First_Name, Customer.Last_Name, " +
                                                      "Rental_Car.Firm, Rental_Car.Model, " +
                                                      "Date_Of_Begin, Date_Of_End, Rental_Contract.Number_of_days, " +
                                                      "Rental_Contract.Price_Per_Day, Total_Price from [Rental_Contract] " +
                                                      "inner join [Customer] on Rental_Contract.Customer_Id = Customer.Customer_Id " +
                                                      "inner join [Rental_Car] on Rental_Contract.Car_Number = Rental_Car.Car_Number", connection);
                reader = rental_contracts.ExecuteReader();
                while (reader.Read())
                    listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + "/" + reader.GetString(2) + " " + reader.GetString(3) + "/" +
                        reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss") + "/" + reader.GetDateTime(5).ToShortDateString() + "/" + reader.GetDecimal(8));

                reader.Close();
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
        /// Метод, который открывает окно со списком контрактов продажи машин
        /// после нажатия на соответствующую кнопку
        /// </summary>
        private void sales_contracts_button_Click(object sender, RoutedEventArgs e)
        {
            scr_window = new Sales_Contracts_Window();
            scr_window.Show();
        }

        /// <summary>
        /// Метод, который открывает окно со списком контраков аренды машин
        /// после нажатия на соответствующую кнопку
        /// </summary>
        private void rental_contracts_button_Click(object sender, RoutedEventArgs e)
        {
            rc_window = new Rental_Contracts_Window();
            rc_window.Show();
        }

        /// <summary>
        /// Метод, который открывает окно добавления контракта продажи после нажатия на соответствующую кнопку
        /// </summary>
        private void new_sales_contract_button_Click(object sender, RoutedEventArgs e)
        {
            New_Sales_Contract_Window nsc_window = new New_Sales_Contract_Window();

            bool? result = nsc_window.ShowDialog();
            if (result.Value == true)
            {
                MessageBox.Show("Contract successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                scr_window = new Sales_Contracts_Window();
                GetSalesContracts(Connection, scr_window.sales_contracts_listbox, reader);
            }
        }

        /// <summary>
        /// Метод, который открывает окно добавления контракта аренды автомобиля после нажатия на соответствующую кнопку
        /// </summary>
        private void new_rental_contract_button_Click(object sender, RoutedEventArgs e)
        {
            New_Rental_Contract_Window nrc_window = new New_Rental_Contract_Window();

            bool? result = nrc_window.ShowDialog();
            if (result.Value == true)
            {
                MessageBox.Show("Contract successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                rc_window = new Rental_Contracts_Window();
                GetRentalContracts(Connection, rc_window.rental_contracts_listbox, reader);
            }
        }

        /// <summary>
        /// Метод, который активирует способность нажимать на кнопку добавления нового контракта аренды
        /// после выбора машины на аренду
        /// </summary>
        private void rental_cars_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rental_cars_listbox.SelectedItem != null)
            {
                CurrentRentalCarNumber = rental_cars_listbox.SelectedItem.ToString().Split(' ')[3];

                if (client_listbox.SelectedItem != null)
                    new_rental_contract_button.IsEnabled = true;
            }
            else
            {
                CurrentRentalCarNumber = "";
                new_rental_contract_button.IsEnabled = false;
            }
        }
    }
}
