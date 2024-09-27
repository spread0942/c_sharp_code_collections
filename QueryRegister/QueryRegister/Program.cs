using Microsoft.Win32;
using System.Security;

namespace QueryRegister
{
    class Program
    {
        private static void RegSearchLocalMachine(string keyPath, string valueName)
        {
            using (RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (rootKey != null)
                {
                    string[] subKeyNames = rootKey.GetSubKeyNames();

                    foreach (string subKeyName in subKeyNames)
                    {
                        try
                        {
                            using (RegistryKey subKey = rootKey.OpenSubKey(subKeyName))
                            {
                                if (subKey != null)
                                {
                                
                                    Object value = subKey.GetValue(valueName);
                                    if (value != null)
                                    {
                                        Console.WriteLine($"[+] SubKeyName: {subKeyName}; Value: {value}");
                                    }

                                }
                            }
                            RegSearchLocalMachine($"{keyPath}\\{subKeyName}", valueName);
                        }
                        catch (SecurityException)
                        {
                            Console.WriteLine($"[-] Require elevated privilege the SubKey: {subKeyName}");
                        }
                    }
                }
            }
        }

        private static void RegSearchCurrentUser(string keyPath, string valueName)
        {
            using (RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(keyPath))
            {
                if (rootKey != null)
                {
                    string[] subKeyNames = rootKey.GetSubKeyNames();

                    foreach (string subKeyName in subKeyNames)
                    {
                        try
                        {
                            using (RegistryKey subKey = rootKey.OpenSubKey(subKeyName))
                            {
                                if (subKey != null)
                                {

                                    Object value = subKey.GetValue(valueName);
                                    if (value != null)
                                    {
                                        Console.WriteLine($"[+] SubKeyName: {subKeyName}; Value: {value}");
                                    }

                                }
                            }
                            RegSearchCurrentUser($"{keyPath}\\{subKeyName}", valueName);
                        }
                        catch (SecurityException)
                        {
                            Console.WriteLine($"[-] Require elevated privilege the SubKey: {subKeyName}");
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                try
                {
                    string hive = args[0];
                    string keyPath = args[1];
                    string valueName = args[2];

                    if (hive == "HKLM")
                    {
                        RegSearchLocalMachine(keyPath, valueName);

                    }
                    else if (hive == "HKCU")
                    {
                        RegSearchCurrentUser(keyPath, valueName);

                    }
                    else
                    {
                        Console.WriteLine("[+] Hive value not find");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[-] {ex}");
                }
            }
            else
            {
                Console.WriteLine("[-] Require three parameters, Hive, Key Path and the Value to find");
                Console.WriteLine($"[-] {AppDomain.CurrentDomain.FriendlyName}.exe \"<HKLM or HKCU>\" \"<KeyPath>\" \"<Value>\"");
            }

            Console.Write("[+] Press Any Key...");
            Console.ReadLine();
        }
    }
}







