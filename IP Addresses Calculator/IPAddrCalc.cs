using static IP_Addresses_Calculator.IPAddrCalcLib;
using static IP_Addresses_Calculator.IPAddrCalcHistory;

namespace IP_Addresses_Calculator
{
    internal class IPAddrCalc
    {
        #region Constants section
        private const int INFO_ALIGN = -45;
        #endregion

        static void Main(string[] args)
        {
            // Save original console text color
            ConsoleColor consoleOriginalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nNo warranties provided. Use at your own risk");
            Console.ForegroundColor = consoleOriginalColor;

            Console.WriteLine("\n******************** IP Address Calculator (ver. {0}) ********************\n", GetProgramVersion());

            // Parsing the command line arguments
            // Case 1: no arguments
            if (args.Length == 0)
            {
                PrintErrorMessage("No arguments have been provided. See help for usage\n", consoleOriginalColor);
                PrintHelp();
                Environment.Exit(0);
            }

            IPv4Address? iPv4Address = new();
            IPv4SubnetMask? subnetMask = new();

            // Case 2: 1 argument
            if (args.Length == 1)
            {
                string argParameter = args[0].ToUpper();

                if (argParameter[0] == '-')
                {
                    switch (argParameter)
                    {
                        // Option shows the version of the program
                        case "-V":
                            {
                                Console.WriteLine("IP Address Calculator version: {0}", GetProgramVersion());
                                Environment.Exit(0);
                                break;
                            }
                        // Option prints usage help
                        case "-U":
                            {
                                PrintHelp();
                                Environment.Exit(0);
                                break;
                            }
                        // Option clears the usage history
                        case "-C":
                            {
                                ClearHistory();
                                Environment.Exit(0);
                                break;
                            }
                        // Options displays the usage history
                        case "-H":
                            {
                                ShowHistory(0);
                                Environment.Exit(0);
                                break;
                            }
                        // Options displays the N-th record
                        case "-S":
                            {
                                PrintErrorMessage($"History record number is missing for the \"-s\" key", consoleOriginalColor);
                                Environment.Exit(0);
                                break;
                            }
                        case "-M":
                            {
                                PrintErrorMessage($"Mask as a parameter is missing for the \"-m\" key", consoleOriginalColor);
                                Environment.Exit(0);
                                break;
                            }
                        default:
                            {
                                PrintErrorMessage($"Unknown argument has been provided", consoleOriginalColor);
                                Environment.Exit(0);
                                break;
                            }
                    }
                }

                string[] addressComponents = args[0].Split("/");

                if (addressComponents.Length == 2)
                {
                    iPv4Address = ParseInputIPAddress(addressComponents[0]);
                    if (iPv4Address is null)
                    {
                        PrintErrorMessage($"The IP address ({addressComponents[0]}) is invalid. Check the inputs", consoleOriginalColor);
                        PrintExitMessageAndWait();
                        Environment.Exit(0);
                    }

                    subnetMask = ParseSubnetMaskString(addressComponents[1]);
                    if (subnetMask is null)
                    {
                        PrintErrorMessage($"The subnet mask ({addressComponents[1]}) is invalid. Check the inputs", consoleOriginalColor);
                        PrintExitMessageAndWait();
                        Environment.Exit(0);
                    }
                }
                ProcessIPAddressAndMask(iPv4Address, subnetMask);
            }

            // Case 3: 2 arguments have been provided
            if (args.Length == 2)
            {
                // Processing the "-h" option with a numerical parameter
                if (args[0].ToUpper() == "-H" && int.TryParse(args[1], out int historyRecords))
                {
                    ShowHistory(historyRecords);
                }

                // Processing the "-s" option with a numerical parameter
                if (args[0].ToUpper() == "-S" && int.TryParse(args[1], out int recordNumber))
                {
                    ShowHistoryRecord(recordNumber);
                    Environment.Exit(0);
                }

                // Processing the "-m" option with the mask in CIDR of 4 bytes format
                if (args[0].ToUpper() == "-M")
                {
                    IPv4SubnetMask? mask = ParseSubnetMaskString(args[1]);
                    if (mask is null)
                    {
                        PrintErrorMessage($"The mask provided is not valid\n", consoleOriginalColor);
                    }
                    else
                    {
                        Console.WriteLine($"{"The mask in 4 bytes format: ",INFO_ALIGN}{mask.ToString()}");
                        Console.WriteLine($"{"The mask in CIDR format: ",INFO_ALIGN}{mask.CIDR.ToString()}\n");
                    }
                    Environment.Exit(0);
                }

                // There could be a space between the IP address and the mask. Let's give it one more chance...
                string[] addressComponents = (args[0] + args[1]).Split("/");

                if (addressComponents.Length == 2)
                {
                    iPv4Address = ParseInputIPAddress(addressComponents[0]);
                    if (iPv4Address is null)
                    {
                        PrintErrorMessage("The IP address provided is invalid. Check the inputs", consoleOriginalColor);
                        Environment.Exit(0);
                    }

                    subnetMask = ParseSubnetMaskString(addressComponents[1]);
                    if (subnetMask is null)
                    {
                        PrintErrorMessage("The subnet mask is is invalid. Check the inputs", consoleOriginalColor);
                        Environment.Exit(0);
                    }
                }

                ProcessIPAddressAndMask(iPv4Address, subnetMask);
            }

            // Case 4: 3 arguments
            // TODO: Implement
            if (args.Length == 3)
            {
                // If the 1st argument is "-a", then try to parse the adresses and mask
                if (args[0] == "-a")
                {
                    string[] tmpArray= args[1].Split("/");
                    IPv4Address? firstAddress= ParseInputIPAddress(tmpArray[0]);
                    IPv4SubnetMask? firstMask= ParseSubnetMaskString(tmpArray[1]);

                    tmpArray = args[2].Split("/");
                    IPv4Address? secondAddress = ParseInputIPAddress(tmpArray[0]);
                    IPv4SubnetMask? secondMask = ParseSubnetMaskString(tmpArray[1]);

                    uint firstNetwork = GetNetworkPart(firstAddress, firstMask);
                    uint secondNetwrok= GetNetworkPart(secondAddress, secondMask);

                    Console.WriteLine($"{"First address:",INFO_ALIGN} {firstAddress.IPAddressAsString}");
                    Console.WriteLine($"{"Second address:", INFO_ALIGN} {secondAddress.IPAddressAsString}\n");

                    if (firstNetwork!= secondNetwrok)
                    {
                        Console.WriteLine($"The IP addresses don't belong to the same network\n");
                    }
                    else
                    {
                        Console.WriteLine($"The IP addresses are in the same network\n");
                    }
                    Environment.Exit(0);
                }
            }

            // Case 4: 4 or more arguments
            if (args.Length > 3)
            {
                PrintErrorMessage("Wrong number of arguments have been provided. See help below for usage\n", consoleOriginalColor);
                PrintHelp();
                Environment.Exit(0);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void ProcessIPAddressAndMask(IPv4Address address, IPv4SubnetMask mask)
        {
            AddHistoryItem(address, mask);

            // Processing special cases
            if (address.IPAddress == 0 & mask.SubnetMask != 0)
            {
                Console.WriteLine($"The {address}/{mask.CIDR} address is ambiguous");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            if (address.IPAddress == 0 & mask.SubnetMask == 0)
            {
                Console.WriteLine($"The {address}/{mask.CIDR} means the \"default rout\" in the network setup");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            // The /0 mask means that a network interface is not bound to any network
            if (address.IPAddress != 0 & mask.SubnetMask == 0)
            {
                Console.WriteLine($"For the IP address {address}/{mask.CIDR} the network interface is not bound to any network");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            Console.WriteLine("******************** General information ********************");

            Console.WriteLine($"{"IP address:",INFO_ALIGN} {address.ToString()}");
            Console.WriteLine($"{"Subnet mask:",INFO_ALIGN} {address.ToString()} (/{mask.CIDR})");
            Console.WriteLine($"{"IP address (BIN):",INFO_ALIGN} {address.ToBinString()}");
            Console.WriteLine($"{"Subnet mask (BIN):",INFO_ALIGN} {mask.ToBinString()}");

            Console.WriteLine();

            uint addressesQty = GetTotalNumberOfIPAddresses(mask);
            Console.WriteLine($"{"Total number of IP addresses:",INFO_ALIGN} {addressesQty}");

            switch (mask.CIDR)
            {
                case 32:
                    {
                        // For the /32 mask the whole address is a network address, so there are no host addresses
                        IPv4Address firstAddress = GetFirstIPAddress(address, mask);
                        Console.WriteLine($"{"Network address:",INFO_ALIGN} {firstAddress.ToString()}");

                        Console.WriteLine($"{"Number of usable IP addresses for hosts:",INFO_ALIGN} 0");

                        break;
                    }
                // For the /31 mask there is no netwrok address and both addresses are used for hosts. RFC 3021
                case 31:
                    {
                        IPv4Address firstAddress = GetFirstIPAddress(address, mask);
                        Console.WriteLine($"{"First host address",INFO_ALIGN} {firstAddress}");

                        IPv4Address secondAddress = GetLastIPAddress(address, mask);
                        Console.WriteLine($"{"Second host address",INFO_ALIGN} {secondAddress}");

                        break;
                    }
                case 30:
                    {
                        IPv4Address firstAddress = GetFirstIPAddress(address, mask);
                        Console.WriteLine($"{"First IP address (network address):",INFO_ALIGN} {firstAddress.ToString()}");

                        IPv4Address defaultGateway = GetDefaultGatewayAddress(address, mask);
                        Console.WriteLine($"{"Default gateway:",INFO_ALIGN} {defaultGateway}");

                        Console.WriteLine($"{"Number of usable IP addresses for hosts:",INFO_ALIGN} {addressesQty - 3}");

                        IPv4Address firstHostAddress = GetFirstHostAddress(address, mask);
                        Console.WriteLine($"{"Host address:",INFO_ALIGN} {firstHostAddress.ToString()}");

                        IPv4Address lastAddress = GetLastIPAddress(address, mask);
                        Console.WriteLine($"{"Last IP address: (broadcast address)",INFO_ALIGN} {lastAddress.ToString()}");

                        break;
                    }
                default:
                    {
                        IPv4Address firstAddress = GetFirstIPAddress(address, mask);
                        Console.WriteLine($"{"First IP address (network address):",INFO_ALIGN} {firstAddress.ToString()}");

                        IPv4Address defaultGateway = GetDefaultGatewayAddress(address, mask);
                        Console.WriteLine($"{"Default gateway:",INFO_ALIGN} {defaultGateway}");

                        Console.WriteLine($"{"Number of usable IP addresses for hosts:",INFO_ALIGN} {addressesQty - 3}");

                        IPv4Address firstHostAddress = GetFirstHostAddress(address, mask);
                        Console.WriteLine($"{"First host address:",INFO_ALIGN} {firstHostAddress.ToString()}");

                        Console.WriteLine($"{' ',INFO_ALIGN - 7}" + '.');
                        Console.WriteLine($"{' ',INFO_ALIGN - 7}" + '.');
                        Console.WriteLine($"{' ',INFO_ALIGN - 7}" + '.');

                        IPv4Address lastHostAddress = GetLastHostAddress(address, mask);
                        Console.WriteLine($"{"Last host address:",INFO_ALIGN} {lastHostAddress.ToString()}");

                        IPv4Address lastAddress = GetLastIPAddress(address, mask);
                        Console.WriteLine($"{"Last IP address: (broadcast address)",INFO_ALIGN} {lastAddress.ToString()}");

                        break;
                    }
            }

            Console.WriteLine("\n******************** Other information ********************");

            string networkClass = GetIPv4NetworkClass(address, mask);
            switch (networkClass)
            {
                case "A":
                case "B":
                case "C":
                    {
                        Console.WriteLine($"{"Network class:",INFO_ALIGN} {networkClass}");
                        break;
                    }
                case "D":
                    {
                        Console.WriteLine($"{"Network class:",INFO_ALIGN} {networkClass} (multicast address)");
                        break;
                    }
                case "E":
                    {
                        Console.WriteLine($"{"Network class:",INFO_ALIGN} {networkClass} (epxerimental address)");
                        break;
                    }
            }

            bool isLoopbackAddress = IsLoopbackAddress(address, mask);
            Console.WriteLine($"{"Loopback address",INFO_ALIGN} {isLoopbackAddress}");

            bool isPrivateAddress = IsPrivateAddress(address);
            Console.WriteLine($"{"Private address",INFO_ALIGN} {isPrivateAddress}");

            switch (mask.CIDR)
            {
                case 32:
                    {
                        Console.WriteLine($"{"Subnet mask is /32",INFO_ALIGN} VPN?");
                        break;
                    }
                case 31:
                    {
                        Console.WriteLine($"{"Subnet mask is /31",INFO_ALIGN} RFC 3021. Both addresses are host addresses");
                        break;
                    }
            }
        }
    }
}
