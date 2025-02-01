using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SimpleEndianBinaryIO;

namespace RE4_PS3X360_PACK_TOOL
{
    internal static class Repack
    {
        internal static void RepackFile(string file)
        {
            StreamReader idx = null;
            FileInfo fileInfo = new FileInfo(file);
            string DirectoryName = fileInfo.DirectoryName;

            try
            {
                idx = fileInfo.OpenText();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + Environment.NewLine + ex);
            }

            if (idx != null)
            {
                uint magic = 0;

                string endLine = "";
                while (endLine != null)
                {
                    endLine = idx.ReadLine();
                    if (endLine != null)
                    {
                        string trim = endLine.ToUpperInvariant().Trim();
                        if (! (trim.StartsWith(":") || trim.StartsWith("#") || trim.StartsWith("/") || trim.StartsWith("\\")))
                        {
                            var split = trim.Split(new char[] { ':' });
                            if (split.Length >= 2)
                            {
                                string key = split[0].Trim();
                                if (key.StartsWith("MAGIC"))
                                {
                                    string value = split[1].Trim();
                                    try
                                    {
                                        magic = uint.Parse(ReturnValidHexValue(value), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }

                        }

                    }

                }

                idx.Close();

                if (magic == 0)
                {
                    Console.WriteLine("=============================");
                    Console.WriteLine("========INVALID MAGIC========");
                    Console.WriteLine("=============================");
                }

                Console.WriteLine("Magic: " + magic.ToString("X8"));

                string ImageFolder = Path.Combine(DirectoryName, magic.ToString("x8"));

                if (Directory.Exists(ImageFolder))
                {

                    EndianBinaryWriter packFile = null;

                    try
                    {
                        string packName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                        packName = packName.Length > 0 ? packName : "NoName.pack";

                        FileInfo packFileInfo = new FileInfo(Path.Combine(DirectoryName, packName));
                        packFile = new EndianBinaryWriter(packFileInfo.Create(), Endianness.BigEndian);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + Environment.NewLine + ex);
                    }

                    if (packFile != null)
                    {
                        uint iCount = 0; // quantidade de imagens
                        bool asFile = true;

                        while (asFile)
                        {
                            string gtfpath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".gtf");
                            string ddspath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".dds");
                            string tgapath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".tga");
                            string tga03path = Path.Combine(ImageFolder, iCount.ToString("D4") + ".tga03");
                            string tga15path = Path.Combine(ImageFolder, iCount.ToString("D4") + ".tga15");
                            string empty = Path.Combine(ImageFolder, iCount.ToString("D4") + ".empty");
                            string reference = Path.Combine(ImageFolder, iCount.ToString("D4") + ".reference");
                            string _null = Path.Combine(ImageFolder, iCount.ToString("D4") + ".null");

                            if (File.Exists(gtfpath)
                                || File.Exists(ddspath)
                                || File.Exists(tgapath)
                                || File.Exists(tga03path)
                                || File.Exists(tga15path)
                                || File.Exists(empty)
                                || File.Exists(reference)
                                || File.Exists(_null))
                            {
                                iCount++;
                            }
                            else 
                            {
                                asFile = false;
                            }
                        }

                        Console.WriteLine("Count: " + iCount);

                        packFile.Write(magic);
                        packFile.Write(iCount);

                        //header calculo
                        uint headerLength = (iCount + 2) * 4; //diferença entre as tools
                        uint line = headerLength / 16;
                        uint rest = headerLength % 16;
                        if (rest != 0)
                        {
                            line++;
                        }
                        if (line < 2)
                        {
                            line = 2;
                        }

                        uint nextOffset = line * 16;

                        //id, offset
                        Dictionary<int, uint> offsetVisiteds = new Dictionary<int, uint>();

                        for (int i = 0; i < iCount; i++)
                        {
                            string gtfpath = Path.Combine(ImageFolder, i.ToString("D4") + ".gtf");
                            string ddspath = Path.Combine(ImageFolder, i.ToString("D4") + ".dds");
                            string tgapath = Path.Combine(ImageFolder, i.ToString("D4") + ".tga");
                            string tga03path = Path.Combine(ImageFolder, i.ToString("D4") + ".tga03");
                            string tga15path = Path.Combine(ImageFolder, i.ToString("D4") + ".tga15");
                            string _null = Path.Combine(ImageFolder, i.ToString("D4") + ".null");

                            FileInfo imageFile = null;
                                 if (File.Exists(gtfpath))   { imageFile = new FileInfo(gtfpath); }
                            else if (File.Exists(ddspath))   { imageFile = new FileInfo(ddspath); }
                            else if (File.Exists(tga03path)) { imageFile = new FileInfo(tga03path); }
                            else if (File.Exists(tga15path)) { imageFile = new FileInfo(tga15path); }
                            else if (File.Exists(tgapath))   { imageFile = new FileInfo(tgapath); }
                            else if (File.Exists(_null))     { imageFile = new FileInfo(_null); }

                            if (imageFile != null)
                            {
                                offsetVisiteds.Add(i, nextOffset);

                                packFile.BaseStream.Position = 8 + (i * 4);
                                packFile.Write(nextOffset);

                                packFile.BaseStream.Position = nextOffset;

                                packFile.Write((uint)imageFile.Length);
                                packFile.Write(0xFFFFFFFF);
                                packFile.Write(magic);

                                string ext = imageFile.Extension.ToUpperInvariant();
                                if (ext.Contains("DDS") || ext.Contains("GTF"))
                                { packFile.Write((uint)1, Endianness.LittleEndian); }
                                else
                                { packFile.Write((uint)0, Endianness.LittleEndian); }

                                var fileStream = imageFile.OpenRead();
                                fileStream.CopyTo(packFile.BaseStream);
                                fileStream.Close();

                                //alinhamento
                                uint aLine = (uint)packFile.BaseStream.Position / 16;
                                uint aRest = (uint)packFile.BaseStream.Position % 16;
                                aLine += aRest != 0 ? 1u : 0u;
                                int aDif = (int)((aLine * 16) - packFile.BaseStream.Position);
                                packFile.Write(new byte[aDif]);

                                nextOffset = (uint)packFile.BaseStream.Position;

                                Console.WriteLine("Add file: " + imageFile.Name);
                            }
                            else 
                            {
                                int Id = 0;
                                uint Offset = 0;

                                string reference = Path.Combine(ImageFolder, i.ToString("D4") + ".reference");
                                if (File.Exists(reference))
                                {
                                    string cont = ReturnValidDecValue(File.ReadAllText(reference));
                                    if (int.TryParse(cont, out Id))
                                    {
                                        if (offsetVisiteds.ContainsKey(Id))
                                        {
                                            Offset = offsetVisiteds[Id];
                                        }
                                    } 
                                }

                                packFile.BaseStream.Position = 8 + (i * 4); //diferença entre as tools
                                packFile.Write(Offset);

                                if (Offset != 0)
                                {
                                    Console.WriteLine("ID: " + i.ToString("D4") + " references the ID " + Id.ToString("D4"));
                                }
                                else 
                                {
                                    Console.WriteLine("ID: " + i.ToString("D4") + " is empty");
                                }
                               
                            }

                        }

                        packFile.Close();
                    }

                }
                else 
                {
                    Console.WriteLine($"The folder {magic:x8} does not exist.");
                }

            }

        }


        private static string ReturnValidHexValue(string cont)
        {
            string res = "";
            foreach (var c in cont.ToUpperInvariant())
            {
                if (char.IsDigit(c)
                    || c == 'A'
                    || c == 'B'
                    || c == 'C'
                    || c == 'D'
                    || c == 'E'
                    || c == 'F'
                    )
                {
                    res += c;
                }
            }
            return res;
        }

        private static string ReturnValidDecValue(string cont)
        {
            string res = "0";
            foreach (var c in cont)
            {
                if (char.IsDigit(c))
                {
                    res += c;
                }
            }
            return res;
        }
    }
}
