using System;
using System.Windows;
using System.Windows.Resources;
using System.Xml;

namespace Chat
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            App app = new App();
            StreamResourceInfo sri = GetResourceStream(new Uri("userData.xml", UriKind.Relative));

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(sri.Stream);
            // получим корневой элемент
            XmlNodeList xNodes = xDoc.DocumentElement.ChildNodes;

            string isAuth = xNodes[0].InnerText;

            if (Convert.ToBoolean(isAuth))
            {
                MainWindow window = new MainWindow();
                app.Run(window);
            }
            else
            {
                Registration reg = new Registration();
                app.Run(reg);
            }
        }
    }
}
