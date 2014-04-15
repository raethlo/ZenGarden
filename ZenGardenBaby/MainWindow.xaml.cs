using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZenGardenBaby.Model;

namespace ZenGardenBaby
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rand = new Random();
        private Board board = new Board();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnHello_Click(object sender, RoutedEventArgs e)
        {
            var monk = new Monk();
            monk.GenerateRandom(15,15);
            foreach (var g in monk.Chromosome)
            {
                t.AppendText("Gene: "+g.ToString() + "\n");
            }

            t.AppendText("Mutated\n");

            monk.Mutate();

            foreach (var g in monk.Chromosome)
            {
                t.AppendText("Gene: " + g.ToString() + "\n");
            }
            
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                board.LoadFromFile("../../Boards/1.txt");
                t.AppendText(board.X.ToString() + " " + board.Y.ToString());
            }
            catch (Exception ex)
            {       
                MessageBox.Show(ex.Message);
            }
        }

    }
}
