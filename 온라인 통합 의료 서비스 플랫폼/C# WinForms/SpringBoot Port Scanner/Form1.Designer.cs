namespace SpringBoot_Port_Scanner
{
    partial class Form1
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
            lblIp = new Label();
            lblStart = new Label();
            lblEnd = new Label();
            txtIp = new TextBox();
            txtStart = new TextBox();
            txtEnd = new TextBox();
            btnScan = new Button();
            btnFilePath = new Button();
            pgbScan = new ProgressBar();
            lblFile = new Label();
            lvScan = new ListView();
            chPID = new ColumnHeader();
            chProcess = new ColumnHeader();
            btnKill = new Button();
            folderBrowserDialog = new FolderBrowserDialog();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblIp
            // 
            lblIp.AutoSize = true;
            lblIp.Location = new Point(39, 29);
            lblIp.Name = "lblIp";
            lblIp.Size = new Size(69, 25);
            lblIp.TabIndex = 0;
            lblIp.Text = "스캔 IP";
            // 
            // lblStart
            // 
            lblStart.AutoSize = true;
            lblStart.Location = new Point(39, 87);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(90, 25);
            lblStart.TabIndex = 1;
            lblStart.Text = "시작 포트";
            // 
            // lblEnd
            // 
            lblEnd.AutoSize = true;
            lblEnd.Location = new Point(39, 144);
            lblEnd.Name = "lblEnd";
            lblEnd.Size = new Size(90, 25);
            lblEnd.TabIndex = 2;
            lblEnd.Text = "종료 포트";
            // 
            // txtIp
            // 
            txtIp.Location = new Point(125, 29);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(153, 31);
            txtIp.TabIndex = 3;
            // 
            // txtStart
            // 
            txtStart.Location = new Point(153, 87);
            txtStart.Name = "txtStart";
            txtStart.Size = new Size(125, 31);
            txtStart.TabIndex = 4;
            // 
            // txtEnd
            // 
            txtEnd.Location = new Point(153, 144);
            txtEnd.Name = "txtEnd";
            txtEnd.Size = new Size(125, 31);
            txtEnd.TabIndex = 5;
            // 
            // btnScan
            // 
            btnScan.Location = new Point(319, 29);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(112, 64);
            btnScan.TabIndex = 6;
            btnScan.Text = "스캔";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // btnFilePath
            // 
            btnFilePath.Location = new Point(39, 222);
            btnFilePath.Name = "btnFilePath";
            btnFilePath.Size = new Size(392, 43);
            btnFilePath.TabIndex = 7;
            btnFilePath.Text = "파일 경로";
            btnFilePath.UseVisualStyleBackColor = true;
            btnFilePath.Click += btnFilePath_Click;
            // 
            // pgbScan
            // 
            pgbScan.Location = new Point(39, 327);
            pgbScan.Name = "pgbScan";
            pgbScan.Size = new Size(392, 34);
            pgbScan.TabIndex = 8;
            // 
            // lblFile
            // 
            lblFile.AutoSize = true;
            lblFile.Location = new Point(39, 288);
            lblFile.Name = "lblFile";
            lblFile.Size = new Size(90, 25);
            lblFile.TabIndex = 9;
            lblFile.Text = "생성 파일";
            // 
            // lvScan
            // 
            lvScan.Columns.AddRange(new ColumnHeader[] { chPID, chProcess });
            lvScan.GridLines = true;
            lvScan.Location = new Point(505, 29);
            lvScan.Name = "lvScan";
            lvScan.Size = new Size(454, 409);
            lvScan.TabIndex = 40;
            lvScan.UseCompatibleStateImageBehavior = false;
            lvScan.View = View.Details;
            // 
            // chPID
            // 
            chPID.Text = "PID";
            chPID.Width = 150;
            // 
            // chProcess
            // 
            chProcess.Text = "Process";
            chProcess.Width = 160;
            // 
            // btnKill
            // 
            btnKill.Location = new Point(39, 392);
            btnKill.Name = "btnKill";
            btnKill.Size = new Size(392, 46);
            btnKill.TabIndex = 41;
            btnKill.Text = "TaskKill";
            btnKill.UseVisualStyleBackColor = true;
            btnKill.Click += btnKill_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(319, 130);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 39);
            btnCancel.TabIndex = 42;
            btnCancel.Text = "중단";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnKill);
            Controls.Add(lvScan);
            Controls.Add(lblFile);
            Controls.Add(pgbScan);
            Controls.Add(btnFilePath);
            Controls.Add(btnScan);
            Controls.Add(txtEnd);
            Controls.Add(txtStart);
            Controls.Add(txtIp);
            Controls.Add(lblEnd);
            Controls.Add(lblStart);
            Controls.Add(lblIp);
            Name = "Form1";
            Text = "Port Scanner";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIp;
        private Label lblStart;
        private Label lblEnd;
        private TextBox txtIp;
        private TextBox txtStart;
        private TextBox txtEnd;
        private Button btnScan;
        private Button btnFilePath;
        private ProgressBar pgbScan;
        private Label lblFile;
        private ListView lvScan;
        private ColumnHeader chPID;
        private ColumnHeader chProcess;
        private Button btnKill;
        private FolderBrowserDialog folderBrowserDialog;
        private Button btnCancel;
    }
}
