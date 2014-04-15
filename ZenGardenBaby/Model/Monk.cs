using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenGardenBaby.Model
{
    class Monk
    {
        public List<Gene> Chromosome { get; set; }
        public double Fitness { get; set; }

        public Monk()
        {
            Chromosome = new List<Gene>();
            Fitness = 0.0;
        }

        public void GenerateRandom(int length,int obvod)
        {
            Random r = new Random();
            Gene g;
            Chromosome.Clear();
            Chromosome.Capacity = length;
            int i = 0;

            while (i<length)
	        {
                bool tr = (r.Next(2) == 1) ? true : false;
                g = new Gene(r.Next(obvod),tr);
	            if((null != Chromosome.Where(t => t.Start == g.Start).FirstOrDefault()))
                    continue;
                else{
                    ++i;
                    Chromosome.Add(g);
                }
	        } 
        }

        public void Mutate()
        {
            //mozno rozne mutacie, 
            //zatial vymenim nahodne dva geny vedla seba
            Random r = new Random();
            int pos = r.Next(Chromosome.Count - 1);
            Console.WriteLine(pos);
            Chromosome.Reverse(pos, 2);
        }

        public void EvaluateOn(Board b)
        {
            double fitness = 0.0 ;


            this.Fitness = fitness;
        }

        
    }
}
