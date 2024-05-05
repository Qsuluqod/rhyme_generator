using System;
using System.Collections.Generic;
using System.Data;

namespace RhymeDictionary {
    /// <summary>
    /// Třída, kde jsou uloženy všechny fonetické a fonologické vlastnosti písmen, na jejichž základě zvládne
    /// sestavit matici podobnosti nebo udržet funkce podobnosti.
    /// </summary>
    public static class IPA {


        // VŠECHNY FONÉMY (dohromady 33)
        public static char[] Chars = new char[] { 'a', 'á', 'b', 'd', 'ď', 'e', 'é', 'f', 'g', 'h', 
                                                'ȟ', 'i', 'í', 'j', 'k', 'l', 'm', 'n', 'ň', 'o', 
                                                'ó', 'p', 'r', 'ř', 's', 'š', 't', 'ť', 'u', 'ú', 
                                                'v', 'z', 'ž' }; // chybí c, č, ě, ch, q, ů, x, y, ý, w

        public static int num_of_chars = Chars.Length;

        // FONOLOGICKÉ ČLENĚNÍ
        public static int[][] Attributes = new int[][] { new int[] { 1, 1, -1, -1, -1 }, // a
                                                        new int[] { 1, 1, -1, -1, 1 }, // á
                                                        new int[] { 1, -1, -1, -1, 0, 1, 0, 1 }, // b
                                                        new int[] { 1, -1, -1, 1, 0, 1, 0, 1 }, // d
                                                        new int[] { 1, -1, -1, 1, -1, -1, 0, 1 }, // ď
                                                        new int[] { 1, -1, -1, -1, -1 }, // e
                                                        new int[] { 1, -1, -1, -1, 1 }, // é
                                                        new int[] { 1, -1, 1, -1, 0, 1, 0, -1 }, // f
                                                        new int[] { 1, -1, -1, -1, 0, -1, 0, 1 }, // g
                                                        new int[] { 1, -1, 1, -1, 0, -1, 0, 1 }, // h

                                                        new int[] { 1, -1, 1, -1, 0, -1, 0, -1 }, // ȟ
                                                        new int[] { 1, -1, -1, 1, -1 }, // i
                                                        new int[] { 1, -1, -1, 1, 1 }, // í
                                                        new int[] { 1, 1, 1, 0, 0, -1, 0, 0 }, // j
                                                        new int[] { 1, -1, -1, -1, 0, -1, 0, -1 }, // k
                                                        new int[] { 1, 1, 1, 0, 0, 1, 0, 0 }, // l
                                                        new int[] { 1, 1, 0, -1, 0, 0, 0, 0 }, // m
                                                        new int[] { 1, 1, -1, 1, 0, 1, 0, 0 }, // n
                                                        new int[] { 1, 1, -1, 1, 0, -1, 0, 0 }, // ň
                                                        new int[] { 1, -1, 1, -1, -1 }, // o

                                                        new int[] { 1, -1, 1, -1, 1 }, // ó
                                                        new int[] { 1, -1, -1, -1, 0, 1, 0, -1 }, // p
                                                        new int[] { 1, 1, 0, 0, 0, 0, 1, 0 }, // r
                                                        new int[] { 1, -1, 0, 0, 0, 0, 1, 0 }, // ř
                                                        new int[] { 1, -1, 1, 1, 0, 1, 0, -1 }, // s
                                                        new int[] { 1, -1, 1, 1, 0, -1, 0, -1 }, // š
                                                        new int[] { 1, -1, -1, 1, -1, 1, 0, -1 }, // t
                                                        new int[] { 1, -1, -1, 1, -1, -1, 0, -1 }, // ť
                                                        new int[] { 1, -1, 1, 1, -1 }, // u
                                                        new int[] { 1, -1, 1, 1, 1 }, // ú

                                                        new int[] { 1, -1, 1, -1, 0, 1, 0, 1 }, // v
                                                        new int[] { 1, -1, 1, 1, 0, 1, 0, 1 }, // z
                                                        new int[] { 1, -1, 1, 1, 0, -1, 0, 1 }, // ž                                             
                                                        };

        // FONETICKÉ ČLENĚNÍ
        // KONSONANTY (dohromady 23)
        // Způsob realizace
        public static char[] Plosive = new char[] { 'p', 'b', 't', 'd', 'ť', 'ď', 'k', 'g' }; // 8
        public static char[] Nasal = new char[] { 'm', 'n', 'ň' }; // 3
        public static char[] Trill = new char[] { 'r', 'ř' }; // 2
        public static char[] Fricative = new char[] { 'f', 'v', 's', 'z', 'š', 'ž', 'ȟ', 'h' }; // 8
        public static char[] Approximant = new char[] { 'j' }; // 1
        public static char[] Lateral = new char[] { 'l' }; // 1
        public static char[][] ConsonantsRealistion = new char[][] { Plosive, Nasal, Trill, Fricative, Approximant, Lateral };

