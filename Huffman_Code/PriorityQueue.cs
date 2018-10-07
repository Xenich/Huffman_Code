using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman_Code
{
        // очередь с приоритетом
    class PriorityQueue<Tpriority, Titem> where Tpriority : IComparable<Tpriority>
    {       // очередь будет в виде сортированного словаря. Элементы с одинаковым приоритетом будут храниться в обычной очереди 
        private SortedDictionary<Tpriority, Queue<Titem>> subQueues;
        private int count;
        public PriorityQueue()
        {
            subQueues = new SortedDictionary<Tpriority, Queue<Titem>>();
        }
    
            // вставка нового item с приоритетом priority
        public void Enqueue(Tpriority priority, Titem item)
        {                // если элементы с таким приоритетом уже существуют, то просто добавляем в соответствующую очередь наш элемент
            if (subQueues.ContainsKey(priority))
                subQueues[priority].Enqueue(item);
            else          // в противном случае  создаём новую подочередь и в неё пихаем элемент с приоритетом
            {
                subQueues.Add(priority, new Queue<Titem>());
                subQueues[priority].Enqueue(item);
            }
            count++;
        }
            // удаление первого элемента из очереди 
        public Titem Dequeue()
        {
            if (subQueues.Count != 0)
            {
                Titem itemToReturn;
                itemToReturn = subQueues.First().Value.Dequeue();
                if (subQueues.First().Value.Count == 0)        // если у первой подочереди (той, с которой мы только что вытащили элемент) не осталось элементов - удаляем её из словаря
                    subQueues.Remove(subQueues.First().Key);
                count--;
                return itemToReturn;
            }
            else
                return default(Titem);
        }

        public bool HasItems()
        {
            return subQueues.Any();
        }

        public int Count
        {
            get
            {
                return count;
            }
        }
    }
}
