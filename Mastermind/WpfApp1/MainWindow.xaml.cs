using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mastermind
{
    public partial class MainWindow : Window

    {string[] chosenColor = new string[4];
        string[] allColors = { "white", "green", "blue", "red", "orange", "yellow" };
        
        public MainWindow()
        {
            InitializeComponent();
            Random rnd = new Random();
            


            for (int i = 0; i < 4; i++)
            {
                int color = rnd.Next(allColors.Length);
                chosenColor[i] = allColors[color];



            }
              MainWindow.Title = "MasterMind (" + string.Join(",", chosenColor) + ")";
  


            FillComboBoxes(ref allColors);
        }
        private void ChoosingLabelColors(object sender, RoutedEventArgs e)
        {


            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string selectedColor = comboBox.SelectedItem.ToString();

                SolidColorBrush colorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(selectedColor));
                switch (comboBox.Name)
                {
                    case "firstComboBox":
                        firstLabel.Background = colorBrush;
                                              
                        break;
                    case "secondComboBox":
                        secondLabel.Background = colorBrush;
                      
                        break;
                    case "thirdComboBox":
                        thirdLabel.Background = colorBrush;
                       
                        break;
                    case "fourthComboBox":
                        fourthLabel.Background = colorBrush;
                        
                        break;
                }
            }
        }
        private void FillComboBoxes(ref string[] items) 
        {
            fourthComboBox.Items.Add(items);

            foreach (var item in items)
            {
                firstComboBox.Items.Add(item);
                secondComboBox.Items.Add(item);
                thirdComboBox.Items.Add(item);
                fourthComboBox.Items.Add(item);
            }
        }

        private void SetBorderColor(int index, Color color)
        {
            SolidColorBrush borderBrush = new SolidColorBrush(color);
            switch (index)
            {
                case 0:
                    firstLabel.BorderBrush = borderBrush;
                    firstLabel.BorderThickness = new Thickness(2);
                    break;
                case 1:
                    secondLabel.BorderBrush = borderBrush;
                    secondLabel.BorderThickness = new Thickness(2);
                    break;
                case 2:
                    thirdLabel.BorderBrush = borderBrush;
                    thirdLabel.BorderThickness = new Thickness(2);
                    break;
                case 3:
                    fourthLabel.BorderBrush = borderBrush;
                    fourthLabel.BorderThickness = new Thickness(2);
                    break;
            }
        }


        private void controlButton_Click(object sender, RoutedEventArgs e)
        {
            
            string[] userPickedColors =  {
                                 firstComboBox.SelectedItem.ToString(),
                                 secondComboBox.SelectedItem.ToString(),
                                 thirdComboBox.SelectedItem.ToString(),
                                 fourthComboBox.SelectedItem.ToString()

            };
            for (int i = 0; i < userPickedColors.Length; i++)
            {
                if (userPickedColors[i] == chosenColor[i])
                {
                    SetBorderColor(i, Colors.DarkRed);
                }
                else if (chosenColor.Contains(userPickedColors[i]))
                {

                    SetBorderColor(i, Colors.Wheat);
                }
                else
                {
                    SetBorderColor(i, Colors.Transparent);
                }
            }
        }
    }
}











 








