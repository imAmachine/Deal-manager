using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerObj server = null;

        public MainWindow()
        {
            InitializeComponent();
            server = new ServerObj(IPAddress.Loopback, 9009);
        }

        private void BtnStartClick(object sender, RoutedEventArgs e)
        {
            server.History.CollectionChanged += History_CollectionChanged;
            server.Start();
        }

        private void History_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            tb_log.Dispatcher.Invoke(() =>
            {
                foreach (KeyValuePair<Client, string> item in e.NewItems)
                    tb_log.Text += "\n" + item.Value;
            });
        }

        private void BtnStopClick(object sender, RoutedEventArgs e)
        {
            server.ServerStop();
        }
    }
}
