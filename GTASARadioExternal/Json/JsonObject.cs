using System;
using System.Collections.Generic;

namespace S.Json {
    public class JsonObject {
        Dictionary<string, dynamic> _content = new Dictionary<string, dynamic>();

        public void AddItem(string key, dynamic value) {
            _content.Add(key, value);
        }

        public bool ContainsKey(string key) {
            return _content.ContainsKey(key);
        }

        public dynamic GetValue(string key) {
            return _content[key];
        }

        public bool TryGetValue<T>(string key, Type type, out T value) {
            object tryValue = _content[key];
            if (tryValue.GetType() == type) {
                value = (T)tryValue;
                return true;
            }
            else {
                value = default;
                return false;
            }
        }

        public bool ValidateItemType(string key, Type type) {
            object value = _content[key];
            if (value.GetType() == type) {
                return true;
            }
            else {
                return false;
            }
        }

    }
}
