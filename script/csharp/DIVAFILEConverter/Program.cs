﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinarySerialization;
using DIVALib.Crypto;
using DIVALib.IO;

namespace DIVAFILEConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("DIVAFILE Converter\r\n----------------------------------------------------\r\n" +
                                  "Converts files to DIVAFILE encryption and vice versa.\r\n\r\nUsage:\r\n" +
                                  "    Just drag\'n\'drop\r\n\r\nTested:\r\n    Project Diva F2nd");
                Console.Read();
                return;
            }

            var serializer = new BinarySerializer();

            if (Directory.Exists(args[0]))
            {
                
            }
            
            //args = new[] {@"C:\Users\waelw.WAELS-PC\Desktop\DIVAFILE\pv_field_723.pfl", @"C:\Users\waelw.WAELS-PC\Desktop\DIVAFILE\pv_field_7231.pfl", "" };
            //args = new[] { @"C:\Users\waelw.WAELS-PC\Desktop\DIVAFILE\pv_field_7231.pfl"};

            using (var file = new FileStream(args[0], FileMode.Open))
            {
                if (file.Length == 0) throw new IOException("File contains no data");

                if (DivaFile.IsValid(file))
                {
                    var divaFile = await serializer.DeserializeAsync<DivaFile>(file);
                    var decrypt = divaFile.DecryptBytes();
                    file.Close();
                    using (var save = new FileStream(args[0], FileMode.Create))
                    {
                        await serializer.SerializeAsync(save, decrypt);
                    }
                }
                else
                {
                    var divaFile = new DivaFile(file);
                    //divaFile.EncryptedData = new MemoryStream(divaFile.EncryptedData);
                    file.Close();
                    using (var save = new FileStream(args[0], FileMode.Create))
                    {
                        await serializer.SerializeAsync(save, divaFile);
                    }
                }
            }
        }
    }
}
