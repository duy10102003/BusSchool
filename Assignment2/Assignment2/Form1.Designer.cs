namespace Assignment2
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
            label1 = new Label();
            txtFile = new TextBox();
            btnShow = new Button();
            btnExit = new Button();
            listUser = new ListBox();
            btnSort = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 23);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 0;
            label1.Text = "File name: ";
            // 
            // txtFile
            // 
            txtFile.Location = new Point(94, 15);
            txtFile.Name = "txtFile";
            txtFile.Size = new Size(310, 23);
            txtFile.TabIndex = 1;
            // 
            // btnShow
            // 
            btnShow.Location = new Point(429, 15);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(75, 23);
            btnShow.TabIndex = 2;
            btnShow.Text = "SHOW";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += btnShow_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(524, 14);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(75, 23);
            btnExit.TabIndex = 3;
            btnExit.Text = "EXIT";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // listUser
            // 
            listUser.FormattingEnabled = true;
            listUser.ItemHeight = 15;
            listUser.Location = new Point(48, 72);
            listUser.Name = "listUser";
            listUser.Size = new Size(536, 274);
            listUser.TabIndex = 4;
            // 
            // btnSort
            // 
            btnSort.Location = new Point(48, 369);
            btnSort.Name = "btnSort";
            btnSort.Size = new Size(75, 23);
            btnSort.TabIndex = 5;
            btnSort.Text = "SORT";
            btnSort.UseVisualStyleBackColor = true;
            btnSort.Click += btnSort_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSort);
            Controls.Add(listUser);
            Controls.Add(btnExit);
            Controls.Add(btnShow);
            Controls.Add(txtFile);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtFile;
        private Button btnShow;
        private Button btnExit;
        private ListBox listUser;
        private Button btnSort;
    }
}
