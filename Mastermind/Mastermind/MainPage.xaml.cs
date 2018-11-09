using System;
using System.IO;
using Xamarin.Forms;

namespace Mastermind
{
    public partial class MainPage : ContentPage
    {
        StackLayout StackLayout_Username;
        StackLayout StackLayout_Play;
        Entry entry;

        public MainPage()
        {
            InitializeComponent();

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "options.txt");
            bool doesExist = File.Exists(fileName);

            StackLayout_Username = (StackLayout)this.FindByName<StackLayout>("StackLayout_username");
            StackLayout_Play = (StackLayout)this.FindByName<StackLayout>("StackLayout_play");

            if (doesExist == false)
            {
                StackLayout_Username.IsVisible = true;
                StackLayout_Play.IsVisible = false;
            }
            else
            {
                StackLayout_Username.IsVisible = false;
                StackLayout_Play.IsVisible = true;
            }

            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("pipo.mp3");
            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Loop = true;
        }

        private void Button_Save_Clicked(object sender, EventArgs e)
        {
            entry = (Entry)this.FindByName<Entry>("Entry");

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "options.txt");
            File.WriteAllText(fileName, "username=[" + entry.Text + "]difficulty=[normal]sound=[on]");

            StackLayout_Username.IsVisible = false;
            StackLayout_Play.IsVisible = true;
        }


        string Sound;
        private void Lecture()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "options.txt");
            string text = File.ReadAllText(fileName);
            int pFrom = text.IndexOf("sound=[") + "sound=[".Length;
            int pTo = text.LastIndexOf("]");

            if (text.Substring(pFrom, pTo - pFrom) == "off")
            {
                Sound = "off";
            }
            else
            {
                Sound = "on";
            }

            if (Sound == "on")
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load("start.mp3");
                Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
            }
        }

        private async void Button_Play_Clicked(object sender, EventArgs e)
        {
            Lecture();

            await Navigation.PushAsync(new Jeux());
        }

        private async void Button_Scores_Clicked(object sender, EventArgs e)
        {
            Lecture();

            await Navigation.PushAsync(new Scores());
        }

        private async void Button_Options_Clicked(object sender, EventArgs e)
        {
            Lecture();

            await Navigation.PushAsync(new Options());
        }
    }
}
