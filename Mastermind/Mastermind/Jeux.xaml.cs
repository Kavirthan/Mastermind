using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;


namespace Mastermind
{
    public partial class Jeux : ContentPage
    {
        #region Code Couleur Hexadecimal
        /*
        blanc	#ebe8e3
        bleu 	#518ec5
        jaune 	#dbcf23
        noir 	#4b4e53
        rouge 	#e44e35
        vert 	#54b36f

        blanc 	#ebe8e3
        bleu 	#518ec5
        jaune 	#dbcf23
        orange 	#cd9900
        rose 	#ff80ff
        rouge 	#e44e35
        vert 	#54b36f
        violet	#a84d9c
        */
        #endregion

        TimeSpan TotalTime = TimeSpan.FromSeconds(0);
        Button Timer_txt;
        bool isPlaying = true;

        SQLiteConnection database;
        int Score;

        StackLayout Pause;
        ScrollView AllLigne;

        StackLayout Ligne_add;
        bool firstLigne = true;

        string[] Couleurs_6 = { "ebe8e3", "518ec5", "dbcf23", "4b4e53", "e44e35", "54b36f" };
        string[] Couleurs_8 = { "ebe8e3", "518ec5", "dbcf23", "cd9900", "ff80ff", "e44e35", "54b36f", "a84d9c" };

        StackLayout Stk_Bottom;

        string Name;
        string Difficulty;
        string Sound;

        List<string[]> Repeat = new List<string[]>();

        string[] Couleurs = new string[4]; // Couleurs choisis aléatoirement

        string[] Resultat = { "1e2b3c", "1e2b3c", "1e2b3c", "1e2b3c" }; // 4 Couleurs du résultat (noir, blanc)
        string[] Choix = { "1e2b3c", "1e2b3c", "1e2b3c", "1e2b3c" }; // Couleurs des choix du joueurs

        public SQLite.SQLiteConnection GetConnection()
        {
            SQLiteConnection sqlitConnection;
            var sqliteFilename = "Scores.db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
            sqlitConnection = new SQLite.SQLiteConnection(path);
            return sqlitConnection;
        }

        public Jeux()
        {
            InitializeComponent();

            Select_Couleurs();

            Timer_txt = (Button)this.FindByName<Button>("Timer");
            Start_Timer();

            Pause = (StackLayout)this.FindByName<StackLayout>("PauseLayout");
            AllLigne = (ScrollView)this.FindByName<ScrollView>("ScrollView");

            Ligne_add = (StackLayout)this.FindByName<StackLayout>("Ligne");

            Stk_Bottom = (StackLayout)this.FindByName<StackLayout>("StackLayout_Bottom");

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "options.txt");
            string text = File.ReadAllText(fileName);

            int pFrom = text.IndexOf("username=[") + "username=[".Length;
            int pTo = text.LastIndexOf("]d");

            Name = text.Substring(pFrom, pTo - pFrom);

            pFrom = text.IndexOf("difficulty=[") + "difficulty=[".Length;
            pTo = text.LastIndexOf("]s");

            if (text.Substring(pFrom, pTo - pFrom) == "normal")
            {
                Difficulty = "normal";
                Score = 1000;
            }
            else
            {
                Difficulty = "hard";
                Score = 2000;
            }

            pFrom = text.IndexOf("sound=[") + "sound=[".Length;
            pTo = text.LastIndexOf("]");

            if (text.Substring(pFrom, pTo - pFrom) == "off")
            {
                Sound = "off";
            }
            else
            {
                Sound = "on";
            }

        }

        private void Select_Couleurs()
        {
            Random aleatoire = new Random();

            for (int i = 0; i <= 3; i++)
            {
                int index = aleatoire.Next(6);
                string couleur;
                if (Difficulty == "normal")
                {
                    couleur = Couleurs_6[index];
                }
                else
                {
                    couleur = Couleurs_8[index];
                }

                Couleurs[i] = couleur;
            }
        }

