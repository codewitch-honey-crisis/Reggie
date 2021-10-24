using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace F
{
	sealed class KeySet<T> : ISet<T>, IEquatable<KeySet<T>>
	{
		HashSet<T> _inner ;
		int _hashCode ;
		public KeySet(IEqualityComparer<T> comparer)
		{
			_inner = new HashSet<T>(comparer);
			_hashCode = 0;
		}
		public KeySet()
		{
			_inner = new HashSet<T>();
			_hashCode = 0;
		}
		public int Count => _inner.Count;

		public bool IsReadOnly => true;

		// hack - we allow this method so the set can be filled
		public bool Add(T item)
		{
			if (null != item)
				_hashCode ^= item.GetHashCode();
			return _inner.Add(item);
		}
		bool ISet<T>.Add(T item)
		{
			_ThrowReadOnly();
			return false;
		}
		public void Clear()
		{
			_ThrowReadOnly();
		}

		public bool Contains(T item)
		{
			return _inner.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_inner.CopyTo(array, arrayIndex);
		}

		void ISet<T>.ExceptWith(IEnumerable<T> other)
		{
			_ThrowReadOnly();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _inner.GetEnumerator();
		}

		void ISet<T>.IntersectWith(IEnumerable<T> other)
		{
			_ThrowReadOnly();
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return _inner.IsProperSubsetOf(other);
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return _inner.IsProperSupersetOf(other);
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return _inner.IsSubsetOf(other);
		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return _inner.IsSupersetOf(other);
		}

		public bool Overlaps(IEnumerable<T> other)
		{
			return _inner.Overlaps(other);
		}

		bool ICollection<T>.Remove(T item)
		{
			_ThrowReadOnly();
			return false;
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			return _inner.SetEquals(other);
		}

		void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
		{
			_ThrowReadOnly();
		}

		void ISet<T>.UnionWith(IEnumerable<T> other)
		{
			_ThrowReadOnly();
		}

		void ICollection<T>.Add(T item)
		{
			_ThrowReadOnly();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _inner.GetEnumerator();
		}
		static void _ThrowReadOnly()
		{
			throw new NotSupportedException("The set is read only");
		}
		public bool Equals(KeySet<T> rhs)
		{
			if (ReferenceEquals(this, rhs))
				return true;
			if (ReferenceEquals(rhs, null))
				return false;
			if (rhs._hashCode != _hashCode)
				return false;
			var ic = _inner.Count;
			if (ic != rhs._inner.Count)
				return false;
			return _inner.SetEquals(rhs._inner);
		}
		public override int GetHashCode()
		{
			return _hashCode;
		}
	}
}
