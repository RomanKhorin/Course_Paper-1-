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
    /// Interaction logic for Add_Sales_Car_Window.xaml
    /// </summary>
    public partial class Add_Sales_Car_Window : Window
    {
        public Add_Sales_Car_Window()
        {
            InitializeComponent();

            MainWindow.GetCenters(MainWindow.Connection, center_combobox, MainWindow.reader);
        }

        /// <summary>
        /// Метод, который добавляет машину продажи в систему и базу
        /// </summary>
        private void add_sales_car_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(firm_textbox.Text) == false &&
                        String.IsNullOrEmpty(model_textbox.Text) == false &&
                            center_combobox.SelectedItem != null &&
                                String.IsNullOrEmpty(VIN_textbox.Text) == false &&
                                    InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                {
                    MainWindow.Connection.Open();
                    var cmd = new SqlCommand("insert into [Sales_Car] (VIN, Firm, Model, Colour, Engine, Country, Center_Id) " +
                            "values (@VIN, @Firm, @Model, @Colour, @Engine, @Country, @Center_Id)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@VIN", VIN_textbox.Text);
                    cmd.Parameters.AddWithValue("@Firm", firm_textbox.Text);
                    cmd.Parameters.AddWithValue("@Model", model_textbox.Text);
                    cmd.Parameters.AddWithValue("@Colour", color_textbox.Text);
                    cmd.Parameters.AddWithValue("@Engine", engine_textbox.Text);
                    cmd.Parameters.AddWithValue("@Country", country_textbox.Text);
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
