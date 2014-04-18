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
        }

        public Population(Board board, int size)
        {
            this.Board = board;
            this.Size = size;
            GenerateFirstPopulation(size);
        }

        public void GenerateFirstPopulation(int size)
        {
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

        private Monk Best(List<Monk> chromosome)
        {

            throw new NotImplementedException();
        }

        public List<Gene> Elites()
        {
            throw new NotImplementedException();
        }

        public Population Roulette(double percent)
        {
            throw new NotImplementedException();
        }

        public void Breed()
        {
            throw new NotImplementedException();
        }

        public void Sort()
        {
            Chromosomes.OrderByDescending(f => f.Fitness);
        }

        //public 
    }
}