        // Místo realizace
        public static char[] Bilabial = new char[] { 'p', 'b', 'm' }; // 3
        public static char[] Labiodental = new char[] { 'f', 'v' }; // 2
        public static char[] Alveolar = new char[] { 't', 'd', 'n', 'r', 'ř', 's', 'z', 'š', 'ž', 'l' }; // 10
        public static char[] Palatal = new char[] { 'ť', 'ď', 'ň', 'j' }; // 4
        public static char[] Velar = new char[] { 'k', 'g', 'ȟ' }; // 3
        public static char[] Glottal = new char[] { 'h' }; // 1
        public static char[][] ConsonantsPlace = new char[][] { Bilabial, Labiodental, Alveolar, Palatal, Velar, Glottal };

        // Znělost
        public static char[][] VoicePairs = new char[][] { new char[] { 'p', 'b' }, 
                                                        new char[] { 't', 'd' },
                                                        new char[] { 'ť', 'ď' },
                                                        new char[] { 'k', 'g' },
                                                        new char[] { 'f', 'v' },
                                                        new char[] { 's', 'z' },
                                                        new char[] { 'š', 'ž' } };

        // VOKÁLY (dohromady 10)
        public static char[] Front = new char[] { 'i', 'í', 'e', 'é', 'a', 'á' };
        public static char[] Back = new char[] { 'u', 'ú', 'o', 'ó' };
        public static char[] Closed = new char[] { 'i', 'í', 'e', 'é' };
        public static char[] OpenClosed = new char[] { 'e', 'é', 'o', 'ó' };
        public static char[] Open = new char[] { 'a', 'á' };
        public static char[][] VocalVertical = new char[][] { Closed, OpenClosed, Open };
        public static char[][] VocalHorizontal = new char[][] { Front, Back };

        public static char[][] DurationPairs = new char[][]  { new char[] { 'a', 'á' }, 
                                                        new char[] { 'e', 'é' },
                                                        new char[] { 'i', 'í' },
                                                        new char[] { 'o', 'ó' },
                                                        new char[] { 'u', 'ú' } };

        // Všechny skupiny
        public static char[][][] AllGroups = new char[][][] { ConsonantsRealistion, ConsonantsPlace, VoicePairs, VocalHorizontal, VocalVertical, DurationPairs };

        // Pravidla pro fonetickou transkripci
        public static string[,] Rules = new string[,]  {
                                                        // ch 
                                                        { "ch", "ȟ"},
                                                        // d, t, n
                                                        { "di", "ďi" },
                                                        { "dí", "ďí" },
                                                        { "ti", "ťi" },
                                                        { "tí", "ťí" },
                                                        { "ni", "ňi" },
                                                        { "ní", "ňí" },
                                                        { "dě", "ďe" },
                                                        { "tě", "ťe" },
                                                        { "ně", "ňe" },
                                                        // ě
                                                        { "mě", "mňe" },
                                                        { "ě", "je" },
                                                        // c, č
                                                        { "c", "ts" },
                                                        { "č", "tš" },
                                                        // y, ý
                                                        { "y", "i" },
                                                        { "ý", "í" },
                                                        // ostatní
                                                        { "q", "kv" },
                                                        { "ů", "ú" },
                                                        { "x", "ks" },
                                                        { "w", "v" } };


        // MATICE FONÉMŮ
        public static PhonemMatrix phoneticMatrix = new PhonemMatrix();
        public static PhonemMatrix phonologicMatrix = new PhonemMatrix();

        // PODOBNOSTNÍ FUNKCE
        public delegate int[] SimilarLetters(int letter);

        /// <summary>
        /// Podobnostní funkce, která vrátí pouze to stejné písmeno. Velice striktní.
        /// </summary>
        /// <param name="letter">Číselná reprezentace znaku, pro který chceme zjistit ten stejný znak.</param>
        /// <returns>Číselná reprezentace tohotéhož znaku.</returns>
        public static int[] SameLetterOnly(int letter) {
            return new int[] { letter };
        }

        /// <summary>
        /// Podobnostní funkce, která pro všechna písmena vrátí všechna písmena. Velice benevolentní.
        /// </summary>
        /// <param name="letter">Číselná reprezentace naku, pro který chceme získat všechna písmena. 
        /// Hodnět na tomto parametru záleží. Nepodceňovat v žádném případě!</param>
        /// <returns>Číselné reprezentace všech znaků.</returns>
        public static int[] AllLetters(int letter) {
            return Enumerable.Range(0, num_of_chars).ToArray();
        }

