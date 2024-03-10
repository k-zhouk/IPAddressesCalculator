namespace IP_Addresses_Calculator
{
    // Class to represent an IPv4 address
    public class IPv4Address
    {
        // Some initializer to get rid of null compiler warnings
        private const uint UINT_INITIALIZER = 100500;

        // Default constructor
        public IPv4Address() { }

        // Constructor for a uint number provided
        public IPv4Address(uint inputValue)
        {
            IPAddress = inputValue;
        }

        // Constructor for an IP address provided as a 4 bytes array
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

        // IP address as an uint. Once is set, it triggers update of other properties of the object
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
        /// The method converts a byte into a BIN string of 8 symbols and adds heading zeros if there are any
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

        // The method returns the IP address as a BIN string
        public string ToBinString() => IPAddressAsBinString;

        private string _IPAddressAsString = string.Empty;
        public string IPAddressAsString
        {
            get => _IPAddressAsString;
            private set
            {
                _IPAddressAsString = value;
            }
        }

        // Overriding the ToString() method, so it returns the IP address in a standard way
        public override string ToString() => IPAddressAsString;

        /*
         * Equals() and GetHash() overriding, otherwise the compiler will emit CS0660 and CS0661 warning for the overloaded operators
         * Logically, 2 IP address objects are the same as they have the same IP address and not pointing to the same object in memory
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
            return IPAddress.GetHashCode();
        }

        // Operators overload to compare 2 IPv4Address
        public static bool operator ==(IPv4Address? left, IPv4Address? right) => left.IPAddress == right.IPAddress;
        public static bool operator !=(IPv4Address? left, IPv4Address? right) => left.IPAddress != right.IPAddress;
        public static bool operator <=(IPv4Address? left, IPv4Address? right) => left.IPAddress <= right.IPAddress;
        public static bool operator >=(IPv4Address? left, IPv4Address? right) => left.IPAddress >= right.IPAddress;
    }

    // Class to represent an IPv4 submask
    public class IPv4SubnetMask
    {
        // Default constructor
        public IPv4SubnetMask() { }

        // Constructor for a uint number provided
        public IPv4SubnetMask(uint inputValue)
        {
            SubnetMask = inputValue;
        }

        // Constructor for a CIDR value provided
        public IPv4SubnetMask(byte cidrBits)
        {
            CIDR = cidrBits;
        }

        // Constructor for an IP address provided as a 4 bytes array
        public IPv4SubnetMask(byte[] inputBytesArray)
        {
            // Cycle to convert a 4 bytes array into a DEC number
            uint tempMask = 0;
            for (int i = 0; i < 4; i++)
            {
                tempMask += (uint)(inputBytesArray[i] << 32 - (i + 1) * 8);
            }

            SubnetMask = tempMask;
        }

        /// <summary>
        /// The method converts the subnet mask represented as an uint into the CIDR notation 
        /// </summary>
        private void UintMaskToCidr()
        {
            // Calculating the number of the CIDR bits by shifting the mask to the right until we find the first 1
            uint tempMask = SubnetMask;

            uint i = 0;
            while (tempMask % 2 == 0)
            {
                tempMask >>= 1;
                i++;
            }

            CIDR = 32 - i;
        }

        /// <summary>
        /// The method converts the subnet mask represented an the CIDR notation into an uint number 
        /// </summary>
        private void CidrToUintMask()
        {
            // MaxValue for the uint is the a 32 bits value
            // The mask need to be shift left to the number of bits equal to the 32-cidrBits value
            SubnetMask = uint.MaxValue << (byte)(32 - CIDR);
        }

        /// <summary>
        /// Private function to split a DEC number into separate bytes
        /// </summary>
        private void SubnetMaskToBytesRepresentation()
        {
            // Processing the bytes
            FirstByte = (byte)((SubnetMask & 0xFF000000) >> 24);
            FirstByteBin = ByteToBinString(FirstByte);

            SecondByte = (byte)((SubnetMask & 0xFF0000) >> 16);
            SecondByteBin = ByteToBinString(SecondByte);

            ThirdByte = (byte)((SubnetMask & 0xFF00) >> 8);
            ThirdByteBin = ByteToBinString(ThirdByte);

            FourthByte = (byte)(SubnetMask & 0xFF);
            FourthByteBin = ByteToBinString(FourthByte);

            SubnetMaskBin = FirstByteBin + '.' + SecondByteBin + '.' + ThirdByteBin + '.' + FourthByteBin;
        }

        /// <summary>
        /// The method convert a byte into a BIN string with heading zeros
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        private string ByteToBinString(byte inputValue)
        {
            string byteAsBin = Convert.ToString(inputValue, 2);
            // Making the byte to be represented as a string of a fixed length of 8 symbols
            return (new string('0', 8 - byteAsBin.Length) + byteAsBin);
        }

        // The property represents the subnet mask as an uint
        uint _subnetMask = default;
        public uint SubnetMask
        {
            get => _subnetMask;
            set
            {
                if (value != _subnetMask)
                {
                    _subnetMask = value;
                    SubnetMaskToBytesRepresentation();
                    UintMaskToCidr();
                }
            }
        }

        // The property represents the subnet mask in the CIDR notation
        uint _cidr = default;
        public uint CIDR
        {
            get => _cidr;
            set
            {
                if (value != _cidr)
                {
                    _cidr = value;
                    CidrToUintMask();
                }
            }
        }

        public byte FirstByte { get; private set; }
        public byte SecondByte { get; private set; }
        public byte ThirdByte { get; private set; }
        public byte FourthByte { get; private set; }

        private string _firstByteBin = string.Empty;
        public string FirstByteBin
        {
            get => _firstByteBin;
            private set
            {
                _firstByteBin = value;
            }
        }

        private string _secondByteBin = string.Empty;
        public string SecondByteBin
        {
            get => _secondByteBin;
            private set
            {
                _secondByteBin = value;
            }
        }

        private string _thirdByteBin = string.Empty;
        public string ThirdByteBin
        {
            get => _thirdByteBin;
            private set
            {
                _thirdByteBin = value;
            }
        }

        private string _fourthByteBin = string.Empty;
        public string FourthByteBin
        {
            get => _fourthByteBin;
            private set
            {
                _fourthByteBin = value;
            }
        }

        private string _subnerMaskBin = string.Empty;
        public string SubnetMaskBin
        {
            get => _subnerMaskBin;
            private set
            {
                _subnerMaskBin = value;
            }
        }

        public string ToBinString() => SubnetMaskBin;

        public override string ToString()
        {
            return FirstByte.ToString() + '.' + SecondByte.ToString() + '.' + ThirdByte.ToString() + '.' + FourthByte.ToString();
        }
    }
}
