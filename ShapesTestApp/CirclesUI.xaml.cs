using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShapesTestApp
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class CirclesUI : UserControl
    {
        #region private Variablen

        /// <summary>
        /// Zufallsgenerator
        /// </summary>
        Random rng = new Random();

        #endregion

        #region Properties

        /// <summary>
        /// Die Mengen Operation
        /// </summary>
        public string Operation { get; set; }

        #endregion

        #region Konstruktor

        /// <summary>
        /// Standard Konstruktor, erstellt das entsprechende Venn Diagramm
        /// </summary>
        /// <param name="farbe1">Farbe für den ersten Kreis</param>
        /// <param name="farbe2">Farbe für den zweiten Kreis</param>
        /// <param name="mengenOperation">Mengen Operation fürs Venn Diagramm</param>
        /// <param name="l1">Menge A</param>
        /// <param name="l2">Menge B</param>
        public CirclesUI(Color farbe1, Color farbe2, string mengenOperation, List<int> l1, List<int> l2)
        {
            InitializeComponent();

            links.Fill = new SolidColorBrush(farbe1);
            rechts.Fill = new SolidColorBrush(farbe2);

            GetArea(mengenOperation);

            InsertNumbers(l1, l2);

            MengenOperation(mengenOperation, l1, l2);

            Operation = mengenOperation;
        }

        #endregion

        #region Hilfsmethoden
        /// <summary>
        /// Gibt den Schnittbereich der Kreise aus und färbt in entsprechend
        /// </summary>
        /// <param name="fall">Mengen Operation</param>
        private void GetArea(string fall)
        {
            var mode = GeometryCombineMode.Union;
            Color farbe = Colors.Transparent;

            switch (fall)
            {
                case "verein":
                    mode = GeometryCombineMode.Intersect;
                    farbe = GetCombinedColor();
                    ((SolidColorBrush)links.Fill).Color = farbe;
                    ((SolidColorBrush)rechts.Fill).Color = farbe;
                    break;
                case "schnitt":
                    mode = GeometryCombineMode.Intersect;
                    farbe = GetCombinedColor();
                    break;
                case "symetrie":
                    mode = GeometryCombineMode.Intersect;
                    farbe = Colors.White;
                    break;
                case "ohne":
                    mode = GeometryCombineMode.Xor;
                    break;
            }

            mitte.Data = Geometry.Combine(kreis1, kreis2, mode, null);
            mitte.Fill = new SolidColorBrush(farbe);
        }

        /// <summary>
        /// Kombiniert zwei Farben
        /// </summary>
        /// <returns>Kombination der zwei Farben</returns>
        private Color GetCombinedColor()
        {
            var f1 = ((SolidColorBrush)links.Fill).Color;
            var f2 = ((SolidColorBrush)rechts.Fill).Color;

            if ((f1 == Colors.Red && f2 == Colors.Blue) || (f2 == Colors.Red && f1 == Colors.Blue)) // violett
            {
                return Colors.DarkViolet;
            }

            if ((f1 == Colors.Red && f2 == Colors.Yellow) || (f2 == Colors.Red && f1 == Colors.Yellow)) // orange
            {
                return Colors.Orange;
            }

            if ((f1 == Colors.Blue && f2 == Colors.Yellow) || (f2 == Colors.Blue && f1 == Colors.Yellow)) // green
            {
                return Colors.Green;
            }

            if ((f1 == f2))
            {
                return f1;
            }

            return Colors.Black;
        }

        /// <summary>
        /// Fügt Zahlen zufällig in die TextBoxen ein
        /// </summary>
        /// <param name="l1">Menge A</param>
        /// <param name="l2">Menge B</param>
        private void InsertNumbers(List<int> l1, List<int> l2)
        {
            List<TextBlock> boxenLeft = new List<TextBlock>();
            List<TextBlock> boxenRight = new List<TextBlock>();

            foreach (var tb in Canva_Left.Children)
            {
                boxenLeft.Add((TextBlock)tb);
            }

            foreach (var tb in Canva_Right.Children)
            {
                boxenRight.Add((TextBlock)tb);
            }

            foreach (int i in l1)
            {
                while (true)
                {
                    int z = rng.Next(boxenLeft.Count);
                    if ((boxenLeft.ElementAt((z))).Text == "")
                    {
                        boxenLeft.ElementAt(z).Text = i.ToString();
                        break;
                    }
                }
            }

            foreach (int i in l2)
            {
                while (true)
                {
                    int z = rng.Next(boxenRight.Count);
                    if ((boxenRight.ElementAt((z))).Text == "")
                    {
                        boxenRight.ElementAt(z).Text = i.ToString();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Führt die jeweilige Mengenoperation durch und erstellt dadurch das Diagramm
        /// </summary>
        /// <param name="m">Mengenoperation</param>
        /// <param name="l1">Menge A</param>
        /// <param name="l2">Menge B</param>
        private void MengenOperation(string m, List<int> l1, List<int> l2)
        {
            // Liste mit NUR Doppelten
            List<int> l4 = new List<int>();

            foreach (int a in l1)
            {
                if (l2.Contains(a))
                {
                    l4.Add(a);
                }
            }

            #region TextBoxen
            // liste mit TextBoxen links
            List<TextBlock> tbsLeft = new List<TextBlock>();
            foreach (UIElement elem in Canva_Left.Children)
            {
                tbsLeft.Add((TextBlock)elem);
            }

            // liste mit TextBoxen Mitte
            List<TextBlock> tbsCenter = new List<TextBlock>();
            foreach (UIElement elem in Canva_Center.Children)
            {
                tbsCenter.Add((TextBlock)elem);
            }

            // liste mit TextBoxen rechts
            List<TextBlock> tbsRight = new List<TextBlock>();
            foreach (UIElement elem in Canva_Right.Children)
            {
                tbsRight.Add((TextBlock)elem);
            }

            // liste mit ALLEN textboxen
            List<TextBlock> tbs = new List<TextBlock>();
            tbs.AddRange(tbsLeft);
            tbs.AddRange(tbsCenter);
            tbs.AddRange(tbsRight);

            // alle TextBoxen clearen
            foreach (var tb in tbs)
            {
                tb.Text = "";
            }
            #endregion

            #region Vereinigungsmenge Outdated
            /*
            if (m == "verein")
            {
                // beide Listen zusammen ohne doppelte
                List<int> l3 = new List<int>();

                foreach (int i in l1)
                {
                    if (!l3.Contains(i))
                        l3.Add(i);
                }
                foreach (int i in l2)
                {
                    if (!l3.Contains(i))
                        l3.Add(i);
                }

                // l3-Werte zufällig in alle Boxen
                foreach (int i in l3)
                {
                    while(true)
                    {
                        int z = rng.Next(tbs.Count);
                        if(tbs[z].Text == "")
                        {
                            tbs[z].Text = i.ToString();
                            break;
                        }
                    }
                }
            }
            */
            #endregion 

            #region Schnittmenge
            if (m == "schnitt")
            {
                foreach (int i in l4)
                {
                    while (true)
                    {
                        int z = rng.Next(tbsCenter.Count);
                        if (tbsCenter[z].Text == "")
                        {
                            tbsCenter[z].Text = i.ToString();
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Symetrische Differenz & Vereinigungsmenge
            if (m == "symetrie" || m == "verein")
            {
                // Menge A ohne B
                List<int> l5 = l1;
                // Menge B ohne A
                List<int> l6 = l2;

                // Menge A und B ohne Doppelte
                foreach (int i in l4)
                {
                    if (l5.Contains(i))
                    {
                        l5.Remove(i);
                    }
                    if (l6.Contains(i))
                    {
                        l6.Remove(i);
                    }
                }

                foreach (int i in l5)
                {
                    while (true)
                    {
                        int z = rng.Next(tbsLeft.Count);
                        if (tbsLeft[z].Text == "")
                        {
                            tbsLeft[z].Text = i.ToString();
                            break;
                        }
                    }
                }

                foreach (int i in l6)
                {
                    while (true)
                    {
                        int z = rng.Next(tbsRight.Count);
                        if (tbsRight[z].Text == "")
                        {
                            tbsRight[z].Text = i.ToString();
                            break;
                        }
                    }
                }

                foreach (int i in l4)
                {
                    while (true)
                    {
                        int z = rng.Next(tbsCenter.Count);
                        if (tbsCenter[z].Text == "")
                        {
                            tbsCenter[z].Text = i.ToString();
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Differenz
            if (m == "ohne")
            {
                // Menge A ohne B
                List<int> l7 = l1;
                foreach (int i in l2)
                {
                    if (l7.Contains(i))
                        l7.Remove(i);
                }

                foreach (int i in l7)
                {
                    while (true)
                    {
                        int z = rng.Next(tbsLeft.Count);
                        if (tbsLeft[z].Text == "")
                        {
                            tbsLeft[z].Text = i.ToString();
                            break;
                        }
                    }
                }

            }

            #endregion

        }
        
        #endregion

    }
}