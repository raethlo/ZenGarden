using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var monk = new Monk(circ / 2, circ, new Random());
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
                var monk = new Monk(circ / 2, circ, randomizer);
                monk.EvaluateOn(Board);
                Chromosomes.Add(monk);
            }
            Sort();
        }


        public void Tournament(int group_size)
        {
            //rozdel na skupiny po x, vyber najlepsieho z kazdej a 
            //tych posli dalej

            throw new NotImplementedException();
        }

        private Monk Best(List<Monk> chromosomes)
        {
            throw new NotImplementedException();
        }

        public List<Monk> Elites(double percent)
        {
            List<Monk> result = new List<Monk>();
            Sort();
            if(percent > 1 || percent < 0)
                throw new ArgumentException("Argument must be in <0,1> interval");
            int count =(int) (percent * Chromosomes.Count);
            result.AddRange(Chromosomes.Take(count)); 
            return result;
        }

        public List<Gene> Roulette(double percent)
        {
            throw new NotImplementedException();
        }

        public void EvaluateAll()
        {
            foreach (var chrom in Chromosomes)
            {
                chrom.EvaluateOn(Board);
            }
        }

        public void Breed(Random randomizer)
        {
            int count = Size - Chromosomes.Count;

            for (int i = 0; i < count; i++)
            {
                Monk mother = Chromosomes.ElementAt(randomizer.Next(Chromosomes.Count));
                Monk father = Chromosomes.ElementAt(randomizer.Next(Chromosomes.Count));

                var kid = new Monk(mother, father);
                kid.Mutate();

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            double avg_fitness = Chromosomes.Average(m => m.Fitness);
            double max_fitness = Chromosomes.Max(m => m.Fitness);
            double min_fitness = Chromosomes.Min(m => m.Fitness);

            sb.AppendFormat("AVG = {0} MAX = {1} MIN = {2}\n", avg_fitness, max_fitness, min_fitness);
            sb.AppendLine(Chromosomes.ElementAt(0).PrintResult());

            return sb.ToString();

        }
    }
}
