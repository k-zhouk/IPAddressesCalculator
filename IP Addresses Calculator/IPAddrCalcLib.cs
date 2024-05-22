namespace IP_Addresses_Calculator
{
    public static class IPAddrCalcLib
    {
        /// <summary>
        /// The method returns the network part of the IP address
        /// </summary>
        /// <param name="address">IPv4Address object</param>
        /// <param name="mask">IPv4SubnetMask object</param>
        /// <returns>Network part of the IP address as an uint number</returns>
        public static uint GetNetworkPart(IPv4Address address, IPv4SubnetMask mask)
        {
            uint networkPart = address.IPAddress & mask.SubnetMask;

            // Getting the network part
            while (networkPart % 2 == 0)
            {
                networkPart = networkPart >> 1;
            }
            return networkPart;
        }

        /// <summary>
        /// The method returns total number of IP addresses in the network
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="mask">Subnet mask</param>
        /// <returns>Number of IP addresses in the netwrok as uint</returns>
        public static uint GetTotalNumberOfIPAddresses(IPv4SubnetMask? mask)
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
            uint total = GetMaxNumberForBits(shifts);

            lastAddress.IPAddress += total;

            return lastAddress;
        }

        /// <summary>
        /// The method returns default gateway as an IPv4Address object
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
        /// The method returns an address of the 1st host in the network
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns>The host address is returned as an IPv4Address object</returns>
        public static IPv4Address GetFirstHostAddress(IPv4Address address, IPv4SubnetMask mask)
        {
            uint firstHostAddress = (address.IPAddress & mask.SubnetMask) + 2;

            return new IPv4Address(firstHostAddress);
        }

        /// <summary>
        /// The method resturns an address of the last host in the network
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mask"></param>
        /// <returns>The host address is returned as an IPv4Address object</returns>
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
        /// The function checks whether a subnet mask is the class A mask
        /// </summary>
        /// <param name="mask">IPv4SubnetMask object</param>
        /// <returns>True/ false</returns>
        public static bool IsClassAMask(IPv4SubnetMask? mask)
        {
            if (mask is not null)
            {
                if (mask.SubnetMask == 0xFF000000) return true; // It's more convinient to have the mask in the HEX format
            }
            return false;
        }

        /// <summary>
        /// The function checks whether a subnet mask is the class B mask
        /// </summary>
        /// <param name="mask">IPv4SubnetMask object</param>
        /// <returns>True/ false</returns>
        public static bool IsClassBMask(IPv4SubnetMask? mask)
        {
            if (mask is not null)
            {
                if (mask.SubnetMask == 0xFFFF0000) return true;
            }
            return false;
        }

        /// <summary>
        /// The function checks whether a subnet mask is the class C mask
        /// </summary>
        /// <param name="mask">IPv4SubnetMask object</param>
        /// <returns>True/ false</returns>
        public static bool IsClassCMask(IPv4SubnetMask? mask)
        {
            if (mask is not null)
            {
                if (mask.SubnetMask == 0xFFFFFF00) return true;
            }
            return false;
        }

        /// <summary>
        /// The method returns network class (A/ B/ C/ D/ E) as a one letter string
        /// </summary>
        /// <param name="iPv4Address"></param>
        /// <returns>Char "A"/ "B"/ "C"/ "D"/ "E" or information if the combination of the IPv4 address and mask is not valid</returns>
        public static string GetIPv4NetworkClass(IPv4Address? iPv4Address, IPv4SubnetMask? subnetMask)
        {
            // Class A network:
            // IP address: between 1.0.0.0 (0x01000000) and 127.255.255.255 (0x7FFFFFFF)
            // Subnet mask: 255.0.0.0
            IPv4Address? lowerAddress = ParseInputIPAddress("1.0.0.0");
            IPv4Address? upperAddress = ParseInputIPAddress("127.255.255.255");

            if ((lowerAddress is not null) && (upperAddress is not null))
            {
                if ((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress) & IsClassAMask(subnetMask))
                {
                    return "A";
                }
            }

            // Class B netwrok:
            // IP address: between 128.0.0.0 (0x80000) and 191.255.255.255 (0xBFFFFFFF)
            // Subnet mask: 255.255.0.0
            lowerAddress = ParseInputIPAddress("128.0.0.0");
            upperAddress = ParseInputIPAddress("191.255.255.255");

            if ((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress) & IsClassBMask(subnetMask))
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
                return "D";
            }

            // Class E netwrok:
            // IP address: between 240.0.0.0 (0xF0000) and 255.255.255.255 (0xFFFFFFFF)
            lowerAddress = ParseInputIPAddress("240.0.0.0");
            upperAddress = ParseInputIPAddress("255.255.255.255");

            if ((iPv4Address >= lowerAddress) & (iPv4Address <= upperAddress))
            {
                return "E";
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
        public static bool IsPrivateAddress(IPv4Address iPv4Address, IPv4SubnetMask subnetMask)
        {
            /* 
             * Test if the address is a private Class A address
             * Range:       10.0.0.0 ~ 10.255.255.255 (10/8)
             * Reference:   RFC 1918
             */


            IPv4Address? lowerAddress = ParseInputIPAddress("10.0.0.0");
            IPv4Address? upperAddress = ParseInputIPAddress("10.255.255.255");

            if ((iPv4Address >= lowerAddress) && (iPv4Address <= upperAddress) && (subnetMask.CIDR >= 8)) return true;

            /* 
             * Test if the address is a private Class B address
             * Range:       172.16.0.0 ~ 172.31.255.255 (172.16/12)
             * Reference:   RFC 1918
            */
            lowerAddress = ParseInputIPAddress("172.16.0.0");
            upperAddress = ParseInputIPAddress("172.31.255.255");

            if ((iPv4Address >= lowerAddress) && (iPv4Address <= upperAddress) && (subnetMask.CIDR >= 12)) return true;

            /* 
             * Test if the address is a private Class C address
             * Range:       192.168.0.0 ~ 192.168.255.255 (192.168/16 prefix)
             * Reference:   RFC 1918
            */

            lowerAddress = ParseInputIPAddress("192.168.0.0");
            upperAddress = ParseInputIPAddress("192.168.255.255");

            if ((iPv4Address >= lowerAddress) && (iPv4Address <= upperAddress) && (subnetMask.CIDR >= 16)) return true;

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
        /// <param name="mask">Mask as an uint number</param>
        /// <returns>True/ false</returns>
        public static bool IsSubnetMaskValid(uint mask)
        {
            // Shifting the bits to the right until we meet the 1st bit set
            int k = 0;
            for (int i = 0; i < 32; i++)
            {
                if (mask % 2 != 0) break;
                mask >>= 1;
                k++;
            }

            // Testing every bit whether it's 1 or 0. If the bit is 0, then the mask is not valid
            for (int j = 0; j < 32 - k; j++)
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
    }
}
