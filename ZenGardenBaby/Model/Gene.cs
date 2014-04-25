using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenGardenBaby.Model
{
    class Gene : IEquatable<Gene>
    {
        //odkial mnich pride na zahradu
        public int Start { get; set; }
        //preferovany smer pri narazeni na prekazku
        public bool TurnsRight { get; set; }

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

            return Start.Equals(other.Start);// && TurnsRight.Equals(other.TurnsRight);
        }
    }


}
