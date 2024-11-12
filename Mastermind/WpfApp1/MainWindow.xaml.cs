using System.ComponentModel.Design.Serialization;
using System.Drawing;
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
        {
            // Beschikbare kleuren
            string colors = { "rood", "geel", "oranje", "wit", "groen", "blauw" };

            // Voeg kleuren toe aan elke ComboBox
            comboBox1.Items.AddRange (colors);
            comboBox2.Items.Add (colors);
            comboBox3.Items.Add (colors);
            comboBox4.Items.Add (colors);

            // Genereer een willekeurige code en sla deze op
            generatedCode = GenerateRandomCode(colors);

            // Toon de geheime code in de titel 
            this.AddText = $"Geheime Code: {string.Join(" ", generatedCode)}";

        }
        private string[] GenerateRandomCode(string[] colors)
        {
            Random random = new Random();
            return colors.OrderBy(x => random.Next()).Take(4).ToArray();
        }


    }
}