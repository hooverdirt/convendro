using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;

namespace libconvendro.Plugins {
    public class BaseConvendroPlugin : IConvendroPlugin{
        private string name = null;
        private string author = null;
        private string description = null;
        private string caption = null;
        private string copyrights = null;
        private Version version = null;
        private Bitmap bitmap = null;
        private Guid guid = Guid.NewGuid();
        private IConvendroHost host;

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public Guid Guid {
            get { return this.guid; }
            set { this.guid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Execute() {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ahost"></param>
        protected virtual void setHost(IConvendroHost ahost) {
            this.host = ahost;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConvendroHost Host {
            get {
                return this.host;
            }
            set {
                setHost(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void setName() {
            this.name = this.GetType().FullName;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize() {
            this.setName();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Uninitialize() {
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description {
            get { return this.description; }
            set { this.description = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Caption {
            get { return this.caption; }
            set { this.caption = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Author {
            get { return this.author; }
            set { this.author = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CopyrightInformation {
            get { return this.copyrights; }
            set { this.copyrights = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Version Version {
            get { return this.version; }
            set { this.version = value; }
        }

        /// <summary>
        /// Gets or sets the Plugin's menu/toolbar image.
        /// </summary>
        public Bitmap MenuBitmap {
            get {
                return this.bitmap;
            }
            set {
                this.bitmap = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Config() {
            return true;
        }

    }
}
