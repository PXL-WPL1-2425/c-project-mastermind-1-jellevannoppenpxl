using System.Collections.ObjectModel;
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
        private List<string> playerNames = new(); // Lijst om spelersnamen op te slaan
        private int currentPlayerIndex = 0; // Index van de huidige speler

        public MainWindow()
        {
            InitializeComponent();
            AttemptsList.ItemsSource = attempts; // Verbind de lijst aan de UI.
            GenerateNewKey();
            StartGame();
            UpdateActivePlayerDisplay();

        }

        private void GenerateNewKey()
        {
            secretKey.Clear();
            Random random = new Random();
            string[] colors = { "rood", "geel", "oranje", "wit", "groen", "blauw" };

            while (secretKey.Count < 4) // Zorg dat je 4 unieke kleuren genereert.
            {
                string color = colors[random.Next(colors.Length)];
                if (!secretKey.Contains(color))
                {
                    secretKey.Add(color);
                }
            }

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
        private void StartGame()
        {
            MessageBox.Show("Welkom bij Mastermind!");

            while (true)
            {
                // Vraag de naam van de speler
                string playerName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Voer de naam van de speler in:",
                    "Speler toevoegen",
                    ""); // Standaardwaarde is leeg

                // Controleer of een naam is ingevoerd
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    playerNames.Add(playerName); // Voeg de naam toe aan de lijst
                }
                else
                {
                    MessageBox.Show("Voer een geldige naam in.");
                    continue; // Herhaal de vraag als de naam niet geldig is
                }

                // Vraag of nog een speler moet worden toegevoegd
                MessageBoxResult result = MessageBox.Show(
                    "Wil je nog een speler toevoegen?",
                    "Nog een speler toevoegen?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    break; // Stop met het vragen van spelersnamen
                }
            }

            // Geef een overzicht van alle ingevoerde spelers
            string playersOverview = string.Join(", ", playerNames);
            MessageBox.Show($"De volgende spelers doen mee: {playersOverview}");
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
                Button lastButton = currentGuess[^1];
                lastButton.Background = colorButton.Background;
                lastButton.Tag = colorButton.Tag;
            }
        }

        private void CheckGuess_Click(object sender, RoutedEventArgs e)
        {
            if (currentGuess.Count == 4)
            {
                List<string> guessedColors = currentGuess.Select(b => b.Tag?.ToString() ?? "").ToList();

                // Bereken feedback
                int correctPositions = guessedColors.Where((color, index) => color == secretKey[index]).Count();
                int correctColors = guessedColors.Intersect(secretKey).Count() - correctPositions;

                // Genereer feedbackpinnen
                List<Brush> feedback = new();
                feedback.AddRange(Enumerable.Repeat(Brushes.Red, correctPositions));
                feedback.AddRange(Enumerable.Repeat(Brushes.White, correctColors));

                // Update pogingen
                attempts.Add(new Attempt
                {
                    Guess = currentGuess.Select(b => b.Background).ToList(),
                    Feedback = feedback
                });

                currentGuess.Clear();
                currentRow++;

                // Controleer of de speler gewonnen heeft of het spel afgelopen is.
                CheckGameOver(correctPositions == 4);
            }
            else
            {
                MessageBox.Show("Please select 4 colors before checking!");
            }
        }

        private void CheckGameOver(bool codeCracked)
        {
            string currentPlayer = playerNames[currentPlayerIndex];
            string nextPlayer = playerNames[(currentPlayerIndex + 1) % playerNames.Count]; // Volgende speler (ronde-robin)

            if (codeCracked)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Gefeliciteerd {currentPlayer}! Je hebt de code gekraakt in {currentRow + 1} pogingen.\n" +
                    $"Nu is het de beurt aan {nextPlayer}. Wil je opnieuw spelen?",
                    $"{currentPlayer} - WINNER",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    SwitchToNextPlayer();
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
                    $"Sorry {currentPlayer}, je hebt gefaald! De correcte code was: {code}.\n" +
                    $"Nu is het de beurt aan {nextPlayer}. Wil je opnieuw spelen?",
                    $"{currentPlayer} - FAILED",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SwitchToNextPlayer();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }
        private void SwitchToNextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playerNames.Count; // Volgende speler
            ResetGame(); // Reset het spelbord voor de volgende speler
            UpdateActivePlayerDisplay();

        }
        private void UpdateActivePlayerDisplay()
        {
            string currentPlayer = playerNames[currentPlayerIndex];
            ActivePlayerLabel.Text = $"Actieve speler: {currentPlayer}";
        }

            private void ResetGame()
            {
                GenerateNewKey();
                attempts.Clear();
                currentRow = 0;
            UpdateActivePlayerDisplay();

        }
    }

        public record Attempt
        {
            public List<Brush> Guess { get; init; } = new();
            public List<Brush> Feedback { get; init; } = new();
        }

    }














