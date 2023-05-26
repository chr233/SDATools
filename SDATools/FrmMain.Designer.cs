namespace SDATools
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            groupBox3 = new GroupBox();
            btnMaFolder = new Button();
            txtMaFolder = new TextBox();
            btnSort = new Button();
            btnUpdate = new Button();
            sS = new StatusStrip();
            tsAuthor = new ToolStripStatusLabel();
            tsGithub = new ToolStripStatusLabel();
            tsVersion = new ToolStripStatusLabel();
            groupBox1 = new GroupBox();
            rbSteamIDAccountName = new RadioButton();
            rbAccountName = new RadioButton();
            rbSteamID = new RadioButton();
            groupBox3.SuspendLayout();
            sS.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(btnMaFolder);
            groupBox3.Controls.Add(txtMaFolder);
            groupBox3.Location = new Point(12, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(505, 58);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "原始 maFiles 目录";
            // 
            // btnMaFolder
            // 
            btnMaFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaFolder.Location = new Point(423, 22);
            btnMaFolder.Name = "btnMaFolder";
            btnMaFolder.Size = new Size(75, 23);
            btnMaFolder.TabIndex = 3;
            btnMaFolder.Text = "…";
            btnMaFolder.UseVisualStyleBackColor = true;
            btnMaFolder.Click += BtnMaFolder_Click;
            // 
            // txtMaFolder
            // 
            txtMaFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtMaFolder.Location = new Point(6, 22);
            txtMaFolder.Name = "txtMaFolder";
            txtMaFolder.PlaceholderText = "保存 .maFile 和 manifest.json 文件的位置";
            txtMaFolder.Size = new Size(411, 23);
            txtMaFolder.TabIndex = 2;
            // 
            // btnSort
            // 
            btnSort.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSort.Location = new Point(12, 137);
            btnSort.Name = "btnSort";
            btnSort.Size = new Size(166, 37);
            btnSort.TabIndex = 8;
            btnSort.Text = "令牌文件整理+去重";
            btnSort.UseVisualStyleBackColor = true;
            btnSort.Click += BtnSort_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnUpdate.Location = new Point(351, 137);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(166, 37);
            btnUpdate.TabIndex = 8;
            btnUpdate.Text = "自动更新 mainifest.json";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += BtnUpdate_Click;
            // 
            // sS
            // 
            sS.Items.AddRange(new ToolStripItem[] { tsAuthor, tsGithub, tsVersion });
            sS.Location = new Point(0, 184);
            sS.Name = "sS";
            sS.Size = new Size(529, 22);
            sS.TabIndex = 10;
            sS.Text = "statusStrip1";
            // 
            // tsAuthor
            // 
            tsAuthor.IsLink = true;
            tsAuthor.LinkBehavior = LinkBehavior.HoverUnderline;
            tsAuthor.LinkColor = Color.Black;
            tsAuthor.Name = "tsAuthor";
            tsAuthor.Size = new Size(64, 17);
            tsAuthor.Text = "作者: Chr_";
            tsAuthor.VisitedLinkColor = Color.Black;
            tsAuthor.Click += TsAuthor_Click;
            // 
            // tsGithub
            // 
            tsGithub.IsLink = true;
            tsGithub.LinkBehavior = LinkBehavior.HoverUnderline;
            tsGithub.LinkColor = Color.Black;
            tsGithub.Name = "tsGithub";
            tsGithub.Size = new Size(387, 17);
            tsGithub.Spring = true;
            tsGithub.Text = "获取源码";
            tsGithub.VisitedLinkColor = Color.Black;
            tsGithub.Click += TsGithub_Click;
            // 
            // tsVersion
            // 
            tsVersion.IsLink = true;
            tsVersion.LinkBehavior = LinkBehavior.HoverUnderline;
            tsVersion.LinkColor = Color.Black;
            tsVersion.Name = "tsVersion";
            tsVersion.Size = new Size(32, 17);
            tsVersion.Text = "版本";
            tsVersion.VisitedLinkColor = Color.Black;
            tsVersion.Click += TsVersion_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(rbSteamIDAccountName);
            groupBox1.Controls.Add(rbAccountName);
            groupBox1.Controls.Add(rbSteamID);
            groupBox1.Location = new Point(12, 76);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(505, 52);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "令牌命名规则";
            // 
            // rbSteamIDAccountName
            // 
            rbSteamIDAccountName.AutoSize = true;
            rbSteamIDAccountName.Location = new Point(315, 22);
            rbSteamIDAccountName.Name = "rbSteamIDAccountName";
            rbSteamIDAccountName.Size = new Size(178, 21);
            rbSteamIDAccountName.TabIndex = 1;
            rbSteamIDAccountName.TabStop = true;
            rbSteamIDAccountName.Text = "账户登录名-SteamID.mafile";
            rbSteamIDAccountName.UseVisualStyleBackColor = true;
            // 
            // rbAccountName
            // 
            rbAccountName.AutoSize = true;
            rbAccountName.Location = new Point(185, 22);
            rbAccountName.Name = "rbAccountName";
            rbAccountName.Size = new Size(124, 21);
            rbAccountName.TabIndex = 0;
            rbAccountName.TabStop = true;
            rbAccountName.Text = "账户登录名.mafile";
            rbAccountName.UseVisualStyleBackColor = true;
            // 
            // rbSteamID
            // 
            rbSteamID.AutoSize = true;
            rbSteamID.Location = new Point(6, 22);
            rbSteamID.Name = "rbSteamID";
            rbSteamID.Size = new Size(173, 21);
            rbSteamID.TabIndex = 0;
            rbSteamID.TabStop = true;
            rbSteamID.Text = "SteamID.mafile (SDA默认)";
            rbSteamID.UseVisualStyleBackColor = true;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(529, 206);
            Controls.Add(groupBox1);
            Controls.Add(sS);
            Controls.Add(groupBox3);
            Controls.Add(btnUpdate);
            Controls.Add(btnSort);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(9999, 245);
            MinimumSize = new Size(545, 245);
            Name = "FrmMain";
            Text = "SDA小工具";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            sS.ResumeLayout(false);
            sS.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox3;
        private Button btnMaFolder;
        private TextBox txtMaFolder;
        private Button btnSort;
        private Button btnUpdate;
        private StatusStrip sS;
        private ToolStripStatusLabel tsAuthor;
        private ToolStripStatusLabel tsGithub;
        private ToolStripStatusLabel tsVersion;
        private GroupBox groupBox1;
        private RadioButton rbAccountName;
        private RadioButton rbSteamID;
        private RadioButton rbSteamIDAccountName;
    }
}