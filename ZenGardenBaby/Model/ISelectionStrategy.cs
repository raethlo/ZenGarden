using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenGardenBaby.Model
{
    interface ISelectionStrategy
    {
        List<Monk> Selection(Population population);
    }

    class RouletteSelection : ISelectionStrategy
    {

        public List<Monk> Selection(Population population)
        {
            throw new NotImplementedException();
        }
    }


    class TournamentSelection : ISelectionStrategy
    {
        public int GroupSize { get; set; }
        public TournamentSelection(int groups)
        {
            if (groups < 2)
                throw new ArgumentException("Group size must be 2 or greater");
            this.GroupSize = groups;
        }
        public List<Monk> Selection(Population population)
        {
            List<Monk> result = new List<Monk>();
            Random r  = new Random();
            int length = population.Chromosomes.Count;

            List<List<Monk>> groups = new List<List<Monk>>();
            List<Monk> group = new List<Monk>();

            for (int i = 0; i < length; i++)
            {
                if (i % GroupSize == 0 && i!=0)
                {
                    groups.Add(group);
                    group.Clear();
                }
                int index = r.Next(population.Chromosomes.Count);
                group.Add(population.Chromosomes.ElementAt(index));
                population.Chromosomes.RemoveAt(index);
            }
            if (group.Count > 0 )
            {
                groups.Add(group);
            }

            foreach (var gr in groups)
            {
                result.Add(gr.OrderByDescending(m => m.Fitness).First());
            }
            
            return result;
        }
    }

    class JustElites : ISelectionStrategy
    {

        public List<Monk> Selection(Population population)
        {
            //blank 
            return new List<Monk>();
        }
    }
}
