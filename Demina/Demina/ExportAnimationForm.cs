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
	public partial class ExportAnimationForm : Form
	{
		public ExportAnimationForm()
		{
			InitializeComponent();
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				pathTextBox.Text = fbd.SelectedPath;
			}
		}

		FolderBrowserDialog fbd = new FolderBrowserDialog();

		private void okButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(pathTextBox.Text) &&
				!string.IsNullOrEmpty(nameTextBox.Text))
				this.DialogResult = DialogResult.OK;
			else
				MessageBox.Show("Please verify settings.");
		}
	}
}
