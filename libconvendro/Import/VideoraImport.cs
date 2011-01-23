using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using libconvendro.Persistence;

namespace libconvendro.Import {

    /// <summary>
    /// 
    /// </summary>
    public static class VideoraItem {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="profiles"></param>
        /// <returns></returns>
        public static bool SerializeVideoraFile(string filename, ProfileList profiles) {
            bool res = false;

            XmlSerializer nser = new XmlSerializer(typeof(ProfileList));
            TextWriter ntext = new StreamWriter(filename);
            try {
                nser.Serialize(ntext, profiles);
                ntext.Flush();
                res = true;
            } catch {
                res = false;
            } finally {
                ntext.Dispose();
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ProfileList DeserializeVideoraFile(string filename) {
            ProfileList pf = null;

            if (File.Exists(filename)) {
                XmlSerializer nser = new XmlSerializer(typeof(ProfileList));
                TextReader ntext = new StreamReader(filename);
                try {
                    pf = (ProfileList)nser.Deserialize(ntext);
                } finally {
                    ntext.Close();
                    ntext.Dispose();
                }
            }
            return pf;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VideoraFile : BaseImporter {

        private CommandLineOptions createCommandLineOptions(profile i) {
            CommandLineOptions res = null;

            try {
                res = new CommandLineOptions();
                if (i.abitrate != 0) {
                    res.Add("ab", i.abitrate.ToString());
                }

                if (i.achannels != 0) {
                    res.Add("ac", i.achannels.ToString());
                }

                if (!String.IsNullOrEmpty(i.acli)) {
                    // not supported yet
                }

                if (!String.IsNullOrEmpty(i.acodec)) {
                    res.Add("acodec", i.acodec);
                }

                if (!String.IsNullOrEmpty(i.amode)) {
                    // not supported yet.
                }

                if (i.asamplerate != 0) {
                    res.Add("ar", i.asamplerate.ToString());
                }

                if (i.avol != 0) {
                    res.Add("vol", i.avol.ToString());
                }

                if (i.duration != 0) {
                    // not supported;
                }

                if (!String.IsNullOrEmpty(i.encoder)) {
                    // not supported;
                }

                if (i.position != 0) {
                    // not supported;
                }

                if (i.vafenabled == true) {
                    // not supported;
                }

                if (i.varenabled == true) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.varmaxres)) {
                    res.Add("s", i.varmaxres);
                }

                if (i.varmod16) {
                    // not supported
                }

                if (i.vartotal) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.vaspect)) {
                    res.Add("aspect", i.vaspect);
                }

                if (i.vavsaudiodelay != 0) {
                    res.Add("muxdelay", i.vavsaudiodelay.ToString());
                }

                if (i.vavsautofps == true) {
                    /// not supported
                }

                if (i.vavsconvertfps == true) {
                    // not supported
                }

                if (i.vavsenabled == true) {
                    // not supported
                }

                if (i.vavsmanualfps != 0) {
                    // not supported
                }

                if (i.vavssubtitles == true) {
                    // not supported
                }

                if (i.vbitrate != 0) {
                    res.Add("vb", i.vbitrate.ToString());
                }

                if (i.vbufsize != 0) {
                    // not supported
                }

                if (i.vcabac == true) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.vcli)) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.vcli2)) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.vcli1)) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.vcodec)) {
                    res.Add("vcodec", i.vcodec);
                }

                if (i.vcqp != 0) {
                    // not supported
                }

                if (i.vcrf != 0) {
                    // not supported
                }

                if (i.vcropbottom != 0) {
                    res.Add("cropbottom", i.vcropbottom.ToString());
                }

                if (i.vcroptop != 0) {
                    res.Add("croptop", i.vcroptop.ToString());
                }

                if (i.vcropleft != 0) {
                    res.Add("cropleft", i.vcropleft.ToString());
                }

                if (i.vcropright != 0) {
                    res.Add("cropright", i.vcropright.ToString());
                }

                if (i.vdeinterlace == true) {
                    res.Add("deinterlace", "");
                }

                if (i.vframerate != 0) {
                    res.Add("r", i.vframerate.ToString());
                }

                if (i.vheight != 0) {
                    // not supported
                }

                if (i.vkeyint != 0) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.vlevel)) {
                    res.Add("v", i.vlevel);
                }

                if (i.vmaxrate != 0) {
                    // not supported
                }

                if (i.vminrate != 0) {
                    // not supported
                }

                if (!String.IsNullOrEmpty(i.vmode)) {
                    // not supported
                }

                if (i.vpadbottom != 0) {
                    res.Add("padbottom", i.vpadbottom.ToString());
                }

                if (i.vpadtop != 0) {
                    res.Add("padtop", i.vpadtop.ToString());
                }

                if (i.vpadleft != 0) {
                    res.Add("padleft", i.vpadleft.ToString());
                }

                if (i.vpadright != 0) {
                    res.Add("padright", i.vpadright.ToString());
                }

                if (!String.IsNullOrEmpty(i.vprofile)) {
                    res.Add("vpre", i.vprofile);
                }

                if (i.vthreads != 0) {
                    res.Add("threads", i.vthreads.ToString());
                }

                if (i.vvgauuid == true) {
                    // not supported
                }

                if (i.vwidth != 0) {
                    // not supported...
                }


            } catch {
                // do something...
            }

            return res;
        }

        private void processVideoraItems(ProfileList afilelist) {
            foreach (profile i in afilelist) {
                Preset pr = new Preset();
                pr.Name = i.name;
                pr.Category = Path.GetFileNameWithoutExtension(this.file);
                pr.Description = i.encoder;
                pr.Extension = "mp4";     
                CommandLineOptions np = this.createCommandLineOptions(i);
                if (np != null) {
                    pr.CommandLineOptions = np;
                }
                this.Presets.Add(pr);
            }
        }

        public override void LoadFile(string filename) {
            base.LoadFile(filename);
            if (File.Exists(filename)) {
                ProfileList newprofile = VideoraItem.DeserializeVideoraFile(filename);

                if (newprofile != null) {
                    processVideoraItems(newprofile);
                }
            }
        }
    }
    /// <summary>
    /// Quick and Dirty....
    /// </summary>
    [XmlRoot("ProfileList")]
    public class ProfileList : List<profile> {
        public ProfileList() {
        }
       
    }

    /// <summary>
    /// Quick and Dirty...
    /// </summary>
    public class profile {
        [XmlAttribute()]
        public string name;
        [XmlAttribute()]
        public string encoder;
        [XmlAttribute()]
        public int position;
        [XmlAttribute()]
        public int duration;
        [XmlAttribute()]
        public string vcodec;
        [XmlAttribute()]
        public string vprofile;
        [XmlAttribute()]
        public string vlevel;
        [XmlAttribute()]
        public string vmode;
        [XmlAttribute()]
        public int vbitrate;
        [XmlAttribute()]
        public int vcrf;
        [XmlAttribute()]
        public int vcqp;
        [XmlAttribute()]
        public int vwidth;
        [XmlAttribute()]
        public int vheight;
        [XmlAttribute()]
        public string vaspect;
        [XmlAttribute()]
        public int vframerate;
        [XmlAttribute()]
        public bool varenabled;
        [XmlAttribute()]
        public string varmaxres;
        [XmlAttribute()]
        public bool varmod16;
        [XmlAttribute()]
        public bool vartotal;
        [XmlAttribute()]
        public bool vafenabled;
        [XmlAttribute()]
        public int vcroptop;
        [XmlAttribute()]
        public int vcropbottom;
        [XmlAttribute()]
        public int vcropleft;
        [XmlAttribute()]
        public int vcropright;
        [XmlAttribute()]
        public int vpadtop;
        [XmlAttribute()]
        public int vpadbottom;
        [XmlAttribute()]
        public int vpadleft;
        [XmlAttribute()]
        public int vpadright;
        [XmlAttribute()]
        public string vcli;
        [XmlAttribute()]
        public string vcli1;
        [XmlAttribute()]
        public string vcli2;
        [XmlAttribute()]
        public bool vavsenabled;
        [XmlAttribute()]
        public bool vavsautofps;
        [XmlAttribute()]
        public int vavsmanualfps;
        [XmlAttribute()]
        public bool vavsconvertfps;
        [XmlAttribute()]
        public int vavsaudiodelay;
        [XmlAttribute()]
        public bool vavssubtitles;
        [XmlAttribute()]
        public int vbufsize;
        [XmlAttribute()]
        public int vminrate;
        [XmlAttribute()]
        public int vmaxrate;
        [XmlAttribute()]
        public int vkeyint;
        [XmlAttribute()]
        public bool vdeinterlace;
        [XmlAttribute()]
        public int vthreads;
        [XmlAttribute()]
        public bool vcabac;
        [XmlAttribute()]
        public bool vvgauuid;
        [XmlAttribute()]
        public string acodec;
        [XmlAttribute()]
        public string amode;
        [XmlAttribute()]
        public int abitrate;
        [XmlAttribute()]
        public int achannels;
        [XmlAttribute()]
        public int asamplerate;
        [XmlAttribute()]
        public int avol;
        [XmlAttribute()]
        public string acli;
    }
}
