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
        public Discord.Discord discord = new Discord.Discord(NoCommit.CLIENT_ID, (UInt64) Discord.CreateFlags.Default);

        // The time that the application was opened
        public long startTime = DateTime.UnixEpoch.Ticks;

        // Object for the exe window
        public MainWindow()
        {
            // Creates the window with the hard coded XAML file
            InitializeComponent();

            // Sets the window title dynamically to easily show the version number
            this.Title = $"DND Panel | {VersionNum}";

            // starts an asyncronous loop to update the Rich Presence
            updateLoop();
        }

        // Called when window closes
        void MinWindow_Closing(object sender, CancelEventArgs e)
        {
            // Clears the game activity when the game is closed
            discord.ActivityManagerInstance.ClearActivity(result => { });
        }

        // Updates Discord Rich presence
        async public void updateRP()
        {
            Discord.Activity activity = new Discord.Activity
            {
                State = "DND",
                Details = "",
                Timestamps = {
                      Start = startTime,
                  },
                Assets = {
                        // Large Image name from Discord developer portal
                      LargeImage = "awaiting logo",
                        // Hovered text for Large Image
                      LargeText = $"DND Panel | {VersionNum}",
                        // Corner Icon name from Discord dev portal
                      SmallImage = "need heart image",
                        // Hovered text for the corner icon
                      SmallText = $"HP: {"not implemented"}",
                  },
                Party = {
                      Id = "foo partyID",
                      Size = {
                          CurrentSize = 1,
                          MaxSize = 4,
                      },
                  },
                Secrets = {},
                Instance = true,
            };
            discord.ActivityManagerInstance.UpdateActivity(activity, result => {});
        }

        // Updates the Discord Rich Presence status every 4 seconds
        async private void updateLoop()
        {
            while (true)
            {
                await Task.Delay(4000);
                updateRP();
            }
        }
    }
}
