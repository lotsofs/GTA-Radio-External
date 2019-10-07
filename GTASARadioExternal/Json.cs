using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S.Json;

namespace GTASARadioExternal {
    class Json {

        const string EXPECTED_CHAR_ERROR = "Expected {0}, received {1} at position {2}";
        const string NOT_IMPLEMENTED_ERROR = "{0} not implemented yet";

        enum ExpectedChars {
            OpeningBracket,
            Key,
            Colon,
            Value,
            Next,
            End,
        }

        /// <summary>
        /// Parse a json file
        /// </summary>
        /// <param name="file"></param>
        public JsonObject Parse(string file) {
            JsonObject mainJsonObject = new JsonObject("main");
            Stack<JsonObject> jsonObjects = new Stack<JsonObject>();

            string keyName = string.Empty;
            JsonObject workingJsonObject;

            ExpectedChars expectedChar = ExpectedChars.OpeningBracket;

            for (int i = 0; i < file.Length; i++) {
                char ch = file[i];

                // ignore all whitespaces
                if (char.IsWhiteSpace(ch)) {
                    continue;
                }

                // there should be a { at the beginning
                if (expectedChar == ExpectedChars.OpeningBracket) {
                    if (ch == '{') {
                        expectedChar = ExpectedChars.Key;
                        jsonObjects.Push(mainJsonObject);
                    }
                    else {
                        throw new Exception(string.Format(EXPECTED_CHAR_ERROR, '{', ch, i));
                    }
                }

                // expecting a key
                if (expectedChar == ExpectedChars.Key) {
                    if (ch == '"') {
                        keyName = GetString(file, out int length, i);
                        i += length;
                        expectedChar = ExpectedChars.Colon;
                    }
                    else {
                        throw new Exception(string.Format(EXPECTED_CHAR_ERROR, '"', ch, i));
                    }
                }

                // expecting a separator
                if (expectedChar == ExpectedChars.Colon) {
                    if (ch == ':') {
                        expectedChar = ExpectedChars.Value;
                    }
                    else {
                        throw new Exception(string.Format(EXPECTED_CHAR_ERROR, ':', ch, i));
                    }
                }

                // expecting a value
                if (expectedChar == ExpectedChars.Value) {
                    switch (ch) {
                        case '"':
                            // string
                            string value = GetString(file, out int length, i);
                            i += length;
                            workingJsonObject = jsonObjects.Peek();
                            workingJsonObject.AddItem(keyName, value);
                            expectedChar = ExpectedChars.Next;
                            break;
                        case '{':
                            // another object
                            workingJsonObject = jsonObjects.Peek();
                            workingJsonObject.AddItem(keyName, new JsonObject(keyName));
                            expectedChar = ExpectedChars.Key;
                            break;
                        case '[':
                            // array
                            throw new Exception(string.Format(NOT_IMPLEMENTED_ERROR, "arrays"));
                        default:
                            // check if the value is a number
                            if (char.IsDigit(ch)) {
                                throw new Exception(string.Format(NOT_IMPLEMENTED_ERROR, "numbers"));
                            }
                            // didnt get anything that makes sense
                            throw new Exception(string.Format(EXPECTED_CHAR_ERROR, "{, [ or \"", ch, i));
                    }
                }

                // expecting indication that a new item is coming up
                if (expectedChar == ExpectedChars.Next) {
                    switch (ch) {
                        case '}':
                            jsonObjects.Pop();
                            if (jsonObjects.Count == 0) {
                                expectedChar = ExpectedChars.End;
                            }
                            break;
                        case ',':
                            expectedChar = ExpectedChars.Key;
                            break;
                        default:
                            break;
                    }
                }

                // expecting the end
                if (expectedChar == ExpectedChars.End) {
                    if (!char.IsWhiteSpace(ch)) {
                        throw new Exception(string.Format(EXPECTED_CHAR_ERROR, "end of document", ch, i));
                    }
                }
            }
            
            return mainJsonObject;
        }

        string GetString(string file, out int length, int startingIndex) {
            startingIndex++;
            for (int i = startingIndex; i < file.Length; i++) {
                char ch = file[i];
                switch (ch) {
                    case '\\':
                        // TODO: Actually parse what is going on here instead of just ignoring
                        i++;
                        break;
                    case '"':
                        length = i - startingIndex;
                        string word = file.Substring(startingIndex, length);
                        return word;
                    default:
                        break;
                }
            }
            // failed to find something
            throw new Exception(string.Format(EXPECTED_CHAR_ERROR, '"', "end of document", file.Length));
        }
    }
}

namespace S.Json { 
    class JsonObject {
        string _name;
        Dictionary<string, dynamic> _content = new Dictionary<string, dynamic>();

        public JsonObject(string name) {
            _name = name;
        }

        public void AddItem(string name, dynamic value) {
            _content.Add(name, value);
        }

        public dynamic GetItem(string key) {
            return _content[key];
        }

    }
}
