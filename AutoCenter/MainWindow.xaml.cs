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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void test_button_Click(object sender, RoutedEventArgs e)
        {
            test_listbox.Items.Clear();
            connection.Open();
            var rental_cars = new SqlCommand(("select Firm, Model, Car_Number as [Car number], Colour, Engine, Country, Description"+
                " from Rental_Car rc inner join Condition c on rc.Condition_Number = c.Condition_Number"), connection);
            SqlDataReader reader = rental_cars.ExecuteReader();
            while (reader.Read())
                test_listbox.Items.Add(reader.GetString(0) + " " + reader.GetString(1) + " | " + reader.GetString(2));

            connection.Close();
        
        
        }
    }
}
