using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using libconvendro.Persistence;

namespace libconvendro.Import {
    
    public class WinFFItem {
        private string tname;
        private string tlabel;
        private string tparamstring;
        private string textension;
        private string tcategory;

        public WinFFItem() {
        }

        public WinFFItem(string aname, string alabel, 
            string aparams, string anextension, string acategory) {
            this.tname = aname;
            this.tlabel = alabel;
            this.tparamstring = aparams;
            this.textension = anextension;
            this.tcategory = acategory;
        }

        [XmlElement(ElementName = "label")]
        public string Label {
            get { return tlabel; }
            set { tlabel = value; }
        }

        [XmlElement(ElementName = "params")]
        public string Params {
            get { return tparamstring; }
            set { tparamstring = value; }
        }

        [XmlElement(ElementName = "extension")]
        public string Extension {
            get { return textension; }
            set { textension = value; }
        }

        [XmlElement(ElementName = "category")]
        public string Category {
            get { return tcategory; }
            set { tcategory = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WinFFFile : BaseImporter {
        public override void LoadFile(string afilename) {
            base.LoadFile(afilename);
            if (File.Exists(afilename)) {
                XmlDocument newdoc = new XmlDocument();
                newdoc.Load(afilename);
                XmlNodeList rootlist = newdoc.DocumentElement.ChildNodes;

                foreach (XmlNode node in rootlist) {
                    Preset newpreset = new Preset();
                    if (node is XmlElement) {
                        if (node.HasChildNodes) {

                            XmlNodeList children = node.ChildNodes;
                            
                            foreach (XmlNode childnode in children) {
                                if (childnode.Name == Functions.WINFF_NODE_LABEL) {
                                    newpreset.Name = childnode.InnerText;
                                }

                                if (childnode.Name == Functions.WINFF_NODE_CATEGORY) {
                                    newpreset.Category = childnode.InnerText;
                                }

                                if (childnode.Name == Functions.WINFF_NODE_EXTENSION) {
                                    newpreset.Extension = childnode.InnerText;
                                }

                                if (childnode.Name == Functions.WINFF_NODE_PARAMS) {                                   
                                    string[] a = childnode.InnerText.Split(new string[] { "-" }, 
                                        StringSplitOptions.RemoveEmptyEntries);

                                    if (a.Length > 0) {
                                        foreach (string l in a) {
                                            string d = l.Trim();
                                            if (d.Contains(" ")) {
                                                string[] t = d.Split(new char[] { ' ' });
                                                if (t.Length == 1) {
                                                    newpreset.CommandLineOptions.Add(
                                                        new CommandOption(t[0].Trim(), ""));
                                                } else if (t.Length == 2) {
                                                    newpreset.CommandLineOptions.Add(
                                                        new CommandOption(t[0].Trim(), t[1].Trim()));
                                                }
                                            } else {
                                                newpreset.CommandLineOptions.Add(
                                                    new CommandOption(d.Trim(), ""));
                                            }
                                        }
                                    }
                                }                   
                            } // end foreach
                        }

                    } // end if

                    if (!String.IsNullOrEmpty(newpreset.Name)) {
                        this.list.Add(newpreset);
                    }
                }
            }
        }
    }
}
