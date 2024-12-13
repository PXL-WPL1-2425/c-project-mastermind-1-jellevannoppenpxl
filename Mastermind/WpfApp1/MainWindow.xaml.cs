using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MasterMind
{
    public partial class MainWindow : Window
    {
        private List<string> secretKey = new();
        private List<Button> currentGuess = new();
        private int currentRow = 0;
        private ObservableCollection<Attempt> attempts = new();
        private List<string> playerNames = new();
        private int currentPlayerIndex = 0;
        private Dictionary<string, int> playerScores = new();

        public MainWindow()
        {
            InitializeComponent();
            AttemptsList.ItemsSource = attempts;
            CollectPlayerNames();
            StartNewGame();
        }

        private void CollectPlayerNames()
        {
            MessageBox.Show("Welkom bij Mastermind!");

            while (true)
            {
                string playerName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Voer de naam van de speler in:",
                    "Speler toevoegen",
                    "");

                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    playerNames.Add(playerName);
                    playerScores[playerName] = 0;
                }
                else
                {
                    MessageBox.Show("Voer een geldige naam in.");
                    continue;
                }

                if (MessageBox.Show("Nog een speler toevoegen?", "Speler toevoegen", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    break;
            }

            UpdateActivePlayerDisplay();
        }

        private void StartNewGame()
        {
            GenerateSecretKey();
            ResetBoard();
            currentRow = 0;
            attempts.Clear();
            UpdateActivePlayerDisplay();
        }

        private void GenerateSecretKey()
        {
            Random random = new Random();
            string[] colors = { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };
            secretKey = Enumerable.Range(0, 4).Select(_ => colors[random.Next(colors.Length)]).ToList();
        }

        private void ResetBoard()
        {
            GameBoard.Children.Clear();
            currentGuess.Clear();
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
                    // Open een venster of laat een lijst met kleuren zien
                    string[] colors = { "rood", "geel", "oranje", "wit", "groen", "blauw" };
                    string chosenColor = colors[new Random().Next(colors.Length)]; // Placeholder, vervang dit met echte invoer.

                    // Stel de achtergrondkleur in op basis van de gekozen kleur
                    switch (chosenColor)
                    {
                        case "rood":
                            button.Background = Brushes.Red;
                            break;
                        case "geel":
                            button.Background = Brushes.Yellow;
                            break;
                        case "oranje":
                            button.Background = Brushes.Orange;
                            break;
                        case "wit":
                            button.Background = Brushes.White;
                            break;
                        case "groen":
                            button.Background = Brushes.Green;
                            break;
                        case "blauw":
                            button.Background = Brushes.Blue;
                            break;
                    }

                    // Bewaar de kleur in de Tag voor later gebruik
                    button.Tag = chosenColor;

                    // Voeg de knop toe aan de huidige gok
                    currentGuess.Add(button);
                }
            }
        }

    }
}

        private void CheckGuess_Click(object sender, RoutedEventArgs e)
        {
            if (currentGuess.Count == 4)
            {
                List<string> guessedColors = currentGuess.Select(b => b.Tag?.ToString() ?? "").ToList();
                int correctPositions = guessedColors.Where((color, index) => color == secretKey[index]).Count();
                int correctColors = guessedColors.Intersect(secretKey).Count() - correctPositions;

                attempts.Add(new Attempt
                {
                    Guess = currentGuess.Select(b => b.Background as Brush).ToList(),
                    Feedback = Enumerable.Repeat<Brush>(Brushes.Red, correctPositions)
                           .Concat(Enumerable.Repeat<Brush>(Brushes.White, correctColors)).ToList()
                });


                if (correctPositions == 4)
                {
                    MessageBox.Show($"Gefeliciteerd, {playerNames[currentPlayerIndex]}! Je hebt de code gekraakt!");
                    SwitchToNextPlayer();
                }
                else if (++currentRow >= 10)
                {
                    MessageBox.Show($"Sorry, {playerNames[currentPlayerIndex]}! De code was: {string.Join(", ", secretKey)}.");
                    SwitchToNextPlayer();
                }

                currentGuess.Clear();
            }
            else
            {
                MessageBox.Show("Vul eerst 4 kleuren in.");
            }
        }

        private void SwitchToNextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playerNames.Count;
            StartNewGame();
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Welke hint wil je kopen?\n1. Juiste kleur (10 strafpunten)\n2. Juiste kleur op juiste plaats (20 strafpunten)",
                "Hint kopen", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                DeductPoints(10);
            else if (result == MessageBoxResult.No)
                DeductPoints(20);
        }

        private void DeductPoints(int points)
        {
            string currentPlayer = playerNames[currentPlayerIndex];
            playerScores[currentPlayer] += points;
            UpdateActivePlayerDisplay();
        }

        private void UpdateActivePlayerDisplay()
        {
            ActivePlayerLabel.Content = playerNames[currentPlayerIndex];
            ScoreLabel.Content = playerScores[playerNames[currentPlayerIndex]];
        }

        private void ResetGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }
    }

    public record Attempt
    {
        public List<Brush> Guess { get; init; } = new();
        public List<Brush> Feedback { get; init; } = new();

        // Optioneel: een expliciete constructor voor extra zekerheid
        public Attempt()
        {
            Guess = new List<Brush>();
            Feedback = new List<Brush>();
        }
    }
}private void ColorChoice_Click(object sender, RoutedEventArgs e)
{
    if (sender is Button colorButton && currentGuess.Count < 4)
    {
        string chosenColor = colorButton.Tag.ToString();
        // Verwerk hier de gekozen kleur
    }
}

