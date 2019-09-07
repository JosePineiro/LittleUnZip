using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Text;

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
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Zip (*.zip)|*.zip";
                openFileDialog.DefaultExt = ".zip";

                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    this.textBoxZipFile.Text = openFileDialog.FileName;
                    this.textBoxOutputPath.Text = Path.Combine(Path.GetDirectoryName(openFileDialog.FileName),
                                                               Path.GetFileNameWithoutExtension(openFileDialog.FileName));
                }
            }
        }

        /// <summary>Select the destination directory for decompress</summary>
        private void buttonOutputPath_Click(object sender, System.EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (Directory.Exists(Path.GetDirectoryName(this.textBoxZipFile.Text)))
                    folderBrowserDialog.SelectedPath = Path.GetDirectoryName(this.textBoxZipFile.Text);
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    this.textBoxOutputPath.Text = folderBrowserDialog.SelectedPath;
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
            StringBuilder zipInformation = new StringBuilder();
            using (LittleUnZip zip = new LittleUnZip(this.textBoxZipFile.Text))
                foreach (LittleUnZip.ZipFileEntry zfe in zip.zipFileEntrys)
                {
                    zipInformation.Append("Filename: ").Append(zfe.filename);
                    zipInformation.Append(" File size: ").Append(zfe.fileSize);
                    zipInformation.Append(" Compresed size: ").Append(zfe.compressedSize);
                    zipInformation.Append(" Metod: ").Append(zfe.method);
                    zipInformation.Append(" Comment: ").Append(zfe.comment);
                    zipInformation.Append("\r\n");
                }
            
            MessageBox.Show(zipInformation.ToString(), Path.GetFileNameWithoutExtension(this.textBoxZipFile.Text), MessageBoxButtons.OK);
        }
    }
}

