namespace IP_Addresses_Calculator
{
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

            SubnetMaskAsBinString = FirstByteBin + '.' + SecondByteBin + '.' + ThirdByteBin + '.' + FourthByteBin;
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
        public string SubnetMaskAsBinString
        {
            get => _subnerMaskBin;
            private set
            {
                _subnerMaskBin = value;
            }
        }

        public string ToBinString() => SubnetMaskAsBinString;

        public override string ToString()
        {
            return FirstByte.ToString() + '.' + SecondByte.ToString() + '.' + ThirdByte.ToString() + '.' + FourthByte.ToString();
        }
    }
}
