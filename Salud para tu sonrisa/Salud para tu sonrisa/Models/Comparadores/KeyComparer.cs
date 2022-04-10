using System;

namespace Salud_para_tu_sonrisa.Models.Comparadores
{
    public class KeyComparer
    {
        public int Comparer(string x, string y)
        {
            return x.CompareTo(y);
        }
    }
}
