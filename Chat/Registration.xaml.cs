using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Xml;

namespace Chat
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load($@"{AppDomain.CurrentDomain.BaseDirectory}/xmls/Registration.xml");

            TcpClient client = DAL.Connect(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1]);
            if (client.Connected)
            {
                DAL.Send_Msg(client, xDoc.InnerXml);
                string answWait = await Task.Run(() => DAL.Read_msg(client));
                MessageBox.Show(answWait);
            }
        }
    }
}
