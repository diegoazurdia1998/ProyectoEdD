using System;

namespace Salud_para_tu_sonrisa.Models.Comparadores
{
    public class DescriptionComparer
    {
        public bool Find(string description, string searchedWord)
        {
            String Search = "";
            for (int i = 0; i < description.Length; i++)
            {
                if (description[i] == ' ' || description[i] == ',' || description[i] == '.')
                {
                    if (Search.CompareTo(searchedWord) == 0)
                    {
                        return true;
                    }
                    else
                        Search = "";
                }
                else
                    Search = Search + description[i];
                
            }
            if (Search.CompareTo(searchedWord) == 0)
            {
                return true;
            }
            return false;
        }
    }
}
