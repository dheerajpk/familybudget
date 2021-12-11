using System;
using System.Collections.Generic;
using System.Text;
using SocialDB.Services;

namespace SocialDB.Query
{
    public class Connection
    {
        public DataQuery ContentQuery { get; private set; }

        public Connection(string token, ServiceConnection serviceConnection)
        {
            ContentQuery = new DataQuery(token,serviceConnection);
        }
    }
}
