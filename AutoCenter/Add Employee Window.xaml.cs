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
    /// Interaction logic for Add_Employee_Window.xaml
    /// </summary>
    public partial class Add_Employee_Window : Window
    {
        public Add_Employee_Window()
        {
            InitializeComponent();

            MainWindow.GetCenters(MainWindow.Connection, emp_center_combobox, MainWindow.reader);
        }

        /// <summary>
        /// Метод, который добавляет новых сотрудников в базу и систему
        /// после нажатия на соответствующую кнопку в окне
        /// </summary>
        private void add_emp_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(emp_name_textbox.Text) == false &&
                        String.IsNullOrEmpty(emp_lastname_textbox.Text) == false &&
                            String.IsNullOrEmpty(emp_phone_textbox.Text) == false &&
                                emp_datepicker.SelectedDate != null &&
                                    emp_center_combobox.SelectedItem != null &&
                                        InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                {
                    MainWindow.Connection.Open();
                    SqlCommand cmd = new SqlCommand("insert into [Employee] (First_Name, Last_Name, Birth_Date, Center_Id, Telephone) values " +
                        "(@First_Name, @Last_Name, @Birth_Date, @Center_Id, @Telephone)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@First_Name", emp_name_textbox.Text);
                    cmd.Parameters.AddWithValue("@Last_Name", emp_lastname_textbox.Text);
                    cmd.Parameters.AddWithValue("@Birth_Date", emp_datepicker.SelectedDate);
                    cmd.Parameters.AddWithValue("@Center_Id", emp_center_combobox.SelectedItem);
                    cmd.Parameters.AddWithValue("@Telephone", emp_phone_textbox.Text);
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
