﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZenGardenBaby.Model
{
    class Population
    {
        public List<Monk> Chromosomes { get; set; }
        public int Size { get; set; }
        //zatial to tu bude ale je to skarede 
        //posuvat si rozmery boardu takto
        protected Board Board { get; set; }

        public Population(Board board)
        {
            this.Board = board;
            this.Chromosomes = new List<Monk>();
        }

        public Population(Board board, int size)
        {
            this.Board = board;
            this.Size = size;
            this.Chromosomes = new List<Monk>();
            GenerateFirstPopulation(size);
        }

        public void GenerateFirstPopulation(int size)
        {
            this.Size = size;
            for (int i = 0; i < size; i++)
            {
                //zatial 15 krokov generujem
                int circ = Board.Circumference();
                var monk = new Monk(circ / 2 + Board.Stones.Count, circ, new Random());
                //monk.EvaluateOnRandomly(Board);
                monk.EvaluateOn(Board);
                Chromosomes.Add(monk);
            }
            Sort();
        }

        public void GenerateFirstPopulation(int size,Random randomizer)
        {
            this.Size = size;
            for (int i = 0; i < size; i++)
            {
                //zatial 15 krokov generujem
                int circ = Board.Circumference();
                var monk = new Monk(circ / 2 + Board.Stones.Count, circ, randomizer);
                monk.EvaluateOnRandomly(Board);
                Chromosomes.Add(monk);
            }
            Sort();
        }

        public List<Monk> Elites(double percent)
        {
            List<Monk> result = new List<Monk>();
            Sort();
            if(percent > 1 || percent < 0)
                throw new ArgumentException("Argument must be in <0,1> interval");
            int count =(int) (percent * Chromosomes.Count);
            result.AddRange(Chromosomes.Take(count));
            //aby sa elity nezapocitali dalej
            Chromosomes.RemoveRange(0, count);
            return result;
        }

        public void EvaluateAllRandomly()
        {
            foreach (var chrom in Chromosomes)
            {
                chrom.EvaluateOnRandomly(Board);
            }
        }

        public void EvaluateAll()
        {
            foreach (var chrom in Chromosomes)
            {
                chrom.EvaluateOn(Board);
            }
            Sort();
        }

        public void Breed(Random randomizer,double mutation_chance)
        {
            int count = Size - Chromosomes.Count;
            List<int> used = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int m = -1;
                int t = -1;
                do
                {
                    m = randomizer.Next(Chromosomes.Count);
                } while (used.Contains(m));
                do
                {
                    t = randomizer.Next(Chromosomes.Count);
                } while (used.Contains(t));

                Monk mother = Chromosomes.ElementAt(m);
                Monk father = Chromosomes.ElementAt(t);

                Monk kid = null;

                if (randomizer.Next(2) == 1)
                {
                    kid = new Monk(mother, father); 
                }
                else
                {
                    kid = new Monk(father, mother);
                }
                kid.Mutate(randomizer,mutation_chance);
                kid.EvaluateOn(Board);
                Chromosomes.Add(kid);
            }

        }

        public void Sort()
        {
            Chromosomes.Sort(
                    delegate(Monk m1, Monk m2)
                    {
                        return m2.Fitness.CompareTo(m1.Fitness);
                    }
             );
        }

        public void Selection(double percent_elites,ISelectionStrategy strategy, bool elitarism_on)
        {
            List<Monk> res = new List<Monk>();
            if (elitarism_on)
            {
                res.AddRange(Elites(percent_elites)); 
            }
            res.AddRange(strategy.Selection(this));
            Chromosomes.Clear();
            Chromosomes.AddRange(res);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            double avg_fitness = Chromosomes.Average(m => m.Fitness);
            double max_fitness = Chromosomes.Max(m => m.Fitness);
            double min_fitness = Chromosomes.Min(m => m.Fitness);

            sb.AppendFormat("AVG = {0} MAX = {1} MIN = {2}\n", avg_fitness, max_fitness, min_fitness);
            sb.AppendLine(Chromosomes.ElementAt(0).PrintResult());

            File.AppendAllText("avg_fitness.txt", avg_fitness.ToString()+"\n");
            File.AppendAllText("max_fitness.txt", max_fitness.ToString() + "\n");
            List<double> first_ten_fitnes = new List<double>();
            first_ten_fitnes.AddRange(Chromosomes.Select(m => m.Fitness).Take(10));

            foreach (var f in first_ten_fitnes)
            {
                File.AppendAllText("ftf_result.txt",f.ToString() + " ");
            }
            File.AppendAllText("ftf_result.txt", "\n");

            return sb.ToString();

        }
    }
}
