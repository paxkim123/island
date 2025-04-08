namespace Spot_News_API
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
            groupBox1 = new GroupBox();
            btnSend = new Button();
            cboxMethod = new ComboBox();
            tboxURL = new TextBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            tboxBody = new TextBox();
            groupBox3 = new GroupBox();
            lboxLog = new ListBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSend);
            groupBox1.Controls.Add(cboxMethod);
            groupBox1.Controls.Add(tboxURL);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(38, 22);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(895, 130);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "전송 위치";
            // 
            // btnSend
            // 
            btnSend.Location = new Point(389, 82);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(112, 34);
            btnSend.TabIndex = 4;
            btnSend.Text = "send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // cboxMethod
            // 
            cboxMethod.FormattingEnabled = true;
            cboxMethod.Location = new Point(127, 82);
            cboxMethod.Name = "cboxMethod";
            cboxMethod.Size = new Size(238, 33);
            cboxMethod.TabIndex = 3;
            cboxMethod.SelectedIndexChanged += cboxMethod_SelectedIndexChanged;
            // 
            // tboxURL
            // 
            tboxURL.Location = new Point(127, 37);
            tboxURL.Name = "tboxURL";
            tboxURL.Size = new Size(747, 31);
            tboxURL.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 82);
            label2.Name = "label2";
            label2.Size = new Size(77, 25);
            label2.TabIndex = 1;
            label2.Text = "Method";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 37);
            label1.Name = "label1";
            label1.Size = new Size(45, 25);
            label1.TabIndex = 0;
            label1.Text = "URL";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tboxBody);
            groupBox2.Location = new Point(38, 158);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(915, 233);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "body";
            // 
            // tboxBody
            // 
            tboxBody.Location = new Point(19, 30);
            tboxBody.Multiline = true;
            tboxBody.Name = "tboxBody";
            tboxBody.Size = new Size(876, 185);
            tboxBody.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lboxLog);
            groupBox3.Location = new Point(47, 397);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1133, 278);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "Log";
            // 
            // lboxLog
            // 
            lboxLog.FormattingEnabled = true;
            lboxLog.ItemHeight = 25;
            lboxLog.Location = new Point(10, 30);
            lboxLog.Name = "lboxLog";
            lboxLog.Size = new Size(1085, 204);
            lboxLog.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1240, 698);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Spot News API";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox tboxURL;
        private Label label2;
        private Label label1;
        private Button btnSend;
        private ComboBox cboxMethod;
        private GroupBox groupBox2;
        private TextBox tboxBody;
        private GroupBox groupBox3;
        private ListBox lboxLog;
    }
}
