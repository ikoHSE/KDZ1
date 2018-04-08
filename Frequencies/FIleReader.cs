using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frequencies {
    public class FileReader {
        private Dictionary<String, int> recorded = new Dictionary<String, int>();

        public KeyValuePair<String, int>[] result {
            get => recorded.OrderBy((arg) => arg.Value).Where(x => x.Value > 1).Reverse().ToArray();
        }

        public void Read(string file, Encoding encoding, int pollInterval = 30) {
            recorded.Clear();
            if (File.Exists(file)) {
                var reader = new StreamReader(file, encoding);
                char c;
                List<char> soFar = new List<char>();
                var i = 0;
                while ((Int16) (c = (char) reader.Read()) != -1) {
                    //Console.WriteLine(c);
                    if (Char.IsLetter(c) || c == '-' || c == '\'') {
                        soFar.Add(Char.ToLower(c));
                    } else {
                        if (soFar.Count > 0) {
                            i += 1;
                            var st = new String(soFar.ToArray());
                            if (!recorded.Keys.Contains(st)) {
                                recorded[st] = 0;
                            }

                            recorded[st] += 1;

                            soFar.Clear();

                            if (i >= pollInterval) {
                                i = 0;
                                poll?.Invoke();
                            }

                        }
                    }
                }
                if (soFar.Count > 0) {
                    i += 1;
                    var st = new String(soFar.ToArray());
                    if (!recorded.Keys.Contains(st)) {
                        recorded[st] = 0;
                    }

                    recorded[st] += 1;
                }
                finished?.Invoke();
            }
        }
        public void Read(string file, Encoding encoding, Action<KeyValuePair<String, int>[]> completion) {
            finished += () => {
                completion(this.result);
            };
            this.Read(file, encoding);
        }
        public event Action poll;
        public event Action finished;
    }
}
