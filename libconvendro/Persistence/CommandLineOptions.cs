using System;
using System.Collections.Generic;
using System.Text;

namespace libconvendro.Persistence {
    /// <summary>
    /// A list of commandline options.
    /// </summary>
    public class CommandLineOptions {
        private List<CommandOption> list;

        public CommandLineOptions() {
            list = new List<CommandOption>();
        }

        public int Count {
            get { return list.Count; }
        }

        public List<CommandOption> Items {
            get { return list; }
            set { list = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anoption"></param>
        /// <returns></returns>
        public int FindIndex(CommandOption anoption) {
            int res = -1;

            for (int i = 0; i < list.Count; i++) {
                if (list[i].Name == anoption.Name) {
                    res = i;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aname"></param>
        /// <returns></returns>
        public int FindIndex(string aname) {
            int res = -1;

            for (int i = 0; i < list.Count; i++) {
                if (list[i].Name == aname) {
                    res = i;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// Adds a CommandOption.
        /// </summary>
        /// <param name="anoption"></param>
        /// <returns></returns>
        public int Add(CommandOption anoption) {
            int res = -1;
            try {
                // see if this is already in there...
                res = FindIndex(anoption);
                if (res == -1) {
                    list.Add(anoption);
                }
            } catch {
                res = -1;
            }

            return res;
        }

        /// <summary>
        /// Adds a single commandline switch.
        /// </summary>
        /// <param name="aname">Name of parameter</param>
        /// <param name="avalue">Value of parameter</param>
        /// <returns></returns>
        public int Add(string aname, string avalue) {
            int res = -1;

            try {
                res = FindIndex(aname);
                if (res == -1) {
                    list.Add(new CommandOption(aname, avalue));
                }
            } catch {
                res = -1;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() {
            this.list.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anidex"></param>
        public void Delete(int anidex) {
            this.list.RemoveAt(anidex);
        }

        public string BuildCommandLine() {
            string res = null;

            foreach (CommandOption nline in this.list) {
                res += nline.Argument + " ";
            }

            return res.Trim();
        }
    }
}
