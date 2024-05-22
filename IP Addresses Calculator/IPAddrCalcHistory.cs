using static IP_Addresses_Calculator.IPAddrCalcOthers;

namespace IP_Addresses_Calculator
{
    // Class with functions to work with the program history

    static class IPAddrCalcHistory
    {
        /*
         * History file name
         * History file format: IPAddress/Mask
         */
        public const string HISTORY_FILE_NAME = "ip_history.txt";

        /// <summary>
        /// The method shows usage history
        /// </summary>
        /// <param name="n">Number of entries to show. In case of 0, the full history is shown</param>
        public static void ShowHistory(int n)
        {
            if (!File.Exists(HISTORY_FILE_NAME))
            {
                Console.WriteLine($"The program history doesn't exist yet");
                Environment.Exit(0);
            }

            if (n < 0)
            {
                PrintTextInColor($"Number of records to show cannot be negative", ConsoleColor.Red);
                Environment.Exit(0);
            }

            List<string> records = GetHistoryRecords(HISTORY_FILE_NAME);
            // If the history is empty, exit the program
            if (records.Count == 0)
            {
                Console.WriteLine($"The usage history is empty");
                Environment.Exit(0);
            }

            int lastItem = n;

            // Show full history if number of items provided is 0 or more than number of records in the history file
            if (n > records.Count)
            {
                Console.WriteLine($"The provided number ({n}) is greater than number of records ({records.Count}), the full history is shown:{Environment.NewLine}");
                lastItem = records.Count;
            }
            if (n == 0)
            {
                lastItem = records.Count;
            }

            // Displaying the history items
            Console.WriteLine($"IP Address Calculator usage history:{Environment.NewLine}");

            int serialNumber = 1;
            for (int i = 0; i < lastItem; i++)
            {
                Console.WriteLine(serialNumber + ". " + records[i]);
                serialNumber++;
            }
        }

        /// <summary>
        /// The methods shows one particular history record
        /// </summary>
        /// <param name="n">Number of record to be shown</param>
        public static void ShowHistoryRecord(int n)
        {
            if (n <= 0)
            {
                PrintTextInColor($"The number of history record cannot be 0 or negative", ConsoleColor.Red);
                Environment.Exit(0);
            }

            if (!File.Exists(HISTORY_FILE_NAME))
            {
                Console.WriteLine($"The program history doesn't exist yet");
                Environment.Exit(0);
            }

            List<string> records = GetHistoryRecords(HISTORY_FILE_NAME);

            if (records.Count == 0)
            {
                Console.WriteLine($"The history file is empty");
                Environment.Exit(0);
            }

            if (n > records.Count)
            {
                Console.WriteLine($"There are total {records.Count} history items in the history");
            }
            else
            {
                Console.WriteLine($"Record number {n}: {records[n - 1]}");
            }
        }

        /// <summary>
        /// The method adds a new item in the history file
        /// </summary>
        /// <param name="newItem">IP address and subnet mask as objects</param>
        public static void AddHistoryItem(IPv4Address address, IPv4SubnetMask mask)
        {
            string histItem = address.ToString() + "/" + mask.CIDR.ToString();

            try
            {
                using (StreamWriter strWriter = new StreamWriter(HISTORY_FILE_NAME, true))
                {
                    strWriter.WriteLine(histItem);
                }
            }
            catch (Exception e)
            {
                PrintTextInColor($"Error happened during adding a history item:{Environment.NewLine}{e.Message}", ConsoleColor.Red);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// The method clears the program usage history
        /// </summary>
        public static void ClearHistory()
        {
            try
            {
                // The Create() method creates a file if it doesn't exist and overwrites the esiting method
                using (FileStream fs = File.Create(HISTORY_FILE_NAME)) { }
            }
            catch (Exception e)
            {
                PrintTextInColor($"Error happened during history clearance:{Environment.NewLine}{e.Message}", ConsoleColor.Red);
                Environment.Exit(1);
            }
            Console.Write($"The history has been cleared{Environment.NewLine}");
        }

        #region Private methods of the class

        /// <summary>
        /// The method returns all records in the history file as a list
        /// </summary>
        /// <returns>List of history record strings</returns>
        private static List<string> GetHistoryRecords(string historyFileName)
        {
            // A list to save history records
            var historyRecords = new List<string>();
            try
            {
                var fs = new FileStream(historyFileName, FileMode.Open);
                using (var strReader = new StreamReader(fs))
                {
                    // Read all records in the history file
                    string? line = string.Empty;
                    while ((line = strReader.ReadLine()) is not null)
                    {
                        historyRecords.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                PrintTextInColor($"Error during reading of the history file occured:{Environment.NewLine}{e.Message}", ConsoleColor.Red);
                Environment.Exit(1);
            }
            return historyRecords;
        }
        #endregion
    }
}
