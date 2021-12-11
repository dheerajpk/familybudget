using System;
using System.Linq;

namespace FamilyBudget.Core.Utility
{
    internal class KeyGenerator
    {
        private static readonly Random RndNumber = new Random();

        public static string GetUniqueKey()
        {
            return new string(Enumerable.Range(0, 6).Select(x => GetRandomLetter()).ToArray());
        }

        private static char GetRandomLetter()
        {
            return (char)RndNumber.Next(65, 90);
        }
    }
}
