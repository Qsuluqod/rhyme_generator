using System;

namespace RhymeDictionary {
    /// <summary>
    /// Reprezentace rýmu, obsahuje finální slovo zapsané ortografickým úzusem a míru 
    /// rýmu. To jest číslo od 1 do 0, které vznikne vynásobením určitého počtu mír podobnosti
    /// mezi písmeny v rýmu a slovem, pro něhož rýmy hledáme. Například pro slova neví a trefí,
    /// kdyby byla míra podobnosti mezi v a f 0.8, bude pro poslední tři písmena míra podobnosti 0.8.
    /// K tomuto číslu jsme došli výpočtem 1 * 0.8 * 1. Míry podobností mezi stejnými písmeny jsou 
    /// totiž vždy jedna.
    /// </summary>
    class Rhyme {
        // Rým zapsaný ortografickým úzusem.
        public string word;
        // Míra podobnosti.
        public float rate;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="_word">Rým zapsaný ortografickým úzusem.</param>
        /// <param name="_rate">Míra podobnosti.</param>
        public Rhyme(string _word, float _rate) {
            word = _word;
            rate = _rate;
        }
    }
}