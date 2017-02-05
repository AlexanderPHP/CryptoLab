using System;
using System.Collections.Generic;

namespace Modern.Set
{
    internal class Set<T>
    {
        private readonly HashSet<T> _set;

        public Set()
        {
            _set = new HashSet<T>();
        }

        public Set(HashSet<T> set)
        {
            _set = set;
        }

        public void Clear()
        {
            _set.Clear();
        }

        public void Add(T item)
        {
            _set.Add(item);
        }

        public void Delete(T item)
        {
            _set.Remove(item);
        }

        public bool IsEmpty()
        {
            return _set.Count == 0;
        }

        public bool Contains(T item)
        {
            return _set.Contains(item);
        }

        public Set<T> Union(Set<T> other)
        {
            var result = new HashSet<T>();
            result.UnionWith(other._set);
            result.UnionWith(_set);
            return new Set<T>(result);
        }

        public Set<T> Intersect(Set<T> other)
        {
            var result = new HashSet<T>();
            result.IntersectWith(other._set);
            result.IntersectWith(_set);
            return new Set<T>(result);
        }

        public Set<T> Except(Set<T> other)
        {
            var result = _set;
            result.ExceptWith(other._set);
            return new Set<T>(result);
        }

        public T At(int num)
        {
            var i = 0;
            var t = default(T);
            if(num > _set.Count)
                throw new ArgumentOutOfRangeException(nameof(num));
            foreach (var item in _set)
            {
                if (i++ == num)
                {
                    t = item;
                    break;
                }
            }
            return t;
        }

        public int Size()
        {
            return _set.Count;
        }

    }
}
