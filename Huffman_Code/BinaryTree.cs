using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman_Code
{
    class BinaryTree<Tkey, Tvalue> where Tkey : IComparable<Tkey>
    {
        private Node<Tkey, Tvalue> root;

        public BinaryTree(Node<Tkey, Tvalue> root)
        {
            this.root = root;
        }
        public Node<Tkey, Tvalue> GetRoot()
        {
            return root;
        }

// *******************************************************************************************************************************************
            // поиск значения по ключю
        public Tvalue FindValuebyKey(Tkey key)
        {
            return FindValuebyKeyRecurs(key, root);       // начинаем искать с корневого элемента
        }
            // рекурсивная часть метода поиска значения по ключю
        private Tvalue FindValuebyKeyRecurs(Tkey key, Node<Tkey, Tvalue> node)
        {
            switch (node.key.CompareTo(key))
            {
                case 0:                 // node.key=key - нашли
                    return node.value;
                case 1:                 // node.key>key
                    if (node.leftChild != null)
                        return FindValuebyKeyRecurs(key, node.leftChild);       // ищем в левом поддереве, если оно существует
                break;
                case -1:
                    if (node.rightChild != null)
                        return FindValuebyKeyRecurs(key, node.rightChild);       // ищем в правом поддереве, если оно существует=
                break;

            }
            return default(Tvalue);
        }

//**********************************************************************************************************************************************
            // поиск ноды по ключю
        public Node<Tkey, Tvalue> FindNodebyKey(Tkey key)
        {
            return FindNodebyKeyRecurs(key, root);       // начинаем искать с корневого элемента
        }
            // рекурсивная часть метода поиска ноды по ключю
        private Node<Tkey, Tvalue> FindNodebyKeyRecurs(Tkey key, Node<Tkey, Tvalue> node)
        {
            switch (node.key.CompareTo(key))
            {
                case 0:                 // node.key=key - нашли
                    return node;
                case 1:                 // node.key>key
                    if (node.leftChild != null)
                        return FindNodebyKeyRecurs(key, node.leftChild);       // ищем в левом поддереве, если оно существует
                    break;
                case -1:
                    if (node.rightChild != null)
                        return FindNodebyKeyRecurs(key, node.rightChild);       // ищем в правом поддереве, если оно существует
                    break;
            }
            return null;
        }


//**********************************************************************************************************************************************
            // вставка ноды
        public bool Insert(Node<Tkey, Tvalue> newNode)
        {
            return InsertRecurs(newNode, root);    // начинаем с корня
        }
            // рекурсивная часть метода вставки
        private bool InsertRecurs(Node<Tkey, Tvalue> insertedNode, Node<Tkey, Tvalue> node)
        {
            switch (insertedNode.key.CompareTo(node.key))
            {
                case 0:                 // нода с таким ключем уже существует
                    Console.WriteLine("Node with this key already exist");
                    return false;
                    
                case 1:                 // если новая нода больше - идём направо
                    if (node.rightChild == null)
                    {
                        node.rightChild = insertedNode;
                        insertedNode.parent = node;
                        return true;
                    }
                    else
                        InsertRecurs(insertedNode, node.rightChild);
                    break;
                case -1:                 // если новая нода меньше - идём налево
                    if (node.leftChild == null)
                    {
                        node.leftChild = insertedNode;
                        insertedNode.parent = node;
                        return true;
                    }
                    else
                        InsertRecurs(insertedNode, node.leftChild);
                    break;
            }
            return false;
        }
//***********************************************************************************************************************************
            //удаление ноды
        public void Delete(Node<Tkey, Tvalue> node)
        {       // если детей не имеется, то просто удаляем ноду
            if (node.leftChild == null && node.rightChild == null)
            {
                node = null;
                return;
            }
                // если имеется 1 ребёнок левый или правый, то ставим его вместо ноды и удаляем ноду
            if (node.leftChild == null || node.rightChild == null)
            {
                Node<Tkey, Tvalue> child;       // узнаём какая нода ребёнок, левая или правая:
                if (node.leftChild!=null)
                    child = node.leftChild;
                else
                    child = node.rightChild;
                                                // узнаём какой мы ребёнок, левый или правый, и вставляем в это место вместо себя нашего единственного ребёнка:
                if (node.parent.leftChild == node)
                    node.parent.leftChild = child;
                else
                    node.parent.rightChild = child;

                node = null;
                return;
            }
                // теперь, если имеется оба ребёнка. 
                // Ищем наименьшую ноду в правом поддереве. Она 100% не будет иметь левого ребёнка, потому что левый - минимальный
            Node<Tkey, Tvalue> minNodeOnRightSubTree = FindMinNodeOnSubTree(node.rightChild);
                // правого ребёнка найденого минимального элемента подставляем вместо него
            Node<Tkey, Tvalue> rChild = minNodeOnRightSubTree.rightChild;
            if (rChild != null)
            {
                minNodeOnRightSubTree.parent.leftChild = rChild;
                rChild.parent = minNodeOnRightSubTree.parent;
            }
                // а сам минимальный элемент подставляем вместо удаляемого
            node.leftChild.parent = minNodeOnRightSubTree;
            node.rightChild.parent = minNodeOnRightSubTree;
            minNodeOnRightSubTree.leftChild = node.leftChild;
            minNodeOnRightSubTree.rightChild = node.rightChild;
            minNodeOnRightSubTree.parent = null;
            if (root == node)   // проверяем, не был ли удаляемый элемент корнем
                root = minNodeOnRightSubTree;
            node = null;
        }
            //*********************************************************************************************
            // Метод, находит минимальную ноду (самую левую) в дереве с корнем subTree
        private Node<Tkey, Tvalue> FindMinNodeOnSubTree(Node<Tkey, Tvalue> subTree)
        {
            if (subTree.leftChild == null)
                return subTree;
            else
                return FindMinNodeOnSubTree(subTree.leftChild);
        }
//****************************************************************************************************************************************************
    }
}
