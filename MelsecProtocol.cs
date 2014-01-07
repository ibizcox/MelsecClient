using System;

namespace Melsec
{
    public abstract class MelsecProtocol
    {
        private ushort lastError = 0;
        private int receiveTimeout = 1000;
        private int sendTimeout = 1000;

        public ushort LastError
        {
            get
            {
                return lastError;
            }
            protected set
            {
                lastError = value;
            }
        }

        public int ReceiveTimeout
        {
            get
            {
                return receiveTimeout;
            }
            set
            {
                if (value > 0)
                    receiveTimeout = value;
                else throw new Exception("Receive timeout must be greater than zero");
            }
        }

        public int SendTimeout
        {
            get
            {
                return sendTimeout;
            }
            set
            {
                if (value > 0)
                    sendTimeout = value;
                else throw new Exception("Send timeout must be greater than zero");
            }
        }

        protected byte[] GetPointBytes(ushort point)
        {
            return GetBytes(point, 3);
        }

        protected byte[] GetPointCount(int count)
        {
            return GetBytes(count, 2);
        }

        protected byte[] GetRequestDataLength(int val)
        {
            return GetBytes(val, 2);
        }

        private byte[] GetBytes(int val, byte cnt)
        {
            byte[] tmp = BitConverter.GetBytes(val);
            byte[] ret = new byte[cnt];
            for (int i = 0; i < tmp.Length; i++)
            {
                ret[i] = tmp[i];
            }
            return ret;
        }

        protected abstract byte[] SendBuffer(byte[] buffer);

        public abstract float ReadReal(ushort point, MelsecDeviceType DeviceType);

        public abstract float[] ReadReal(ushort point, MelsecDeviceType DeviceType, byte count);

        public abstract float[] ReadReal(ushort[] point, MelsecDeviceType DeviceType);

        public abstract void WriteReal(ushort point, float val, MelsecDeviceType DeviceType);

        public abstract uint ReadDword(ushort point, MelsecDeviceType DeviceType);

        public abstract uint[] ReadDword(ushort point, MelsecDeviceType DeviceType, byte count);

        public abstract void WriteDword(ushort point, uint val, MelsecDeviceType DeviceType);

        public abstract ushort ReadWord(ushort point, MelsecDeviceType DeviceType);

        public abstract ushort[] ReadWord(ushort point, MelsecDeviceType DeviceType, byte count);

        public abstract void WriteWord(ushort point, ushort val, MelsecDeviceType DeviceType);

        public abstract bool ReadByte(ushort point, MelsecDeviceType DeviceType);

        public abstract bool[] ReadByte(ushort point, MelsecDeviceType DeviceType, byte count);

        public abstract void WriteByte(ushort point, bool state, MelsecDeviceType DeviceType);
    }
}