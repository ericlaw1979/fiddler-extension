namespace RunscopeFiddlerExtension
{
    partial class ConfigureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cboBuckets = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdGetKey = new System.Windows.Forms.Button();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOk.Location = new System.Drawing.Point(131, 340);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(129, 34);
            this.cmdOk.TabIndex = 0;
            this.cmdOk.Text = "&Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(266, 340);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(129, 34);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cboBuckets
            // 
            this.cboBuckets.FormattingEnabled = true;
            this.cboBuckets.Location = new System.Drawing.Point(37, 171);
            this.cboBuckets.Name = "cboBuckets";
            this.cboBuckets.Size = new System.Drawing.Size(296, 28);
            this.cboBuckets.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Target Bucket";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(37, 65);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(296, 28);
            this.txtApiKey.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "API Key";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cmdGetKey
            // 
            this.cmdGetKey.Location = new System.Drawing.Point(204, 99);
            this.cmdGetKey.Name = "cmdGetKey";
            this.cmdGetKey.Size = new System.Drawing.Size(129, 28);
            this.cmdGetKey.TabIndex = 6;
            this.cmdGetKey.Text = "&Get Key";
            this.cmdGetKey.UseVisualStyleBackColor = true;
            this.cmdGetKey.Click += new System.EventHandler(this.cmdGetKey_Click);
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.Location = new System.Drawing.Point(38, 245);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(330, 58);
            this.chkUseProxy.TabIndex = 7;
            this.chkUseProxy.Text = "Use Fiddler proxy to talk to Runscope API";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            this.chkUseProxy.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ConfigureForm
            // 
            this.AcceptButton = this.cmdOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(409, 392);
            this.Controls.Add(this.chkUseProxy);
            this.Controls.Add(this.cmdGetKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboBuckets);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Font = new System.Drawing.Font("Verdana", 9.134328F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureForm";
            this.Text = "Configure Runscope Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ComboBox cboBuckets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdGetKey;
        private System.Windows.Forms.CheckBox chkUseProxy;
    }
}