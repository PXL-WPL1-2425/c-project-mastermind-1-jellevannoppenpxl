using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();
        }
        private void InitializeGame(object?[] generatedCode)
        { }
           
        
            private List<string> _availableColors = new List<string> { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };
            private List<string> _randomCode;

            public MainWindow()
            {
                InitializeComponent();
                GenerateRandomCode();
                FillComboBoxes();
            }

            private void GenerateRandomCode()
            {
                Random rnd = new Random();
                _randomCode = Enumerable.Range(0, 4)
                    .Select(_ => _availableColors[rnd.Next(_availableColors.Count)])
                    .ToList();

                this.Title = $"Mastermind - Code: {string.Join(", ", _randomCode)}";
            }

            private void FillComboBoxes()
            {
                var comboBoxes = new[] { comboBox1, comboBox2, comboBox3, comboBox4 };
                foreach (var comboBox in comboBoxes)
                {
                    comboBox.ItemsSource = _availableColors;
                }
            }
        }

    }


