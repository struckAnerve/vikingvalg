namespace Demina
{
	partial class AnimationPacker
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
            this.filesListBox = new System.Windows.Forms.ListBox();
            this.buildButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textureTextBox = new System.Windows.Forms.TextBox();
            this.textureButton = new System.Windows.Forms.Button();
            this.dictionaryButton = new System.Windows.Forms.Button();
            this.dictionaryTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.addFilesDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // filesListBox
            // 
            this.filesListBox.FormattingEnabled = true;
            this.filesListBox.Location = new System.Drawing.Point(12, 12);
            this.filesListBox.Name = "filesListBox";
            this.filesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.filesListBox.Size = new System.Drawing.Size(415, 264);
            this.filesListBox.TabIndex = 0;
            // 
            // buildButton
            // 
            this.buildButton.Location = new System.Drawing.Point(352, 396);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(75, 23);
            this.buildButton.TabIndex = 1;
            this.buildButton.Text = "Build";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 343);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Texture";
            // 
            // textureTextBox
            // 
            this.textureTextBox.Location = new System.Drawing.Point(72, 340);
            this.textureTextBox.Name = "textureTextBox";
            this.textureTextBox.ReadOnly = true;
            this.textureTextBox.Size = new System.Drawing.Size(274, 20);
            this.textureTextBox.TabIndex = 3;
            // 
            // textureButton
            // 
            this.textureButton.Location = new System.Drawing.Point(352, 338);
            this.textureButton.Name = "textureButton";
            this.textureButton.Size = new System.Drawing.Size(75, 23);
            this.textureButton.TabIndex = 4;
            this.textureButton.Text = "Browse ...";
            this.textureButton.UseVisualStyleBackColor = true;
            this.textureButton.Click += new System.EventHandler(this.textureButton_Click);
            // 
            // dictionaryButton
            // 
            this.dictionaryButton.Location = new System.Drawing.Point(352, 367);
            this.dictionaryButton.Name = "dictionaryButton";
            this.dictionaryButton.Size = new System.Drawing.Size(75, 23);
            this.dictionaryButton.TabIndex = 7;
            this.dictionaryButton.Text = "Browse ...";
            this.dictionaryButton.UseVisualStyleBackColor = true;
            this.dictionaryButton.Click += new System.EventHandler(this.dictionaryButton_Click);
            // 
            // dictionaryTextBox
            // 
            this.dictionaryTextBox.Location = new System.Drawing.Point(72, 369);
            this.dictionaryTextBox.Name = "dictionaryTextBox";
            this.dictionaryTextBox.ReadOnly = true;
            this.dictionaryTextBox.Size = new System.Drawing.Size(274, 20);
            this.dictionaryTextBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 372);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Dictionary";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(352, 282);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 8;
            this.addButton.Text = "Add ...";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(271, 282);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 9;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // addFilesDialog
            // 
            this.addFilesDialog.Filter = "Demina Anim files|*.anim";
            this.addFilesDialog.Multiselect = true;
            // 
            // AnimationPacker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 428);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.dictionaryButton);
            this.Controls.Add(this.dictionaryTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textureButton);
            this.Controls.Add(this.textureTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buildButton);
            this.Controls.Add(this.filesListBox);
            this.Name = "AnimationPacker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AnimationPacker";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox filesListBox;
		private System.Windows.Forms.Button buildButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textureTextBox;
		private System.Windows.Forms.Button textureButton;
		private System.Windows.Forms.Button dictionaryButton;
		private System.Windows.Forms.TextBox dictionaryTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.OpenFileDialog addFilesDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
	}
}