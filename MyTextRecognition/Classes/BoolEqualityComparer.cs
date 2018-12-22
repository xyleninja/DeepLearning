using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTextRecognition
{
    public class BoolEqualityComparer : IEqualityComparer<bool[]>
    {
        public bool Equals(bool[] x, bool[] y)
        {
            try
            {
                for (int i = 0; i < x.Length; i++)
                    if (x[i] != y[i])
                        return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetHashCode(bool[] obj)
        {
            int result = 21;

            foreach (bool b in obj)
            {
                if (b) { result++; }
                result *= 14;
            }

            return result;
        }
    }
}
