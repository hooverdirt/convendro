using System;
using System.Collections.Generic;
using System.Text;

namespace libconvendro.Persistence {
    /// <summary>
    /// A single CommandOption.
    /// </summary>
    public class CommandOption {
        public string ArgumentSeparator { get; set; }
        public string ValueSeparator { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CommandOption() {
            // set default separators.
            this.ArgumentSeparator = "-";
            this.ValueSeparator = " ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public CommandOption(string name, string value)
            : this() {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string buildArgument() {
            string res = null;

            if ((this.Name != null) && (this.Value != null)) {

                string svalue = this.Value;

                if (this.Value.Contains(" ")) {
                    svalue = "\"" + this.Value + "\"";
                }

                res = String.Format("{0}{1}{2}{3}", this.ArgumentSeparator,
                    this.Name, (String.IsNullOrEmpty(this.ValueSeparator) ? " " : this.ValueSeparator),
                    svalue);
            }

            return res;
        }

        /// <summary>
        /// Gets the complete commandline argument, based on both separators.
        /// </summary>
        public string Argument {
            get { return this.buildArgument(); }
        }
    }
}
