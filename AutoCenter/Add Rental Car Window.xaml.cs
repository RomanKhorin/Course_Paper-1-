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
    /// Interaction logic for Add_Rental_Car_Window.xaml
    /// </summary>
    public partial class Add_Rental_Car_Window : Window
    {
        public Add_Rental_Car_Window()
        {
            InitializeComponent();

            MainWindow.Connection.Open();
            var condition_query = new SqlCommand("select * from [Condition]", MainWindow.Connection);
            MainWindow.reader = condition_query.ExecuteReader();
            while (MainWindow.reader.Read())
                condition_combobox.Items.Add(MainWindow.reader.GetInt32(0) + " (" + MainWindow.reader.GetString(1) + ")");
            MainWindow.Connection.Close();

            MainWindow.GetCenters(MainWindow.Connection, center_combobox, MainWindow.reader);
        }

        /// <summary>
        /// Метод, который добавляет машину аренды в систему и базу
        /// </summary>
        private void add_rental_car_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(car_number_textbox.Text) == false &&
                        String.IsNullOrEmpty(firm_textbox.Text) == false &&
                            String.IsNullOrEmpty(model_textbox.Text) == false &&
                                condition_combobox.SelectedItem != null &&
                                    center_combobox.SelectedItem != null &&
                                        InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                {
                    MainWindow.Connection.Open();
                    var cmd = new SqlCommand("insert into [Rental_Car] (Car_Number, Firm, Model, Colour, Engine, Country, Condition_Number, Center_Id) " +
                             "values (@Car_Number, @Firm, @Model, @Colour, @Engine, @Country, @Condition_Number, @Center_Id)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@Car_Number", car_number_textbox.Text);
                    cmd.Parameters.AddWithValue("@Firm", firm_textbox.Text);
                    cmd.Parameters.AddWithValue("@Model", model_textbox.Text);
                    cmd.Parameters.AddWithValue("@Colour", color_textbox.Text);
                    cmd.Parameters.AddWithValue("@Engine", engine_textbox.Text);
                    cmd.Parameters.AddWithValue("@Country", country_textbox.Text);
                    cmd.Parameters.AddWithValue("@Condition_Number", (int.Parse(condition_combobox.SelectedItem.ToString().Split(' ')[0])));
                    cmd.Parameters.AddWithValue("@Center_Id", center_combobox.SelectedItem);
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