        private void Start_Timer()
        {
            TimeSpan TimeElement = new TimeSpan();
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                TotalTime = TotalTime + TimeElement.Add(new TimeSpan(0, 0, 1));
                Timer_txt.Text = TotalTime.ToString(@"mm\:ss");
                // returning true will fire task again in 2 minutes.
                return isPlaying;
            });

        }
        private void Button_Pause_Clicked(object sender, EventArgs e)
        {
            if (isPlaying == true)
            {
                isPlaying = false;
                AllLigne.IsVisible = false;
                Pause.IsVisible = true;
            }
            else
            {
                Pause.IsVisible = false;
                isPlaying = true;
                AllLigne.IsVisible = true;
                Start_Timer();
            }
        }

        private async void Button_Close_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Exit", "Do you want to exit the game ?", "Yes", "No"))
            {
                if (Sound == "on")
                {
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load("loser.mp3");
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
                }
                var x = Navigation.PopAsync();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Choix 1
        private void Choix_Couleur_1(object sender, EventArgs e)
        {
            Stk_Bottom.Children.RemoveAt(0);

            StackLayout stacklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            if (Difficulty == "normal")
            {

                Button Blanc = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_1;

                Button Bleu = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_1;
                Button Jaune = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_1;
                Button Noir = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("4b4e53") // Noir
                };
                Noir.Clicked += Noir_1;
                Button Rouge = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_1;
                Button Vert = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_1;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Noir);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);

            }
            else
            {
                stacklayout.Padding = new Thickness(0, 6, 0, 6);

                Button Blanc = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_1;
                Button Bleu = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_1;
                Button Jaune = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_1;
                Button Orange = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("cd9900") // Orange
                };
                Orange.Clicked += Orange_1;
                Button Rose = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ff80ff") // Rose
                };
                Rose.Clicked += Rose_1;
                Button Rouge = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_1;
                Button Vert = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_1;
                Button Violet = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("a84d9c") // Violet
                };
                Violet.Clicked += Violet_1;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Orange);
                stacklayout.Children.Add(Rose);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);
                stacklayout.Children.Add(Violet);
            }

            Stk_Bottom.Children.Add(stacklayout);
        }
        private void Blanc_1(object sender, EventArgs e)
        {
            Choix[0] = "ebe8e3";
            Bottom();
        }
        private void Bleu_1(object sender, EventArgs e)
        {
            Choix[0] = "518ec5";
            Bottom();
        }
        private void Jaune_1(object sender, EventArgs e)
        {
            Choix[0] = "dbcf23";
            Bottom();
        }
        private void Noir_1(object sender, EventArgs e)
        {
            Choix[0] = "4b4e53";
            Bottom();
        }
        private void Orange_1(object sender, EventArgs e)
        {
            Choix[0] = "cd9900";
            Bottom();
        }
        private void Rose_1(object sender, EventArgs e)
        {
            Choix[0] = "ff80ff";
            Bottom();
        }
        private void Rouge_1(object sender, EventArgs e)
        {
            Choix[0] = "e44e35";
            Bottom();
        }
        private void Vert_1(object sender, EventArgs e)
        {
            Choix[0] = "54b36f";
            Bottom();
        }
        private void Violet_1(object sender, EventArgs e)
        {
            Choix[0] = "a84d9c";
            Bottom();
        }
        #endregion

        #region Choix 2
        private void Choix_Couleur_2(object sender, EventArgs e)
        {
            Stk_Bottom.Children.RemoveAt(0);

            StackLayout stacklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            if (Difficulty == "normal")
            {
                Button Blanc = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_2;
                Button Bleu = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_2;
                Button Jaune = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_2;
                Button Noir = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("4b4e53") // Noir
                };
                Noir.Clicked += Noir_2;
                Button Rouge = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_2;
                Button Vert = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_2;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Noir);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);
            }
            else
            {
                stacklayout.Padding = new Thickness(0, 6, 0, 6);

                Button Blanc = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_2;
                Button Bleu = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_2;
                Button Jaune = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_2;
                Button Orange = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("cd9900") // Orange
                };
                Orange.Clicked += Orange_2;
                Button Rose = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ff80ff") // Rose
                };
                Rose.Clicked += Rose_2;
                Button Rouge = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_2;
                Button Vert = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_2;
                Button Violet = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("a84d9c") // Violet
                };
                Violet.Clicked += Violet_2;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Orange);
                stacklayout.Children.Add(Rose);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);
                stacklayout.Children.Add(Violet);
            }

            Stk_Bottom.Children.Add(stacklayout);
        }
        private void Blanc_2(object sender, EventArgs e)
        {
            Choix[1] = "ebe8e3";
            Bottom();
        }
        private void Bleu_2(object sender, EventArgs e)
        {
            Choix[1] = "518ec5";
            Bottom();
        }
        private void Jaune_2(object sender, EventArgs e)
        {
            Choix[1] = "dbcf23";
            Bottom();
        }
        private void Noir_2(object sender, EventArgs e)
        {
            Choix[1] = "4b4e53";
            Bottom();
        }
        private void Orange_2(object sender, EventArgs e)
        {
            Choix[1] = "cd9900";
            Bottom();
        }
        private void Rose_2(object sender, EventArgs e)
        {
            Choix[1] = "ff80ff";
            Bottom();
        }
        private void Rouge_2(object sender, EventArgs e)
        {
            Choix[1] = "e44e35";
            Bottom();
        }
        private void Vert_2(object sender, EventArgs e)
        {
            Choix[1] = "54b36f";
            Bottom();
        }
        private void Violet_2(object sender, EventArgs e)
        {
            Choix[1] = "a84d9c";
            Bottom();
        }
        #endregion

        #region Choix 3
        private void Choix_Couleur_3(object sender, EventArgs e)
        {
            Stk_Bottom.Children.RemoveAt(0);

            StackLayout stacklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            if (Difficulty == "normal")
            {
                Button Blanc = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_3;
                Button Bleu = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_3;
                Button Jaune = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_3;
                Button Noir = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("4b4e53") // Noir
                };
                Noir.Clicked += Noir_3;
                Button Rouge = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_3;
                Button Vert = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_3;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Noir);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);
            }
            else
            {
                stacklayout.Padding = new Thickness(0, 6, 0, 6);

                Button Blanc = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_3;
                Button Bleu = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_3;
                Button Jaune = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_3;
                Button Orange = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("cd9900") // Orange
                };
                Orange.Clicked += Orange_3;
                Button Rose = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ff80ff") // Rose
                };
                Rose.Clicked += Rose_3;
                Button Rouge = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_3;
                Button Vert = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_3;
                Button Violet = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("a84d9c") // Violet
                };
                Violet.Clicked += Violet_3;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Orange);
                stacklayout.Children.Add(Rose);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);
                stacklayout.Children.Add(Violet);
            }

            Stk_Bottom.Children.Add(stacklayout);
        }
        private void Blanc_3(object sender, EventArgs e)
        {
            Choix[2] = "ebe8e3";
            Bottom();
        }
        private void Bleu_3(object sender, EventArgs e)
        {
            Choix[2] = "518ec5";
            Bottom();
        }
        private void Jaune_3(object sender, EventArgs e)
        {
            Choix[2] = "dbcf23";
            Bottom();
        }
        private void Noir_3(object sender, EventArgs e)
        {
            Choix[2] = "4b4e53";
            Bottom();
        }
        private void Orange_3(object sender, EventArgs e)
        {
            Choix[2] = "cd9900";
            Bottom();
        }
        private void Rose_3(object sender, EventArgs e)
        {
            Choix[2] = "ff80ff";
            Bottom();
        }
        private void Rouge_3(object sender, EventArgs e)
        {
            Choix[2] = "e44e35";
            Bottom();
        }
        private void Vert_3(object sender, EventArgs e)
        {
            Choix[2] = "54b36f";
            Bottom();
        }
        private void Violet_3(object sender, EventArgs e)
        {
            Choix[2] = "a84d9c";
            Bottom();
        }
        #endregion

        #region Choix 4
        private void Choix_Couleur_4(object sender, EventArgs e)
        {
            Stk_Bottom.Children.RemoveAt(0);

            StackLayout stacklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            if (Difficulty == "normal")
            {
                Button Blanc = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_4;
                Button Bleu = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_4;
                Button Jaune = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_4;
                Button Noir = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("4b4e53") // Noir
                };
                Noir.Clicked += Noir_4;
                Button Rouge = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_4;
                Button Vert = new Button
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 25,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_4;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Noir);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);
            }
            else
            {
                stacklayout.Padding = new Thickness(0, 6, 0, 6);

                Button Blanc = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ebe8e3"), // blanc
                };
                Blanc.Clicked += Blanc_4;
                Button Bleu = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("518ec5") // bleu
                };
                Bleu.Clicked += Bleu_4;
                Button Jaune = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("dbcf23") // Jaune
                };
                Jaune.Clicked += Jaune_4;
                Button Orange = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("cd9900") // Orange
                };
                Orange.Clicked += Orange_4;
                Button Rose = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("ff80ff") // Rose
                };
                Rose.Clicked += Rose_4;
                Button Rouge = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("e44e35") // Rouge
                };
                Rouge.Clicked += Rouge_4;
                Button Vert = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("54b36f") // Vert
                };
                Vert.Clicked += Vert_4;
                Button Violet = new Button
                {
                    HeightRequest = 38,
                    WidthRequest = 38,
                    CornerRadius = 19,
                    BorderWidth = 2,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex("a84d9c") // Violet
                };
                Violet.Clicked += Violet_4;

                stacklayout.Children.Add(Blanc);
                stacklayout.Children.Add(Bleu);
                stacklayout.Children.Add(Jaune);
                stacklayout.Children.Add(Orange);
                stacklayout.Children.Add(Rose);
                stacklayout.Children.Add(Rouge);
                stacklayout.Children.Add(Vert);
                stacklayout.Children.Add(Violet);
            }

            Stk_Bottom.Children.Add(stacklayout);
        }
        private void Blanc_4(object sender, EventArgs e)
        {
            Choix[3] = "ebe8e3";
            Bottom();
        }
        private void Bleu_4(object sender, EventArgs e)
        {
            Choix[3] = "518ec5";
            Bottom();
        }
        private void Jaune_4(object sender, EventArgs e)
        {
            Choix[3] = "dbcf23";
            Bottom();
        }
        private void Noir_4(object sender, EventArgs e)
        {
            Choix[3] = "4b4e53";
            Bottom();
        }
        private void Orange_4(object sender, EventArgs e)
        {
            Choix[3] = "cd9900";
            Bottom();
        }
        private void Rose_4(object sender, EventArgs e)
        {
            Choix[3] = "ff80ff";
            Bottom();
        }
        private void Rouge_4(object sender, EventArgs e)
        {
            Choix[3] = "e44e35";
            Bottom();
        }
        private void Vert_4(object sender, EventArgs e)
        {
            Choix[3] = "54b36f";
            Bottom();
        }
        private void Violet_4(object sender, EventArgs e)
        {
            Choix[3] = "a84d9c";
            Bottom();
        }
        #endregion
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Repeat
        private void Ligne_1(object sender, EventArgs e)
        {
            string[] x = Repeat[0];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_2(object sender, EventArgs e)
        {
            string[] x = Repeat[1];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_3(object sender, EventArgs e)
        {
            string[] x = Repeat[2];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_4(object sender, EventArgs e)
        {
            string[] x = Repeat[3];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_5(object sender, EventArgs e)
        {
            string[] x = Repeat[4];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_6(object sender, EventArgs e)
        {
            string[] x = Repeat[5];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_7(object sender, EventArgs e)
        {
            string[] x = Repeat[6];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_8(object sender, EventArgs e)
        {
            string[] x = Repeat[7];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_9(object sender, EventArgs e)
        {
            string[] x = Repeat[8];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_10(object sender, EventArgs e)
        {
            string[] x = Repeat[9];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        private void Ligne_11(object sender, EventArgs e)
        {
            string[] x = Repeat[10];
            Choix[0] = x[0];
            Choix[1] = x[1];
            Choix[2] = x[2];
            Choix[3] = x[3];
            Bottom();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Bottom()
        {
            Stk_Bottom.Children.RemoveAt(0);

            StackLayout stacklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            if (Choix[0] != "1e2b3c" && Choix[1] != "1e2b3c" && Choix[2] != "1e2b3c" && Choix[3] != "1e2b3c")
            {
                Button button_Fake = new Button
                {
                    IsEnabled = false,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    BackgroundColor = Color.FromHex("354660")
                };
                stacklayout.Children.Add(button_Fake);
            }

            Button button_1 = new Button
            {
                HeightRequest = 50,
                WidthRequest = 50,
                CornerRadius = 25,
                BorderWidth = 2,
                BorderColor = Color.FromHex("ebe8e3"),
                BackgroundColor = Color.FromHex(Choix[0])
            };
            button_1.Clicked += Choix_Couleur_1;

            Button button_2 = new Button
            {
                HeightRequest = 50,
                WidthRequest = 50,
                CornerRadius = 25,
                BorderWidth = 2,
                BorderColor = Color.FromHex("ebe8e3"),
                BackgroundColor = Color.FromHex(Choix[1])
            };
            button_2.Clicked += Choix_Couleur_2;

            Button button_3 = new Button
            {
                HeightRequest = 50,
                WidthRequest = 50,
                CornerRadius = 25,
                BorderWidth = 2,
                BorderColor = Color.FromHex("ebe8e3"),
                BackgroundColor = Color.FromHex(Choix[2])
            };
            button_3.Clicked += Choix_Couleur_3;

            Button button_4 = new Button
            {
                HeightRequest = 50,
                WidthRequest = 50,
                CornerRadius = 25,
                BorderWidth = 2,
                BorderColor = Color.FromHex("ebe8e3"),
                BackgroundColor = Color.FromHex(Choix[3])
            };
            button_4.Clicked += Choix_Couleur_4;

            stacklayout.Children.Add(button_1);
            stacklayout.Children.Add(button_2);
            stacklayout.Children.Add(button_3);
            stacklayout.Children.Add(button_4);

            if (Choix[0] != "1e2b3c" && Choix[1] != "1e2b3c" && Choix[2] != "1e2b3c" && Choix[3] != "1e2b3c")
            {
                Button button_OK = new Button
                {
                    Text = "OK",
                    HeightRequest = 50,
                    WidthRequest = 50,
                    CornerRadius = 5,
                    BackgroundColor = Color.FromHex("ebe8e3"),
                    FontFamily = Device.RuntimePlatform == Device.iOS ? "TitanOne-Regular" : Device.RuntimePlatform == Device.Android ? "TitanOne-Regular.ttf#TitanOne-Regular" : null
                };
                button_OK.Clicked += Button_OK_Clicked;
                stacklayout.Children.Add(button_OK);
            }

            Stk_Bottom.Children.Add(stacklayout);
        }

        private async void Button_OK_Clicked(object sender, EventArgs e)
        {
            if (firstLigne)
            {
                Ligne_add.Children.RemoveAt(0);
                firstLigne = false;
            }

            bool win = false;

            if (Ligne_add.Children.Count() < 12)
            {
                StackLayout stacklayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center
                };

                Button frameNumber = new Button
                {
                    CornerRadius = 5,
                    WidthRequest = 55,
                    Text = Convert.ToString(Ligne_add.Children.Count() + 1),
                    BackgroundColor = Color.FromHex("ebe8e3"),
                    TextColor = Color.FromHex("4b4e53"),
                };

                string[] x = { Choix[0], Choix[1], Choix[2], Choix[3] };

                Repeat.Add(x);

                int RepeatLigne = Ligne_add.Children.Count() + 1;
                #region Repeat
                if (RepeatLigne == 1)
                {
                    frameNumber.Clicked += Ligne_1;
                }
                if (RepeatLigne == 2)
                {
                    frameNumber.Clicked += Ligne_2;
                }
                if (RepeatLigne == 3)
                {
                    frameNumber.Clicked += Ligne_3;
                }
                if (RepeatLigne == 4)
                {
                    frameNumber.Clicked += Ligne_4;
                }
                if (RepeatLigne == 5)
                {
                    frameNumber.Clicked += Ligne_5;
                }
                if (RepeatLigne == 6)
                {
                    frameNumber.Clicked += Ligne_6;
                }
                if (RepeatLigne == 7)
                {
                    frameNumber.Clicked += Ligne_7;
                }
                if (RepeatLigne == 8)
                {
                    frameNumber.Clicked += Ligne_8;
                }
                if (RepeatLigne == 9)
                {
                    frameNumber.Clicked += Ligne_9;
                }
                if (RepeatLigne == 10)
                {
                    frameNumber.Clicked += Ligne_10;
                }
                if (RepeatLigne == 11)
                {
                    frameNumber.Clicked += Ligne_11;
                }
                #endregion

                Frame frameCouleur_1 = new Frame
                {
                    HeightRequest = 15,
                    WidthRequest = 15,
                    CornerRadius = 75,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex(Choix[0])
                };
                Frame frameCouleur_2 = new Frame
                {
                    HeightRequest = 15,
                    WidthRequest = 15,
                    CornerRadius = 75,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex(Choix[1])
                };
                Frame frameCouleur_3 = new Frame
                {
                    HeightRequest = 15,
                    WidthRequest = 15,
                    CornerRadius = 75,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex(Choix[2])
                };
                Frame frameCouleur_4 = new Frame
                {
                    HeightRequest = 15,
                    WidthRequest = 15,
                    CornerRadius = 75,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BackgroundColor = Color.FromHex(Choix[3])
                };

                // Pour le résultat
                int Resultat_noir = 0; // bonne couleur et bien placé
                int Resultat_blanc = 0; // bonne couleur et mal placé

                if (Choix[0] == Couleurs[0])
                {
                    Resultat_noir++;
                }
                if (Choix[1] == Couleurs[1])
                {
                    Resultat_noir++;
                }
                if (Choix[2] == Couleurs[2])
                {
                    Resultat_noir++;
                }
                if (Choix[3] == Couleurs[3])
                {
                    Resultat_noir++;
                }

                if (Resultat_noir == 4)
                {
                    win = true;
                }

                for (int i = 0; i < Resultat_noir; i++)
                {
                    Resultat[i] = "e44e35";
                }

                if (win == false)
                {
                    if (Couleurs[0] != Couleurs[1] || Couleurs[0] != Couleurs[2] || Couleurs[0] != Couleurs[3] || Couleurs[1] != Couleurs[2] || Couleurs[1] != Couleurs[3])
                    {
                        if (Choix[0] == Couleurs[1] || Choix[0] == Couleurs[2] || Choix[0] == Couleurs[3])
                        {
                            Resultat_blanc++;
                        }
                        if (Choix[1] == Couleurs[0] || Choix[1] == Couleurs[2] || Choix[1] == Couleurs[3])
                        {
                            Resultat_blanc++;
                        }
                        if (Choix[2] == Couleurs[0] || Choix[2] == Couleurs[1] || Choix[2] == Couleurs[3])
                        {
                            Resultat_blanc++;
                        }
                        if (Choix[3] == Couleurs[0] || Choix[3] == Couleurs[1] || Choix[3] == Couleurs[2])
                        {
                            Resultat_blanc++;
                        }
                        for (int i = 0; i < Resultat_blanc; i++)
                        {
                            Resultat[Resultat_noir + i] = "ffffff";
                        }
                    }
                }

                for (int i = 0; i < Resultat.Length; i++)
                {
                    if (Resultat[i] == "e44e35")
                    {
                        // on perd pas de point
                    }
                    else if (Resultat[i] == "ffffff")
                    {
                        // on perd 25 points
                        Score = Score - 10;
                    }
                    else
                    {
                        // on perd 50 points
                        Score = Score - 10;
                    }
                }

                // Resultat
                StackLayout stacklayout_Resultat = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                StackLayout stacklayout_resultat_horizontal_1 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                Button result_1_1 = new Button
                {
                    WidthRequest = 15,
                    HeightRequest = 15,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BorderWidth = 1,
                    CornerRadius = 15,
                    BackgroundColor = Color.FromHex(Resultat[0]),
                    IsEnabled = false
                };
                Button result_1_2 = new Button
                {
                    WidthRequest = 15,
                    HeightRequest = 15,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BorderWidth = 1,
                    CornerRadius = 15,
                    BackgroundColor = Color.FromHex(Resultat[1]),
                    IsEnabled = false
                };
                stacklayout_resultat_horizontal_1.Children.Add(result_1_1);
                stacklayout_resultat_horizontal_1.Children.Add(result_1_2);

                StackLayout stacklayout_resultat_horizontal_2 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                Button result_2_1 = new Button
                {
                    WidthRequest = 15,
                    HeightRequest = 15,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BorderWidth = 1,
                    CornerRadius = 15,
                    BackgroundColor = Color.FromHex(Resultat[2]),
                    IsEnabled = false
                };
                Button result_2_2 = new Button
                {
                    WidthRequest = 15,
                    HeightRequest = 15,
                    BorderColor = Color.FromHex("ebe8e3"),
                    BorderWidth = 1,
                    CornerRadius = 15,
                    BackgroundColor = Color.FromHex(Resultat[3]),
                    IsEnabled = false
                };
                stacklayout_resultat_horizontal_2.Children.Add(result_2_1);
                stacklayout_resultat_horizontal_2.Children.Add(result_2_2);

                stacklayout_Resultat.Children.Add(stacklayout_resultat_horizontal_1);
                stacklayout_Resultat.Children.Add(stacklayout_resultat_horizontal_2);
                // Fin resultat

                // Reset valeurs
                Choix[0] = Choix[1] = Choix[2] = Choix[3] = "1e2b3c";
                Resultat[0] = Resultat[1] = Resultat[2] = Resultat[3] = "1e2b3c";
                Bottom();

                stacklayout.Children.Add(frameNumber);
                stacklayout.Children.Add(frameCouleur_1);
                stacklayout.Children.Add(frameCouleur_2);
                stacklayout.Children.Add(frameCouleur_3);
                stacklayout.Children.Add(frameCouleur_4);
                stacklayout.Children.Add(stacklayout_Resultat);

                if (Sound == "on")
                {
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load("ok.mp3");
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
                }
                Ligne_add.Children.Add(stacklayout);

                // Descendre en bas du ScrollView
                var lastChild = Ligne_add.Children.LastOrDefault();
                if (lastChild != null)
                    await AllLigne.ScrollToAsync(lastChild, ScrollToPosition.MakeVisible, true);
            }

            if (win == true)
            {
                isPlaying = false;

                if (Sound == "on")
                {
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load("win.mp3");
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
                }

                // Enregistrement du Score
                database = GetConnection();
                // create the tables  
                database.CreateTable<ScoresItem>();

                ScoresItem Scores = new ScoresItem();
                Scores.Name = Name;
                Scores.Score = Score;
                Scores.Time = TotalTime.ToString(@"mm\:ss");

                database.Insert(Scores);


                await DisplayAlert("You Win !", "You Win in " + TotalTime.ToString(@"mm\:ss") + "\n\n" + "Your Score is " + Score, "OK");

                await Navigation.PopAsync();
            }
            else if (Ligne_add.Children.Count() == 12)
            {
                if (Sound == "on")
                {
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load("lose.mp3");
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
                }

                await DisplayAlert("Game Over !", "Game Over", "OK");

                await Navigation.PopAsync();
            }

        }
    }
}