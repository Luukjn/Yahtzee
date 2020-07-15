using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yahtzee.Bll.Extensions;
using Yahtzee.Model.Domain;

namespace Yahtzee.Model.Data
{
    class Combinations
    {
        public List<Combination> CombinationsList { get; }
        public int Total1 { get; private set; }
        public int Total2 { get; private set; }
        public int Bonus { get; private set; }
        public int GrandTotal { get; private set; }

        public Combinations()
        {
            CombinationsList = new List<Combination>
            {
                new Combination
                {
                    Type = CombinationsEnum.Ones,
                    CalculatedResult = (dice) => dice
                        .Where(d => d.Result == 1)
                        .Sum(d => d.Result)
                },
                new Combination
                {
                    Type = CombinationsEnum.Twoes,
                    CalculatedResult = (dice) => dice
                        .Where(d => d.Result == 2)
                        .Sum(d => d.Result)
                },
                new Combination
                {
                    Type = CombinationsEnum.Threes,
                    CalculatedResult = (dice) => dice
                        .Where(d => d.Result == 3)
                        .Sum(d => d.Result)
                },
                new Combination
                {
                    Type = CombinationsEnum.Fours,
                    CalculatedResult = (dice) => dice
                        .Where(d => d.Result == 4)
                        .Sum(d => d.Result)
                },
                new Combination
                {
                    Type = CombinationsEnum.Fives,
                    CalculatedResult = (dice) => dice
                        .Where(d => d.Result == 5)
                        .Sum(d => d.Result)
                },
                new Combination
                {
                    Type = CombinationsEnum.Sixes,
                    CalculatedResult = (dice) => dice
                        .Where(d => d.Result == 6)
                        .Sum(d => d.Result)
                },
                new Combination
                {
                    Type = CombinationsEnum.ThreeOfaKind,
                    CalculatedResult = (dice) =>
                    {
                        var duplicates = dice.GetDuplicateCounts(d => d.Result);
                        if(duplicates.First().Count >= 3)
                        {
                            return dice.Sum(d => d.Result);
                        }
                        return 0;
                    }
                },
                new Combination
                {
                    Type = CombinationsEnum.Carre,
                    CalculatedResult = (dice) => {
                        var duplicates = dice.GetDuplicateCounts(d => d.Result);
                        if(duplicates.First().Count >= 4)
                        {
                            return dice.Sum(d => d.Result);
                        }
                        return 0;
                    }
                },
                new Combination
                {
                    Type = CombinationsEnum.FullHouse,
                    CalculatedResult = (dice) =>
                    {
                        var duplicates = dice.GetDuplicateCounts(d => d.Result);
                        if(duplicates.First().Count == 3)
                        {
                            if(duplicates.Last().Count == 2)
                            {
                                return 25;
                            }
                        }

                        return 0;
                    }
                },
                new Combination
                {
                    Type = CombinationsEnum.SmallStreet,
                    CalculatedResult = (dice) =>
                    {
                        var sequences = dice.GetSequences(d => d.Result);
                        if(sequences.First().Count() >= 4)
                        {
                            return 30;
                        }
                        return 0;
                    }
                },
                new Combination
                {
                    Type = CombinationsEnum.BigStreet,
                    CalculatedResult = (dice) =>
                    {
                        var sequences = dice.GetSequences(d => d.Result);
                        if(sequences.First().Count() >= 5)
                        {
                            return 40;
                        }
                        return 0;
                    }
                },
                new Combination
                {
                    Type = CombinationsEnum.Yahtzee,
                    CalculatedResult = (dice) =>
                    {
                        var duplicates = dice.GetDuplicateCounts(d => d.Result);
                        if(duplicates.First().Count >= 5)
                        {
                            return 50;
                        }
                        return 0;
                    }
                },
                new Combination
                {
                    Type = CombinationsEnum.Change,
                    CalculatedResult = (dice) => dice.Sum(d => d.Result)
                }
            };
        }

        public void CalculateTotals()
        {
            foreach(var combination in CombinationsList)
            {
                switch(combination.Type)
                {
                    case CombinationsEnum.Ones:
                    case CombinationsEnum.Twoes:
                    case CombinationsEnum.Threes:
                    case CombinationsEnum.Fours:
                    case CombinationsEnum.Fives:
                    case CombinationsEnum.Sixes:
                        Total1 += combination.CalculatedResult(combination.Dice);
                        break;
                    case CombinationsEnum.ThreeOfaKind:
                    case CombinationsEnum.Carre:
                    case CombinationsEnum.FullHouse:
                    case CombinationsEnum.SmallStreet:
                    case CombinationsEnum.BigStreet:
                    case CombinationsEnum.Yahtzee:
                    case CombinationsEnum.Change:
                        Total2 += combination.CalculatedResult(combination.Dice);
                        break;
                }

                Bonus = Total1 > 63 ? 35 : 0;
                GrandTotal = Total1 + Bonus + Total2;
            }
        }

        public void AddDiceToCombination(CombinationsEnum type, List<Die> dice)
        {
            var combination = CombinationsList.First(c => c.Type == type);
            combination.Dice = dice;
        }
    }
}