        /// <summary>
        /// Podobnostní funkce, která pro všechna písmena, vrátí všechna písmena, která s ním sdílí alespoň jednu
        /// fonetickou vlastnost.
        /// </summary>
        /// <param name="letter">Číselná reprezentace znaku, pro který hledáme podobné znaky.</param>
        /// <returns>Číselné reprezentace všech znaků, které sdílejí s hledaným znakem alespoň jeden rys</returns>
        public static int[] SimilarLettersFromAllGroups(int letter) {
            return GetLettersFromAllGroups(AllGroups, Chars[letter]);
        }

        /// <summary>
        /// Podobnostní funkce, která
        /// </summary>
        /// <param name="letter">Číselná reprezentace znaku, pro který hledáme podobné znaky.</param>
        /// <returns></returns>
        public static int[] SimilarLettersFromPairsOnly(int letter) {
            return GetLettersFromAllGroups(new char[][][] { VoicePairs, DurationPairs }, Chars[letter]);
        }

        public static int count = 5;
        public static float tolerance = 0;
        /// <summary>
        /// Podobnostní funkce, která
        /// </summary>
        /// <param name="letter">Číselná reprezentace znaku, pro který hledáme podobné znaky.</param>
        /// <returns></returns>
        public static int[] SimilarLettersByPhoneticMatrix(int letter) {
            int[] res = phoneticMatrix.GetBestLettersForLetter(letter, count, tolerance);
            return res;
        }

        /// <summary>
        /// Podobnostní funkce, která
        /// </summary>
        /// <param name="letter">Číselná reprezentace znaku, pro který hledáme podobné znaky.</param>
        /// <returns></returns>
        public static int[] SimilarLettersByPhonologicMatrix(int letter) {
            int[] res = phoneticMatrix.GetBestLettersForLetter(letter, count, tolerance);
            return res;
        }

        /// <summary>
        /// Vepíše hodnoty do matice fonému, kde podobnostní vztahy jsou určeny na základě fonologických rysů.
        /// </summary>
        public static void GeneratePhonologicMatrix() {
            for (int r = 0; r < num_of_chars; r++) {
                for (int c = 0; c < num_of_chars; c++) {
                    // pokud jsou písmena stejná nebo porovnáváme konsonant a vokál, přeskočíme
                    if (r <= c || Attributes[r].Length != Attributes[c].Length)
                        continue;

                    char[] pair = new char[] { Chars[r], Chars[c] };
                    // vypočítáme míru společných rysů                    
                    int rate = 0;
                    for (int i = 0; i < Attributes[r].Length; i++) {
                        if (Attributes[r][i] == Attributes[c][i])
                            rate++;
                    }
                    // jedná se o dva vokály
                    if (Attributes[r].Length == Attributes[0].Length) {
                        phonologicMatrix.CreateRelationUsingChar(pair, (float)0.5 / Attributes[r].Length * rate);
                    }
                    // jedná se o dva konsonanty
                    else {
                        phonologicMatrix.CreateRelationUsingChar(pair, (float)1 / Attributes[r].Length * rate);
                    }
                }
            }
            // posílení podobnosti znělostních páru
            foreach (char[] pair in VoicePairs) {
                phonologicMatrix.CreateRelationUsingChar(pair, 0.1f);
            }
            // posílení podobnosti délkových párů
            foreach (char[] pair in DurationPairs) {
                phonologicMatrix.CreateRelationUsingChar(pair, 0.5f);
            }
        }

