/*
    The way everything is done is very inefficient.
    Someday, I'll improve on it. But for now, I'm gonna leave it be.
    Anyone is welcome to make this better :)
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sys = Cosmos.System;
using System.IO;
using System.Linq;

namespace DogOSInstall
{
    public enum SetupStage
    {
        Exit = -1,
        Welcome = 0,
        Format = 1,
        PickDisk = 2,
        FormatDiskQuestion = 3,
        FormatDisk = 4,
        InstallDogOS = 5
    }

    public class Kernel : Sys.Kernel
    {
        public static SetupStage setup_stage = SetupStage.Welcome;
        public Sys.FileSystem.CosmosVFS vfs;
        public int drive_index = -1;
        public List<DriveInfo> drives;

        public void Header()
        {
            Console.Write("                                                                                "); // 1
            Console.Write("    DogOS Installer                                                             "); // 2
            Console.Write("    ===============                                                             "); // 3
            Console.Write("                                                                                "); // 4
        }

        public void WelcomeScreen()
        {
            setup_stage = SetupStage.Welcome;
            Console.BackgroundColor = ConsoleColor.Blue;
            // Wanna do it this way for easy visualization. :)
            // Mode : 80x25
            Header();
            Console.Write("    Welcome to the DogOS installer. This setup program prepares your            "); // 5
            Console.Write("    computer to be used with DogOS.                                             "); // 6
            Console.Write("                                                                                "); // 7
            Console.Write("      • To enter setup, press 'Enter'.                                          ".Replace('•', (char)7)); // 8
            Console.Write("                                                                                "); // 9
            Console.Write("    Note: Do not install DogOS on your personal computer! Any hard drives in    "); // 10
            Console.Write("          your PC could accidentally be formatted, and data could be lost.      "); // 11
            Console.Write("          If you want to install DogOS on real hardware, please use a old PC    "); // 12
            Console.Write("          that you could use for testing / using DogOS safely.                  "); // 13
            Console.Write("          Press 'esc' to stop setup.                                            "); // 14
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
            Console.Write("Enter - Continue  Esc - Exit                                                   "); // 25
            // One character is excluded since it would scroll the console
        }

        public void Welcome()
        {
            var looking_for_key = true;
            WelcomeScreen();

            while (looking_for_key)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    looking_for_key = false;
                    setup_stage = SetupStage.Exit;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    looking_for_key = false;
                    setup_stage = SetupStage.Format;
                }
            }
        }

        public void FormatScreen()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Blue;
            Header();
            Console.Write("    This part of the setup will format your drive for use with DogOS.           "); // 5
            Console.Write("    On the next screen, you can choose the drive you want to format.            "); // 6
            Console.Write("                                                                                "); // 7
            Console.Write("    Make sure the drive has already been formatted with FAT32.                  "); // 8
            Console.Write("    Check the website for more information.                                     "); // 9
            Console.Write("    https://DogOSdev.github.io/wiki/formatting-drive/                           "); // 10
            Console.Write("                                                                                "); // 11
            Console.Write("                                                                                "); // 12
            Console.Write("                                                                                "); // 13
            Console.Write("                                                                                "); // 14
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
            Console.Write("Enter - Continue  Esc - Go back                                                "); // 25
        }

        public void Format()
        {
            var looking_for_key = true;
            FormatScreen();

            while (looking_for_key)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    looking_for_key = false;
                    setup_stage = SetupStage.Welcome;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    looking_for_key = false;
                    setup_stage = SetupStage.PickDisk;
                }
            }
        }

        public void PickDiskScreen()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Blue;
            Header();
            Console.Write("    Please select a drive by the number given.                                  "); // 5

            for (int i = 0; i < drives.Count; i++)
            {
                var str = $"        • {i}. {drives[i].VolumeLabel} : {drives[i].TotalSize / 1048576} mb total.".Replace('•', (char)7);
                str = str + new string(' ', 80 - str.Length);
                Console.Write(str);
            }

            for (int i = 0; i < 25 - (5 + drives.Count) - 1; i++)
            {
                Console.Write("                                                                                ");
            }

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Enter - Continue  Esc - Go back                                                "); // 25

            var drive = 0;
            var looking_for_key = true;
            
            while (looking_for_key)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    looking_for_key = false;
                    setup_stage = SetupStage.Exit;
                }
                else if (char.IsDigit(key.KeyChar))
                {
                    if (Convert.ToInt32(char.GetNumericValue(key.KeyChar)) < drives.Count)
                    {
                        looking_for_key = false;
                        drive = Convert.ToInt32(char.GetNumericValue(key.KeyChar));
                        setup_stage = SetupStage.FormatDiskQuestion;
                    }
                }
            }

            drive_index = drive;
        }

        public void PickDisk()
        {
            PickDiskScreen();
        }

        public void FormatDiskQuestionScreen()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Blue;
            Header();
            Console.Write("    This will format your drive and change the drive's MBR, this is             "); // 5
            Console.Write("    irreversible! Please be sure this is the correct drive...                   "); // 6
            var drive = drives[drive_index];
            var str = $"        • {drive.VolumeLabel} {drive.AvailableFreeSpace / 1048576}/{drive.TotalSize / 1048576} mb".Replace('•', (char)7);
            str = str + new string(' ', 80 - str.Length);
            Console.Write(str); // 7
            Console.Write("                                                                                "); // 8
            Console.Write("                                                                                "); // 9
            Console.Write("                                                                                "); // 10
            Console.Write("                                                                                "); // 11
            Console.Write("                                                                                "); // 12
            Console.Write("                                                                                "); // 13
            Console.Write("                                                                                "); // 14
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
            Console.Write("Enter - Continue  Esc - Go back                                                "); // 25

            var looking_for_key = true;

            while (looking_for_key)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    looking_for_key = false;
                    drive_index = -1;
                    setup_stage = SetupStage.PickDisk;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    looking_for_key = false;
                    setup_stage = SetupStage.FormatDisk;
                }
            }
        }

        //TODO: Remove non-write statements in FormatDiskQuestion
        public void FormatDiskQuestion()
        {
            FormatDiskQuestionScreen();
        }

        //TODO: Add FormatDiskScreen
        public void FormatDisk()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Blue;
            Header();
            Console.Write("    Please wait while we format your drive...                                   "); // 5
            Console.Write("                                                                                "); // 6
            Console.Write("                                                                                "); // 7
            Console.Write("                                                                                "); // 8
            Console.Write("                                                                                "); // 9
            Console.Write("                                                                                "); // 10
            Console.Write("                                                                                "); // 11
            Console.Write("                                                                                "); // 12
            Console.Write("                                                                                "); // 13
            Console.Write("                                                                                "); // 14
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
            Console.Write("                                                                               "); // 25
            vfs.Format(Convert.ToString(drives[drive_index].Name[0]), "FAT32", false);

            try
            {
                File.WriteAllText($"{Convert.ToString(drives[drive_index].Name[0])}:\\setup.ini",
                    "[INSTALLER]\n" +
                    "STAGE=FORMATDISK\n" +
                    "FORMATTEDDISK=1\n" +
                    $"DRIVE={Convert.ToString(drives[drive_index].Name[0])}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"A exception has occurred while writing to 'setup.ini'.\n{e}\n\nPress any key to restart the computer.");
                Console.ReadKey();
                Sys.Power.Reboot();
            }
        }

        public SetupStage StringToStage(string str)
        {
            SetupStage ret;
            switch (str)
            {
                case "EXIT":
                    ret = SetupStage.Exit;
                    break;
                case "FORMATDISK":
                    ret = SetupStage.FormatDisk;
                    break;
                case "FORMAT":
                    ret = SetupStage.Format;
                    break;
                case "FORMATDISKQUESTION":
                    ret = SetupStage.FormatDiskQuestion;
                    break;
                case "PICKDISK":
                    ret = SetupStage.PickDisk;
                    break;
                case "WELCOME":
                    ret = SetupStage.Welcome;
                    break;
                case "INSTALLDOGOS":
                    ret = SetupStage.InstallDogOS;
                    break;
                default:
                    ret = SetupStage.Welcome;
                    break;
            }

            return ret;
        }

        protected override void BeforeRun()
        {
            vfs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(vfs);

            Console.Clear();
            Console.WriteLine("Please wait while we initialize the installer...");

            Sys.Graphics.VGAScreen.SetFont(Fonts.AVGA2, 16);

            var drives = new List<DriveInfo>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (!drive.VolumeLabel.Contains("CDROM"))
                {
                    drives.Add(drive);
                }
            }
            this.drives = drives;

            foreach (var drive in drives)
            {
                var dir = Directory.GetFiles($"{drive.Name}");

                if (dir.Contains($"{drive.Name}setup.ini"))
                {
                    if (IniFile.Read("setup.ini", "INSTALLER", "FORMATTEDDISK", "0") == "1")
                    {
                        int num;
                        if (int.TryParse(IniFile.Read("setup.ini", "INSTALLER", "DRIVE", "-1"), out num))
                        {
                            drive_index = num;
                        }
                        else
                        {
                            drive_index = -1;
                        }

                        if (drive_index != -1)
                        {
                            vfs.SetFileSystemLabel($"{drive_index}", "DOGOS");
                        }

                        setup_stage = StringToStage(IniFile.Read("setup.ini", "INSTALLER", "STAGE", "WELCOME"));

                        if (setup_stage != SetupStage.FormatDisk)
                        {
                            Console.WriteLine("Something happened on our end.\nPlease re-format the disk using external tools.\n\nRemove any installation media and press any key to restart.");
                            Console.ReadKey();
                            Sys.Power.Reboot();
                        }

                        // File.Delete($"{drive_index}:\\setup.ini");
                        setup_stage = SetupStage.InstallDogOS;
                    }
                    
                }
            }

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.CursorVisible = false;
            Console.Clear();
        }

        protected override void Run()
        {
            switch (setup_stage)
            {
                case SetupStage.Exit:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();

                    Console.WriteLine("Please remove the setup media and press any key to restart.");
                    Console.ReadKey(true);
                    Sys.Power.Reboot();
                    break;
                case SetupStage.Welcome:
                    Welcome();
                    break;
                case SetupStage.Format:
                    Format();
                    break;
                case SetupStage.PickDisk:
                    PickDisk();
                    break;
                case SetupStage.FormatDiskQuestion:
                    FormatDiskQuestion();
                    break;
                case SetupStage.FormatDisk:
                    FormatDisk();
                    break;
                case SetupStage.InstallDogOS:
                    Console.WriteLine("Install DogOS here :)");
                    Console.ReadLine();
                    break;
                default:
                    throw new Exception("Unknown Setup Stage.");
                    break;
            }

            Console.BackgroundColor = ConsoleColor.Gray;

        }
    }
}
