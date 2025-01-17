﻿using System;
using System.Collections.Generic;

namespace DSDecmp.Utils
{
    /// <summary>
    /// Very simplistic implementation of a priority queue that returns items with lowest priority first.
    /// This is not the most efficient implementation, but required the least work while using the classes
    /// from the .NET collections, and without requiring importing another dll or several more class files
    /// in order to make it work.
    /// </summary>
    /// <typeparam name="TPrio">The type of the priority values.</typeparam>
    /// <typeparam name="TValue">The type of item to put into the queue.</typeparam>
    public class SimpleReversedPrioQueue<TPrio, TValue>
    {
        private SortedDictionary<TPrio, LinkedList<TValue>> items;
        private int itemCount;

        /// <summary>
        /// Gets the number of items in this queue.
        /// </summary>
        public int Count
        {
            get { return itemCount; }
        }

        /// <summary>
        /// Creates a new, empty reverse priority queue.
        /// </summary>
        public SimpleReversedPrioQueue()
        {
            items = new SortedDictionary<TPrio, LinkedList<TValue>>();
            itemCount = 0;
        }

        /// <summary>
        /// Enqueues the given value, using the given priority.
        /// </summary>
        /// <param name="priority">The priority of the value.</param>
        /// <param name="value">The value to enqueue.</param>
        public void Enqueue(TPrio priority, TValue value)
        {
            if (!items.ContainsKey(priority))
                items.Add(priority, new LinkedList<TValue>());
            items[priority].AddLast(value);
            itemCount++;
        }

        /// <summary>
        /// Gets the current value with the lowest priority from this queue, without dequeueing the value.
        /// </summary>
        /// <param name="priority">The priority of the returned value.</param>
        /// <returns>The current value with the lowest priority.</returns>
        /// <exception cref="IndexOutOfRangeException">If there are no items left in this queue.</exception>
        public TValue Peek(out TPrio priority)
        {
            if (itemCount == 0)
                throw new IndexOutOfRangeException();
            foreach (KeyValuePair<TPrio, LinkedList<TValue>> kvp in items)
            {
                priority = kvp.Key;
                return kvp.Value.First.Value;
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Dequeues the current value at the head of thisreverse priority queue.
        /// </summary>
        /// <param name="priority">The priority of the dequeued value.</param>
        /// <returns>The dequeued value, that used to be at the head of this queue.</returns>
        /// <exception cref="IndexOutOfRangeException">If this queue does not contain any items.</exception>
        public TValue Dequeue(out TPrio priority)
        {
            if (itemCount == 0)
                throw new IndexOutOfRangeException();
            LinkedList<TValue> lowestLL = null!; // Count > 0: always set once
            priority = default!;
            foreach (KeyValuePair<TPrio, LinkedList<TValue>> kvp in items)
            {
                lowestLL = kvp.Value;
                priority = kvp.Key;
                break;
            }

            TValue returnValue = lowestLL.First.Value;
            lowestLL.RemoveFirst();
            // remove unused linked lists. priorities will only grow.
            if (lowestLL.Count == 0)
            {
                items.Remove(priority);
            }

            itemCount--;
            return returnValue;
        }
    }
}
