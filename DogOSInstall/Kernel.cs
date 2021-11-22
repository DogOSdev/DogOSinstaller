using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace DogOSInstall
{
    public class Kernel : Sys.Kernel
    {
        public void WelcomeScreen()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            // Wanna do it this way for easy visualization. :)
            // Mode : 80x25
            Console.Write("                                                                                "); // 1
            Console.Write("    DogOS Installer                                                             "); // 2
            Console.Write("    ===============                                                             "); // 3
            Console.Write("                                                                                "); // 4
            Console.Write("    Welcome to the DogOS installer. This setup program prepares your            "); // 5
            Console.Write("    computer to be used with DogOS.                                             "); // 6
            Console.Write("                                                                                "); // 7
            Console.Write("      • To enter setup, press 'Enter'.                                          "); // 8
            Console.Write("                                                                                "); // 9
            Console.Write("    Note: Do not install DogOS on your personal computer! Any hard drives in    "); // 10
            Console.Write("          your pc could corrupt, be formatted, and data could be lost.          "); // 11
            Console.Write("          If you want to install DogOS on real hardware, please use a old pc    "); // 12
            Console.Write("          that you could use for testing / using DogOS safely.                  "); // 13
            Console.Write("          Exiting setup during this screen is safe. Press 'esc' to stop setup.  "); // 14
            Console.Write("                                                                                "); // 15
            Console.Write("                                                                                "); // 16
            Console.Write("                                                                                "); // 17
            Console.Write("                                                                                "); // 18
            Console.Write("                                                                                "); // 19
            Console.Write("                                                                                "); // 20
            Console.Write("                                                                                "); // 21
            Console.Write("                                                                                "); // 22
            Console.Write("                                                                                "); // 23
            Console.Write("                                                                                "); // 24
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Enter - Continue     Esc - Exit                                                "); // 25
        }

        protected override void BeforeRun()
        {
            Console.WriteLine("Please wait while we initialize the installer...");
            Sys.Graphics.VGAScreen.SetFont(Fonts.AVGA2, 16);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.CursorVisible = false;
            Console.Clear();
        }

        protected override void Run()
        {
            var exit_setup = false;
            var looking_for_key = true;
            WelcomeScreen();

            while(looking_for_key)
            {
                var key = Console.ReadKey(true);

                if(key.Key == ConsoleKey.Escape)
                {
                    looking_for_key = false;
                    exit_setup = true;
                }
                else if(key.Key == ConsoleKey.Enter)
                {
                    looking_for_key = false;
                }
            }

            if(exit_setup)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                Console.WriteLine("Please remove the setup media and press any key to restart.");
                Console.ReadKey(true);
                Sys.Power.Reboot();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();
            Console.WriteLine("Go on with setup.");
            Console.ReadLine();
        }
    }
}
