namespace LittleUnzip
{
    partial class LittleUnzipTest
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonUnzip = new System.Windows.Forms.Button();
            this.textBoxZipFile = new System.Windows.Forms.TextBox();
            this.buttonZipFile = new System.Windows.Forms.Button();
            this.textBoxOutputPath = new System.Windows.Forms.TextBox();
            this.buttonOutputPath = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonZipInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 136);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(615, 23);
            this.progressBar.TabIndex = 18;
            // 
            // buttonUnzip
            // 
            this.buttonUnzip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUnzip.Location = new System.Drawing.Point(78, 77);
            this.buttonUnzip.Name = "buttonUnzip";
            this.buttonUnzip.Size = new System.Drawing.Size(75, 30);
            this.buttonUnzip.TabIndex = 3;
            this.buttonUnzip.Text = "UnZIP";
            this.buttonUnzip.UseVisualStyleBackColor = true;
            this.buttonUnzip.Click += new System.EventHandler(this.buttonUnzip_Click);
            // 
            // textBoxZipFile
            // 
            this.textBoxZipFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxZipFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxZipFile.Location = new System.Drawing.Point(12, 7);
            this.textBoxZipFile.Name = "textBoxZipFile";
            this.textBoxZipFile.Size = new System.Drawing.Size(539, 22);
            this.textBoxZipFile.TabIndex = 16;
            this.textBoxZipFile.Text = "ZipFile";
            // 
            // buttonZipFile
            // 
            this.buttonZipFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZipFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonZipFile.Location = new System.Drawing.Point(557, 7);
            this.buttonZipFile.Name = "buttonZipFile";
            this.buttonZipFile.Size = new System.Drawing.Size(46, 22);
            this.buttonZipFile.TabIndex = 1;
            this.buttonZipFile.Text = ". . .";
            this.buttonZipFile.UseVisualStyleBackColor = true;
            this.buttonZipFile.Click += new System.EventHandler(this.buttonZipFile_Click);
            // 
            // textBoxOutputPath
            // 
            this.textBoxOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOutputPath.Location = new System.Drawing.Point(12, 35);
            this.textBoxOutputPath.Name = "textBoxOutputPath";
            this.textBoxOutputPath.Size = new System.Drawing.Size(539, 22);
            this.textBoxOutputPath.TabIndex = 14;
            this.textBoxOutputPath.Text = "Output Path";
            // 
            // buttonOutputPath
            // 
            this.buttonOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOutputPath.Location = new System.Drawing.Point(557, 35);
            this.buttonOutputPath.Name = "buttonOutputPath";
            this.buttonOutputPath.Size = new System.Drawing.Size(46, 22);
            this.buttonOutputPath.TabIndex = 2;
            this.buttonOutputPath.Text = ". . .";
            this.buttonOutputPath.UseVisualStyleBackColor = true;
            this.buttonOutputPath.Click += new System.EventHandler(this.buttonOutputPath_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTest.Location = new System.Drawing.Point(266, 77);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 30);
            this.buttonTest.TabIndex = 19;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonZipInfo
            // 
            this.buttonZipInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonZipInfo.Location = new System.Drawing.Point(454, 77);
            this.buttonZipInfo.Name = "buttonZipInfo";
            this.buttonZipInfo.Size = new System.Drawing.Size(83, 30);
            this.buttonZipInfo.TabIndex = 20;
            this.buttonZipInfo.Text = "Information";
            this.buttonZipInfo.UseVisualStyleBackColor = true;
            this.buttonZipInfo.Click += new System.EventHandler(this.buttonZipInfo_Click);
            // 
            // LittleUnzipTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 159);
            this.Controls.Add(this.buttonZipInfo);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonUnzip);
            this.Controls.Add(this.textBoxZipFile);
            this.Controls.Add(this.buttonZipFile);
            this.Controls.Add(this.textBoxOutputPath);
            this.Controls.Add(this.buttonOutputPath);
            this.Name = "LittleUnzipTest";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonUnzip;
        private System.Windows.Forms.TextBox textBoxZipFile;
        private System.Windows.Forms.Button buttonZipFile;
        private System.Windows.Forms.TextBox textBoxOutputPath;
        private System.Windows.Forms.Button buttonOutputPath;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonZipInfo;
    }
}