        /// <summary>
        /// Vepíše hodnoty do matice fonému, kde podobnostní vztahy jsou určeny na základě fonetických rysů.
        /// </summary>
        public static void GeneratePhoneticMatrix() {
            // Konsonanty způsob tvoření
            phoneticMatrix.CreateRelationUsingChar(Plosive, 0f);
            phoneticMatrix.CreateRelationUsingChar(Nasal, 0.6f);
            phoneticMatrix.CreateRelationUsingChar(Trill, 0.4f);
            phoneticMatrix.CreateRelationUsingChar(Fricative, 0.1f);
            phoneticMatrix.CreateRelationUsingChar(Approximant, 0.1f);
            phoneticMatrix.CreateRelationUsingChar(Lateral, 0.1f);

            // Konsonanty místo tvoření
            phoneticMatrix.CreateRelationUsingChar(Bilabial, 0.3f);
            phoneticMatrix.CreateRelationUsingChar(Labiodental, 0.1f);
            phoneticMatrix.CreateRelationUsingChar(Alveolar, 0.0f);
            phoneticMatrix.CreateRelationUsingChar(Palatal, 0.3f);
            phoneticMatrix.CreateRelationUsingChar(Velar, 0.3f);
            phoneticMatrix.CreateRelationUsingChar(Glottal, 0.1f);

            // Vokály horizontální
            phoneticMatrix.CreateRelationUsingChar(Closed, 0f);
            phoneticMatrix.CreateRelationUsingChar(OpenClosed, 0.1f);
            phoneticMatrix.CreateRelationUsingChar(Open, 0.1f);

            // Vokály vertikální
            phoneticMatrix.CreateRelationUsingChar(Front, 0.1f);
            phoneticMatrix.CreateRelationUsingChar(Back, 0.1f);
        
            // Znělostní páry
            foreach (char[] pair in VoicePairs) {
                phoneticMatrix.CreateRelationUsingChar(pair, 0.6f);
            }
            // Délkové páry
            foreach (char[] pair in DurationPairs) {
                phoneticMatrix.CreateRelationUsingChar(pair, 0.7f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="over_groups"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        private static int[] GetLettersFromAllGroups(char[][][] over_groups, char letter) {
            bool[] already_in = new bool[num_of_chars];
            List<int> res = new List<int>();
            foreach (char[][] over_group in over_groups) {
                foreach (char ch in GetLettersFromGroup(over_group, letter)) {
                    int index = GetIndex(Chars, ch);
                    if (!already_in[index]) {
                        already_in[index] = true;
                        res.Add(index);
                    }
                }
            }
            return res.ToArray();
        }

        /// <summary>
        /// Pro zadané písmeno a pole polí, najde první pole, které písmeno obsahuje. Předpokládá se
        /// že pole se bude sestávat z disjunktních polí.
        /// </summary>
        /// <param name="groups">Pole polí, ze kterého chceme najít to jedno, které obsahuje náš znak.</param>
        /// <param name="letter">Znak, jehož pole chceme najít.</param>
        /// <returns>Pole ve kterém se znak nachází.</returns>
        private static char[] GetLettersFromGroup(char[][] groups, char letter) {
            foreach (char[] group in groups) {
                foreach (char ch in group) {
                    if (ch == letter) {
                        return group;
                    }
                }
            }
            return new char[0];
        }

        /// <summary>
        /// Vytvoří fonetickou transkripci, kterou následně převede do číselné reprezentace znaků.
        /// </summary>
        /// <param name="word">Slovo, které chceme převést.</param>
        /// <returns>Pole číselných reprezentací znaků onoho řetězce.</returns>
        public static int[] TranscriptAndConvertString(string word) {
            return ConvertString(TranscriptString(word));
        }

        /// <summary>
        /// Vytvoří fonetickou transkripci řetězce zapsaného dle ortografického úzusu mluvnice české.
        /// </summary>
        /// <param name="word">Slovo, které cheme převést.</param>
        /// <returns>Fonetická transkripce word.</returns>
        private static string TranscriptString(string word) {
            string cur = word;
            // základní fonetické transkripce
            for (int i = 0; i < Rules.Length / 2; i++) {
                cur = cur.Replace(Rules[i, 0], Rules[i, 1]);
            }

            // odebrání znělosti koncovým obstruentům
            for (int i = 0; i < VoicePairs.Length; i++) {
                if (cur[cur.Length - 1] == VoicePairs[i][1])
                    cur = string.Concat(cur.Substring(0, cur.Length - 1), VoicePairs[i][0]);
            }

            // přidej zpodobu znělosti ve skupině obstruentů
            return cur;
        }

        /// <summary>
        /// Převede textový řetězec na pole číselných reprezentací znaků onoho řetězce.
        /// </summary>
        /// <param name="word">Řetězec, který chceme převést.</param>
        /// <returns>Pole číselných reprezentací znaků onoho řetězce.</returns>
        private static int[] ConvertString(string word) {
            int[] res = new int[word.Length];
            for (int i = 0; i < word.Length; i++) {
                int index = GetIndex(Chars, word[i]);
                if (index == num_of_chars) {
                    Console.WriteLine("Problém");
                    res[i] = -1;
                }
                res[i] = index;
            }
            return res;
        }

        /// <summary>
        /// Převede pole znaků na jejich číselné reprezentace.
        /// </summary>
        /// <param name="array">Pole znaků.</param>
        /// <returns>Pole číselných reprezentací znaků.</returns>
        private static int[] ConvertCharArray(char[] array) {
            int[] res = new int[array.Length];
            for (int i = 0; i < array.Length; i++) {
                res[i] = GetIndex(Chars, array[i]);
            }
            return res;
        }

        /// <summary>
        /// Vrátí index znaku z pole znaků.
        /// </summary>
        /// <param name="array">Pole znaků, v němž chceme najít index prvku.</param>
        /// <param name="wanted">Znak, jehož index cheme znát.</param>
        /// <returns>Index prvku wanted v array.</returns>
        public static int GetIndex(char[] array, char wanted) {
            int res = 0;
            foreach (char item in array) {
                if (item == wanted) {
                    return res;
                }
                res++;
            }
            Console.WriteLine("Malér s písmenem: " + wanted);
            return res;
        }
        
    }
}