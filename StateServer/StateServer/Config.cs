﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
namespace StateServer
{
    public class Config
    {
         public const int areaCount = 5;
         public const int singleAreaSize = 64;

        /* NetworkManager*/
        public const int numConnectionsMax = 10;
        public const int receiveBufferSize = 1024;
        public const string ip = "127.0.0.1";
        public const int port = 9900;
       

    }
}