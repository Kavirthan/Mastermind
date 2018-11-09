using System;
using System.IO;
using Xamarin.Forms;

namespace Mastermind
{
    public partial class Options : ContentPage
    {
        Entry entry;
        Switch difficulty;
        Switch sound;

        public Options()
        {
            InitializeComponent();

            entry = (Entry)this.FindByName<Entry>("Entry");
            difficulty = (Switch)this.FindByName<Switch>("Difficulty");
            sound = (Switch)this.FindByName<Switch>("Sound");

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "options.txt");
            string text = File.ReadAllText(fileName);

            int pFrom = text.IndexOf("username=[") + "username=[".Length;
            int pTo = text.LastIndexOf("]d");

            entry.Text = text.Substring(pFrom, pTo - pFrom);

            pFrom = text.IndexOf("difficulty=[") + "difficulty=[".Length;
            pTo = text.LastIndexOf("]s");

            if (text.Substring(pFrom, pTo - pFrom) == "normal")
            {
                difficulty.IsToggled = false;
            }
            else
            {
                difficulty.IsToggled = true;
            }

            pFrom = text.IndexOf("sound=[") + "sound=[".Length;
            pTo = text.LastIndexOf("]");

            if (text.Substring(pFrom, pTo - pFrom) == "off")
            {
                sound.IsToggled = false;
            }
            else
            {
                sound.IsToggled = true;
            }
        }

        private void Button_Close_Clicked(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "options.txt");

            string text = "username=[" + entry.Text + "]difficulty=[";

            if (difficulty.IsToggled == false)
            {
                text += "normal";
            }
            else
            {
                text += "hard";
            }

            text += "]sound=[";

            if (sound.IsToggled == false)
            {
                text += "off";
            }
            else
            {
                text += "on";
            }

            text += "]";

            File.WriteAllText(fileName, text);

            Navigation.PopAsync();
        }

    }
}