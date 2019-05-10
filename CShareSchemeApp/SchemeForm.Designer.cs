namespace CShareSchemeApp
{
    partial class SchemeForm
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
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.BackColor = System.Drawing.Color.Black;
            this.consoleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleTextBox.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleTextBox.ForeColor = System.Drawing.Color.White;
            this.consoleTextBox.Location = new System.Drawing.Point(0, 0);
            this.consoleTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.Size = new System.Drawing.Size(892, 529);
            this.consoleTextBox.TabIndex = 0;
            this.consoleTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.consoleTextBox_KeyDown);
            this.consoleTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.consoleTextBox_KeyUp);
            this.consoleTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.consoleTextBox_MouseDown);
            // 
            // SchemeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 529);
            this.Controls.Add(this.consoleTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SchemeForm";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Text = "Scheme语言交互界面";
            this.Load += new System.EventHandler(this.SchemeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox consoleTextBox;

    }
}