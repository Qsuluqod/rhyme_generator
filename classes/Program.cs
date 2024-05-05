using System;

namespace RhymeDictionary
 {
    /// <summary>
    /// Hlavní řídící struktura, která incializuje ostatní třídy a udělá základní prezentaci programu.
    /// </summary>
    public static class RhymeDictionary {

        // Vstupní data pro sestavení trie
        static string file_name = "words_data/data.txt";
        static char separator = ',';

        // Vstupní data pro hledání rýmů
        static string[] words = { "rozpustnost", "afrodiziakum", "máj" };
        static float rhyme_tolerance = 0.1f;

        // Nastavení výstupu programu
        static int rhyme_range = 6;
        static int print_max_rhymes = 5;


        static Trie trie = new Trie();

        static void Main() {
            // STROM TRIE
            // Sestaví trie
            trie.BuildFromFile(file_name, separator);
            // Vypíše strom trie
            //trie.PrintTrie(true);

            // MATICE PODOBNOSTI PÍSMEN
            // Vytvoří matice podobností písmen
            IPA.GeneratePhoneticMatrix();
            IPA.GeneratePhonologicMatrix();
            // Každému písmenu vytvoří seznam jeho nejlepších shod
            IPA.phoneticMatrix.GenerateBestLetters();
            IPA.phonologicMatrix.GenerateBestLetters();
            // Vypíše matici podobnosti písmen
            // IPA.phonemMatrix.PrintMatrix();
            IPA.phonologicMatrix.PrintMatrix();
            // Vypíše seznamy nejpodobnějších písmen pro všechna písmena
            // IPA.phonemMatrix.PrintBestLetters();
            // IPA.phonologicMatrix.PrintBestLetters();

            // HLEDÁNÍ RÝMŮ SAMOTNÉ
            foreach (string word in words) {
                TryRhymeRangeWithMatrix(word, rhyme_range, IPA.phonologicMatrix, rhyme_tolerance);
                TryRhymeRangeWithMatrix(word, rhyme_range, IPA.phoneticMatrix, rhyme_tolerance);
                TryRhymeRangeWithFunction(word, rhyme_range, IPA.SameLetterOnly);
            }
        }

        /// <summary>
        /// Vyhledá rýmy s důrazem na rýmování pro všechny možnosti v určitém rozsahu.
        /// K vyhledání rýmu používá funkci. Rýmy nevrací, ale rovnou je přehledně vytiskne.
        /// </summary>
        /// <param name="word">Slovo, pro které hledáme rým.</param>
        /// <param name="range">Rozsah pro důrazy, v jakých budeme hledat rýmy.</param>
        /// <param name="similarLetters">Podobnostní funkce.</param>
        static void TryRhymeRangeWithFunction(string word, int range, IPA.SimilarLetters similarLetters) {
            int[] converted = IPA.TranscriptAndConvertString(word);

            Console.WriteLine("Generuji rýmy FUNKCÍ\n    rozsah: 1 až " 
            + Math.Min(range, converted.Length) + "\n    slovo: " + word);

            for (int i = 1; i < Math.Min(range, converted.Length) + 1; i++) {
                PrintLettersInPlural(i);
                PrintStrings(trie.FindRhymes(converted, i, similarLetters));
            }
            Console.Write("\n");
        }

        /// <summary>
        /// Vyhledá rýmy s důrazem na rýmování pro všechny možnosti v určitém rozsahu.
        /// K vyhledání rýmu používá podobnostní matici a toleranci. Rýmy nevrací, ale rovnou je přehledně vytiskne.
        /// </summary>
        /// <param name="word">Slovo, pro které hledáme rým.</param>
        /// <param name="range">Rozsah pro důrazy, v jakých budeme hledat rýmy.</param>
        /// <param name="matrix">Podobnostní funkce.</param>
        /// <param name="tolerance">Tolerance, s jakou hledáme rýmy.</param>
        static void TryRhymeRangeWithMatrix(string word, int range, PhonemMatrix matrix, float tolerance) {
            int[] converted = IPA.TranscriptAndConvertString(word);

            Console.WriteLine("Generuji rýmy MATICÍ\n    rozsah: 1 až " 
            + Math.Min(range, converted.Length) + "\n    slovo: " + word);

            for (int i = 1; i < Math.Min(range, converted.Length) + 1; i++) {
                PrintLettersInPlural(i);
                PrintRhymes(trie.FindRhymesWithTolerance(converted, i, tolerance, matrix));
            }
            Console.Write("\n");
        }

        /// <summary>
        /// Přehledně vytiskne seznam rýmů v podobě stringů.
        /// </summary>
        /// <param name="rhymes">Seznam rýmů, který chceme vytisknout.</param>
        static void PrintStrings(List<string> rhymes) {
            for (int i = 0; i < Math.Min(print_max_rhymes, rhymes.Count()); i++) {
                Console.Write(rhymes[i] + ", ");
            }
            Console.Write("\n");
        }

        /// <summary>
        /// Přehledně vytiskne seznam rýmů uložené pomocí třídy rým.
        /// </summary>
        /// <param name="_rhymes">Seznam rýmů.</param>
        /// <param name="verbose">Když pravda, tak vytiskne k rýmům i jejich míry rýmování se.</param>
        static void PrintRhymes(List<Rhyme> _rhymes, bool verbose=false) {
            Rhyme[] rhymes = _rhymes.ToArray();
            Array.Sort(rhymes, (x, y) => y.rate.CompareTo(x.rate));
            for (int i = 0; i < Math.Min(print_max_rhymes, rhymes.Length); i++) {
                Console.Write(rhymes[i].word);
                if (verbose)
                    Console.Write(" (" + rhymes[i].rate.ToString("F1") + "), ");
                else
                    Console.Write(", ");
            }
            Console.Write("\n");
        }

        static void PrintLettersInPlural(int count) {
            Console.Write("Důraz na poslední " + count + " písmen");
                if (count == 1)
                    Console.Write("o: ");
                else if (count < 5) 
                    Console.Write("a: ");
                else
                    Console.Write(": ");
        }
    }
}



