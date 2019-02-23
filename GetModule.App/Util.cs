using System.Collections.Generic;
using System.Diagnostics;

namespace GetModule.App
{
    public static class Util
    {
        public static void PrintList<T>(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                Debug.WriteLine(item);
            }
        }
    }
}
