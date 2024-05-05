using System;

namespace RhymeDictionary {
    /// <summary>
    /// Reprezentuje jeden průchod hledání do hloubky ve stromu Trie.
    /// </summary>
    class March {
        // Dosavadní míra rýmování se.
        public float rate;
        // Aktuální pozice ve které se průchod nachází.
        public Node position;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="node">Kde průchod začíná.</param>
        /// <param name="_rate">Aktůální míra rýmování se.</param>
        public March(Node node, float _rate=1f) {
            position = node;
            rate = _rate;
        }
    }
}