using System;
using System.Collections.Generic;

namespace Cannons.Slots
{
    public class CircularQueue<T>
    {
        private T[] items;
        private int _frontIndex;
        private int _nearIndex;
        private int _count;
        private int _capacity;
        
        public CircularQueue(int capacity)
        {
            items = new T[capacity];
            _frontIndex = 0;
            _nearIndex = 0;
            _count = 0;
            _capacity = capacity;
        }

        public T this[int idx] => items[(idx + _frontIndex)% _capacity];
        public int Count => _count;
        public bool IsFull => _count == _capacity;
        public bool IsEmpty => _count == 0;

        public void Enqueue(T item)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Queue is full");
            }

            items[_nearIndex] = item;
            
            _nearIndex = (_nearIndex + 1) % _capacity;
            _count++;
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            
            var item = items[_frontIndex];
            
            _frontIndex = (_frontIndex+1) % _capacity;
            _count--;
            
            return item;
        }

        public void RemoveIdx(int idx)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            if (idx < 0 || idx >= _capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(idx), "Index is outside the bounds of the queue.");
            }
            
            int moveCount = (_count - (idx + 1));
            for (int i = 0; i < moveCount; i++)
            {
                int nextIdx = idx + i;
                int currentQueueIdx = (nextIdx + _frontIndex) % _capacity;
                int nextQueueIdx = (currentQueueIdx + 1) % _capacity;
                
                items[currentQueueIdx] = items[nextQueueIdx];
            }
            _nearIndex = (_nearIndex - 1 + _capacity) % _capacity;
            items[_nearIndex] = default;
            
            _count--;
        }
        
        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            
            return items[_frontIndex];
        }
    }
}