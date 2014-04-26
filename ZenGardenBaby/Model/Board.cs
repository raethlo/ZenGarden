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
        public const char Nothing = '_';

        public Board()
        {
            Stones = new List<Obstacle>();
        }

        public void LoadFromFile(string path)
        {
            string[] lines = null;
            if(Stones.Count > 0)
                Stones.Clear();
            try
            {
                lines = File.ReadAllLines(path);
                var c = lines[0].Split(' ');
                this.X = int.Parse(c[0]);
                this.Y = int.Parse(c[1]);
                this.Map = new char[this.X,this.Y];
                initBoard();
                for (int i = 1; i < lines.Length; i++)
                {
                    c = lines[i].Split(' ');
                    int x = int.Parse(c[0]);
                    int y = int.Parse(c[1]);
                    Stones.Add(new Obstacle(x, y));
                    Map[x, y] = 'X';
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

        private void initBoard()
        {
            for (int i = 0; i < this.Y; i++)
            {
                for (int j = 0; j < this.X; j++)
                {
                    this.Map[j, i] = Board.Nothing; 
                }
            }
        }

        public void Reset()
        {
            initBoard();
            foreach (var stone in Stones)
            {
                Map[stone.X, stone.Y] = 'X';
            }
        }

        public int Circumference()
        {
            return (2 * (this.X + this.Y));
        }

        public int Surface()
        {
            return (X * Y);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Y; i++)
            {
                for (int j = 0; j < this.X; j++)
                {
                    sb.Append(Map[j, i]+ " ");
                }
                sb.AppendLine();
                
            }
            return sb.ToString();
        }

    }
}
