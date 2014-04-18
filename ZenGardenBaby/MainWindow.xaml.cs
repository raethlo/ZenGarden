using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Board board = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnHello_Click(object sender, RoutedEventArgs e)
        {
            if (board!=null)
            {
                int obvod = board.Circumference();
                //var father = new Monk(obvod / 2 + board.Stones.Count, obvod,rand);
                //var mother = new Monk(obvod / 2 + board.Stones.Count, obvod,rand);
                var father = new Monk(obvod / 4, obvod, rand);
                var mother = new Monk(obvod / 4, obvod, rand);
                father.EvaluateOn(board);
                AppendLine("FATHER");
                AppendLine("Fitness = " + father.Fitness.ToString());
                AppendLine(father.PrintResult()); 

                mother.EvaluateOn(board);
                AppendLine("MOTHER");
                AppendLine("Fitness = " + mother.Fitness.ToString());
                AppendLine(mother.PrintResult());

                var kid = new Monk(mother, father);

                kid.EvaluateOn(board);
                AppendLine("KID");
                AppendLine("Fitness = " + kid.Fitness.ToString());
                AppendLine(kid.PrintResult());

                kid.Mutate();
                kid.EvaluateOn(board);
                AppendLine("MUTANT");
                AppendLine("Fitness = " + kid.Fitness.ToString());
                AppendLine(kid.PrintResult()); 
            }
            else
            {
                AppendLine("Board not loaded yet");
            }
            
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            
            if (board == null)
            {
                board = new Board(); 
            }
            try
            {
                board.LoadFromFile("../../Boards/test1.txt");
                AppendLine(board.X.ToString() + " " + board.Y.ToString());
                AppendLine(board.ToString());
            }
            catch (Exception ex)
            {       
                MessageBox.Show(ex.Message);
            }
        }

        private void AppendLine(string text)
        {
            t.AppendText(text + "\n");
        }
        private void AppendLine()
        {
            t.AppendText("\n");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //BackgroundWorker bw = new BackgroundWorker();
            ////Thread t = new Thread();
            //bw.DoWork += bw_DoWork;
            ////bw.ReportPr
            //bw.RunWorkerAsync(this);

            Monk father = new Monk(8, 14, rand);
            Monk mother = new Monk(8, 14, rand);


            Console.WriteLine("----Father");
            foreach (var gene in father.Chromosome)
            {
                Console.WriteLine(gene.ToString());
            }

            Console.WriteLine("----Mother");
            foreach (var gene in mother.Chromosome)
            {
                Console.WriteLine(gene.ToString());
            }

            Console.WriteLine("----Kid");
            var kid = new Monk(mother, father);
            foreach (var gene in kid.Chromosome)
            {
                Console.WriteLine(gene.ToString());
            }
            
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 1000000; i++)
            {
                int k = 2 * i + i ^ 2;
            }
        }

    }
}
