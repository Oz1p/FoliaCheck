using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoliaCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "FoliaCheck by Oz1p";
            if (args.Length != 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please drag a .jar file onto the program!");
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
                        ZipEntry pluginEntry = jarFile.GetEntry("plugin.yml");
                        if (pluginEntry != null)
                        {
                            using (Stream pluginStream = jarFile.GetInputStream(pluginEntry))
                            using (StreamReader reader = new StreamReader(pluginStream))
                            {
                                bool found = false;
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    if (line.Trim() == "folia-supported: true")
                                    {
                                        found = true;
                                        break;
                                    }
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
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Not a valid plugin!");
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
