using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    static class CompareHelper
    {
        public static bool AreAlmostEqual(int a, int b, int maxDiff)
        {
            return a > b ? a - b <= maxDiff : b - a <= maxDiff;
        }
    }
}
