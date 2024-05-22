using System.Reflection;

namespace IP_Addresses_Calculator
{
    static class IPAddrCalcOthers
    {
        // Constant used for text formatting
        public const int INFO_ALIGN = -45;

        /// <summary>
        /// The method prints usage help
        /// </summary>
        public static void PrintHelp()
        {
            Console.WriteLine($"IP calculator usage:{Environment.NewLine}");
            Console.WriteLine($"ipaddrcalc 192.168.5.100/24 OR ipaddrcalc 192.168.5.100/255.255.255.0");
            Console.WriteLine($"{Environment.NewLine}Other options (could be both in upper and lower case):");
            Console.WriteLine($"-v           --> for program version");
            Console.WriteLine($"-u           --> for usage");
            Console.WriteLine($"-h           --> for the full history");
            Console.WriteLine($"-h N         --> for the first N history entries (\"-h 0\" equal to \"-h\")");
            Console.WriteLine($"-s N         --> to show details of the Nth history item");
            Console.WriteLine($"-c           --> to clear the history");
            Console.WriteLine($"-m           --> convert from CIDR to 4 bytes notation and vice versa");
            Console.WriteLine($"(For the \"-m\" option use ipaddrcalc -m 32 OR ipaddrcalc -m 255.255.0.0)");
            Console.WriteLine($"-a           --> to checke whether 2 addresses are on the same network");
            Console.WriteLine($"(For the \"-a\" option use ipaddrcalc -a 100.101.102.103/24 100.101.102.1/24){Environment.NewLine}");
        }

        /// <summary>
        /// The method prints a text in color. After printing, the original color of the text is restored
        /// </summary>
        /// <param name="message"></param>
        /// <param name="textColor"></param>
        public static void PrintTextInColor(string message, ConsoleColor textColor)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        /// <summary>
        /// The method prints the version of the program to the console
        /// </summary>
        public static void PrintProgramVersion()
        {
            Console.WriteLine($"IP Address Calculator version: {GetProgramVersion()}");
        }

        /// <summary>
        /// The method returns the version of the program
        /// </summary>
        /// <returns>Program version as a string</returns>
        /// <exception cref="FormatException">Exception is thrown if a program version is missing in the assembly</exception>
        public static string GetProgramVersion()
        {
            Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            if (assemblyVersion is null)
            {
                throw new FormatException($"The program version is missing in the assembly");
            }
            else
            {
                return assemblyVersion.ToString();
            }
        }
    }
}
