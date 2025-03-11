using DownloadManager.Domain;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadManager.DataAccess
{
    public class UrlPersister:IUrlPersister
    {
        private readonly IOpenFileDialog _loadFileDialog;
        private readonly IOpenFileDialog _saveFileDialog;
        private readonly IFileSystem _fileSystem;

        public UrlPersister(IOpenFileDialog loadFileDialog, 
            IOpenFileDialog saveFileDialog,
            IFileSystem file)
        {
            ArgumentNullException.ThrowIfNull(loadFileDialog, nameof(loadFileDialog));
            ArgumentNullException.ThrowIfNull(saveFileDialog, nameof(saveFileDialog));
            ArgumentNullException.ThrowIfNull(file, nameof(file));

            _loadFileDialog = loadFileDialog;
            _saveFileDialog = saveFileDialog;
            _fileSystem = file;
        }

        public void SaveUrlsToFile(IEnumerable<string> lines)
        {
            _saveFileDialog.Filter = "Text Files |*.txt" ;

            if (true == _saveFileDialog.ShowDialog())
            {
                try
                {
                    // Save data from the DataGrid to the named file.
                    _fileSystem.WriteLinesToFile(_saveFileDialog.FileName, lines);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public string[]? LoadUrls()
        {
            _loadFileDialog.Filter = "Text Files |*.txt";

            string[]? dataFromFile = null;

            // Did they click on the OK button?
            if (true == _loadFileDialog.ShowDialog())
            {
                try
                {
                    // Load all text of selected file.
                    dataFromFile =  _fileSystem.ReadFileLines(_loadFileDialog.FileName);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dataFromFile;
        }
    }
}
