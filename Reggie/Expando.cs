using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
namespace Reggie {
    class Expando : DynamicObject, IDictionary<string, object> {
        Dictionary<string, object> _inner = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        public override IEnumerable<string> GetDynamicMemberNames() {
            return _inner.Keys;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (0 == string.Compare(binder.Name, "_indent", StringComparison.InvariantCultureIgnoreCase)) {
                object o;
                if (_inner.TryGetValue("$Response", out o) && o is IndentedTextWriter) {
                    result = ((IndentedTextWriter)o).IndentLevel;
                    return true;
                }
            }
            return _inner.TryGetValue(binder.Name, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value) {
            if (0 == string.Compare(binder.Name, "_indent", StringComparison.InvariantCultureIgnoreCase)) {
                object o;
                if (_inner.TryGetValue("$Response", out o) && o is IndentedTextWriter) {
                    ((IndentedTextWriter)o).IndentLevel = (int)value;
                    return true;
                }
            }
            _inner[binder.Name] = value;
            return true;
        }
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
           object o;
            var s = binder.Name;
            
            if(_inner.TryGetValue("$Response", out o)) {
                result = null;
                if (o is Stream) {
                    
                    return TemplateCore.Generate(s, this, new StreamWriter((Stream)o),false, args);
                } 
                return TemplateCore.Generate(s, this, (TextWriter)o, false, args);
            } 
            return base.TryInvokeMember(binder, args, out result);
        }
        public object this[string key] { get => _inner[key]; set => _inner[key] = value; }

        public ICollection<string> Keys => _inner.Keys;

        public ICollection<object> Values => _inner.Values;

        public int Count => _inner.Count;

        bool ICollection<KeyValuePair<string,object>>.IsReadOnly => false;

        public void Add(string key, object value) {
            _inner.Add(key, value);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) {
            ((IDictionary<string, object>)_inner).Add(item);
        }

        public void Clear() {
            _inner.Clear();
        }

        bool ICollection<KeyValuePair<string, object> >.Contains(KeyValuePair<string, object> item) {
            return ((ICollection< KeyValuePair<string, object> >)_inner).Contains(item);
        }

        public bool ContainsKey(string key) {
            return _inner.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
            ((IDictionary<string,object>)_inner).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            return _inner.GetEnumerator();
        }

        public bool Remove(string key) {
            return _inner.Remove(key);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) {
            return ((IDictionary<string, object>)_inner).Remove(item);
        }

        public bool TryGetValue(string key, out object value) {
            return _inner.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)_inner).GetEnumerator();
        }
    }
}
