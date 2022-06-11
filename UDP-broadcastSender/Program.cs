using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace UDP_broadcastSender
{
    class Program
    {
        //Unique navn for hver windData
        //This is used to differentiate between who is sending the data 
        private static string MyWindDataGenerator = "WindGenerator";
        static void Main(string[] args)
        {
            Console.WriteLine("UDP -broadcast sender(client)!");

            //vha. UDP-broadcast kan sende falske vindmålinger
         

            // UdpClient bliver omringet af en using,
            // for automatisk rydde op efter den er blevet anvendt
            //socket bliver reused og en while loop er indeni her,
            //loopet fortsætter indtil console bliver lukket.
            using (UdpClient socket = new UdpClient())
            {
                //sender vindhastighed, så længe den ikke bliver lukket
                while (true)
                {
                    //opret et nyt WindData object der har hastighed og retning på,
                    //og det unikke navn fra the WindGenerator
                    WindDataGenerator windDataGenerator = new WindDataGenerator();

                    int speed = windDataGenerator.NextSpeed();
                    string direction = windDataGenerator.NextDirection();
                    Wind wind = new Wind(1, speed, direction);
                    //serializer mit wind obj, jeg laver det om
                    //til en string i JSON format
                    string windSerialize = JsonConvert.SerializeObject(wind);
                    //readline hvor message kan indtastes - derefter press enter 
                    Console.ReadLine();
                    //det er message der skal sendes til server
                    //string laver jeg om til et byte array
                    byte[] data = Encoding.UTF8.GetBytes(windSerialize);
                    //- Clienten vil gerne sende en besked til serveren
                    //byte array, og længden på denne, modtagerens IP adresse
                    //den port den skal benytte
                    socket.Send(data, data.Length, "127.0.0.1", 5005);
                }
            }

        }
    }
}
