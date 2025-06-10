using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe
{
    public static class Helper
    {
        public static IEnumerable<T> RepeatFor<T>(this T element, int count)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element), "Cannot be null!");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Cannot be negative!"); 
            }

            for (int i = 0; i < count; i++)
            {
                yield return element;
            }
        }
    }
}
