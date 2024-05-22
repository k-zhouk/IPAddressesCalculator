using static IP_Addresses_Calculator.IPAddrCalcLib;
using static IP_Addresses_Calculator.IPAddrCalcHistory;
using static IP_Addresses_Calculator.IPAddrCalcOthers;

namespace IP_Addresses_Calculator
{
    class IPAddrCalc
    {
        static void Main(string[] args)
        {
            // Print warning message
            PrintTextInColor($"{Environment.NewLine}No warranties provided. Use at your own risk", ConsoleColor.Red);

            Console.WriteLine($"{Environment.NewLine}******************** IP Address Calculator (ver. {GetProgramVersion()}) ********************{Environment.NewLine}");

            // Parsing command line arguments
            switch (args.Length)
            {
                // Case 1: no program arguments
                case 0:
                    PrintTextInColor($"No arguments have been provided{Environment.NewLine}", ConsoleColor.Red);
                    PrintHelp();
                    Environment.Exit(0);
                    break;

                // Case 2: 1 program argument
                case 1:
                    switch (args[0].ToUpper())
                    {
                        // Clear the usage history
                        case "-C":
                            ClearHistory();
                            Environment.Exit(0);
                            break;

                        // Display the usage history
                        case "-H":
                            ShowHistory(0);
                            Environment.Exit(0);
                            break;

                        // Print help
                        case "-U":
                            PrintHelp();
                            Environment.Exit(0);
                            break;

                        // Shows program version
                        case "-V":
                            PrintProgramVersion();
                            Environment.Exit(0);
                            break;

                        // Checking the keys with a parameter, but the parameter is missing
                        // Check if the "-s" key was provided without the argument
                        case "-S":
                            PrintTextInColor($"The \"-s\" key requires a numberical argument", ConsoleColor.Red);
                            Environment.Exit(0);
                            break;

                        // Check if the "-m" key was provided without the argument
                        case "-M":
                            PrintTextInColor($"The \"-m\" key requires a numberical argument or subnet mask", ConsoleColor.Red);
                            Environment.Exit(0);
                            break;

                        // Checking the keys with 2 parameters, but the parameter is missing
                        case "-A":
                            PrintTextInColor($"The \"-a\" key requires 2 parameters (2 IP addresses with subnet masks)", ConsoleColor.Red);
                            Environment.Exit(0);
                            break;

                        // No more keys matches, so trying to parse the argument as a IP address with a mask
                        default:

                            string[] addressComponents = args[0].Split("/");
                            if (addressComponents.Length == 2)
                            {
                                var (iPv4Address, subnetMask) = (ParseInputIPAddress(addressComponents[0]), ParseSubnetMaskString(addressComponents[1]));

                                if (iPv4Address is null)
                                {
                                    PrintTextInColor($"The IP address ({addressComponents[0]}) is invalid. Check the inputs", ConsoleColor.Red);
                                    Environment.Exit(0);
                                }

                                if (subnetMask is null)
                                {
                                    PrintTextInColor($"The subnet mask ({addressComponents[1]}) is invalid. Check the inputs", ConsoleColor.Red);
                                    Environment.Exit(0);
                                }

                                ProcessIPAddressAndMask(iPv4Address, subnetMask);
                                Environment.Exit(0);
                            }
                            break;
                    }

                    PrintTextInColor($"Unknown argument has been provided", ConsoleColor.Red);
                    Environment.Exit(0);
                    break;

                // Case 3: 2 program arguments
                case 2:
                    switch (args[0].ToUpper())
                    {
                        case "-H":
                            if (int.TryParse(args[1], out int historyRecords))
                            {
                                ShowHistory(historyRecords);
                            }
                            else
                            {
                                PrintTextInColor($"Wrong parameter for the number of history records{Environment.NewLine}", ConsoleColor.Red);
                            }
                            Environment.Exit(0);
                            break;

                        case "-S":
                            if (int.TryParse(args[1], out int recordNumber))
                            {
                                ShowHistoryRecord(recordNumber);
                            }
                            else
                            {
                                PrintTextInColor($"Wrong parameter for the record number{Environment.NewLine}", ConsoleColor.Red);
                            }
                            Environment.Exit(0);
                            break;

                        case "-M":
                            IPv4SubnetMask? mask = ParseSubnetMaskString(args[1]);
                            if (mask is null)
                            {
                                PrintTextInColor($"The mask is not valid{Environment.NewLine}", ConsoleColor.Red);
                            }
                            else
                            {
                                Console.WriteLine($"{"The mask in 4 bytes format: ",INFO_ALIGN}{mask.ToString()}");
                                Console.WriteLine($"{"The mask in CIDR format: ",INFO_ALIGN}{mask.CIDR.ToString()}{Environment.NewLine}");
                            }
                            Environment.Exit(0);
                            break;

                        // Checking the keys with 2 parameters, but the parameter is missing
                        case "-A":
                            PrintTextInColor($"The \"-a\" key requires 2 parameters (2 IP addresses with subnet masks)", ConsoleColor.Red);
                            Environment.Exit(0);
                            break;

                        default:
                            PrintTextInColor($"Unknown arguments have been provided", ConsoleColor.Red);
                            Environment.Exit(0);
                            break;
                    }
                    break;

                // Case 4: 3 program arguments
                case 3:
                    switch (args[0].ToUpper())
                    {
                        case "-A":
                            string[] tmpArray;

                            var (firstAddress, firstMask) = (new IPv4Address(), new IPv4SubnetMask());

                            tmpArray = args[1].Split("/");
                            if (tmpArray.Length == 2)
                            {
                                (firstAddress, firstMask) = (ParseInputIPAddress(tmpArray[0]), ParseSubnetMaskString(tmpArray[1]));

                                if (firstAddress is null)
                                {
                                    PrintTextInColor($"The first IP address ({tmpArray[0]}) is invalid", ConsoleColor.Red);
                                    Environment.Exit(0);
                                }

                                if (firstMask is null)
                                {
                                    PrintTextInColor($"The first subnet mask ({tmpArray[1]}) is invalid", ConsoleColor.Red);
                                    Environment.Exit(0);
                                }
                            }
                            else
                            {
                                PrintTextInColor($"The first parameter is not an IP address/ subnet mask pair", ConsoleColor.Red);
                                Environment.Exit(0);
                            }

                            var (secondAddress, secondMask) = (new IPv4Address(), new IPv4SubnetMask());

                            tmpArray = args[2].Split("/");
                            if (tmpArray.Length == 2)
                            {
                                (secondAddress, secondMask) = (ParseInputIPAddress(tmpArray[0]), ParseSubnetMaskString(tmpArray[1]));

                                if (secondAddress is null)
                                {
                                    PrintTextInColor($"The second IP address ({tmpArray[0]}) is invalid", ConsoleColor.Red);
                                    Environment.Exit(0);
                                }

                                if (secondMask is null)
                                {
                                    PrintTextInColor($"The second mask ({tmpArray[1]}) is invalid", ConsoleColor.Red);
                                    Environment.Exit(0);
                                }
                            }
                            else
                            {
                                PrintTextInColor($"The second parameter is not an IP address/ subnet mask pair", ConsoleColor.Red);
                                Environment.Exit(0);
                            }

                            uint firstNetwork = GetNetworkPart(firstAddress, firstMask);
                            uint secondNetwrok = GetNetworkPart(secondAddress, secondMask);

                            Console.WriteLine($"{"First address:",INFO_ALIGN} {firstAddress.IPAddressAsString} /{firstMask.CIDR}");
                            Console.WriteLine($"{"Second address:",INFO_ALIGN} {secondAddress.IPAddressAsString} /{secondMask.CIDR}{Environment.NewLine}");

                            if (firstNetwork != secondNetwrok)
                            {
                                PrintTextInColor($"The IP addresses are not in the same network{Environment.NewLine}", ConsoleColor.Red);
                            }
                            else
                            {
                                PrintTextInColor($"The IP addresses are in the same network{Environment.NewLine}", ConsoleColor.Green);
                            }
                            Environment.Exit(0);
                            break;

                        default:
                            PrintTextInColor($"Unknown arguments have been provided", ConsoleColor.Red);
                            Environment.Exit(0);
                            break;
                    }
                    break;

                default:
                    PrintTextInColor($"Wrong number of arguments have been provided. See help for usage{Environment.NewLine}", ConsoleColor.Red);
                    PrintHelp();
                    Environment.Exit(0);
                    break;
            }

            Console.WriteLine($"{Environment.NewLine}Press any key to exit...");
            Console.ReadKey();
        }

