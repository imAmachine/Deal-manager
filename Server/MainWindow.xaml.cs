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
        public MainWindow()
        {
            InitializeComponent();
        }

        Encoding defaultEncode = Encoding.UTF8;
        int port = 9009;
        TcpListener server = null;
        List<TcpClient> clients = new List<TcpClient>();

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
            IPAddress localIP = IPAddress.Parse(ip.ToString());

            server = new TcpListener(localIP, port);
            server.Start();

            ServerStart();

            ServerRead();
        }

        private async void ServerRead()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (clients.Count > 0)
                    {
                        clients.AsParallel().ForAll(el =>
                        {
                            NetworkStream ns = el.GetStream();

                            if (ns != null)
                            {
                                if (ns.DataAvailable)
                                {
                                    StringBuilder resultMessage = new StringBuilder();
                                    byte[] buff = null;
                                    while (ns.DataAvailable && ns.CanRead)
                                    {
                                        buff = new byte[64];
                                        ns.Read(buff, 0, buff.Length);
                                        resultMessage.AppendFormat("{0}", defaultEncode.GetString(buff));
                                    }
                                    string msg_str = resultMessage.ToString();
                                    Dispatcher.Invoke((ThreadStart)delegate { tb_log.Text += msg_str; });
                                    ServerSend(msg_str);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Test");
                                el.Dispose();
                                clients.Remove(el);
                            }
                        });
                    }
                    Thread.Sleep(1);
                }
            });
        }

        /// <summary>
        /// Отправляет сообщение с сервера
        /// </summary>
        /// <param name="msg">Сообщение, которое передаётся</param>
        private async void ServerSend(string msg)
        {
            await Task.Run(() =>
            {
                if (clients.Count > 0)
                {
                    byte[] buff = defaultEncode.GetBytes(msg);
                    clients.AsParallel().ForAll(el =>
                    {
                        NetworkStream ns = el.GetStream();
                        ns.Write(buff, 0, buff.Length);
                    });
                }
            });
        }


        /// <summary>
        /// Запускает сервер и прослушивает сокет на подключение
        /// </summary>
        private async void ServerStart()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke((ThreadStart)delegate { tb_log.Text = "Сервер запущен. Ожидание подключений...\n"; });
                while (true)
                {
                    //Ожидание нового клиента
                    if (server.Pending())
                    {
                        //Добавление нового клиента
                        TcpClient client = server.AcceptTcpClient();
                        clients.Add(client);
                        this.Dispatcher.Invoke((ThreadStart)delegate { tb_log.Text = "Подключен клиент. Выполнение запроса...\n"; });
                        Dispatcher.Invoke((ThreadStart)delegate { lb_users.Items.Add(client.Client.RemoteEndPoint.ToString()); });

                        //Получение потока данных получаемых от клиента
                        NetworkStream stream = client.GetStream();

                        //Генерация строки для клиента
                        string response = DataSender();
                        byte[] data = defaultEncode.GetBytes(response);

                        //Отправляет данные по потоку на клиент
                        stream.Write(data, 0, data.Length);
                    }
                    Thread.Sleep(1);
                }
            });
        }

        private string DataSender()
        {
            List<Product> products = new List<Product>();
            using (StreamReader sr = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}/products.xml"))
            {
                return sr.ReadToEnd();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
        }
    }
}
