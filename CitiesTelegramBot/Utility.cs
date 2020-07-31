using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesTelegramBot
{
    static class Utility
    {
        public static char GetLastLetter(string word)
        {
            char letter;
            int i = 0;
            do
            {
                i++;
                letter = word[word.Length - i];
            }
            while (letter == 'ы' || letter == 'ъ' || letter == 'ь');
            word = word.ToUpper();
            return word[word.Length - i];
        }
    }
}
