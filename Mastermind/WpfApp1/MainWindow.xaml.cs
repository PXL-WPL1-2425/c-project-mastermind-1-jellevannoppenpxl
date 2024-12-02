using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MasterMind
{
    public partial class MainWindow : Window
    {
        private static readonly List<string> value = [];
        private List<string> secretKey = value;

        public MainWindow(List<string> secretKey)
        {
            this.secretKey = secretKey;
        }

        private static readonly List<Button> value1 = []; 
        private List<Button> currentGuess = value1;

        public MainWindow(List<Button> currentGuess)
        {
            this.currentGuess = currentGuess;
        }

        private int currentRow = 0;

        private ObservableCollection<Attempt> attempts = [];
         public MainWindow()
        {
            InitializeComponent();
            AttemptsList.ItemsSource = attempts; // Verbind direct aan ObservableCollection.
            GenerateNewKey();
        }


        private void GenerateNewKey()
        {
            secretKey.Clear();
            Random random = new Random();
            string[] colors = ["rood", "geel", "oranje", "wit", "groen", "blauw"];

            for (int i = 0; i < colors.Length; i++)
            {
                secretKey.Add(colors[random.Next(colors.Length)]);
            }
            
            
            ResetBoard();
        }
        
        private void ResetBoard()
        {
            MessageBox.Show("u score is ");
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
                Button lastButton = currentGuess[^1];
                lastButton.Background = colorButton.Background;
                lastButton.Tag = colorButton.Tag;
            }
        }

        private void CheckGuess_Click(object sender, RoutedEventArgs e)
        {
            if (currentGuess.Count == 4)
            {
                int totalPenalty = 0; // Totaal aantal strafpunten
                List<int> penalizedPositions = []; // Lijst voor het bijhouden van de strafpuntenposities

                // Loop over alle kleuren in de gok
                for (int i = 0; i < 4; i++)
                {
                    string guessedColor = currentGuess[i].Tag?.ToString() ?? string.Empty;


                    if (guessedColor == null)
                        continue; // Als de kleur null is, ga door naar de volgende iteratie

                    // Als de kleur overeenkomt met de geheime code op de zelfde positie
                    if (guessedColor == secretKey[i])
                    {
                        totalPenalty += 0; // Correcte kleur en positie
                    }
                    else if (secretKey.Contains(guessedColor) && !penalizedPositions.Contains(secretKey.IndexOf(guessedColor)))
                    {
                        totalPenalty += 1; // Kleur komt voor, maar op een andere positie
                        penalizedPositions.Add(secretKey.IndexOf(guessedColor)); // Zorg ervoor dat dezelfde kleur niet meerdere keren bestraft wordt
                    }
                    else
                    {
                        totalPenalty += 1; // Kleur komt niet voor in de geheime code
                    }
                }

                // Update de score in het label (je moet een `ScoreLabel` in de XAML hebben)
                ScoreLabel.Content = $"Score: {totalPenalty} Strafpunten";

                // Genereer feedback (bijvoorbeeld witte of rode stippen)
                List<Brush> feedback = [];
                for (int i = 0; i < totalPenalty; i++)
                {
                    feedback.Add(Brushes.Red); // Voeg een rode stip toe voor een fout geraden kleur
                }

                // Voeg de poging en de feedback toe aan de lijst van pogingen
                attempts.Add(new Attempt
                {
                    Guess = currentGuess.ConvertAll(b => b.Background), // Haal de achtergrondkleur van elke knop in de poging
                    Feedback = feedback // Voeg de feedback toe
                });

                // Update de weergave van de pogingen in de ListBox
                AttemptsList.ItemsSource = null;
                AttemptsList.ItemsSource = attempts;

                // Reset de huidige gok voor de volgende poging
                currentGuess.Clear();
                currentRow++;

                // Controleer of het spel afgelopen is (bijvoorbeeld, als de penalty 0 is, betekent dat de code is gekraakt)
                CheckGameOver(totalPenalty == 0); // Het spel eindigt als er geen strafpunten zijn (code gekraakt)
            }
            else
            {
                MessageBox.Show("Please select 4 colors before checking!");
            }
        }





        private void NewKey_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("geef jou naam in  [ ]");
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
       

           
        private void ResetGame()
        {
            GenerateNewKey();
            attempts.Clear();
            currentRow = 0;
            ScoreLabel.Content = "Score: 0 Strafpunten"; // Score resetten.
           
        }

    }
}
public record Attempt(List<Brush> Guess, List<Brush> Feedback)
{
    public Attempt() : this([], [])
    {
        // Initialiseer Guess en Feedback met lege lijsten


    }
}


























