using System;
using System.Collections;
using System.Collections.Generic;

namespace _3DPathSearch
{
    public class PriorityQueue<T> : IEnumerable
    {
        List<T> items;
        List<float> priorities;

        public PriorityQueue()
        {
            items = new List<T>();
            priorities = new List<float>();
        }

        //-----------------------------------------------------------------------------------------------------------------

        public IEnumerator GetEnumerator() { return items.GetEnumerator(); }
        public int Count { get { return items.Count; } }

        //-----------------------------------------------------------------------------------------------------------------

        /// <returns>Index of new element</returns>
        public int Enqueue(T item, float priority)
        {
            for (int i = 0; i < priorities.Count; i++) //go through all elements...
            {
                if (priorities[i] > priority) //...as long as they have a lower priority.    low priority = low index
                {
                    items.Insert(i, item);
                    priorities.Insert(i, priority);
                    return i;
                }
            }

            items.Add(item);
            priorities.Add(priority);
            return items.Count - 1;
        }

        //-----------------------------------------------------------------------------------------------------------------

        public int UpdateItems(T item, float priority)
        {
            bool itemFound = false;
            int itemIndex = 0;

            foreach (var cutIt in items)
            {
                if (cutIt.Equals(item))
                {
                    itemFound = true;
                    break;
                }
                if (itemFound)
                {
                    break;
                }
                itemIndex++;
            }

            if (!itemFound)
            {
                throw new Exception("Method :UpdateItenm(T item, int priority) ; Failed to find object");
            }

            items.RemoveAt(itemIndex);
            priorities.RemoveAt(itemIndex);
            return Enqueue(item, priority);
        }

        //-----------------------------------------------------------------------------------------------------------------

        public T Dequeue()
        {
            T item = items[0];
            priorities.RemoveAt(0);
            items.RemoveAt(0);
            return item;
        }

        //-----------------------------------------------------------------------------------------------------------------

        public T Peek()
        {
            return items[0];
        }

        //-----------------------------------------------------------------------------------------------------------------

        public float PeekPriority()
        {
            return priorities[0];
        }

        //-----------------------------------------------------------------------------------------------------------------    }
    }
}
