namespace IP_Addresses_Calculator
{
    // Class to represent an IPv4 address
    public class IPv4Address
    {
        // Initializer to get rid of null warnings. Not equal to zero in order not to mix up with something like 0.0.0.0 address
        private const uint UINT_INITIALIZER = 100500;

        // Default ctor
        public IPv4Address() { }

        // Ctor for a uint number provided
        public IPv4Address(uint inputValue)
        {
            IPAddress = inputValue;
        }

        // Ctor for an IP address provided as a 4 bytes array
        public IPv4Address(byte[] inputBytesArray)
        {
            // Short cycle to save the IP address as an uint
            uint tempAddress = 0;
            for (int i = 0; i < 4; i++)
            {
                tempAddress += (uint)(inputBytesArray[i] << 32 - (i + 1) * 8);
            }

            IPAddress = tempAddress;
        }

        // IP address as an uint. Once is set, it triggers update of other properties
        private uint _iPAddress = UINT_INITIALIZER;
        public uint IPAddress
        {
            get => _iPAddress;
            set
            {
                if (_iPAddress != value)
                {
                    _iPAddress = value;
                    IPAddressToBytesRepresentation();

                    IPAddressAsString = FirstByte.ToString() + '.' + SecondByte.ToString() + '.' + ThirdByte.ToString() + '.' + FourthByte.ToString();
                }
            }
        }

        /// <summary>
        /// Private function to split a DEC number into separate bytes
        /// </summary>
        private void IPAddressToBytesRepresentation()
        {
            // Processing the bytes
            FirstByte = (byte)((IPAddress & 0xFF000000) >> 24);
            FirstByteAsBinString = ByteToBinString(FirstByte);

            SecondByte = (byte)((IPAddress & 0xFF0000) >> 16);
            SecondByteAsBinString = ByteToBinString(SecondByte);

            ThirdByte = (byte)((IPAddress & 0xFF00) >> 8);
            ThirdByteAsBinString = ByteToBinString(ThirdByte);

            FourthByte = (byte)(IPAddress & 0xFF);
            FourthByteAsBinString = ByteToBinString(FourthByte);

            IPAddressAsBinString = FirstByteAsBinString + '.' + SecondByteAsBinString + '.' + ThirdByteAsBinString + '.' + FourthByteAsBinString;
        }

        /// <summary>
        /// Private method converts a byte into a BIN string of 8 symbols and adds heading zeros, if necessary
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        private string ByteToBinString(byte inputValue)
        {
            string byteAsBin = Convert.ToString(inputValue, 2);

            // Making the byte to be represented as a string of a fixed length of 8 symbols
            return new string('0', 8 - byteAsBin.Length) + byteAsBin;
        }

        #region Single bytes of the IP address
        private byte _firstByte = default;
        public byte FirstByte
        {
            get => _firstByte;
            private set
            {
                if (value != _firstByte) _firstByte = value;
            }
        }

        private byte _secondByte = default;
        public byte SecondByte
        {
            get => _secondByte;
            private set
            {
                if (value != _secondByte) _secondByte = value;
            }
        }

        private byte _thirdByte = default;
        public byte ThirdByte
        {
            get => _thirdByte;
            private set
            {
                if (value != _thirdByte) _thirdByte = value;
            }
        }

        private byte _fourthByte = default;
        public byte FourthByte
        {
            get => _fourthByte;
            private set
            {
                if (value != _fourthByte) _fourthByte = value;
            }
        }
        #endregion

        #region Single bytes of the IP address as BIN strings
        private string _firstByteAsBinString = "00000000";
        public string FirstByteAsBinString { get => _firstByteAsBinString; private set { _firstByteAsBinString = value; } }

        private string _secondByteAsBinString = "00000000";
        public string SecondByteAsBinString { get => _secondByteAsBinString; private set { _secondByteAsBinString = value; } }

        private string _thirdByteAsBinString = "00000000";
        public string ThirdByteAsBinString { get => _thirdByteAsBinString; private set { _thirdByteAsBinString = value; } }

        private string _fourthByteAsBinString = "00000000";
        public string FourthByteAsBinString { get => _fourthByteAsBinString; private set { _fourthByteAsBinString = value; } }
        #endregion

        // Property represents the IP address as a BIN string
        private string _IPAddressAsBinString = string.Empty;
        public string IPAddressAsBinString
        {
            get => _IPAddressAsBinString;
            private set
            {
                _IPAddressAsBinString = value;
            }
        }

        /// <summary>
        /// Method returns the IP address as a BIN string 
        /// </summary>
        /// <returns></returns>
        public string ToBinString() => IPAddressAsBinString;

        private string _IPAddressAsString = string.Empty;
        public string IPAddressAsString
        {
            get => _IPAddressAsString;
            private set => _IPAddressAsString = value;
        }

        // Overriding the ToString() method, so it returns the IP address in a standard way
        public override string ToString() => IPAddressAsString;

        /*
         * Equals() and GetHash() has to be overrided, otherwise the compiler will emit CS0660 and CS0661 warnings for the overloaded operators
         * Logically, 2 IP address objects are the same if they have the same IP address and not pointing to the same object in memory
         */

        public override bool Equals(object? obj)
        {
            if (!(obj is IPv4Address address)) return false;

            if (address.IPAddress == this.IPAddress)
            {
                return true;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Operators overload to compare 2 IPv4Address
        public static bool operator ==(IPv4Address? left, IPv4Address? right) => left.IPAddress == right.IPAddress;
        public static bool operator !=(IPv4Address? left, IPv4Address? right) => left.IPAddress != right.IPAddress;
        public static bool operator <=(IPv4Address? left, IPv4Address? right) => left.IPAddress <= right.IPAddress;
        public static bool operator >=(IPv4Address? left, IPv4Address? right) => left.IPAddress >= right.IPAddress;
    }
}
