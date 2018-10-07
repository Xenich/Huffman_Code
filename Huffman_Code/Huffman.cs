using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Huffman_Code
{
    class Huffman
    {
        private Dictionary<char, BitArray> dicChar;                         // словарь будет содержать двоичный код для каждого символа
        private Dictionary<Node<char, int>, BitArray> dicNode;              // вспомогательный словарь будет содержать двоичный путь до каждого узла дерева
        BinaryTree<char, int> huffmanTree;

//*************************************************************************************************************************
                // кодирование сообщений
        public BitArray Encode(string str)
        {
            MakeTable(str);
            BitArray bitArr = new BitArray(0);
            int startbBit = 0;
            foreach (char ch in str)
            {
                bitArr.Length += dicChar[ch].Length;
                for (int i = 0; i < dicChar[ch].Length; i++)
                {
                    bitArr[startbBit + i] = dicChar[ch][i];
                }
                startbBit += dicChar[ch].Length;
            }
            return bitArr;
        }

//************************************************************************************************************************
                // декодирование сообщений
        public string Decode(BitArray bitArr)
        {
            string str = "";
            Node<char, int> currentNode = huffmanTree.GetRoot();
            int i = 0;
            while (i< bitArr.Length)
            {
                if (bitArr[i])
                    currentNode = currentNode.rightChild;
                else
                    currentNode = currentNode.leftChild;    
                if (currentNode.leftChild == null && currentNode.rightChild == null)
                {
                    str = str + currentNode.key;
                    currentNode = huffmanTree.GetRoot();
                }
                i++;
            }
            return str;
        }
//***************************************************************************************************************************
                // построение кодовой таблицы
        private Dictionary<char, BitArray> MakeTable(string str)
        {
            dicNode = new Dictionary<Node<char, int>, BitArray>();      // вспомогательный словарь будет содержать двоичный путь до каждого узла дерева
            dicChar = new Dictionary<char, BitArray>();                 // словарь будет содержать двоичный код для каждого символа
            MakeHuffmanTree(str);
            dicNode.Add(huffmanTree.GetRoot(), new BitArray(0));        // заносим в словарь корневой узел с пустым битовым массивом
            Traversing(huffmanTree.GetRoot());                          // обходим дерево начиная с корня и заполняем словарь битами
            foreach (Node<char, int> node in dicNode.Keys)              // заполнение словаря dicChar
            {
                if (node.key!=default(char))
                {
                    dicChar.Add(node.key, dicNode[node]);
                    Console.Write(node.key + " ");
                    Program.WriteBits(dicNode[node]);
                }
            }
            return dicChar;
        }
                // метод обхода дерева
        private void Traversing(Node<char, int> node)
        {
            if (node.leftChild != null)
            {
                MakeBits(node, node.leftChild,false);
            }
            if (node.rightChild != null)
            {
                MakeBits(node, node.rightChild, true);
            }
        }
                // заполнение битами словаря dicNode
        private void MakeBits(Node<char, int> node, Node<char, int> child, bool isRightChild)
        {
            BitArray bitArr = new BitArray(dicNode[node].Length + 1);
            bitArr[dicNode[node].Length] = isRightChild;
            for (int i = 0; i < dicNode[node].Length; i++)
            {
                bitArr[i] = dicNode[node][i];
            }
            dicNode.Add(child, bitArr);
            Traversing(child);
        }
//***************************************************************************************************************

                // построение дерева Хаффмана
        private void MakeHuffmanTree(string str)
        {               
            Dictionary<char, int> dict = new Dictionary<char, int>();       // упорядоченный словарь с частотой вхождения символов 
            PriorityQueue<int, Node<char, int>> priorityQueue = new PriorityQueue<int, Node<char, int>>();
                    // строим словарь
            foreach (char key in str)
            {
                if (dict.ContainsKey(key))
                    dict[key]++;
                else
                    dict.Add(key, 0);
            }
            dict = dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                //  формируем приоритетную очередь для каждого символа. приоритет - частота появления символа.  наименьшая частота обладает наибольшим приоритетом
            foreach (char ch in dict.Keys)
            {
                Node<char, int> node = new Node<char, int>(ch, dict[ch]);   // нода символ-приоритет
                priorityQueue.Enqueue(dict[ch], node);      // приоритетная очередь
            }
            while (priorityQueue.Count>1)
            {
                Node<char, int> node1 = priorityQueue.Dequeue();
                Node<char, int> node2 = priorityQueue.Dequeue();
                int newPriority = node1.value + node2.value;
                Node<char, int> newNode = new Node<char, int>(char.MinValue, newPriority);
                newNode.leftChild = node1;
                newNode.rightChild = node2;
                node1.parent = newNode;
                node2.parent = newNode;
                priorityQueue.Enqueue(newPriority, newNode);
            }
            huffmanTree = new BinaryTree<char, int>(priorityQueue.Dequeue());
        }
    }
}