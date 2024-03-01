using System.Reflection;

namespace IP_Addresses_Calculator
{
    public static class IPAddrCalcLib
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static uint GetTotalNumberOfIPAddresses(IPv4SubnetMask mask)
        {
            uint shifts = 32 - mask.CIDR;

            // Need to add one to take into account the address of the network itself
            return GetMaxNumberForBits(shifts) + 1;
        }

        /// <summary>
        /// The method returns the first IP address of a network
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns>IPv4Address object for the 1st IP address of the network</returns>
        public static IPv4Address GetFirstIPAddress(IPv4Address address, IPv4SubnetMask mask)
        {
            uint firstAddress = address.IPAddress & mask.SubnetMask;

            return new IPv4Address(firstAddress);
        }

        /// <summary>
        /// The method returns the last IP address of the network (the broadcast address)
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns>IPv4Address object representing the broadcast address</returns>
        public static IPv4Address GetLastIPAddress(IPv4Address address, IPv4SubnetMask mask)
        {
            // Getting the first IP address and adding the maximum number of addresses
            IPv4Address lastAddress = GetFirstIPAddress(address, mask);

            uint shifts = 32 - mask.CIDR;
            uint total= GetMaxNumberForBits(shifts);

            lastAddress.IPAddress += total;

            return lastAddress;
        }

