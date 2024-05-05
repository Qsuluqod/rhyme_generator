using System;

namespace RhymeDictionary {
    /// <summary>
    /// Tato třída obsahuje různé vychytávky, určené pouze pro vypisování do konzole převážně pro účely debugování.
    /// </summary>
    public static class Tools {
        public static void PrintArray(int[] array) {
            foreach (int num in array) {
                Console.Write(num + ", ");
            }
            Console.Write("\n");
        }
        

        public static void PrintIntCharArray(int[] array) {
            foreach (int num in array) {
                Console.Write(IPA.Chars[num] + ", ");
            }
            Console.Write("\n");
        }

        public static ushort[] colors = new ushort[] { 0x0000, 0x0008, 0x0008, 0x0005, 0x0001, 
                                                0x0003, 0x0002, 0x0006, 0x000E, 0x000C,  0x000F };
        public static ushort GenerateColor(float intensity) {
            float multiplied = intensity * 10;

            return colors[(int)multiplied];

        }

    }
}