using System;
using System.Collections.Generic;
using System.Text;

namespace UnityDocsBot
{
    public static class LevenshteinDistance
    {
        public static bool IsPartialMatch(this string s, string t) //gotta love the complete lifting of algorithms from dotnetpearls amirite lads
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0 && m == 0)
            {
                return true;
            }
            // Step 1
            if (n == 0 && m != 0)
            {
                return false;
            }

            if (m == 0 && n != 0)
            {
                return false;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            //return d[n, m];
            if ((100 - (((1.0 - (d[n, m] / (double)Math.Max(s.Length, t.Length)))) * 100)) > 99) //fiddled with this to return a boolean based on percentage
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
