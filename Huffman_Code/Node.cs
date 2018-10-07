using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman_Code
{
    class Node<Tkey, Tvalue> where Tkey : IComparable<Tkey>
    {
        public Node(Tkey key, Tvalue value)
        {
            this.key = key;
            this.value = value;
        }

        public Tkey key;
        public Tvalue value;
        public Node<Tkey, Tvalue> leftChild;
        public Node<Tkey, Tvalue> rightChild;
        public Node<Tkey, Tvalue> parent;
    }
}
