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
                int totalPenalty = 0; // Totaal aantal strafpunten
                List<int> penalizedPositions = new List<int>(); // Voor dubbele strafpunten

                for (int i = 0; i < 4; i++)
                {
                    string guessedColor = currentGuess[i].Tag?.ToString();

                    if (guessedColor == null)
                        continue;

                    if (guessedColor == secretKey[i])
                    {
                        // 0 strafpunten: correcte kleur en positie
                        totalPenalty += 0;
                    }
                    else if (secretKey.Contains(guessedColor) && !penalizedPositions.Contains(secretKey.IndexOf(guessedColor)))
                    {
                        // 1 strafpunt: kleur komt voor maar staat op de verkeerde plaats
                        totalPenalty += 1;
                        penalizedPositions.Add(secretKey.IndexOf(guessedColor)); // Vermijd dubbele punten
                    }
                    else
                    {
                        // 2 strafpunten: kleur komt niet voor in de code
                        totalPenalty += 2;
                    }
                }

                // Update de score in het Label
                ScoreLabel.Content = $"Score: {totalPenalty} Strafpunten";

                // Genereer feedback (zoals eerder)
                List<Brush> feedback = new List<Brush>();
                for (int i = 0; i < totalPenalty; i++)
                {
                    feedback.Add(Brushes.Red); // Correct aantal 1x helperlogicpunited score caluclaly injc punt.

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
        private void StopGame_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Poging {currentRow + 1}/10\n\nWilt u het spel vroegtijdig beëindigen?",
                "Spel Beëindigen",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void CheckGameOver(bool codeCracked)
        {
            if (codeCracked)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Code is gekraakt in {currentRow + 1} pogingen. Wil je nog eens?",
                    "WINNER",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    ResetGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
            else if (currentRow >= 10)
            {
                string code = string.Join(", ", secretKey);
                MessageBoxResult result = MessageBox.Show(
                    $"Je hebt gefaald! De correcte code was: {code}. Nog eens proberen?",
                    "FAILED",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ResetGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }


    }
}





















