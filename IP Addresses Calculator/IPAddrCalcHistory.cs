using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IP_Addresses_Calculator
{
    // Class with functions to work with the program history

    public static class IPAddrCalcHistory
    {
        // History file name
        // History file format:
        // N IPAddress:Mask Date, where:
        // N- entry serial number,
        // IPAddress:Mask- IP address with a mask in the CIDR format
        // Date- the date when the IP address was checked
        public const string HISTORY_FILE_NAME = "ip_history.txt";

        /// <summary>
        /// The method shows usage history
        /// </summary>
        /// <param name="n">Number of entries to show. If case of 0, the full history is shown</param>
        public static void ShowHistory(int n)
        {
            if (n < 0)
            {
                Console.WriteLine($"Number of records to show cannot be negative");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            if (!File.Exists(HISTORY_FILE_NAME))
            {
                Console.WriteLine($"The program history doesn't exist yet");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            List<string> records = GetHistoryRecords(HISTORY_FILE_NAME);
            // If the history is empty, exit the program
            if (records.Count == 0)
            {
                Console.WriteLine($"The history file is empty");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            // Displaying the history items
            if (n == 0 || n > records.Count)
            {
                if (n == 0)
                {
                    Console.WriteLine($"IP Address Calculator usage history:\n");
                }
                else
                {
                    Console.WriteLine($"The provided number ({n}) is greater than number of records ({records.Count}), the full history will be shown:\n");
                }

                int serialNumber = 1;
                for (int i = 0; i < records.Count; i++)
                {
                    Console.WriteLine(serialNumber + ". " + records[i]);
                    serialNumber++;
                }
            }
            else if (n <= records.Count)
            {
                Console.WriteLine($"IP Address Calculator usage history:\n");
                int serialNumber = 1;
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine(serialNumber + ". " + records[i]);
                    serialNumber++;
                }
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
                Console.WriteLine($"The number of history record cannot be 0 or negative");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            if (!File.Exists(HISTORY_FILE_NAME))
            {
                Console.WriteLine("The program history doesn't exist yet");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            List<string> records = GetHistoryRecords(HISTORY_FILE_NAME);

            if (records.Count == 0)
            {
                Console.WriteLine($"The history file is empty");
                PrintExitMessageAndWait();
                Environment.Exit(0);
            }

            if (n > records.Count)
            {
                Console.WriteLine($"There are {records.Count} history items in the history");
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

            using (StreamWriter strWriter = new StreamWriter(HISTORY_FILE_NAME, true))
            {
                strWriter.WriteLine(histItem);
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
                Console.WriteLine($"Error happened during history clearance:\n{e.Message}");
                Environment.Exit(1);
            }
            Console.Write("The history has been cleared\n");
        }

        #region Private methods of the class

        /// <summary>
        /// The method returns all records in the history file as a list
        /// </summary>
        /// <returns>List of history record strings</returns>
        private static List<string> GetHistoryRecords(string historyFileName)
        {
            // A list to save history records
            List<string> historyRecords = new List<string>();
            try
            {
                FileStream fs = new FileStream(historyFileName, FileMode.Open);
                using (StreamReader strReader = new StreamReader(fs))
                {
                    // Read all records in the history file
                    while (strReader.Peek() != -1)
                    {
                        string? line = strReader.ReadLine();
                        if(line is not null)
                        {
                            historyRecords.Add(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during reading of the history file occured:\n{e.Message}");
            }
            return historyRecords;
        }
        #endregion
    }
}
