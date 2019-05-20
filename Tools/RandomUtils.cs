using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Tools
{
    public static class RandomUtils
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            try
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static int RealRandom(int lowerBound, int upperBound)
        {
            int res;
            try
            {
                if (lowerBound > upperBound)
                {
                    var temp = lowerBound;
                    lowerBound = upperBound;
                    upperBound = temp;
                }

                if (upperBound - lowerBound < 500)
                {
                    var randomList = new List<int>();
                    for (int i = lowerBound; i < upperBound + 1; i++) randomList.Add(i);
                    randomList.Shuffle();
                    res = randomList[0];
                }
                else
                {
                    MersenneTwister randGen = new MersenneTwister();

                    res = lowerBound + randGen.Next(upperBound - lowerBound);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
            return res;
        }

        public static Color RandomColor()
        {
            var colorNumbers = new List<int>();
            for (int i = 0; i < 256; i++) colorNumbers.Add(i);
            colorNumbers.Shuffle();

            return Color.FromRgb(colorNumbers[0], colorNumbers[1], colorNumbers[2]);
        }
    }
}
