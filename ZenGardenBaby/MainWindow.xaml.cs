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
        private bool elitarism_on = true;
        private double elite_rate = 0.05;
        private double mutation_chance = 0.6;
        private int runtime = 5000;
        private static ISelectionStrategy tournament = new TournamentSelection(4);
        private static ISelectionStrategy just_elites = new JustElites();
        private ISelectionStrategy selection = tournament;
        private int pop_size = 100;

        public MainWindow()
        {
            InitializeComponent();
            bw.DoWork += bw_DoWork;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            cbSelection.Checked += cbSelection_Checked;
            cbSelection.Unchecked += cbSelection_Unchecked;
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

                        if (File.Exists("ftf_result.txt"))
                            File.Delete("ftf_result.txt");
                        if (File.Exists("avg_fitness.txt"))
                            File.Delete("avg_fitness.txt");
                        if (File.Exists("max_fitness.txt"))
                            File.Delete("max_fitness.txt");

                        AppendLine(board.X.ToString() + " " + board.Y.ToString());
                        AppendLine(board.ToString());
                        AppendLine("Surface = " + board.Surface());
                        AppendLine("Strategy = " + selection.ToString());
                        AppendLine("Elites = " + elitarism_on);
                        AppendLine("Population size = " + pop_size);
                        AppendLine("Elite rate = " + elite_rate);
                        AppendLine("Mutation rate = " + mutation_chance);
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
                    if (File.Exists("ftf_result.txt"))
                        File.Delete("ftf_result.txt");
                    if (File.Exists("avg_fitness.txt"))
                        File.Delete("avg_fitness.txt");
                    if (File.Exists("max_fitness.txt"))
                        File.Delete("max_fitness.txt");
                    var args = new List<object>();
                    args.Add(runtime);
                    
                    int voila = board.Surface() - board.Stones.Count;
                    args.Add(voila);
                    args.Add(cbElites.IsChecked.Value);
                    tbMut.IsEnabled = false;
                    tbRun.IsEnabled = false;
                    tbPop.IsEnabled = false;
                    cbElites.IsEnabled = false;
                    cbSelection.IsEnabled = false;
                    prgBar.Visibility = Visibility.Visible;
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
            tbMut.IsEnabled = true;
            tbRun.IsEnabled = true;
            tbPop.IsEnabled = true;
            cbElites.IsEnabled = true;
            cbSelection.IsEnabled = true;
            prgBar.Visibility = Visibility.Hidden;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            List<object> args= e.Argument as List<object>;
            int loops = (int)args.ElementAt(0);
            int max_fitness = (int) args.ElementAt(1);
            elitarism_on = (bool)args.ElementAt(2);

            if (board != null)
            {
                int i = 0;
                Population pop = new Population(board);
                pop.GenerateFirstPopulation(pop_size, rand);
                worker.ReportProgress(0, pop.ToString());

                
                while ((i < loops) && (!pop.Chromosomes.First().Fitness.Equals(max_fitness)) )
                {
                    pop.Selection(elite_rate, selection,elitarism_on);
                    pop.Breed(rand,mutation_chance);
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

        private void tbRun_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                runtime = int.Parse(tbRun.Text);
                tbRun.Text = runtime.ToString();
            }
            catch (Exception)
            {
                //runtime =
                MessageBox.Show("Nespravne zadany pocet iteracii");
            }
        }

        private void tbPop_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pop_size = int.Parse(tbPop.Text);
                tbPop.Text = pop_size.ToString();
            }
            catch (Exception)
            {
                //runtime =
                MessageBox.Show("Nespravne zadana velkost populacie");
            }
        }

        //private void tbMut_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    double old = mutation_chance;
        //    try
        //    {
                
        //        mutation_chance = double.Parse(tbMut.Text);
        //        if (mutation_chance < 0.0 || mutation_chance > 1.0)
        //            throw new Exception();
        //        tbMut.Text = mutation_chance.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        mutation_chance = old;
        //        MessageBox.Show("Mutation chance musi byt z intervalu <0,1>");
        //        tbMut.Text = mutation_chance.ToString();
        //    }
        //}

        //private void tbEli_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    double old = elite_rate;
        //    try
        //    {
        //        elite_rate = double.Parse(tbEli.Text);
        //        if (elite_rate < 0.0 || elite_rate > 1.0)
        //            throw new Exception();
        //        tbEli.Text = elite_rate.ToString();
        //    }      
        //    catch (Exception)
        //    {
        //        //runtime =
        //        elite_rate = old;
        //        MessageBox.Show("Elite rate musi byt z intervalu <0,1>");
        //        tbEli.Text = elite_rate.ToString();
        //    }
        //}

        private void cbSelection_Checked(object sender, RoutedEventArgs e)
        {
            selection = tournament;
        }

        private void cbSelection_Unchecked(object sender, RoutedEventArgs e)
        {
            selection = just_elites;
        }

        private void btnMutSave_Click(object sender, RoutedEventArgs e)
        {
            double old = mutation_chance;
            try
            {

                mutation_chance = double.Parse(tbMut.Text);
                if (mutation_chance < 0.0 || mutation_chance > 1.0)
                    throw new Exception();
                tbMut.Text = mutation_chance.ToString();
                AppendLine("Saved");
            }
            catch (Exception)
            {
                mutation_chance = old;
                MessageBox.Show("Mutation chance musi byt z intervalu <0,1>");
                tbMut.Text = mutation_chance.ToString();
            }
        }

        private void btnEliSave_Click(object sender, RoutedEventArgs e)
        {
            double old = elite_rate;
            try
            {
                elite_rate = double.Parse(tbEli.Text);
                if (elite_rate < 0.0 || elite_rate > 1.0)
                    throw new Exception();
                tbEli.Text = elite_rate.ToString();
                AppendLine("Saved");
            }
            catch (Exception)
            {
                //runtime =
                elite_rate = old;
                MessageBox.Show("Elite rate musi byt z intervalu <0,1>");
                tbEli.Text = elite_rate.ToString();
            }
        }
    }
}
