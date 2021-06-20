using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Server
{
    public class ServerObj
    {
        //  Кодировка приложения
        public static Encoding defaultEncode = Encoding.UTF8;

        private TcpListener server = null;

        private List<TcpClient> clients = new List<TcpClient>();    //  Список подключенных клиентов

        public ObservableCollection<KeyValuePair<Client, string>> History = new ObservableCollection<KeyValuePair<Client, string>>();

        private ManualResetEvent mre = new ManualResetEvent(true);

        private bool started = false;

        public ServerObj(IPAddress ip, int port)
        {
            //  Инициализация сервера
            server = new TcpListener(ip, port);
            server.Start();
        }

        public void Start()
        {
            mre.Set();
            if (!started)
            {
                ThreadPool.QueueUserWorkItem(WaitConnections);
                ThreadPool.QueueUserWorkItem(ServerListen);
                started = true;
            }
        }

        /// <summary>
        /// Запускает сервер и прослушивает сокет на подключение
        /// </summary>
        public void WaitConnections(object stateInfo)
        {
            while (true)
            {
                mre.WaitOne();
                //Ожидание нового клиента
                if (server.Pending())
                {
                    //Добавление нового клиента
                    TcpClient client = server.AcceptTcpClient();
                    clients.Add(client);

                    //Получение потока данных получаемых от клиента
                    NetworkStream stream = client.GetStream();

                    //Генерация строки для клиента
                    string response = DW.GetXml("products.xml");
                    byte[] data = defaultEncode.GetBytes(response);

                    //Отправляет данные по потоку на клиент
                    stream.Write(data, 0, data.Length);

                    string msg_str = string.Format("[{0}] {1}: {2}", DateTime.Now, client.Client.RemoteEndPoint, "Connected");
                    History.Add(new KeyValuePair<Client, string>(null, msg_str));
                }
                Thread.Sleep(10);
            }
        }

        public void ServerStop()
        {
            mre.Reset();
        }

        private void ServerListen(object stateInfo)
        {
            
            while (true)
            {
                //  В случае, если поступил запрос на остановку задачи, выполнить выход из задачи
                if (clients.Count > 0)
                {
                    clients.AsParallel().ForAll(el =>
                    {
                        mre.WaitOne();
                        NetworkStream ns = el.GetStream();

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
                            string msg_str = string.Format("[{0}] {1}: {2}", DateTime.Now, el.Client.RemoteEndPoint, resultMessage.ToString());
                            History = new ObservableCollection<KeyValuePair<Client, string>>() { new KeyValuePair<Client, string>(null, msg_str) };
                        }
                    });
                }
                Thread.Sleep(10);
            }
        }

        private void CheckClient(TcpClient client)
        {

        }

        /// <summary>
        /// Отправляет сообщение с сервера
        /// </summary>
        /// <param name="msg">Сообщение, которое передаётся</param>
        private void ServerSend(string msg)
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
        }
    }
}
