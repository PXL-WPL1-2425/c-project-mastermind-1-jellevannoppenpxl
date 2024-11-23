using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MasterMind
{
    public partial class MainWindow : Window
    {
        private List<string> secretKey = new List<string>();
        private List<Button> currentGuess = new List<Button>();
        private int currentRow = 0;

        public MainWindow()
        {
            InitializeComponent();
            GenerateNewKey();
        }

        private void GenerateNewKey()
        {
            secretKey.Clear();
            Random random = new Random();
            string[] colors = { "rood", "geel", "oranje", "wit", "groen", "blauw" };

            for (int i = 0; i < 4; i++)
            {
                secretKey.Add(colors[random.Next(colors.Length)]);
            }

            MessageBox.Show("A new key has been generated!");
            ResetBoard();
        }

        private void ResetBoard()
        {
            GameBoard.Children.Clear();
            currentGuess.Clear();
            currentRow = 0;

            for (int i = 0; i < 40; i++)
            {
                Button cell = new Button
                {
                    Background = Brushes.Gray,
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(5),
                    Tag = ""
                };
                cell.Click += BoardCell_Click;
                GameBoard.Children.Add(cell);
            }
        }

        private void BoardCell_Click(object sender, RoutedEventArgs e)
        {
            if (currentGuess.Count < 4 && sender is Button button)
            {
                int buttonIndex = GameBoard.Children.IndexOf(button);
                int rowIndex = buttonIndex / 4;

                if (rowIndex == currentRow && !currentGuess.Contains(button))
                {
                    currentGuess.Add(button);
                    button.Background = Brushes.LightGray; // Placeholderkleur
                }
            }
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentGuess.Count > 0 && sender is Button colorButton)
            {
                Button lastButton = currentGuess[currentGuess.Count - 1];
                lastButton.Background = colorButton.Background;
                lastButton.Tag = colorButton.Tag;
            }
        }

        private void CheckGuess_Click(object sender, RoutedEventArgs e)
        {
            if (currentGuess.Count == 4)
            {
                int correctPosition = 0;
                int correctColor = 0;

                // Controleer de gok
                for (int i = 0; i < 4; i++)
                {
                    if (currentGuess[i].Tag.ToString() == secretKey[i])
                    {
                        correctPosition++;
                    }
                    else if (secretKey.Contains(currentGuess[i].Tag.ToString()))
                    {
                        correctColor++;
                    }
                }

                // Genereer feedback
                List<Brush> feedback = new List<Brush>();
                for (int i = 0; i < correctPosition; i++)
                    feedback.Add(Brushes.Red); // Correcte positie

                for (int i = 0; i < correctColor; i++)
                    feedback.Add(Brushes.White); // Correcte kleur

                // Voeg poging en feedback toe aan de lijst
                attempts.Add(new Attempt
                {
                    Guess = currentGuess.ConvertAll(b => b.Background),
                    Feedback = feedback
                });

                // Update de lijstweergave
                AttemptsList.ItemsSource = null;
                AttemptsList.ItemsSource = attempts;

                // Reset de gok voor de volgende poging
                currentGuess.Clear();
                currentRow++;
            }
            else
            {
                MessageBox.Show("Please select 4 colors before checking!");
            }
        }


        private void NewKey_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewKey();
        }
    }
}





















