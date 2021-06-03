using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class User
    {
        public bool Auth { get; set; }
        public int id { get; set; }
        public TcpClient client { get; set; }

        public User(bool auth, int id, TcpClient client)
        {
            Auth = auth;
            this.id = id;
            this.client = client;
        }

    }
}
