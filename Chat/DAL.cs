using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public static class DAL
    {
        /// <summary>
        /// Создаёт новое подключение к серверу
        /// </summary>
        /// <param name="host">IP адрес сервера</param>
        /// <returns>TcpClient</returns>
        public static TcpClient Connect(IPAddress host)
        {
            //Подкючаемся к серверу
            TcpClient client = new TcpClient(host.ToString(), 9009);
            //Получаем поток данных с сервера
            return client;
        }

        /// <summary>
        /// Прослушивает поток, и получает сообщение от сервера, как только оно становится доступным
        /// </summary>
        public static string Read_msg(TcpClient client)
        {
            if (client.GetStream().DataAvailable)
            {
                StringBuilder resultMessage = new StringBuilder();

                while (client.GetStream().DataAvailable && client.GetStream().CanRead)
                {
                    byte[] buff = new byte[64];
                    client.GetStream().Read(buff, 0, buff.Length);
                    resultMessage.AppendFormat("{0}", DW.defaultEncode.GetString(buff));
                }
                return resultMessage.ToString();
            }
            return null;
        }

        public static async void Send_Msg(TcpClient client, string message)
        {
            await Task.Run(() =>
            {
                byte[] msg = DW.defaultEncode.GetBytes(message);
                client.GetStream().Write(msg, 0, msg.Length);
            });
        }
    }
}
