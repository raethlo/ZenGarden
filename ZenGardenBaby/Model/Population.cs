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
        //zatial to tu bude ale je to skarede 
        //posuvat si rozmery boardu takto
        protected Board Board { get; set; }

        public Population(Board board)
        {
            this.Board = board;
        }

        public void GenerateFirstPopulation(int size)
        {
            for (int i = 0; i < size; i++)
            {
                var monk = new Monk();
                //zatial 15 krokov generujem
                monk.GenerateRandom(15,(Board.X+Board.Y)*2);
                Chromosomes.Add(monk);
            }
        }


        public Population Tournament()
        {
            throw new NotImplementedException();
        }

        public Population Roulette()
        {
            throw new NotImplementedException();
        }

        //public 
    }
}
