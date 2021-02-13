//Created by Justin Marshall (Jumza)
//Start Date: February 13th 2021
using System;
using System.IO;
using System.Text;

namespace FFT_Bard
{
    class Program
    {
        static void Main(string[] args)
            {
            string fileName = "Jumza Song Swap.xml";


            string xmlUpper = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
            "<Patches>\n" +
            "  <Patch name=\"Swap Formation Screen Music\">\n" +
            "    <Location file=\"WORLD_WORLD_BIN\" offset=\"338C8\">";
                
            string xmlLower = "</Location>\n" +
            "  </Patch>\n" +
            "</Patches>";

            Console.WriteLine("Welcome to FFT Bard!\nThis program generates xml files that allow you to swap songs in various places in FFT (ps1 only)");
            Console.WriteLine("\nEnter song ID to change the Formation Screen's song to: ");
            string id = Console.ReadLine();

            try{
                //If the xml already exists, delete it first
                if (File.Exists(fileName)){
                    File.Delete(fileName);
                }

                //Create the xml
                using (FileStream fileS = File.Create(fileName)){
                    Byte[] contents = new UTF8Encoding(true).GetBytes(id);
                    Byte[] upper = new UTF8Encoding(true).GetBytes(xmlUpper);
                    Byte[] lower = new UTF8Encoding(true).GetBytes(xmlLower);
                    //contents = upper + contents + lower;
                    fileS.Write(upper, 0, upper.Length);
                    fileS.Write(contents, 0, contents.Length);
                    fileS.Write(lower, 0, lower.Length);
                    //With using, FileStream does not require explicit closing
                }

                Console.WriteLine("Successfully generated xml!");
                Console.WriteLine("Press any key to exit program.");
                Console.ReadKey();
            }
            catch (Exception e){
                Console.WriteLine("Exception caught: " + e.ToString());
                Console.WriteLine("Report this error, then press any key to close the program.");
            }
        }
    }
}
