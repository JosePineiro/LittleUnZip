using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace LittleUnzip
{
    public partial class LittleUnzipTest : Form
    {
        public LittleUnzipTest()
        {
            InitializeComponent();
            if (IntPtr.Size == 8)
                this.Text = Application.ProductName + " v" + Application.ProductVersion + " [x64]";
            else
                this.Text = Application.ProductName + " v" + Application.ProductVersion + " [x86]";

            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>Select the source ZIP file</summary>
        private void buttonZipFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "Zip (*.zip)|*.zip";
            saveFileDialog.DefaultExt = ".zip";
            saveFileDialog.OverwritePrompt = false;
            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                this.textBoxZipFile.Text = saveFileDialog.FileName;
                this.textBoxOutputPath.Text = Path.GetDirectoryName(this.textBoxZipFile.Text);
            }
        }

        /// <summary>Select the destination directory for decompress</summary>
        private void buttonOutputPath_Click(object sender, System.EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (Directory.Exists(Path.GetDirectoryName(this.textBoxZipFile.Text)))
                        folderBrowserDialog.SelectedPath = Path.GetDirectoryName(this.textBoxZipFile.Text);
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                        this.textBoxOutputPath.Text = folderBrowserDialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nEn frmMain.buttonInPath_Click\r\nCapture esta pantalla y reporte en el foro de soporte.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Extract Zip source files to destination ZIP file</summary>
        private void buttonUnzip_Click(object sender, System.EventArgs e)
        {
            /*
            //UnZIP all files
            using (LittleUnZip zip = new LittleUnZip(this.textBoxZipFile.Text))
            {
                for (int f = 0; f < files.Length; f++)
                {
                    //zip.AddFile(files[f], files[f].Substring(this.textBoxSource.Text.Length), 13, "");
                    this.progressBar.Value++;
                    Application.DoEvents();
                }
            }
            */
            using (LittleUnZip zip = new LittleUnZip(this.textBoxZipFile.Text))
                zip.Extract(this.textBoxOutputPath.Text, true, this.progressBar);
            this.progressBar.Value = 0;
        }

        private void buttonTest_Click(object sender, System.EventArgs e)
        {
            bool result;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Test ZIP file
            using (LittleUnZip zip = new LittleUnZip(this.textBoxZipFile.Text))
                result = zip.Test();

            sw.Stop();

            if (result == true)
                MessageBox.Show("Zip file integrity OK. Tested in " + sw.Elapsed + "ms.");
            else
                MessageBox.Show("Zip file integrity FAIL. Tested in " + sw.Elapsed + "ms.");

        }

        private void buttonZipInfo_Click(object sender, EventArgs e)
        {
            string zipInformation = "";

            using (LittleUnZip zip = new LittleUnZip(this.textBoxZipFile.Text))
                foreach (LittleUnZip.ZipFileEntry zfe in zip.zipFileEntrys)
                    zipInformation = zipInformation +
                        "Filename: " + zfe.ToString() + ", " +
                        "File size: " + zfe.fileSize + ", " +
                        "Compresed size: " + zfe.compressedSize + ", " +
                        "Metod: " + zfe.method.ToString() + ", " +
                        "Comment: " + zfe.comment + "\r\n";
            
            MessageBox.Show(zipInformation, Path.GetFileNameWithoutExtension(this.textBoxZipFile.Text), MessageBoxButtons.OK);
        }
    }
}

