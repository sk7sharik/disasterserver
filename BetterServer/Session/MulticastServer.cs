﻿using ExeNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BetterServer.Session
{
    /// <summary>
    /// Server used for unimportant packets (Player data)
    /// </summary>
    public class MulticastServer : UdpServer
    {
        protected Server _server;

        public MulticastServer(Server server, int port) : base(port)
        {
            _server = server;
        }

       protected override void OnReady()
        {
            Thread.CurrentThread.Name = $"Server {_server.UID}";
            Logger.LogDiscord($" UDP port {Port}");
       
           base.OnReady();
       }

        protected override void OnSocketError(IPEndPoint endpoint, SocketError error)
        {
            Thread.CurrentThread.Name = $"Server {_server.UID}";
            _server.State.UDPSocketError(endpoint, error);

            base.OnSocketError(endpoint, error);
        }
        protected override void OnError(IPEndPoint endpoint, string message)
        {
            Thread.CurrentThread.Name = $"Server {_server.UID}";
            Logger.LogDiscord($"Caught Error: {message}");

            base.OnError(endpoint, message);
        }

        protected override void OnData(IPEndPoint sender, byte[] data)
       {
           Thread.CurrentThread.Name = $"Server {_server.UID}";
       
           using var stream = new MemoryStream(data);
           using var reader = new BinaryReader(stream);
           _server.State.PeerUDPMessage(_server, sender, reader);
       
           base.OnData(sender, data);
        }
    }
}