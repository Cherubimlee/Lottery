using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace LottoryUWP.Utils
{
    public class RandomUtil
    {
        public static RandomUtil Instance { get; private set; } = new RandomUtil();

        private const int maxRangeforNonRepeatRandom = 500;
        private const int reasonableMaxValueDiffeRange = 10;

        private Random random = new Random();

        private List<int> nonRepeatRandomList;

        private object listLock = new object();

        public int Next(int maxValue)
        {
            if (maxValue > maxRangeforNonRepeatRandom)
                return random.Next(maxValue);
            else
            {
                lock (listLock)
                {

                    if (nonRepeatRandomList == null)
                    {
                        nonRepeatRandomList = new List<int>(maxValue);
                    }

                    if (nonRepeatRandomList.Capacity != maxValue)
                    {
                        // MaxValue changed in a small range since last random next, we try to fix the non repeat randon list 
                        if (Math.Abs(nonRepeatRandomList.Capacity - maxValue) <= reasonableMaxValueDiffeRange)
                        {
                            var list = new List<int>(maxValue);
                            list.AddRange(nonRepeatRandomList.Where((x) => x < maxValue));
                            nonRepeatRandomList = list;
                        }
                        else
                        {
                            nonRepeatRandomList.Clear();
                        }
                    }

                    if (nonRepeatRandomList.Count == 0)
                    {
                        var list = new List<int>(maxValue);
                        //init the list
                        for (int i = 0; i < maxValue; i++)
                        {
                            list.Add(i);
                        }

                        nonRepeatRandomList = list;
                    }
                 
                    var index = random.Next(nonRepeatRandomList.Count);
                    var result = nonRepeatRandomList[index];
                    nonRepeatRandomList.RemoveAt(index);

                    return result;
                }
            }
        }

        public Random RandomCore { get { return random; } }

        public void Reset()
        {
            lock(listLock)
            {
                nonRepeatRandomList.Clear();
            }
        }
       
        public Color RandomColor(byte alpha = 0xff)
        {
            byte r = (byte)(random.Next(128) + 128);
            byte g = (byte)(random.Next(256));
            byte b = (byte)(random.Next(256));

            return new Color() { R = r, G = g, B = b, A = alpha };
        }
    }
}
