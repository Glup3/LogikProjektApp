using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapesTestApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        /// <summary>
        /// Farbe für den linken Kreis (Menge A)
        /// </summary>
        private Color farbeA;

        /// <summary>
        /// Farbe für den rechten Kreise (Menge B)
        /// </summary>
        private Color farbeB;

        /// <summary>
        /// Das Diagram rechts
        /// </summary>
        private CirclesUI elem;

        #endregion

        #region Konstruktor

        /// <summary>
        /// Standard Konstruktor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //elem = new CirclesUI(Colors.White, Colors.White, "verein", new List<int>(), new List<int>());
            //StackPanel_Container.Children.Add(elem);
            LoadColors();
        }

        #endregion

        #region Events

        /// <summary>
        /// Wenn ein RadioButton gedrückt wird, dann ändert sich die Farbe 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            string name = button.Name;

            switch(name)
            {
                case "RotA":
                    farbeA = Colors.Red; break;

                case "BlauA":
                    farbeA = Colors.Blue; break;

                case "GelbA":
                    farbeA = Colors.Yellow; break;

                case "RotB":
                    farbeB = Colors.Red; break;

                case "BlauB":
                    farbeB = Colors.Blue; break;

                case "GelbB":
                    farbeB = Colors.Yellow; break;
            }
        }

        /// <summary>
        /// Zahlen Checkboxen werden ausgecheckt. Das Venn Diagramm wird gecleart
        /// </summary>
        /// <param name="sender">Der Button "Clear"</param>
        /// <param name="e"></param>
        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            foreach (UIElement elem in Grid_A.Children)
            {
                if (elem is CheckBox check)
                {
                    check.IsChecked = false;
                }
            }
            foreach (UIElement elem in Grid_B.Children)
            {
                if (elem is CheckBox check)
                {
                    check.IsChecked = false;
                }
            }
            StackPanel_Container.Children.Remove(elem);
            elem = null;
        }

        /// <summary>
        /// Wenn sich eine Farbe ändert dann soll sich der Hintergrund entsprechend ändern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color selectedColor = (Color)(cmbColors.SelectedItem as PropertyInfo).GetValue(null, null);
            this.Background = new SolidColorBrush(selectedColor);
        }

        private void CheckBox_Aktualisieren(object sender, RoutedEventArgs e)
        {
            if (elem != null)
                MengenOperation(elem.Operation);
        }

        #endregion

        #region Hilfsmethoden

        /// <summary>
        /// Wenn man auf eine Mengen Operation Button drückt dann soll das jeweilige Venn Diagramm kommen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MengenOperation(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            MengenOperation(b.Name);
        }

        /// <summary>
        /// Erstellung des Venn Diagramms
        /// </summary>
        /// <param name="mengenOperation">Mengen Operation</param>
        private void MengenOperation(string mengenOperation)
        {
            StackPanel_Container.Children.Remove(elem);
            var l1 = GetCheckedBoxes("A");
            var l2 = GetCheckedBoxes("B");

            elem = new CirclesUI(farbeA, farbeB, mengenOperation, l1, l2);
            StackPanel_Container.Children.Add(elem);
        }

        /// <summary>
        /// Man bekommt die Zahlen der Menge A oder B
        /// </summary>
        /// <param name="menge">Menge A oder B</param>
        /// <returns>Liste mit den Zahlen aus Menge</returns>
        private List<int> GetCheckedBoxes(string menge)
        {
            List<int> list = new List<int>();
            UIElementCollection coll;
            if (menge == "A")
            {
                coll = Grid_A.Children;
            }
            else
            {
                coll = Grid_B.Children;
            }

            foreach (UIElement elem in coll)
            {
                if (elem is CheckBox check)
                {
                    var txt = check.Content.ToString();
                    if (check.IsChecked == true)
                        list.Add(int.Parse(txt));
                }
            }

            return list;
        }

        /// <summary>
        /// Lädt die Farben in den ColorPicker und wählt weiß als Default aus
        /// </summary>
        private void LoadColors()
        {
            cmbColors.ItemsSource = typeof(Colors).GetProperties();
            cmbColors.SelectedIndex = cmbColors.Items.Count - 4;
        }



        #endregion
        
    }
}