        /// <summary>
        /// Method returns default gateway as an IPv4Address object
        /// </summary>
        /// <param name="address">IPv4Address</param>
        /// <param name="mask">IPv4SubnetMask</param>
        /// <returns>Default gateway as an IPv4Address object</returns>
        public static IPv4Address GetDefaultGatewayAddress(IPv4Address address, IPv4SubnetMask mask)
        {
            uint totalAddresses = GetTotalNumberOfIPAddresses(mask);

            switch (totalAddresses)
            {
                case 1:
                    {
                        return address;
                    }
            }

            uint defaultGateway = (address.IPAddress & mask.SubnetMask) + 1;

            return new IPv4Address(defaultGateway);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static IPv4Address GetFirstHostAddress(IPv4Address address, IPv4SubnetMask mask)
        {
            uint firstHostAddress = (address.IPAddress & mask.SubnetMask) + 2;

            return new IPv4Address(firstHostAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static IPv4Address GetLastHostAddress(IPv4Address address, IPv4SubnetMask mask)
        {
            IPv4Address tmpAddr = GetLastIPAddress(address, mask);
            tmpAddr.IPAddress -= 1;

            return tmpAddr;
        }

        /// <summary>
        /// The method returns the maximum number that can be represented by a given number of bits. E.g. 3 bits represent 7, 5 bits 31 and son on
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Maximum number as uint</returns>
        private static uint GetMaxNumberForBits(uint n)
        {
            if (n == 0)
            {
                return 0;
            }
            else
            {
                uint maximumNum = 1;
                for (int i = 0; i < n - 1; i++)
                {
                    maximumNum = (maximumNum << 1) + 1;
                }

                return maximumNum;
            }
        }

        #region Methods to check the class (A/ B/ C/ D/ E) of a network and the subnet mask (A/ B/ C)
        /// <summary>
        /// The function checks the subnet mask provided and returns true if it's network class A mask and else otherwise
        /// </summary>
        /// <param name="mask">IPv4SubnetMask object</param>
        /// <returns>True/ false</returns>
        public static bool IsClassAMask(IPv4SubnetMask mask)
        {
            if (mask.SubnetMask == 0xFF000000) return true;

            return false;
        }

        /// <summary>
        /// The function checks the subnet mask provided and returns true if it's network class B mask and else otherwise
        /// </summary>
        /// <param name="mask">IPv4SubnetMask object</param>
        /// <returns>True/ false</returns>
        public static bool IsClassBMask(IPv4SubnetMask mask)
        {
            if (mask.SubnetMask == 0xFFFF0000) return true;

            return false;
        }

        /// <summary>
        /// The function checks the subnet mask provided and returns true if it's network class C mask and else otherwise
        /// </summary>
        /// <param name="mask">IPv4SubnetMask object</param>
        /// <returns>True/ false</returns>
        public static bool IsClassCMask(IPv4SubnetMask mask)
        {
            if (mask.SubnetMask == 0xFFFFFF00) return true;

            return false;
        }

        /// <summary>
        /// The method returns network class (A/ B/ C/ D/ E) as a string
        /// </summary>
        /// <param name="iPv4Address"></param>
        /// <returns>Char 'A'/ 'B'/ 'C'/ 'D'/ 'E' or information if the combination of the IPv4 address and mask is not valid</returns>
        public static string GetIPv4NetworkClass(IPv4Address iPv4Address, IPv4SubnetMask subnetMask)
        {
            IPv4Address? lowerAddress = new IPv4Address();
            IPv4Address? upperAddress = new IPv4Address();

            // Class A network:
            // IP address: between 1.0.0.0 (0x01000000) and 127.255.255.255 (0x7FFFFFFF)
            // Subnet mask: 255.0.0.0
            lowerAddress = ParseInputIPAddress("1.0.0.0");
            upperAddress = ParseInputIPAddress("127.255.255.255");

            if (((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress)) & IsClassAMask(subnetMask))
            {
                return "A";
            }

            // Class B netwrok:
            // IP address: between 128.0.0.0 (0x80000) and 191.255.255.255 (0xBFFFFFFF)
            // Subnet mask: 255.255.0.0
            lowerAddress = ParseInputIPAddress("128.0.0.0");
            upperAddress = ParseInputIPAddress("191.255.255.255");

            if (((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress)) & IsClassBMask(subnetMask))
            {
                return "B";
            }

            // Class C netwrok:
            // IP address: between 192.0.0.0 (0xC0000) and 223.255.255.255 (0xDFFFFFFF)
            // Subnet mask: 255.255.255.0
            lowerAddress = ParseInputIPAddress("192.0.0.0");
            upperAddress = ParseInputIPAddress("223.255.255.255");

            if (((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress)) & IsClassCMask(subnetMask))
            {
                return "C";
            }

            // Class D netwrok:
            // IP address: between 224.0.0.0 (0xE0000) and 239.255.255.255 (0xEFFFFFFF)
            lowerAddress = ParseInputIPAddress("224.0.0.0");
            upperAddress = ParseInputIPAddress("239.255.255.255");

            if ((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress))
            {
                return "D (multicast address)";
            }

            // Class E netwrok:
            // IP address: between 240.0.0.0 (0xF0000) and 255.255.255.255 (0xFFFFFFFF)
            lowerAddress = ParseInputIPAddress("240.0.0.0");
            upperAddress = ParseInputIPAddress("255.255.255.255");

            if ((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress))
            {
                return "E (epxerimental address)";
            }

            return "No specific class (CIDR notation)";
        }
        #endregion

        #region Methods to check special uses of the IP address
        /// <summary>
        /// Method checks if the address is a loopback address
        /// </summary>
        /// <param name="iPv4Address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool IsLoopbackAddress(IPv4Address iPv4Address, IPv4SubnetMask subnetMask)
        {
            /* Test if the address is a loopback address
             * Range:       127.0.0.0 ~ 127.255.255.255 (127.0.0.0/8)
             * Mask:        255.x.x.x
             * Reference:   RFC6890
             * Note:        The first byte of the mask is 255, the other bytes could be any
             */

            IPv4Address? lowerAddress = ParseInputIPAddress("127.0.0.0");
            IPv4Address? upperAddress = ParseInputIPAddress("127.255.255.255");

            if ((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress) & (subnetMask.FirstByte == 255)) return true;

            return false;
        }

        /// <summary>
        /// Method checks whether the IP address is a private one
        /// </summary>
        /// <param name="iPv4Address">IP address</param>
        /// <param name="SubnetMask">Subnet mask</param>
        /// <returns>"Yes" if the IP address is a private one and "No" otherwise</returns>
        public static bool IsPrivateAddress(IPv4Address iPv4Address)
        {
            /* Test if the address is a private Class A address
             * Range:       10.0.0.0 ~ 10.255.255.255 (10.0.0.0/8)
             * Reference:   RFC 1918
             */

            IPv4Address? lowerAddress = ParseInputIPAddress("10.0.0.0");
            IPv4Address? upperAddress = ParseInputIPAddress("10.255.255.255");

            if ((iPv4Address >= lowerAddress) && (iPv4Address <= upperAddress)) return true;

            /* Test if the address is a private Class B address
             * Range:       172.16.0.0 ~ 172.31.255.255 (172.16/12)
             * Reference:   RFC 1918
            */
            lowerAddress = ParseInputIPAddress("172.16.0.0");
            upperAddress = ParseInputIPAddress("172.31.255.255");

            if ((iPv4Address >= lowerAddress) && (iPv4Address <= upperAddress)) return true;

            /* Test if the address is a private Class C address
             * Range:       192.168.0.0 ~ 192.168.255.255 (192.168/16 prefix)
             * Reference:   RFC 1918
            */

            lowerAddress = ParseInputIPAddress("192.168.0.0");
            upperAddress = ParseInputIPAddress("192.168.255.255");

            if ((iPv4Address >= lowerAddress) && (iPv4Address <= upperAddress)) return true;

            return false;
        }
        #endregion

        #region Methods to parse the IP address and subnet mask provided as strings
        /// <summary>
        /// Method parses the IPv4 address provided
        /// </summary>
        /// <param name="parameterIpAddress">IPv4 address represented as a string</param>
        /// <returns>IPv4Address object fully set if the address has been parsed sucesfuuly or null otherwise</returns>
        public static IPv4Address? ParseInputIPAddress(string parameterIpAddress)
        {
            if (!IsStringValidForProcessing(parameterIpAddress)) return null;

            string[] addressBytes = parameterIpAddress.Split('.');

            // If less than 4 strings were returned, it means the input string was wrong
            if (addressBytes.Length != 4) return null;

            // Temporary array for 4 bytes
            byte[] tmpArray = new byte[4];

            for (int i = 0; i <= 3; i++)
            {
                if (!byte.TryParse(addressBytes[i], out tmpArray[i])) return null;
            }

            return new IPv4Address(tmpArray);
        }

        /// <summary>
        /// Method parses the IPv4 subnet mask provided
        /// </summary>
        /// <param name="parameterSubnetMask">IPv4 subnet mask represented as a string</param>
        /// <returns>IPv4SubnetMask object if the subnet mask has been parsed or null otherwise</returns>
        public static IPv4SubnetMask? ParseSubnetMaskString(string parameterSubnetMask)
        {
            if (!IsStringValidForProcessing(parameterSubnetMask)) return null;

            // If the input string can be converted into a byte, then the mask is in the CIDR format
            if (byte.TryParse(parameterSubnetMask, out byte cidrBits))
            {
                if (cidrBits > 32) return null;

                return new IPv4SubnetMask(cidrBits);
            }
            // Trying to parse the string if it's not in the CIDR format
            else
            {
                // Checking if the mask specified as a string
                string[] maskBytes = parameterSubnetMask.Split('.');
                if (maskBytes.Length != 4) return null;

                // Converting the bytes in the mask. Return null, if convertion is not possible
                byte[] bytesArray = new byte[4];
                for (int i = 0; i <= 3; i++)
                {
                    if (!byte.TryParse(maskBytes[i], out bytesArray[i])) return null;
                }

                // Converting a 4 bytes array into a DEC number
                uint tempMask = 0;
                for (int i = 0; i < 4; i++)
                {
                    tempMask += (uint)(bytesArray[i] << 32 - (i + 1) * 8);
                }

                if (IsSubnetMaskValid(tempMask))
                {
                    return new IPv4SubnetMask(tempMask);
                }
                return null;
            }
        }

        /// <summary>
        /// The method checks whether the subnet mask in a DEC format is valid
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>The method returns true if the mask is valid and false otherwise</returns>
        public static bool IsSubnetMaskValid(uint mask)
        {
            // Shifting the bits to the right until we meet the 1st bit set
            int i = 0;
            while(mask % 2 == 0)
            {
                mask >>= 1;
                i++;
            }

            // Testing every bit whether it's 1 or 0. If the bit is 0, then the mask is not valid
            for (int j = 0; j < 32 - i; j++)
            {
                if ((mask & 0x1) != 1) return false;
                mask >>= 1;
            }

            return true;
        }

        /// <summary>
        /// The method checks whether the string provided not null, empty or consist of spaces
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool IsStringValidForProcessing(string inputString)
        {
            if (string.IsNullOrEmpty(inputString) || string.IsNullOrWhiteSpace(inputString)) return false;

            return true;
        }
        #endregion

        #region Other methods of the program
        /// <summary>
        /// Method prints program usage help
        /// </summary>
        public static void PrintHelp()
        {
            Console.WriteLine($"IP calculator version {GetProgramVersion()} usage:\n");
            Console.WriteLine($"ipaddrcalc 192.168.5.100/24 OR ipaddrcalc 192.168.5.100/255.255.255.0");
            Console.WriteLine($"\nOther options (could be both in upper and lower case):");
            Console.WriteLine($"-v           --> for program version");
            Console.WriteLine($"-u           --> for usage");
            Console.WriteLine($"-h           --> for the full history");
            Console.WriteLine($"-h N         --> for the last N history entries");
            Console.WriteLine($"-s N         --> to show details of the Nth history item");
            Console.WriteLine($"-c           --> to clear the history");
            Console.WriteLine($"-m           --> convert from CIDR to 4 bytes notation and vice versa");
            Console.WriteLine($"For the \"-m\" option use as ipaddrcalc -m 32 OR ipaddrcalc -m 255.255.0.0\n");
        }
        
        /// <summary>
        /// The method prints the error message in red color. After pringing, the original color of the console text is set back
        /// </summary>
        /// <param name="message"></param>
        /// <param name="originalColor"></param>
        public static void PrintErrorMessage(string message, ConsoleColor originalColor)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        public static void PrintExitMessageAndWait()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Methods get version of the program
        /// </summary>
        /// <returns>Program version as a string</returns>
        /// <exception cref="FormatException">Exception is thrown if program version is missing in the assembly</exception>
        public static string GetProgramVersion()
        {
            Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            if (assemblyVersion is null)
            {
                throw new FormatException("The program version is missing in the assembly");
            }
            else
            {
                return assemblyVersion.ToString();
            }
        }
        #endregion
    }
}
