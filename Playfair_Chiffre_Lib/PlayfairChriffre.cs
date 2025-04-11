using System;
using System.Collections.Generic;
using System.Text;
using Playfair_Chiffre_Lib;

namespace PlayfairChriffreLib
{
    public class PlayfairChriffre : IChriffre
    {
        private char[,] matrix;
        private Dictionary<char, (int Row, int Col)> positions;
        private readonly string key;

        public PlayfairChriffre(string key)
        {
            this.key = key;
            BuildMatrix(key);
        }

        private void BuildMatrix(string key)
        {
            matrix = new char[5, 5];
            positions = new Dictionary<char, (int, int)>();

            key = key.ToUpper().Replace(" ", "").Replace("J", "I");
            bool[] used = new bool[26];
            used['J' - 'A'] = true;
            List<char> matrixList = new List<char>();

            foreach (char c in key)
            {
                if (c < 'A' || c > 'Z') 
                    continue;
                int index = c - 'A';
                if (!used[index])
                {
                    used[index] = true;
                    matrixList.Add(c);
                }
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (c == 'J') 
                    continue;
                int index = c - 'A';
                if (!used[index])
                {
                    used[index] = true;
                    matrixList.Add(c);
                }
            }

            int pos = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = matrixList[pos];
                    positions[matrixList[pos]] = (i, j);
                    pos++;
                }
            }
        }

        private string Preprocess(string text)
        {
            text = text.ToUpper().Replace(" ", "").Replace("J", "I");
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c >= 'A' && c <= 'Z')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// In dieser Variante wird der zweite Buchstabe bei Doppelung
        /// komplett verworfen und durch ein X ersetzt, sodass er nicht
        /// erneut in der nächsten Schleife auftaucht.
        /// </summary>
        private List<string> CreateDigraphs(string text)
        {
            List<string> digraphs = new List<string>();
            int i = 0;
            while (i < text.Length)
            {
                char first = text[i];
                char second;
                
                if (i + 1 < text.Length)
                {
                    char nextChar = text[i + 1];
                    
                    if (first == nextChar)
                    {
                        // Zweiter Buchstabe ist derselbe => weg damit,
                        // stattdessen X und Pointer i um 2 erhöhen.
                        second = 'X';
                        i += 2;
                    }
                    else
                    {
                        second = nextChar;
                        i += 2;
                    }
                }
                else
                {
                    // Einzelner Buchstabe am Ende => X anhängen
                    second = 'X';
                    i++;
                }
                digraphs.Add($"{first}{second}");
            }
            return digraphs;
        }

        public string Encrypt(string msg)
        {
            string preprocessed = Preprocess(msg);
            List<string> digraphs = CreateDigraphs(preprocessed);
            StringBuilder cipherText = new StringBuilder();

            foreach (string digraph in digraphs)
            {
                char a = digraph[0];
                char b = digraph[1];
                var (rowA, colA) = positions[a];
                var (rowB, colB) = positions[b];

                if (rowA == rowB)
                {
                    int newColA = (colA + 1) % 5;
                    int newColB = (colB + 1) % 5;
                    cipherText.Append(matrix[rowA, newColA]);
                    cipherText.Append(matrix[rowB, newColB]);
                }
                else if (colA == colB)
                {
                    int newRowA = (rowA + 1) % 5;
                    int newRowB = (rowB + 1) % 5;
                    cipherText.Append(matrix[newRowA, colA]);
                    cipherText.Append(matrix[newRowB, colB]);
                }
                else
                {
                    cipherText.Append(matrix[rowA, colB]);
                    cipherText.Append(matrix[rowB, colA]);
                }
            }
            return cipherText.ToString();
        }

        public string Decrypt(string msg)
        {
            msg = msg.ToUpper().Replace(" ", "");
            StringBuilder plainText = new StringBuilder();

            for (int i = 0; i < msg.Length; i += 2)
            {
                char a = msg[i];
                char b = msg[i + 1];
                var (rowA, colA) = positions[a];
                var (rowB, colB) = positions[b];

                if (rowA == rowB)
                {
                    int newColA = (colA + 4) % 5;
                    int newColB = (colB + 4) % 5;
                    plainText.Append(matrix[rowA, newColA]);
                    plainText.Append(matrix[rowB, newColB]);
                }
                else if (colA == colB)
                {
                    int newRowA = (rowA + 4) % 5;
                    int newRowB = (rowB + 4) % 5;
                    plainText.Append(matrix[newRowA, colA]);
                    plainText.Append(matrix[newRowB, colB]);
                }
                else
                {
                    plainText.Append(matrix[rowA, colB]);
                    plainText.Append(matrix[rowB, colA]);
                }
            }

            return plainText.ToString();
        }
    }
}
