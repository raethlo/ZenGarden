using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZenGardenBaby.Model
{
    class Board
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char[,] Map { get; set; }
        public List<Obstacle> Stones { get; set; }

        public Board()
        {
            Stones = new List<Obstacle>();
        }

        public void LoadFromFile(string path)
        {
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(path);
                var c = lines[0].Split(' ');
                this.X = int.Parse(c[0]);
                this.Y = int.Parse(c[1]);
                this.Map = new char[this.X,this.Y];
                for (int i = 1; i < lines.Length; i++)
                {
                    c = lines[i].Split(' ');
                    int x = int.Parse(c[0]);
                    int y = int.Parse(c[1]);
                    Stones.Add(new Obstacle(x, y)); 
                }
                
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
