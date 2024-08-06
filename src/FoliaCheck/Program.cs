using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Reflection;

namespace FoliaCheck
{
    class Program
    {
        private static bool ismcplugin = false;
        static void Main(string[] args)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.Title = $"FoliaCheck v{version} by Oz1p";
            Console.WriteLine($"Thanks for using FoliaCheck v{version}");
            if (args.Length != 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please drag a .jar file onto the program or use arguments!");
                Console.WriteLine("Example: FoliaCheck.exe <plugin_path>");
                Console.ResetColor();
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadKey();
                return;
            }

            string jarFilePath = args[0];

            if (!File.Exists(jarFilePath) || Path.GetExtension(jarFilePath) != ".jar")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The provided file is not a valid .jar file!");
                Console.ResetColor();
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadKey();
                return;
            }
            
            try
            {
                using (FileStream fs = File.OpenRead(jarFilePath))
                {
                    using (ZipFile jarFile = new ZipFile(fs))
                    {
                        bool found = false;
                        bool paper = false;

                        ZipEntry pluginEntry = jarFile.GetEntry("plugin.yml");
                        if (pluginEntry != null)
                        {
                            ismcplugin = true;
                            using (Stream pluginStream = jarFile.GetInputStream(pluginEntry))
                            using (StreamReader reader = new StreamReader(pluginStream))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    if (line.Trim() == "folia-supported: true")
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!found)
                        {
                            ZipEntry paperEntry = jarFile.GetEntry("paper-plugin.yml");
                            if (paperEntry != null)
                            {
                                ismcplugin = true;
                                paper = true;
                                using (Stream paperStream = jarFile.GetInputStream(paperEntry))
                                using (StreamReader reader = new StreamReader(paperStream))
                                {
                                    string line;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        if (line.Trim() == "folia-supported: true")
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!ismcplugin)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This is not a valid Bukkit/Paper plugin!");
                            Console.ResetColor();
                            Console.WriteLine("Press ENTER to exit...");
                            Console.ReadKey();
                            return;
                        }
                        if (paper)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("This Plugin is a Paper Plugin.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("This Plugin is a Bukkit Plugin.");
                        }
                        if (found)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("This plugin supports folia!");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This plugin does not support folia!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + ex.Message);
            }
            Console.ResetColor();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadKey();
        }
    }
}
