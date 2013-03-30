using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demina
{
	public partial class BoneInfoForm : Form
	{
		public BoneInfoForm()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(texturePathTextBox.Text) &&
				!string.IsNullOrEmpty(nameTextBox.Text) &&
				(insertTextBox.ReadOnly || !string.IsNullOrEmpty(insertTextBox.Text)))
				this.DialogResult = DialogResult.OK;
			else
				MessageBox.Show("Please verify settings.");
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			ofd.Filter = "PNG files|*.png";

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				texturePathTextBox.Text = ofd.FileName;
			}
		}

		OpenFileDialog ofd = new OpenFileDialog();

		private void beforeRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			afterRadioButton.Checked = !beforeRadioButton.Checked;
		}

		private void afterRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			beforeRadioButton.Checked = !afterRadioButton.Checked;
		}
	}
}
