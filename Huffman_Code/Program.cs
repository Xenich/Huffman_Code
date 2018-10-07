using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace Huffman_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "лавировали, лавировали, да не выловировали";
            Console.WriteLine("Сообщение для кодировки: " +str+"\r\n");
            Huffman huff = new Huffman();
            BitArray bitArr = huff.Encode(str);
            WriteBits(bitArr);
            string s = huff.Decode(bitArr);
            Console.WriteLine("Сообщение после декодирования: "+s);
            Console.ReadLine();
        }

        public static void WriteBits(BitArray bits)
        {
            foreach (bool b in bits)
                Console.Write(b ? 1 : 0);
            Console.WriteLine("\n");
        }
    }
}
