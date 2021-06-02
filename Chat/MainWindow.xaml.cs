using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Threading;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollectionViewSource productsViewSource;

        public MainWindow()
        {
            InitializeComponent();
            productsViewSource = (CollectionViewSource)FindResource(nameof(productsViewSource));
        }

        TcpClient client = null;
        string msg = null;

        private async void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            client = DAL.Connect(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1]);
            if (client.Connected)
                await Task.Run(() => {
                    do
                    {
                        msg = DAL.Read_msg(client);
                        if (msg != null)
                        {
                            List<Product> products = DW.Deserialize(msg, "Products");
                            msg = null;
                            Dispatcher.Invoke(() => productsViewSource.Source = products);
                        }
                        Thread.Sleep(1);
                    } while (true);
                });
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            Close();
        }
    }
}
