using System;
using System.Collections.Generic;
using System.Text;

namespace libconvendro {
    /// <summary>
    /// CommandLine helper class
    /// </summary>
    public static class CommandLineSession {
        public const string ARGUMENT_LONGPREFIX = "--";
        public const string ARGUMENT_SHORTPREFIX = "-";
        public const string ARGUMENT_SEPARATOR = "=";

        public static string[] Arguments;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="avalue"></param>
        /// <returns></returns>
        public static bool GetArgumentValue(string argument, ref string avalue) {
            bool b = false;

            int i = Array.IndexOf(Arguments, argument.ToLower());
            if (i > -1) {
                b = true;
                string[] s = Arguments[i].Split('=');

                if (s.Length > 1) {
                    avalue = s[1];
                } else {
                    avalue = null;
                }
            }

            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static int ArgumentIndex(string argument) {
            return Array.IndexOf(Arguments, argument.ToLower());
        }
    }
}
