using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenGardenBaby.Model
{
    class Gene : IEquatable<Gene>
    {
        public int Start { get; set; }
        public bool TurnsRight { get; set; }
        //tu by mohlo byt. ci sa otoci doprava alebo dolava pri rpekazke
        //public bool MyProperty { get; set; }

        public Gene(int value, bool turnsRight)
        {
            this.Start = value;
            this.TurnsRight = turnsRight;
        }

        public override String ToString()
        {
            return Start.ToString() + " turns right? = " + TurnsRight.ToString();
        }

        public bool Equals(Gene other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return Start.Equals(other.Start) && TurnsRight.Equals(other.TurnsRight);
        }
    }
}
