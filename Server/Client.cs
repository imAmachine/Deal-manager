using System;

namespace Server
{
    [Serializable]
    public class Client
    {
        public int IdClient { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Client()
        {
        }

        public Client(int idClient, string login, string password)
        {
            IdClient = idClient;
            Login = login;
            Password = password;
        }
    }
}
