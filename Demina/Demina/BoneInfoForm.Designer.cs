namespace Demina
{
	partial class BoneInfoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.texturePathTextBox = new System.Windows.Forms.TextBox();
            this.parentTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.insertTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.beforeRadioButton = new System.Windows.Forms.RadioButton();
            this.afterRadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Parent";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(79, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(100, 20);
            this.nameTextBox.TabIndex = 0;
            // 
            // texturePathTextBox
            // 
            this.texturePathTextBox.Location = new System.Drawing.Point(79, 90);
            this.texturePathTextBox.Name = "texturePathTextBox";
            this.texturePathTextBox.ReadOnly = true;
            this.texturePathTextBox.Size = new System.Drawing.Size(351, 20);
            this.texturePathTextBox.TabIndex = 3;
            this.texturePathTextBox.TabStop = false;
            // 
            // parentTextBox
            // 
            this.parentTextBox.Location = new System.Drawing.Point(79, 38);
            this.parentTextBox.Name = "parentTextBox";
            this.parentTextBox.Size = new System.Drawing.Size(100, 20);
            this.parentTextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Texture Path";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(436, 88);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(53, 23);
            this.browseButton.TabIndex = 4;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(169, 118);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(250, 118);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // insertTextBox
            // 
            this.insertTextBox.Location = new System.Drawing.Point(79, 64);
            this.insertTextBox.Name = "insertTextBox";
            this.insertTextBox.Size = new System.Drawing.Size(100, 20);
            this.insertTextBox.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Insert";
            // 
            // beforeRadioButton
            // 
            this.beforeRadioButton.AutoSize = true;
            this.beforeRadioButton.Checked = true;
            this.beforeRadioButton.Location = new System.Drawing.Point(185, 65);
            this.beforeRadioButton.Name = "beforeRadioButton";
            this.beforeRadioButton.Size = new System.Drawing.Size(58, 17);
            this.beforeRadioButton.TabIndex = 3;
            this.beforeRadioButton.TabStop = true;
            this.beforeRadioButton.Text = "Behind";
            this.beforeRadioButton.UseVisualStyleBackColor = true;
            this.beforeRadioButton.CheckedChanged += new System.EventHandler(this.beforeRadioButton_CheckedChanged);
            // 
            // afterRadioButton
            // 
            this.afterRadioButton.AutoSize = true;
            this.afterRadioButton.Location = new System.Drawing.Point(247, 65);
            this.afterRadioButton.Name = "afterRadioButton";
            this.afterRadioButton.Size = new System.Drawing.Size(56, 17);
            this.afterRadioButton.TabIndex = 3;
            this.afterRadioButton.Text = "Ahead";
            this.afterRadioButton.UseVisualStyleBackColor = true;
            this.afterRadioButton.CheckedChanged += new System.EventHandler(this.afterRadioButton_CheckedChanged);
            // 
            // BoneInfoForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(495, 153);
            this.Controls.Add(this.afterRadioButton);
            this.Controls.Add(this.beforeRadioButton);
            this.Controls.Add(this.insertTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.parentTextBox);
            this.Controls.Add(this.texturePathTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BoneInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bone Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		public System.Windows.Forms.TextBox nameTextBox;
		public System.Windows.Forms.TextBox texturePathTextBox;
		public System.Windows.Forms.TextBox parentTextBox;
		public System.Windows.Forms.TextBox insertTextBox;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.RadioButton beforeRadioButton;
		public System.Windows.Forms.RadioButton afterRadioButton;
        public System.Windows.Forms.Button browseButton;
	}
}