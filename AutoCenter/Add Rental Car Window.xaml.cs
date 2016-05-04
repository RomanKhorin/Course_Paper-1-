using System;
using System.Collections.Generic;
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
        }

        private void add_rental_car_button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
