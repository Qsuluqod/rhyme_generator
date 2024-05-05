using System;
using System.Runtime.InteropServices;

namespace RhymeDictionary {
    /// <summary>
    /// Matice podobností fonémů. Jejím hlavním attributem je Matice, kde pro každá dvě písmena máme
    /// míru jejich podobnosti. Písmena jsou reprezentována opět čísly, takže například vztah mezi
    /// písmeny b a p najdeme na souřadnicích [2, 21] nebo [21, 2] (b je třetí foném a b dvacátý druhý).
    /// Relace podobnosti je symetrická, tudíž i matice je symetrická. Diagonála by vždy měla být vyplněna
    /// samými jedničkami, ježto stejná písmena jsou si nejpodobnější a všechna ostatní čísla by neměla
    /// převyšovat jedničku, ani být nižší než nula. Po vygenerování matice můžeme rovnou pro každé písmeno
    /// vygenerovat seřazené pole jeho nejlepších podobností, aby se to nemuselo dělat pokaždé znova.
    /// </summary>
    public class PhonemMatrix {

        // Věci kvůli barvičkám v konsoli
        //[DllImport("kernel32.dll")]
        // public static extern IntPtr GetStdHandle(int nStdHandle);
        // [DllImport("kernel32.dll")]
        // public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);
        // public const int STD_OUTPUT_HANDLE = -11;
    
        // Pole podobností fonémů
        public float[,] Matrix;
        // Rozměr matice
        int extent = IPA.num_of_chars;
        
        // Pole nejlepších podobností pro každé písmeno
        public int[][] BestLetters = new int[IPA.num_of_chars][];
        // Pole mír nejlepších podobností pro každé písmeno (zde jsou hodnoty z matice podobností fonémů)
        public float[][] RateOfBestLetters = new float[IPA.num_of_chars][];

        public int[] GetBestLettersForLetter(int letter, int count, float minimal_rate=0f) {
            // zjistíme, kdy ještě písmena splňují minimální míru podobnosti
            int breaking_index;
            for (breaking_index = 0; breaking_index < extent; breaking_index++) {
                if (RateOfBestLetters[letter][breaking_index] < minimal_rate) {
                    break;
                }
            }
            
            // vybereme prvních několik písmen
            int[] res = new int[Math.Min(count, breaking_index)];
            for (int i = 0; i < Math.Min(count, breaking_index); i++) {
                res[i] = BestLetters[letter][i];
            }
            
            return res;
        }

        /// <summary>
        /// Vyplní pole nejlepších podobností a jejich mír pro všechna písmena.
        /// </summary>
        public void GenerateBestLetters() {
            for (int i = 0; i < extent; i++) {
                GenerateBestLettersForLetter(i);
            }
        }

        /// <summary>
        /// Vyplní pole nejlepších podobností a jejich mír pro jedno písmeno.
        /// </summary>
        /// <param name="letter">Písmeno, pro které vyplňujeme podobnosti a jejich míry.</param>
        public void GenerateBestLettersForLetter(int letter) {
            float[][] relations = new float[extent][];

            // vybere celý jeden řádek matice, ke každé hodnotě přidá i index (tedy o jaké písmeno se jedná)
            for (int i = 0; i < extent; i++) {
                relations[i] = new float[2] { Matrix[letter,i], i };
            }

            // seřadí podle hodnot matice, indexy se tedy zpřechází
            Array.Sort(relations, (x, y) => y[0].CompareTo(x[0]));

            // nahrajeme hodnoty do tabulek
            BestLetters[letter] = new int[extent];
            RateOfBestLetters[letter] = new float[extent];
            for (int i = 0; i < extent; i++) {
                BestLetters[letter][i] = (int)relations[i][1];
                RateOfBestLetters[letter][i] = relations[i][0];
            }

        }

        /// <summary>
        /// Předá funkci CreateRelation() seznam číselné reprezentace znaků.
        /// </summary>
        /// <param name="group">Seznam fonémů ve znakové reprezentaci.</param>
        /// <param name="rate">Míra, o kterou se zvýší podobnost zavoláním CreateRelation().</param>
        public void CreateRelationUsingChar(char[] group, float rate) {
            int[] indexes = new int[group.Length];
            for (int i = 0; i < group.Length; i++) {
                indexes[i] = IPA.GetIndex(IPA.Chars, group[i]);
            }
            CreateRelation(indexes, rate);
        }

        /// <summary>
        /// Vytvoří relaci (zvýši míry podobností) mezi skupinou fonémů. Zvýší míru mezi každými dvěma
        /// fonémy ve skupině. 
        /// </summary>
        /// <param name="group">Seznam fonémů.</param>
        /// <param name="rate">Míra, o kterou se zvýší míra podobnosti.</param>
        public void CreateRelation(int[] group, float rate) {
            for (int i = 0; i < group.Length; i++) {
                for (int j = 0; j < group.Length; j++) {
                    // Podobnost na diagonále nezvyšujeme
                    if (i == j)
                        continue;
                    Matrix[group[i],group[j]] += rate;
                    // Podobnost mezi dvěma odlišnými písmeny by nikdy neměla přesáhnout 1
                    if (Matrix[group[i],group[j]] >= 1) {
                        Console.WriteLine("PROBLÉM, vztah mezi písmeny " + 
                        IPA.Chars[group[i]] + " a " + IPA.Chars[group[j]] + " převýšil nebo se rovná 1");
                    }
                }
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public PhonemMatrix() {
            Matrix = new float[extent,extent];

            // dá všude nuly
            FillWith(0.0f);

            // vypní diagonálu jedničkami
            for (int i = 0; i < extent; i++) {
                Matrix[i,i] = 1.0f;
            }
        }

        /// <summary>
        /// Vypíše tabulky nejlepších podobností pro všechna písmena.
        /// </summary>
        public void PrintBestLetters() {
            
            for (int r = 0; r < extent; r++) {
                Console.Write(" " + IPA.Chars[r] + ": ");
                for (int c = 0; c < extent; c++) {
                    Console.Write(IPA.Chars[BestLetters[r][c]]);
                    Console.Write(", ");
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Vytiskne matici podobností v intuitivní podobě s obarvenými mírami podobností.
        /// </summary>
        public void PrintMatrix() {
            string divider = " ";
            
            Console.Write(" ." + divider);
            foreach (char ch in IPA.Chars) {
                Console.Write(" " + ch + " " + divider);
            }
            Console.Write("\n");
            for (int r = 0; r < extent; r++) {
                Console.Write(" " + IPA.Chars[r] + divider);
                for (int c = 0; c < extent; c++) {
                    //IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
                    //SetConsoleTextAttribute(hConsole, Tools.GenerateColor(Matrix[r,c]));
                    Console.Write(Matrix[r,c].ToString("F1") + divider);
                    //Console.ResetColor();
                }
                Console.Write("\n");
            }
            
        }

        /// <summary>
        /// Zaplní celou matici jedním číslem.
        /// </summary>
        /// <param name="filling">Číslo, kterým zaplní celou matici.</param>
        public void FillWith(float filling) {
            for (int i = 0; i < extent; i++) {
                for (int j = 0; j < extent; j++) {
                    Matrix[i, j] = filling;
                }
            }
            
        }
    }
}