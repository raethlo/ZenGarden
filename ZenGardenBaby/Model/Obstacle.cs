using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenGardenBaby.Model
{
    class Obstacle
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Obstacle(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
