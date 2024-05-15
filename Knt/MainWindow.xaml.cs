using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;

namespace Knt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void serv_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(name.Text))
            {
                Regex regexx = new Regex(@"^[a-zA-Zа-яА-Я0-9]+$");
                if (regexx.IsMatch(name.Text))
                {
                    Mes Mes = new Mes(name.Text, 1, ipp.Text);
                    Mes.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Некорректный ввод");
                }
            }
            else
            {
                MessageBox.Show("Введите имя");
            }
        }

        private void client_Click(object sender, RoutedEventArgs e)
        {
            if (valid())
            {
                Mes Mes = new Mes(name.Text, 2, ipp.Text);
                Mes.Show();
                this.Close();
            }
        }
        private bool valid()
        {
            IPAddress ip;
            Regex regexx = new Regex(@"^[a-zA-Zа-яА-Я0-9]+$");
            if (!string.IsNullOrWhiteSpace(name.Text) && !string.IsNullOrWhiteSpace(ipp.Text))
            {
                if (!regexx.IsMatch(name.Text) || !IPAddress.TryParse(ipp.Text, out ip))
                {
                    MessageBox.Show("Некорректный ввод");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
                return false;
            }
            return true;
        }
    }
}
