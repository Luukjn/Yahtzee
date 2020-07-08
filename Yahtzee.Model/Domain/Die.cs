using System;
using System.Collections.Generic;
using System.Text;
using Yahtzee.Bll.Helpers;

namespace Yahtzee.Model.Domain
{
    public class Die
    {
        public int Sides { get; set; }

        public int Result { get; private set; }

        public void Roll()
        {
            Result = RandomGenerator.TlRng.Value.Next(1, Sides);
        }
    }
}
