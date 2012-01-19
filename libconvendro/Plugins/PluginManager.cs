using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace libconvendro.Plugins {

    public delegate void PluginLoadedEvent (object anobject, IConvendroPlugin plugin);
    public delegate void PluginManagerErrorEvent(object anobject, Exception exception);


    /// <summary>
    /// Simple PluginManager
    /// </summary>
    public class PluginManager {
        private string pluginfolder = null;
        private ArrayList plugins = new ArrayList();

        /// <summary>
        /// This event fires everytime a plugin is added to the ArrayList
        /// </summary>
        public event PluginLoadedEvent OnPluginLoad = null;

        public event PluginManagerErrorEvent OnPluginManagerError = null;


        /// <summary>
        /// This event fires whenever the PluginList is cleared.
        /// </summary>
        public event EventHandler OnClear = null;

        /// <summary>
        /// 
        /// </summary>
        public PluginManager (){
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginfolder"></param>
        public PluginManager(string pluginfolder) : this() {
            if (!String.IsNullOrEmpty(pluginfolder)) {
                this.pluginfolder = pluginfolder;
            }
        }
       
        /// <summary>
        /// Gets the current plugin folder
        /// </summary>
        public string PluginFolder {
            get { return this.pluginfolder; }
        }

        /// <summary>
        /// Gets or sets the Plugins individually.
        /// </summary>
        public ArrayList Items {
            get { return this.plugins; }
            set { this.plugins = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IConvendroPlugin FindPlugin(Guid guid) {
            // I'm going to so regret doing this this way:
            // Should have made and used a strongly typed list, in
            // case I want to serialize this...


            IConvendroPlugin currentplugin = null;

            foreach (IConvendroPlugin ip in this.plugins) {
                if (ip.Guid.Equals(guid)) {
                    currentplugin = ip;
                    break;
                }
            }

            return currentplugin;
        }

        /// <summary>
        /// Reloads or Loads the plugin list.
        /// </summary>
        public void Load() {            
            this.loadPlugins();
        }

        /// <summary>
        /// 
        /// </summary>
        private void clearPlugins() {
            if (OnClear != null) {
                this.OnClear(this, new EventArgs());
            }
            this.plugins.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        private void loadPlugins() {
            try {
                string[] dirinfo = Directory.GetFiles(this.pluginfolder + "\\", "*.dll");            
                foreach (string sfile in dirinfo) {
                    if (!String.IsNullOrEmpty(sfile)) {
                        try {
                            Assembly asm = Assembly.LoadFrom(sfile);

                            Type[] arraytypes = asm.GetTypes();

                            foreach (Type atype in arraytypes) {
                                if (atype != null) {

                                    try {
                                        object iobj = Activator.CreateInstance(atype);
                                        if (iobj != null) {

                                            if (iobj is IConvendroPlugin) {

                                                plugins.Add((IConvendroPlugin)iobj);
                                                if (OnPluginLoad != null) {
                                                    OnPluginLoad(this, (IConvendroPlugin)iobj);
                                                }
                                            }
                                        }
                                    } catch (Exception ex) {
                                        if (OnPluginManagerError != null) {
                                            OnPluginManagerError(this, ex);
                                        }
                                    }

                                }
                            }
                        } catch (Exception ex) {
                            // not loadable...
                        }

                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
