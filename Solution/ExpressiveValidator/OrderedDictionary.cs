using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ExpressiveValidator
{
    public class OrderedDictionary<TKey, TValue> : IEnumerable<(TKey Key, TValue Value)>
    {
        private readonly OrderedDictionary _dictionary = new OrderedDictionary();

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_dictionary.Contains(key))
            {
                value = (TValue) _dictionary[key];
                return true;
            }

            value = default(TValue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<(TKey Key, TValue Value)> GetEnumerator()
        {
            foreach (DictionaryEntry pair in _dictionary)
            {
                yield return ((TKey) pair.Key, (TValue) pair.Value);
            }
        }

        public IEnumerable<TValue> Values()
        {
            return _dictionary.Values.Cast<TValue>();
        }
        
        public TValue this[TKey key]
        {
            get => (TValue)_dictionary[key];
            set => _dictionary[key] = value;
        }
    }
}
