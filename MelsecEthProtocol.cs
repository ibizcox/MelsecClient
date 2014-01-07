using System;
using System.Net;
using System.Net.Sockets;

namespace Melsec
{
    public abstract class MelsecEthProtocol : MelsecProtocol
    {
        private IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
        private readonly int ErrorCodePosition;
        private readonly int MinResponseLength;
        protected readonly int ReturnValuePosition;
        private readonly byte ReturnPacketHeader;

        protected MelsecEthProtocol(string ip, ushort port, int errorCodePosition, int minResponseLength, int returnValuePosition, byte returnPacketHeader)
        {
            ipep = new IPEndPoint(IPAddress.Parse(ip), port);
            ErrorCodePosition = errorCodePosition;
            MinResponseLength = minResponseLength;
            ReturnValuePosition = returnValuePosition;
            ReturnPacketHeader = returnPacketHeader;
        }

        public string Ip
        {
            get
            {
                return ipep.Address.ToString();
            }
            set
            {
                ipep.Address = IPAddress.Parse(value);
            }
        }

        public ushort Port
        {
            get
            {
                return (ushort)ipep.Port;
            }
            set
            {
                if (value > 0)
                    ipep.Port = value;
                else throw new Exception("Port number must be greater than zero");
            }
        }

        public abstract void ErrLedOff();

        protected override byte[] SendBuffer(byte[] buffer)
        {
            using (UdpClient uc = new UdpClient())
            {
                byte[] buff = new byte[0];
                uc.Client.SendTimeout = SendTimeout;
                uc.Client.ReceiveTimeout = ReceiveTimeout;
                uc.Connect(ipep);
                uc.Send(buffer, buffer.Length);
                buff = uc.Receive(ref ipep);
                if (buff.Length > MinResponseLength)
                {
                    if (buff[0] != ReturnPacketHeader)
                        throw new Exception(string.Format("Response header PLC is corrupt: {0} <> {1}",
                                                                        ReturnPacketHeader, buff[0]));
                    LastError = BitConverter.ToUInt16(buff, ErrorCodePosition);
                    if (LastError != 0)
                        throw new Exception(string.Format("PLC return error code: {0}", LastError));
                }
                else throw new Exception(string.Format("PLC returned buffer is too small: {0}", buff.Length));
                uc.Close();
                return buff;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Ip, Port);
        }
    }
}