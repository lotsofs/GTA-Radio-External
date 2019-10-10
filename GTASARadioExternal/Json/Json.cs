using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace S.Json {
    static class Json {

        private const string EXPECTED_CHAR_ERROR = "Expected {0}, received {1} at position {2}";
        private const string NOT_IMPLEMENTED_ERROR = "{0} not implemented yet";
        private const string UNEXPECTED_ERROR = "An unexpected error occured";      // todo: replace these withalready provided ones

        private enum ExpectedChars {
            OpeningBracket,
            Key,
            Colon,
            Value,
            Next,
            End,
        }

        /// <summary>
        /// Opens a file and parses it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static JsonObject OpenFile(string path) {
            Debug.WriteLine(path);
            if (File.Exists(path)) {
                string jsonText = File.ReadAllText(path);
                JsonObject jsonObject = Parse(jsonText);
                return jsonObject;
            }
            else {
                return null;
                // todo: throw a warning or something
            }
        }

        /// <summary>
        /// Parse a json string
        /// </summary>
        /// <param name="file"></param>
        public static JsonObject Parse(string file) {
            JsonObject mainJsonObject = new JsonObject();
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

                switch (expectedChar) {
                    case ExpectedChars.OpeningBracket:
                        if (ch == '{') {
                            expectedChar = ExpectedChars.Key;
                            jsonObjects.Push(mainJsonObject);
                        }
                        else {
                            throw new Exception(string.Format(EXPECTED_CHAR_ERROR, '{', ch, i));
                        }
                        break;
                    case ExpectedChars.Key:
                        if (ch == '"') {
                            keyName = GetString(file, out int length, i);
                            i += length;    // weve processed a string, we do not need to look at its characters further
                            i++;            // also do not look at the closing quotes
                            expectedChar = ExpectedChars.Colon;
                        }
                        else {
                            throw new Exception(string.Format(EXPECTED_CHAR_ERROR, '"', ch, i));
                        }
                        break;
                    case ExpectedChars.Colon:
                        if (ch == ':') {
                            expectedChar = ExpectedChars.Value;
                        }
                        else {
                            throw new Exception(string.Format(EXPECTED_CHAR_ERROR, ':', ch, i));
                        }
                        break;
                    case ExpectedChars.Value:
                        if (char.IsDigit(ch) || ch == '-') {
                            // number
                            decimal value = GetNumber(file, out int length, i);
                            i += length - 1;    // number length - first character
                            workingJsonObject = jsonObjects.Peek();
                            workingJsonObject.AddItem(keyName, value);
                            expectedChar = ExpectedChars.Next;
                            break;
                        }
                        switch (ch) {
                            case '"':
                                // string
                                string value = GetString(file, out int length, i);
                                i += length + 1; // word length + two quotation marks - first character
                                workingJsonObject = jsonObjects.Peek();
                                workingJsonObject.AddItem(keyName, value);
                                expectedChar = ExpectedChars.Next;
                                break;
                            case '{':
                                // another object  
                                workingJsonObject = jsonObjects.Peek();             // get parent
                                JsonObject subJsonObject = new JsonObject();        // create child
                                workingJsonObject.AddItem(keyName, subJsonObject);  // add child to parent
                                jsonObjects.Push(subJsonObject);                    // add child to stack
                                expectedChar = ExpectedChars.Key;
                                break;
                            case '[':
                                // array
                                throw new Exception(string.Format(NOT_IMPLEMENTED_ERROR, "arrays"));
                            default:
                                // todo: check if the value is a number
                                if (char.IsDigit(ch)) {
                                    throw new Exception(string.Format(NOT_IMPLEMENTED_ERROR, "numbers"));
                                }
                                // didnt get anything that makes sense
                                throw new Exception(string.Format(EXPECTED_CHAR_ERROR, "{, [ or \"", ch, i));
                        }
                        break;      // todo: fix this terrible nesting
                    case ExpectedChars.Next:
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
                        break;
                    case ExpectedChars.End:
                        if (!char.IsWhiteSpace(ch)) {
                            throw new Exception(string.Format(EXPECTED_CHAR_ERROR, "end of document", ch, i));
                        }
                        break;
                    default:
                        throw new Exception(UNEXPECTED_ERROR);
                }
            }

            if (jsonObjects.Count == 0) {
                return mainJsonObject;
            }
            else {
                throw new Exception(string.Format(EXPECTED_CHAR_ERROR, "}", "end of document", file.Length));
            }
        }

        private static decimal GetNumber(string file, out int length, int startingIndex) {
            for (int i = startingIndex; i < file.Length; i++) {
                char ch = file[i];
                if (char.IsDigit(ch) || ch == '.' || ch == '-' || ch == 'e' || ch == 'E' || ch == '+') {
                    continue;
                }
                else if (char.IsWhiteSpace(ch) || ch == ',' || ch == '}' || ch == ']') {
                    length = i - startingIndex;
                    string numberStr = file.Substring(startingIndex, length);
                    decimal numberDec = decimal.Parse(numberStr);
                    return numberDec;
                }
                else {
                    throw new Exception(string.Format(EXPECTED_CHAR_ERROR, "a digit", ch, i));
                }
            }
            throw new Exception(string.Format(EXPECTED_CHAR_ERROR, ",", "end of document", file.Length));
        }

        /// <summary>
        /// Get a string from a keyname or string value
        /// </summary>
        /// <param name="file"></param>
        /// <param name="length"></param>
        /// <param name="startingIndex"></param>
        /// <returns></returns>
        private static string GetString(string file, out int length, int startingIndex) {
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