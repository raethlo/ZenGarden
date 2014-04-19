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
        BackgroundWorker bw = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            bw.DoWork += bw_DoWork;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        private void BtnHello_Click(object sender, RoutedEventArgs e)
        {
            if (board!=null)
            {
                int obvod = board.Circumference();
                var father = new Monk(obvod / 2 + board.Stones.Count, obvod, rand);
                var mother = new Monk(obvod / 2 + board.Stones.Count, obvod, rand);
                //var father = new Monk(obvod / 4, obvod, rand);
                //var mother = new Monk(obvod / 4, obvod, rand);
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
                board.LoadFromFile("../../Boards/1.txt");
                AppendLine(board.X.ToString() + " " + board.Y.ToString());
                AppendLine(board.ToString());
                AppendLine("Surface = " + board.Surface());
            }
            catch (Exception ex)
            {       
                MessageBox.Show(ex.Message);
            }
        }

        private void AppendLine(string text)
        {
            t.AppendText(text + "\n");
            ScrollViewer.ScrollToEnd();
        }
        private void AppendLine()
        {
            t.AppendText("\n");
            ScrollViewer.ScrollToEnd();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (board != null)
            {
                var args = new List<object>();
                args.Add(300);
                args.Add(board.Surface() - board.Stones.Count);
                bw.RunWorkerAsync(args); 
            }
            else
            {
                MessageBox.Show("board not loaded yet");
            }
            
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AppendLine(e.UserState.ToString());

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            List<object> args= e.Argument as List<object>;
            int loops = (int)args.ElementAt(0);
            int surface = (int) args.ElementAt(1);

            if (board != null)
            {
                int i = 0;
                Population pop = new Population(board);
                pop.GenerateFirstPopulation(200, rand);
                worker.ReportProgress(0, pop.ToString());

                while ((i < loops) && (pop.Chromosomes.ElementAt(0).Fitness != (double)surface))
                {
                    //IEnumerable<Monk> elite = pop.Elites(0.1);
                    //pop.Chromosomes.Clear();
                    //pop.Chromosomes.AddRange(elite);
                    //pop.Breed(rand);
                    //pop.EvaluateAllRandomly();
                    //pop.Sort();
                    pop.Selection(0.1,new JustElites());
                    pop.Breed(rand);
                    pop.EvaluateAll();
                    i++;
                    worker.ReportProgress(0, pop.ToString());
                } 
            }
            else
            {
                MessageBox.Show("Board not yet loaded");
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            t.Clear();
        }



    }
}
