using System;
using System.Collections.Generic;
using System.Text;

namespace StateClient
{
    public sealed class Config
    {
        public const int numRobotsMax = 100;
        public const int numRobotsCreate = 5;

        public const int sendBufferSize = 1024;
        public const int receiveBufferSize = 1024;

        public const string serverIp = "127.0.0.1";
        public const int serverPort = 9900;

        public const uint movingSpeed = 1;

        //robot 每隔 1.0/fps 输入一次操作
        public const double fps = 30;

    }
}
