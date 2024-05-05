using System;

namespace RhymeDictionary {
    /// <summary>
    /// Reprezentuje Jeden vrchol ve strom Trie.
    /// </summary>
    public class Node {
        public int character;
        public int depth;
        // Pole vrcholů (písmen) do kterých se můžeme dostat
        public Node[] Following;
        // Konečná slova, která se ve vrcholu nachází
        public List<String> Words;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="_character">Znak, který se ve stromě nachází reprezentovaný číslem, které indexuje IPA.Chars</param>
        /// <param name="_depth">Hloubka, ve které se vrchol nachází od kořene.</param>
        public Node(int _character, int _depth) {
            character = _character;
            depth = _depth;
            Following = new Node[IPA.num_of_chars];
            Words = new List<string>();
        }

        /// <summary>
        /// Přidá slovo do vrcholu. V každém vrcholu totiž může být i více slov. Například
        /// slova oběd a objet vypadají obě ve fonetické transkripci [objet], takže se budou
        /// nacházet ve stejném vrcholu. Slova se sem ukládají zapsaná podle ortografického úzusu
        /// češtiny.
        /// </summary>
        /// <param name="new_word">Slovo zapsané podle ortografického úzusu.</param>
        public void AddWord(string new_word) {
            foreach(string word in Words) {
                if (word == new_word)
                    return;
            }
            Words.Add(new_word);
        }

        /// <summary>
        /// Vrátí zákládní informace o vrcholu přehledně zapsané.
        /// </summary>
        /// <returns>Základní informace o vrcholu.</returns>
        public override string ToString()
        {
            string res;
            if (character == -1)
                res = "KOŘEN (" + depth;
            else
                res = "Vrchol " + IPA.Chars[character] + " (" + depth;

            if (Words.Count > 0) {
                res += "): " + Words[0];
            }
            else {
                res += ")";
            }
            res += ", -> ";
            foreach (Node node in Following) {
                if (node != null)
                    res += IPA.Chars[node.character] + ", ";
            }
            return res;
        }
    }
}