        static void ProcessIPAddressAndMask(IPv4Address address, IPv4SubnetMask mask)
        {
            AddHistoryItem(address, mask);

            // Processing special cases
            if (address.IPAddress == 0 & mask.SubnetMask != 0)
            {
                Console.WriteLine($"The {address}/{mask.CIDR} address is ambiguous");
                Environment.Exit(0);
            }

            if (address.IPAddress == 0 & mask.SubnetMask == 0)
            {
                Console.WriteLine($"The {address}/{mask.CIDR} means the \"default rout\" in the network setup");
                Environment.Exit(0);
            }

            // The /0 mask means that a network interface is not bound to any network
            if (address.IPAddress != 0 & mask.SubnetMask == 0)
            {
                Console.WriteLine($"For the IP address {address}/{mask.CIDR} the network interface is not bound to any network");
                Environment.Exit(0);
            }

            Console.WriteLine($"******************** General information ********************");

            Console.WriteLine($"{"IP address:",INFO_ALIGN} {address.ToString()}");
            Console.WriteLine($"{"Subnet mask:",INFO_ALIGN} {mask.ToString()} (/{mask.CIDR})");
            Console.WriteLine($"{"IP address (BIN):",INFO_ALIGN} {address.IPAddressAsBinString}");
            Console.WriteLine($"{"Subnet mask (BIN):",INFO_ALIGN} {mask.SubnetMaskAsBinString}");

            Console.WriteLine();

            uint addressesQty = GetTotalNumberOfIPAddresses(mask);
            Console.WriteLine($"{"Total number of IP addresses:",INFO_ALIGN} {addressesQty}");

            switch (mask.CIDR)
            {
                // Masks /32, /31, /30 should be processed separately
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

            Console.WriteLine($"{Environment.NewLine}******************** Other information ********************");

            string networkClass = GetIPv4NetworkClass(address, mask);
            switch (networkClass)
            {
                case "A":
                case "B":
                case "C":
                    Console.WriteLine($"{"Network class:",INFO_ALIGN} {networkClass}");
                    break;

                case "D":
                    Console.WriteLine($"{"Network class:",INFO_ALIGN} {networkClass} (multicast address)");
                    break;
                case "E":
                    Console.WriteLine($"{"Network class:",INFO_ALIGN} {networkClass} (epxerimental address)");
                    break;
            }

            bool isLoopbackAddress = IsLoopbackAddress(address, mask);
            Console.WriteLine($"{"Loopback address",INFO_ALIGN} {isLoopbackAddress}");

            bool isPrivateAddress = IsPrivateAddress(address, mask);
            Console.WriteLine($"{"Private address",INFO_ALIGN} {isPrivateAddress}");

            switch (mask.CIDR)
            {
                case 32:
                    Console.WriteLine($"{"Subnet mask is /32",INFO_ALIGN} VPN?");
                    break;
                case 31:
                    Console.WriteLine($"{"Subnet mask is /31",INFO_ALIGN} RFC 3021. Both addresses are host addresses");
                    break;
            }
        }
    }
}
