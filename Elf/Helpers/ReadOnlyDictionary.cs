using System;
using System.Collections;
using System.Collections.Generic;

namespace Elf.Helpers
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> _inner;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> innerDictionary)
        {
            _inner = innerDictionary;
        }

        public bool ContainsKey(TKey key)
        {
            return _inner.ContainsKey(key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException(String.Format(
                "Cannot add item '{0}' to the dictionary: dictionary is read-only", item));
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException(String.Format(
                "Cannot clear the dictionary: dictionary is read-only"));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _inner.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException(String.Format(
                "Cannot remove item '{0}' from the dictionary: dictionary is read-only", item));
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException(String.Format(
                "Cannot add item '{0}' to the dictionary: dictionary is read-only",
                new KeyValuePair<TKey, TValue>(key, value)));
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException(String.Format(
                "Cannot remove item with key '{0}' from the dictionary: dictionary is read-only", key));
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _inner.TryGetValue(key, out value);
        }

        public int Count
        {
            get
            {
                return _inner.Count;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _inner[key];
            }
            set
            {
                throw new NotSupportedException(String.Format(
                    "Cannot add item '{0}' to the dictionary: dictionary is read-only",
                    new KeyValuePair<TKey, TValue>(key, value)));
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _inner.Keys;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _inner.Values;
            }
        }
    }
}