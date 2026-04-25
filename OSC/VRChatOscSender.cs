using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace OsuOscVRC.OSC
{
    public class VRChatOscSender : IDisposable
    {
        private readonly UdpClient _udpClient;

        public VRChatOscSender(string host, int port)
        {
            _udpClient = new UdpClient(host, port);
        }

        public void SendChatbox(string message, bool immediate = true)
        {
            if (string.IsNullOrEmpty(message)) return;

            // Address: /chatbox/input\0... (padded to 4 bytes)
            var address = "/chatbox/input";
            var addressBytes = GetAlignedStringBytes(address);

            // Type tags: ,sb\0\0\0 (for string, bool)
            // Wait, standard OSC: 's' for string, 'T' for True, 'F' for False
            var typeTags = immediate ? ",sT" : ",sF";
            var typeTagBytes = GetAlignedStringBytes(typeTags);

            // Argument 1: string
            var stringBytes = GetAlignedStringBytes(message);

            var packet = new List<byte>();
            packet.AddRange(addressBytes);
            packet.AddRange(typeTagBytes);
            packet.AddRange(stringBytes);
            // Argument 2 (bool) is already encoded in the type tag ('T' or 'F'), no payload needed.

            _udpClient.Send(packet.ToArray(), packet.Count);
        }

        public void ClearChatbox()
        {
            SendChatbox(" ", true);
        }

        private byte[] GetAlignedStringBytes(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            int len = bytes.Length;
            int pad = 4 - (len % 4);
            var padded = new byte[len + pad];
            Array.Copy(bytes, padded, len);
            return padded;
        }

        public void Dispose()
        {
            _udpClient?.Dispose();
        }
    }
}
