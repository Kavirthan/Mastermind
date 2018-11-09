using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Mastermind
{
    public partial class Scores : ContentPage
    {
        SQLiteConnection database;
        StackLayout Ligne_Score;

        public SQLite.SQLiteConnection GetConnection()
        {
            SQLiteConnection sqlitConnection;
            var sqliteFilename = "Scores.db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
            sqlitConnection = new SQLite.SQLiteConnection(path);
            return sqlitConnection;
        }

        public Scores()
        {
            InitializeComponent();

            database = GetConnection();
            // create the tables  
            database.CreateTable<ScoresItem>();

            Ligne_Score = (StackLayout)this.FindByName<StackLayout>("Ligne");


            int i = 0;

            SQLiteConnection var = GetConnection();
            List<ScoresItem> y = var.Table<ScoresItem>().OrderByDescending(x => x.Score).ToList();
            foreach (var x1 in y)
            {
                if (i <= 20)
                {
                    string str_Name = x1.Name;
                    int str_Score = x1.Score;
                    string str_Time = x1.Time;

                    StackLayout stacklayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal
                    };

                    if (i == 0) // Le 1er du classement
                    {
                        Image image = new Image
                        {
                            Source = "Score_1.png",
                            HeightRequest = 50,
                            WidthRequest = 50
                        };
                        stacklayout.Children.Add(image);
                    }
                    else if (i == 1) // Le 2eme du classement
                    {
                        Image image = new Image
                        {
                            Source = "Score_2.png",
                            HeightRequest = 50,
                            WidthRequest = 50
                        };
                        stacklayout.Children.Add(image);
                    }
                    else if (i == 2) // Le 3eme du classement
                    {
                        Image image = new Image
                        {
                            Source = "Score_3.png",
                            HeightRequest = 50,
                            WidthRequest = 50
                        };
                        stacklayout.Children.Add(image);
                    }
                    else
                    {
                        Button image = new Button
                        {
                            Text = Convert.ToString(i + 1),
                            TextColor = Color.FromHex("4b4e53"),
                            FontSize = 20,
                            HeightRequest = 50,
                            WidthRequest = 50,
                            BackgroundColor = Color.FromHex("ebe8e3"),
                            CornerRadius = 20,
                            BorderWidth = 2,
                            BorderColor = Color.FromHex("4b4e53"),
                            IsEnabled = false,
                            FontFamily = Device.RuntimePlatform == Device.iOS ? "TitanOne-Regular" : Device.RuntimePlatform == Device.Android ? "TitanOne-Regular.ttf#TitanOne-Regular" : null
                        };
                        stacklayout.Children.Add(image);
                    }

                    StackLayout Texte = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    Label Name = new Label
                    {
                        Text = str_Name,
                        FontSize = 25,
                        TextColor = Color.FromHex("ebe8e3"),
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        FontFamily = Device.RuntimePlatform == Device.iOS ? "TitanOne-Regular" : Device.RuntimePlatform == Device.Android ? "TitanOne-Regular.ttf#TitanOne-Regular" : null
                    };

                    StackLayout stkLyt = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    Label Score = new Label
                    {
                        Text = str_Score.ToString(),
                        FontSize = 20,
                        TextColor = Color.FromHex("ebe8e3"),
                        HorizontalTextAlignment = TextAlignment.End,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        FontFamily = Device.RuntimePlatform == Device.iOS ? "TitanOne-Regular" : Device.RuntimePlatform == Device.Android ? "TitanOne-Regular.ttf#TitanOne-Regular" : null
                    };
                    Label Time = new Label
                    {
                        Text = str_Time,
                        FontSize = 20,
                        TextColor = Color.FromHex("ebe8e3"),
                        HorizontalTextAlignment = TextAlignment.End,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        FontFamily = Device.RuntimePlatform == Device.iOS ? "TitanOne-Regular" : Device.RuntimePlatform == Device.Android ? "TitanOne-Regular.ttf#TitanOne-Regular" : null
                    };

                    Texte.Children.Add(Name);
                    stkLyt.Children.Add(Score);
                    stkLyt.Children.Add(Time);
                    Texte.Children.Add(stkLyt);

                    stacklayout.Children.Add(Texte);

                    Ligne_Score.Children.Add(stacklayout);
                }

                i++;
            }
        }
        private void Button_Close_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}