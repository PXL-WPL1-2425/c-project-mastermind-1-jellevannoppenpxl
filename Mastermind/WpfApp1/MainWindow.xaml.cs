using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define available colors and the randomly generated code
        private readonly List<string> _availableColors = new List<string> { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };
        private List<string> _randomCode;

        public MainWindow()
        {
            InitializeComponent();
            GenerateRandomCode();
            FillComboBoxes();
        }

        // Generates a random code of 4 colors
        private void GenerateRandomCode()
        {
            Random rnd = new Random();
            _randomCode = Enumerable.Range(0, 4)
                .Select(_ => _availableColors[rnd.Next(_availableColors.Count)])
                .ToList();

            // Display the generated code in the title (for debugging purposes)
            this.Title = $"Mastermind - Code: {string.Join(", ", _randomCode)}";
        }

        // Fills ComboBoxes with the available colors
        private void FillComboBoxes()
        {
            // Ensure comboBox1, comboBox2, etc., are defined in XAML
            var comboBoxes = new[] { comboBox1, comboBox2, comboBox3, comboBox4 };
            foreach (var comboBox in comboBoxes)
            {
                comboBox.ItemsSource = _availableColors;
            }
        }
    }
}
