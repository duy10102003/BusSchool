namespace Test2
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
            txtFileText = new TextBox();
            listCal = new ListBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 30);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 0;
            label1.Text = "File Name: ";
            // 
            // txtFile
            // 
            txtFile.Location = new Point(111, 29);
            txtFile.Name = "txtFile";
            txtFile.Size = new Size(375, 23);
            txtFile.TabIndex = 1;
            // 
            // btnShow
            // 
            btnShow.Location = new Point(514, 30);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(75, 23);
            btnShow.TabIndex = 2;
            btnShow.Text = "SHOW";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Click += btnShow_Click;
            // 
            // txtFileText
            // 
            txtFileText.Location = new Point(84, 87);
            txtFileText.Name = "txtFileText";
            txtFileText.Size = new Size(606, 23);
            txtFileText.TabIndex = 3;
            // 
            // listCal
            // 
            listCal.FormattingEnabled = true;
            listCal.ItemHeight = 15;
            listCal.Location = new Point(106, 146);
            listCal.Name = "listCal";
            listCal.Size = new Size(283, 109);
            listCal.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listCal);
            Controls.Add(txtFileText);
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
        private TextBox txtFileText;
        private ListBox listCal;
    }
}
