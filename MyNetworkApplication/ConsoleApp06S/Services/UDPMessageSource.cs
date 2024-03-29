﻿using ConsoleApp06S.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp06S.Services
{
    public class UdpMessageSource : IMessageSource
    {
        private readonly UdpClient _udpClient;
        public UdpMessageSource(int portL)
        {
            _udpClient = new UdpClient(portL);
        }

        public NetMessage ReceiveNetMes(ref IPEndPoint ep)
        {
            byte[] data = _udpClient.Receive(ref ep);
            string str = Encoding.UTF8.GetString(data);
            return NetMessage.DeserializeMessageFromJSON(str)?? new NetMessage();
        }
        public async Task SendAsyncNetMes(NetMessage message, IPEndPoint ep)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message.SerializeMessageToJSON());
            await _udpClient.SendAsync(buffer, buffer.Length, ep);
        }
    }
}
