﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_PS3X360_PACK_TOOL
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            Console.WriteLine("# RE4 PS3X360 PACK TOOL");
            Console.WriteLine("# By: JADERLINK");
            Console.WriteLine("# youtube.com/@JADERLINK");
            Console.WriteLine("# github.com/JADERLINK");
            Console.WriteLine("# VERSION 1.0.8 (2025-01-31)");

            if (args.Length == 0)
            {
                Console.WriteLine("For more information read:");
                Console.WriteLine("https://github.com/JADERLINK/RE4-PS3X360-PACK-TOOL");
                Console.WriteLine("Press any key to close the console.");
                Console.ReadKey();
            }
            else
            {
                bool usingBatFile = false;
                int start = 0;
                if (args[0].ToLowerInvariant() == "-bat")
                {
                    usingBatFile = true;
                    start = 1;
                }

                for (int i = start; i < args.Length; i++)
                {
                    if (File.Exists(args[i]))
                    {
                        FileInfo info = null;

                        try
                        {
                            info = new FileInfo(args[i]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error in the path: " + Environment.NewLine + ex);
                        }
                        if (info != null)
                        {
                            Console.WriteLine("File: " + info.Name);

                            if (info.Name.ToUpperInvariant().EndsWith(".PACK") || info.Name.ToUpperInvariant().EndsWith(".PACK.YZ2"))
                            {
                                try
                                {
                                    Extract.ExtractFile(info.FullName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error: " + Environment.NewLine + ex);
                                }

                            }
                            else if (info.Extension.ToUpperInvariant() == ".IDXBIGPACK")
                            {
                                try
                                {
                                    Repack.RepackFile(info.FullName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error: " + Environment.NewLine + ex);
                                }
                            }
                            else
                            {
                                Console.WriteLine("The extension is not valid: " + info.Extension);
                            }

                        }

                    }
                    else
                    {
                        Console.WriteLine("File specified does not exist: " + args[i]);
                    }

                }

                Console.WriteLine("Finished!!!");

                if (!usingBatFile)
                {
                    Console.WriteLine("Press any key to close the console.");
                    Console.ReadKey();
                }
            }

        }
    }
}
