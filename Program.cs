//Created by Justin Marshall (Jumza)
//Start Date: February 13th 2021
using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace FFT_Bard
{
    //Offsets:
    //In WORLD_WORLD_BIN
    //Party Screen: 338C8
    //Memory Card Menu: 4F2D4
    //The following have their song ID set to r19 earlier than the above (IE way before the jal)
    //Shop: 535F4
    //Soldier's Office: 535D0
    //Fur Shop: 535B0

    //The following require changes to WLDCORE, not WORLD
    //Pub, Brave Story, World Map, Tutorial, Formation Screen
    //Formation Screen has BATTLE.BIN loaded instead of WORLD.BIN
    //This means that logical music change structure probably needs to take place in SCUS

    //Title screen uses song based on currently playing Movie (STR)
    class Program
    {
        static string fileName = "Jumza Song Swap.xml";

        static void Main(string[] args)
            {
            string xmlUpper = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
            "<Patches>\n" +
            "  <Patch name=\"Swap Music\">\n" +
            "    <Location file=\"WORLD_WORLD_BIN\" offset=\"";

            string xmlMiddle = "\">";
                
            string xmlLower = "</Location>\n" +
            "  </Patch>\n" +
            "</Patches>";

            Console.WriteLine("Welcome to FFT-Song-Swapper!\nThis program generates xml files that allow you to swap songs in various places in FFT (ps1 only)");
            Console.WriteLine("Please select the number of the area you'd like to change the song of:\n" +
            "(1) Party Screen\n" +
            "(2) Memory Card Menu\n" +
            "(3) Shop\n" +
            "(4) Soldier's Office\n" +
            "(5) Fur Shop\n");

            string area = "";
            string areaName = "";
            while(true){
                area = Console.ReadLine();
                if (area != "1" && area != "2" && area != "3" && area != "4" && area != "5"){
                    Console.WriteLine("Invalid input, please type 1, 2, 3, 4, or 5 and hit enter (no other characters)");
                }
                else{
                    break;
                }
            }

            string offset = "";

            switch(area){
                case "1":
                    areaName = "Party Screen";
                    offset = "338C8";
                    break;
                case "2":
                    areaName = "Memory Card Menu";
                    offset = "4F2D4";
                    break;
                case "3":
                    areaName = "Shop";
                    offset = "535F4";
                    break;
                case "4":
                    areaName = "Soldier Office";
                    offset = "535D0";
                    break;
                case "5":
                    areaName = "Fur Shop";
                    offset = "535B0";
                    break;
                default:
                    break;
            }

            Console.WriteLine("\nEnter song ID to change the " + areaName + "'s song to: (Valid ID's are hex from 01-63)\n");
            string id = "";

            while(true){
                id = Console.ReadLine(); //Do not try to add 0x, TryParse reads hex just fine
                if (UInt32.TryParse(id, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out UInt32 idVal)){
                    if (idVal >= 0x01 && idVal <= 0x63){
                        break;
                    }
                    else{
                        Console.WriteLine("Given ID out of range! (Valid ID's are hex from 01-63)\n");
                    }
                }
                else{
                    Console.WriteLine("Could not parse " + id + " to int, try again:\n");
                }
            }

            //If it's a single character, we have to add a leading 0
            if(id.Length == 1){
                id = "0" + id;
            }

            try{
                //If the xml already exists, delete it first
                if (File.Exists(fileName)){
                    File.Delete(fileName);
                    //Console.Writeline("Warning: Old Jumza Song Swap.xml detected and deleted");
                }

                //Create the xml
                using (FileStream fileS = File.Create(fileName)){
                    Byte[] offsetContent = new UTF8Encoding(true).GetBytes(offset);
                    Byte[] idContent = new UTF8Encoding(true).GetBytes(id);
                    Byte[] upper = new UTF8Encoding(true).GetBytes(xmlUpper);
                    Byte[] middle = new UTF8Encoding(true).GetBytes(xmlMiddle);
                    Byte[] lower = new UTF8Encoding(true).GetBytes(xmlLower);
                    fileS.Write(upper, 0, upper.Length);
                    fileS.Write(offsetContent, 0, offsetContent.Length);
                    fileS.Write(middle, 0, middle.Length);
                    fileS.Write(idContent, 0, idContent.Length);
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
