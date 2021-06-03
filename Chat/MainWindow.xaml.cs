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
        CancellationTokenSource cts = new CancellationTokenSource();
        string msg = null;

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            client = DAL.Connect(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1]);
            if (client.Connected)
            {
                var readTask = Task.Factory.StartNew(WaitProducts, cts.Token);
                productsViewSource.Source = readTask.Result;
            }
        }

        private List<Product> WaitProducts()
        {
            do
            {
                if (cts.Token.IsCancellationRequested)
                    return null;

                msg = DAL.Read_msg(client);

                if (msg != null)
                {
                    List<Product> products = (List<Product>)DW.DeserializeFromText<Product>(msg);
                    return products;
                }
                Thread.Sleep(10);
            } while (true);
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cts.Cancel();
            client.Close();
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            Close();
        }
    }
}
