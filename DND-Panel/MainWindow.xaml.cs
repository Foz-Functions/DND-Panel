using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Discord;

namespace DND_Panel {
    public partial class MainWindow : Window {

        // The Semantic Versioning number for the software
        public const string VersionNum = "0.1.0";
        
        // Object to access the Discord api
        public Discord.Discord discord;
        public Discord.ActivityManager aM;


        // The time that the application was opened
        public long startTime;

        // Object for the exe window
        public MainWindow()
        {

            // Creates the window with the hard coded XAML file
            InitializeComponent();

            // Sets the window title dynamically to easily show the version number
            this.Title = $"DND Panel | {VersionNum}";

            this.discord = new Discord.Discord(NoCommit.CLIENT_ID, (UInt64)Discord.CreateFlags.NoRequireDiscord);
            
            discord.SetLogHook(Discord.LogLevel.Debug, (level, message) =>
            {
                consoleOutput.Text += $"Log[{level}] {message}\n";
            });

            this.aM = discord.GetActivityManager();

            this.startTime = DateTime.Now.Ticks;

            // starts an asyncronous loop to update the Rich Presence
            updateLoop();
        }

        // Called when window closes
        void MinWindow_Closing(object sender, CancelEventArgs e)
        {
            // Clears the game activity when the game is closed
            aM.ClearActivity(result => {
                consoleOutput.Text += result;
            });
        }

        // Updates Discord Rich presence
        async public void updateRP()
        {
            Discord.Activity activity = new Discord.Activity
            {
                State = $"DND | {VersionNum}",
                Details = "-=-=-",
                Timestamps = {
                      Start = startTime,
                  },
                Assets = {
                        // Large Image name from Discord developer portal
                      LargeImage = "test",
                        // Hovered text for Large Image
                      LargeText = $"DND Panel | {VersionNum}",
                        // Corner Icon name from Discord dev portal
                      SmallImage = "test",
                        // Hovered text for the corner icon
                      SmallText = $"HP: {"not implemented"}",
                  },
                Party =
                  {
                    Id = "6942069420",
                    Size = 
                    {
                        CurrentSize = 1,
                        MaxSize = 4,
                    },
                  },
                Secrets =
                  {},
                Instance = true,
            };

            consoleOutput.Text += "attempting update\n";
            try
            {
                consoleOutput.Text += "Debug1\n";

                aM.UpdateActivity(activity, result =>
                {
                    if (result == Discord.Result.Ok)
                    {
                        consoleOutput.Text += $"updated\t{DateTime.Now.Ticks - startTime}\n";
                    } else
                    {
                        consoleOutput.Text += "Failed to update";
                    }
                consoleOutput.Text += "Debug2\n";
                });

                consoleOutput.Text += "Debug3\n";

            } catch (Exception e)
            {
                consoleOutput.Text += "Debug4\n";
                consoleOutput.Text += e.ToString();
            }
        }

        // Updates the Discord Rich Presence status every 5 seconds
        async private void updateLoop()
        {
            while (true)
            {
                updateRP();
                await Task.Delay(5000);
            }
        }
    }
}
