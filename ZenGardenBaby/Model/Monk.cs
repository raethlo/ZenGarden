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
        public int Length { get; set; }
        public double Fitness { get; set; }
        public int Treshold { get; set; }
        private string RakedSurfaceMap = null;

        public Monk(int length, int obvod, Random randomizer)
        {
            Chromosome = new List<Gene>();
            Length = 0;
            Fitness = 0.0;
            Treshold = obvod;
            GenerateRandom(length, obvod, randomizer);
        }

        public Monk(Monk mother, Monk father)
        {
            if(mother.Length != father.Length)
                throw new ArgumentException("Parents must be of the same length");

            Random rand = new Random();
            Chromosome = new List<Gene>();
            Length = father.Length;
            Treshold = father.Treshold;
            Fitness = 0.0;
            
            Chromosome.AddRange(mother.Chromosome.Take(Length / 2));
            //var remains = father.Chromosome.Except(Chromosome);
            var remains = father.Chromosome.Except(Chromosome);
            Chromosome.AddRange(remains.Take(Length / 2));
            if (Chromosome.Count < Length)
            {
                int i = Chromosome.Count;

                while (i <= Length)
                {
                    bool tr = (rand.Next(2) == 1) ? true : false;
                    var g = new Gene(rand.Next(father.Treshold), tr);
                    if ((null != Chromosome.Where(t => t.Start == g.Start).FirstOrDefault()))
                        continue;
                    else
                    {
                        ++i;
                        Chromosome.Add(g);
                    }
                }
            }
        }

        protected void GenerateRandom(int length,int obvod,Random randomizer)
        {
            Gene g;
            Chromosome.Clear();
            Chromosome.Capacity = length;
            Length = length;
            int i = 0;

            while (i<length)
	        {
                bool tr = (randomizer.Next(2) == 1) ? true : false;
                g = new Gene(randomizer.Next(obvod),tr);
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

        public void Mutate(Random randomizer, double percent)
        {
            //mozno rozne mutacie, 
            //zatial vymenim nahodne dva geny vedla seba
            if (randomizer.NextDouble()<=percent)
            {
                int pos = randomizer.Next(Chromosome.Count - 1);

                Chromosome.Reverse(pos, 2);
                pos = randomizer.Next(Chromosome.Count);


                pos = randomizer.Next(Chromosome.Count);
                bool tr = Chromosome.ElementAt(pos).TurnsRight;
                Chromosome.ElementAt(pos).TurnsRight = !tr;

                pos = randomizer.Next(Chromosome.Count);

                tr = randomizer.Next(2) == 1 ? true : false;
                int start = randomizer.Next(this.Treshold);
                while (null != Chromosome.Where(t => t.Start == start).FirstOrDefault())
                {
                    start  = randomizer.Next(this.Treshold);
                }
                pos = randomizer.Next(Chromosome.Count);
                Chromosome.ElementAt(pos).Start = start;

            }
            
        }

        protected int Rake(Board b,bool turnsRight, int start_x,int start_y, char mark, Direction dir)
        {
            int x = start_x;
            int y = start_y;
            int new_x;
            int new_y;
            Random rand = new Random();
            int raked = 0;

            while ((x >= 0) && (x < b.X) && (y >= 0) && (y < b.Y))
            {
                if (b.Map[x, y] == Board.Nothing)
                {
                    b.Map[x, y] = mark;
                    ++raked;

                    switch (dir)
                    {
                        case Direction.Up:
                            new_y = y - 1;
                            if (new_y < 0)
                                return raked;
                            else if (b.Map[x, new_y] != Board.Nothing)
                            {
                                
                                if (!turnsRight && (((x - 1) < 0) || b.Map[x - 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Left;
                                    x--;
                                    continue;
                                }
                                else if (turnsRight && (((x + 1) == b.X) || b.Map[x + 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Right;
                                    x++;
                                    continue;
                                }
                                else if (turnsRight && (((x - 1) < 0) || b.Map[x - 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Left;
                                    x--;
                                    continue;
                                }
                                else if (!turnsRight && (((x + 1) == b.X) || b.Map[x + 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Right;
                                    x++;
                                    continue;
                                }
                            }
                            y = new_y;
                            break;
                        case Direction.Down:
                            new_y = y + 1;
                            if (new_y == b.Y)
                                return raked;
                            //ak nie je volno
                            else if (b.Map[x, new_y] != Board.Nothing)
                            {
                                //pozri vlavo, pre mnicha iduceho dole je to vpravo
                                if (turnsRight && (((x - 1) < 0) || b.Map[x - 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Left;
                                    x--;
                                    continue;
                                }
                                //pozri vpravo
                                else if (!turnsRight && (((x + 1) == b.X) || b.Map[x + 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Right;
                                    x++;
                                    continue;
                                }
                                else if (!turnsRight && (((x - 1) < 0) || b.Map[x - 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Left;
                                    x--;
                                    continue;
                                }
                                //pozri vpravo
                                else if (turnsRight && (((x + 1) == b.X) || b.Map[x + 1, y] == Board.Nothing))
                                {
                                    dir = Direction.Right;
                                    x++;
                                    continue;
                                }
                            }
                            y = new_y;
                            break;
                        case Direction.Right:
                            new_x = x + 1;
                            //return 1;
                            if (new_x == b.X)
                                return raked;
                            else if (b.Map[new_x, y] != Board.Nothing)
                            {
                                //pozri hore
                                if (!turnsRight && (((y - 1) < 0) || b.Map[x, y - 1] == Board.Nothing))
                                {
                                    dir = Direction.Up;
                                    y--;
                                    continue;
                                }
                                //pozri dole
                                else if (turnsRight && (((y + 1) == b.Y) || b.Map[x, y + 1] == Board.Nothing))
                                {
                                    dir = Direction.Down;
                                    y++;
                                    continue;
                                }
                                else if (turnsRight && (((y - 1) < 0) || b.Map[x, y - 1] == Board.Nothing))
                                {
                                    dir = Direction.Up;
                                    y--;
                                    continue;
                                }
                                //pozri dole
                                else if (!turnsRight && (((y + 1) == b.Y) || b.Map[x, y + 1] == Board.Nothing))
                                {
                                    dir = Direction.Down;
                                    y++;
                                    continue;
                                }
                            }
                            x = new_x;
                            break;
                        case Direction.Left:
                            new_x = x - 1;
                            //return 1;
                            if (new_x < 0)
                                return raked;
                            else if (b.Map[new_x, y] != Board.Nothing)
                            {
                                //pozri hore
                                if (turnsRight && (((y - 1) < 0) || b.Map[x, y - 1] == Board.Nothing))
                                {
                                    dir = Direction.Up;
                                    y--;
                                    continue;
                                }
                                //pozri dole
                                else if (!turnsRight && (((y + 1) == b.Y) || b.Map[x, y + 1] == Board.Nothing))
                                {
                                    dir = Direction.Down;
                                    y++;
                                    continue;
                                }
                                else if (!turnsRight && (((y - 1) < 0) || b.Map[x, y - 1] == Board.Nothing))
                                {
                                    dir = Direction.Up;
                                    y--;
                                    continue;
                                }
                                //pozri dole
                                else if (turnsRight && (((y + 1) == b.Y) || b.Map[x, y + 1] == Board.Nothing))
                                {
                                    dir = Direction.Down;
                                    y++;
                                    continue;
                                }
                            }
                            x = new_x;
                            //return 1;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    return 0;
                }

            }
            return raked;
        }

        protected int RakeRandomly(Board b, int start_x, int start_y, char mark, Direction dir)
        {
            int x = start_x;
            int y = start_y;
            int new_x;
            int new_y;
            Random rand = new Random();
            int raked = 0;
   
            while ( (x >= 0) && (x< b.X) && (y>=0) && (y<b.Y))
            {
                if (b.Map[x, y] == Board.Nothing)
                {
                    b.Map[x, y] = mark;
                    ++raked;

                    switch (dir)
                    {
                        case Direction.Up:
                            new_y = y - 1;
                            if (new_y < 0)
                                return raked;
                            else if (b.Map[x, new_y] != Board.Nothing)
                            {
                                if (((x - 1) < 0) || b.Map[x - 1, y] == Board.Nothing)
                                {
                                    dir = Direction.Left;
                                    x--;
                                    continue;
                                }
                                else if (((x + 1) == b.X) || b.Map[x + 1, y] == Board.Nothing)
                                {
                                    dir = Direction.Right;
                                    x++;
                                    continue;
                                }
                            }
                            y = new_y;
                            break;
                        case Direction.Down:
                            new_y = y + 1;
                            if (new_y == b.Y)
                                return raked;
                            //ak nie je volno
                            else if (b.Map[x, new_y] != Board.Nothing)
                            {
                                //pozri vlavo
                                if (((x - 1) < 0) || b.Map[x - 1, y] == Board.Nothing)
                                {
                                    dir = Direction.Left;
                                    x--;
                                    continue;
                                }
                                //pozri vpravo
                                else if (((x + 1) == b.X) || b.Map[x + 1, y] == Board.Nothing)
                                {
                                    dir = Direction.Right;
                                    x++;
                                    continue;
                                }
                            }
                            y = new_y;
                            break;
                        case Direction.Right:
                            new_x = x + 1;
                            //return 1;
                            if (new_x == b.X)
                                return raked;
                            else if (b.Map[new_x, y] != Board.Nothing)
                            {
                                //pozri hore
                                if (((y - 1) < 0) || b.Map[x , y - 1] == Board.Nothing)
                                {
                                    dir = Direction.Up;
                                    y--;
                                    continue;
                                }
                                //pozri dole
                                else if (((y + 1) == b.Y) || b.Map[x, y + 1] == Board.Nothing)
                                {
                                    dir = Direction.Down;
                                    y++;
                                    continue;
                                }
                            }
                            x = new_x;
                            break;
                        case Direction.Left:
                            new_x = x - 1;
                            //return 1;
                            if (new_x < 0)
                                return raked;
                            else if (b.Map[new_x, y] != Board.Nothing)
                            {
                                //pozri hore
                                if (((y - 1) < 0) || b.Map[x , y - 1] == Board.Nothing)
                                {
                                    dir = Direction.Up;
                                    y--;
                                    continue;
                                }
                                //pozri dole
                                else if (((y + 1) == b.Y) || b.Map[x, y + 1] == Board.Nothing)
                                {
                                    dir = Direction.Down;
                                    y++;
                                    continue;
                                }
                            }
                            x = new_x;
                            //return 1;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    return 0;
                }

            }
            return raked;
        }

        public void EvaluateOnRandomly(Board b)
        {
            double fitness = 0.0 ;
            var a = Encoding.ASCII.GetBytes("a")[0];
            //poprechadzaj celu mapu podla instrukcii, pojde to postupne, kedze list je zoradeny
            //Console.WriteLine("X= {0} Y={1} obvod={2}", b.X, b.Y, 2 * (b.X + b.Y));
            byte i = 0;
            int y = -1;
            int x = -1;
            int sum = 0;
            Direction dir = Direction.Up; ;

            b.Reset();

            foreach (var gene in Chromosome)
            {
                
                char mark = (char)(i+a);
                
                if (gene.Start < b.X)
                {
                    //this is case when the monk is entering from the upper side
                    //Console.WriteLine(" {0} => upper side",gene.ToString());
                    x = gene.Start;
                    y = 0;
                    dir = Direction.Down;
                    //RakeRandomly(b, x, y, mark, Direction.Down);
                }
                else if ( (gene.Start>= b.X) && (gene.Start < (b.X+b.Y)) )
                {
                    //the right side
                    //Console.WriteLine(" {0} => right side", gene.ToString());
                    x = b.X - 1;
                    y = gene.Start % b.X;
                    dir = Direction.Left;
                    
                }
                else if ((gene.Start >= (b.X + b.Y)) && (gene.Start < (b.X + b.X + b.Y)))
                {
                    //the lower side
                    //Console.WriteLine(" {0} => lower side", gene.ToString());
                    x = b.X - (gene.Start % (b.X+b.Y)) - 1;
                    y = b.Y - 1;
                    dir = Direction.Up;
                }
                else if ((gene.Start >= (b.X + b.X + b.Y)) && (gene.Start < 2 * (b.X + b.Y)))
                {
                    //the left side
                    //Console.WriteLine(" {0} => left side", gene.ToString());
                    x = 0;
                    y = b.Y - (gene.Start % (b.X + b.X + b.Y)) - 1 ;
                    dir = Direction.Right;
                }
                else
                {
                    //error
                    Console.WriteLine("ERROR: {0}",gene.ToString());
                    throw new Exception("Gene.Start out of range");
                }

                int raked = RakeRandomly(b, x, y, mark, dir);
                if (raked > 0){
                    ++i;
                    sum += raked;
                    //.RakedSurfaceMap = b.ToString();
                    //Console.WriteLine(PrintResult());
                }
            }


            RakedSurfaceMap = b.ToString();
            fitness = sum;
            this.Fitness = fitness;
        }

        public void EvaluateOn(Board b)
        {
            double fitness = 0.0;
            var a = Encoding.ASCII.GetBytes("a")[0];
            //poprechadzaj celu mapu podla instrukcii, pojde to postupne, kedze list je zoradeny
            //Console.WriteLine("X= {0} Y={1} obvod={2}", b.X, b.Y, 2 * (b.X + b.Y));
            byte i = 0;
            int y = -1;
            int x = -1;
            int sum = 0;
            Direction dir = Direction.Up; ;

            b.Reset();

            foreach (var gene in Chromosome)
            {

                char mark = (char)(i + a);

                if (gene.Start < b.X)
                {
                    //this is case when the monk is entering from the upper side
                    //Console.WriteLine(" {0} => upper side",gene.ToString());
                    x = gene.Start;
                    y = 0;
                    dir = Direction.Down;
                    //RakeRandomly(b, x, y, mark, Direction.Down);
                }
                else if ((gene.Start >= b.X) && (gene.Start < (b.X + b.Y)))
                {
                    //the right side
                    //Console.WriteLine(" {0} => right side", gene.ToString());
                    x = b.X - 1;
                    y = gene.Start % b.X;
                    dir = Direction.Left;

                }
                else if ((gene.Start >= (b.X + b.Y)) && (gene.Start < (b.X + b.X + b.Y)))
                {
                    //the lower side
                    //Console.WriteLine(" {0} => lower side", gene.ToString());
                    x = b.X - (gene.Start % (b.X + b.Y)) - 1;
                    y = b.Y - 1;
                    dir = Direction.Up;
                }
                else if ((gene.Start >= (b.X + b.X + b.Y)) && (gene.Start < 2 * (b.X + b.Y)))
                {
                    //the left side
                    //Console.WriteLine(" {0} => left side", gene.ToString());
                    x = 0;
                    y = b.Y - (gene.Start % (b.X + b.X + b.Y)) - 1;
                    dir = Direction.Right;
                }
                else
                {
                    //error
                    Console.WriteLine("ERROR: {0}", gene.ToString());
                    throw new Exception("Gene.Start out of range");
                }

                int raked = Rake(b,gene.TurnsRight, x, y, mark, dir);
                if (raked > 0)
                {
                    ++i;
                    sum += raked;
                    //.RakedSurfaceMap = b.ToString();
                    //Console.WriteLine(PrintResult());
                }
            }


            RakedSurfaceMap = b.ToString();
            fitness = sum;
            this.Fitness = fitness;
        }

        public string PrintResult()
        {
            return (RakedSurfaceMap == null) ? "The monk hasn't been evaluated yet\n" : RakedSurfaceMap;
        }
        
    }

    enum Direction
    {
        Up,Down,Right,Left
    }
}
