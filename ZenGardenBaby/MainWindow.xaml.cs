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
using System.IO;
using ZenGardenBaby.Model;
using Microsoft.Win32;

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
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (board == null)
            {
                board = new Board(); 
            }
            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if (!openFileDialog1.FileName.Equals(null))
                    {
                        //board.LoadFromFile("../../Boards/test1.txt");
                        board.LoadFromFile(openFileDialog1.FileName);

                        File.Create("ftf.txt");
                        File.Create("avg.txt");
                        File.Create("max.txt");
                        AppendLine(board.X.ToString() + " " + board.Y.ToString());
                        AppendLine(board.ToString());
                        AppendLine("Surface = " + board.Surface());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } 
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
                if ((!bw.IsBusy))
                {
                    var args = new List<object>();
                    args.Add(5000);
                    int voila = board.Surface() - board.Stones.Count;
                    args.Add(voila);
                    bw.RunWorkerAsync(args); 
                } 
                else
                {
                    MessageBox.Show("in progress");
                }
            }
            else
            {
                MessageBox.Show("board not loaded yet");
            }
            
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //var pop = e.UserState as Population;

            AppendLine(e.UserState.ToString());

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int res = (int)e.Result;
            if ( (res) == (-1))
            {
                AppendLine("Solution couldn't be found in given time with given method");
            }
            else
            {
                AppendLine(String.Format("Solution found in {0}. iteration",res));
            }
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
                pop.GenerateFirstPopulation(1000, rand);
                worker.ReportProgress(0, pop.ToString());

                
                while ((i < loops) && (!pop.Chromosomes.First().Fitness.Equals((double)surface)) )
                {
                    pop.Selection(0.05, new TournamentSelection(),true);
                    pop.Breed(rand,0.4);
                    //pop.EvaluateAllRandomly();
                    pop.Sort();
                    i++;
                    worker.ReportProgress(0, pop.ToString());
                }
                if (i != loops)
                {
                    e.Result = i;
                }
                else
                    e.Result = -1;
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
