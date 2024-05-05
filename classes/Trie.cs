using System;
using System.IO;


namespace RhymeDictionary {
    /// <summary>
    /// Reprezentace stromu Trie. Slova v trii jsou uložená ve fonetické transkripci a každý foném
    /// je reprezentovaný číslem, kvůli následnému rychlejšímu indexovaní. Slova jsou také uložená
    /// pozpátku, aby se v Trii dali efektivně hledat rýmy.
    /// </summary>
    class Trie {

        // Kořen Trie
        private Node Root = new Node(-1, 0);

        /// <summary>
        /// Postaví strom Trie z csv souboru.
        /// </summary>
        /// <param name="path"> Cesta k souboru </param>
        /// <param name="separator"> Znak, jímž jsou slova v souboru oddělená </param>
        public void BuildFromFile(string path, char separator) {
            string content = File.ReadAllText(path);
            string[] words = content.Split(separator);

            // Všechna slova přidá do trie
            foreach (string word in words) {
                // Najde vrchol pro slovo (slovo transkribujeme a převedeme do čísel)
                Node node = FindNodeForWord(IPA.TranscriptAndConvertString(word));
                // Přidá slovo do vrcholu
                node.AddWord(word);
            }
        }

        /// <summary>
        /// Najde v Trie vrchol pro dané slovo, pokuď po cestě potřebuje vytvořit vrcholy, tak je vytvoří.
        /// </summary>
        /// <param name="word"> Slovo, pro které hledá vrchol </param>
        /// <returns> Vrchol, kam můžeme slovo uložit </returns>
        private Node FindNodeForWord(int[] word) {
            int depth = 1;
            // Aktuální vrchol
            Node cur = Root;
            foreach (int letter in word.Reverse()) {
                // Pokud vrchol kam chceme ze slova jít neexsituje, vytvoříme ho
                if (cur.Following[letter] == null) {
                    cur.Following[letter] = new Node(letter, depth);
                }
                cur = cur.Following[letter];
                depth++;
            }
            return cur;
        }

        /// <summary>
        /// Najde rýmy pro slovo za použití Matice podobností hlásek. Rýmy hledá s určitou tolerancí.
        /// </summary>
        /// <param name="_word"> Slovo, pro které hledá rým. </param>
        /// <param name="_depth"> Počet, kolik posledních písmen slov, se ještě musí rýmovat. </param>
        /// <param name="tolerance"> Desetinné číslo od 0 do 1, jak moc musí být nalezený rým dokonalý. </param>
        /// <param name="matrix"> Matice podobností hlásek, podle které hledá podobná písmena. </param>
        /// <returns> Seznam nalezených rýmů. </returns>
        public List<Rhyme> FindRhymesWithTolerance(int[] _word, int _depth, float tolerance, PhonemMatrix matrix) {
            List<Rhyme> res = new List<Rhyme>();
            int[] word = _word.Reverse().ToArray();
            int depth = Math.Min(Math.Abs(_depth), word.Length - 1);

            // Vytvoří první úroveň hledání
            Queue<March> queue = new Queue<March>();
            queue.Enqueue(new March(Root));

            // Pokračuje dál v hledání
            March cur;
            while (queue.Count() > 0) {
                cur = queue.Dequeue();
                
                // přidá nové vrcholy do fronty
                // jsme moc hluboko ve stromě, na rýmování už záleží
                if (cur.position.depth >= depth) {
                    // přidání rýmů do výsledku
                    foreach (string rhyme in cur.position.Words) {
                        res.Add(new Rhyme(rhyme, cur.rate));
                    }
                    // pridání následujících vrcholů do fronty
                    foreach (Node follower in cur.position.Following) {
                        if (follower != null)
                            queue.Enqueue(new March(follower, cur.rate));
                    }
                }
                // jsme ještě moc na povrchu, na rýmech ještě záleží
                else {
                    for (int i = 0; i < IPA.num_of_chars; i++) {
                        // zkontroluje, jestli další vrchol podobného písmena existuje
                        if (cur.position.Following[matrix.BestLetters[word[cur.position.depth]][i]] != null) {
                            // zkontroluje, jestli se další písmena rýmují dostatečně, pokud ne skončí
                            if (cur.rate * matrix.RateOfBestLetters[word[cur.position.depth]][i] >= tolerance) {
                                queue.Enqueue(new March(cur.position.Following[matrix.BestLetters[word[cur.position.depth]][i]],
                                                cur.rate * matrix.RateOfBestLetters[word[cur.position.depth]][i]));
                                // Console.WriteLine(cur.rate);
                                // Console.WriteLine(matrix.RateOfBestLetters[word[cur.position.depth]][i]);
                            }
                            else {
                                break;
                            }
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Najde rýmy za použití podobností funkce pro hlásky. Rýmy nehledá s nějakou tolerancí.
        /// </summary>
        /// <param name="_word">Slovo, pro které hledá rýmy.</param>
        /// <param name="_depth">Počet, kolik posledních písmen slov, se ještě musí rýmovat.</param>
        /// <param name="similarLetters">Funkce, podle které vyhledávání nachází podobná písmena.</param>
        /// <returns>Seznam nalezených rýmů.</returns>
        public List<string> FindRhymes(int[] _word, int _depth, IPA.SimilarLetters similarLetters) {
            List<string> res = new List<string>();
            int[] word = _word.Reverse().ToArray();
            int depth = Math.Min(Math.Abs(_depth), word.Length - 1);

            // Vytvoří první úroveň hledání
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(Root);

            // Pokračuje dál v hledání
            Node cur;
            while (queue.Count > 0) {                
                cur = queue.Dequeue();
                                
                // přídáváme nové vrcholy do fronty
                // jsme moc hluboko ve slově, na rýmování se už nezáleží
                if (cur.depth >= depth) {
                    // přídání rýmů
                    foreach (string rhyme in cur.Words) {
                        res.Add(rhyme);
                    }
                    // pridání následujících vrcholů do fronty
                    foreach (Node follower in cur.Following) {
                        if (follower != null) 
                            queue.Enqueue(follower);
                    }
                }
                // jsme na povrchu slova, na rýmování ještě záleží
                else {
                    foreach (int letter in similarLetters(word[cur.depth])) {
                        if (cur.Following[letter] != null)
                            queue.Enqueue(cur.Following[letter]);
                    }
                }
            }
          
            return res;
        }

        /// <summary>
        /// Vypíše strom trie do konzole v přehledném schéma.
        /// </summary>
        /// <param name="verbose">O vrcholech vypíše dodatečné množství informací, převážně pro debugování.</param>
        public void PrintTrie(bool verbose = false) {
            Stack<Node> stack = new Stack<Node>();
            stack.Push(Root);
            
            Node cur;
            while(stack.Count > 0) {
                cur = stack.Pop();
                for (int i = 1; i < cur.depth; i++) {
                    Console.Write("    ");
                }
                if (verbose)
                    Console.Write(cur);
                else {
                    Console.Write(IPA.Chars[cur.character] + " (" + cur.character + ")");
                }
                Console.Write("\n");

                foreach (Node node in cur.Following.Reverse()) {
                    if (node != null) 
                        stack.Push(node);
                }
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Trie() {
            
        }
        
    }
}