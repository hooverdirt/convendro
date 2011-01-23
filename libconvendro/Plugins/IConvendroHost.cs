using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using libconvendro.Persistence;

namespace libconvendro.Plugins {
    /// <summary>
    /// Allows the thread to work on the mainform.
    /// </summary>
    public interface IThreadingHost {
        void SetControlsThreading(Boolean var);
        ListView FileListView { get; set; }
        StatusStrip Statusbar { get; set; }
        TextBox LogBox { get; set; }
    }

    /// <summary>
    /// Allows access to the current worklist, including presets.
    /// </summary>
    public interface IConvendroHost {
        /// <summary>
        /// Gets the current known MediaFileList (after processing)
        /// </summary>
        MediaFileList MediaFileList {get; }

        /// <summary>
        /// Gets or sets the selected items in the process list
        /// </summary>
        int[] SelectedIndices { get; set; }

        /// <summary>
        /// Gets the current selected file item in the process list
        /// </summary>
        string[] SelectedItems { get; }

        /// <summary>
        /// Gets the presetname in the process list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetPresetNameItem(int index);

        /// <summary>
        /// Sets the presetname in the process list
        /// </summary>
        /// <param name="presetname"></param>
        void SetPresetNameItem(int index, string presetname);

        /// <summary>
        /// Gets the complete presetsfile object
        /// </summary>
        PresetsFile PresetsFile { get; set; }

        /// <summary>
        /// Adds a file to the processing list
        /// </summary>
        /// <param name="filename">An existing filename</param>
        void AddMediaFile(string filename);

        /// <summary>
        /// Starts conversion process using the default
        /// converter
        /// </summary>
        void Start();

        /// <summary>
        /// Stops current conversion process
        /// </summary>
        void Stop();
    }
}
