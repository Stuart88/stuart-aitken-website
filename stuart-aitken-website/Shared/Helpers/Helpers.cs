using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public static class Helpers
    {
        public static string RandomToken()
        {
            Random rand = new Random();

            char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', '1', '2', '3', '4', '5', };

            string val = "";
            for (int i = 0; i < 30; i++)
            {
                val += letters[rand.Next(0, letters.Length)];
            }

            return val;
        }
    }
}
