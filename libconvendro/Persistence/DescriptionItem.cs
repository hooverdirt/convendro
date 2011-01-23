using System;
using System.Collections.Generic;
using System.Text;

namespace libconvendro.Persistence {
    /// <summary>
    /// Single serializable name description item.
    /// </summary>
    public class DescriptionItem {
        private string name;
        private string description;

        /// <summary>
        /// 
        /// </summary>
        public DescriptionItem() {
            name = null;
            description = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aname"></param>
        /// <param name="adescription"></param>
        public DescriptionItem(string aname, string adescription)
            : this() {
            name = aname;
            description = adescription;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return this.name;
        }
    }
}
