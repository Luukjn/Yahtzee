using System;
using System.Collections.Generic;
using System.Text;

namespace Yahtzee.Model.Domain
{
    public class Combination
    {
        public CombinationsEnum Type { get; set; }
        public List<Die> Dice { get; set; }
        public int part { get; set; }

        public Func<List<Die>,int> CalculatedResult { get; set; }

        public int Result => CalculatedResult(Dice);
    }
}
