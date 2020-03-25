namespace WinFormExpl
{
	partial class InputDialog
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
            this.tPath = new System.Windows.Forms.TextBox();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tPath
            // 
            this.tPath.AccessibleName = "tPath";
            this.tPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tPath.Location = new System.Drawing.Point(40, 69);
            this.tPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tPath.Multiline = true;
            this.tPath.Name = "tPath";
            this.tPath.Size = new System.Drawing.Size(277, 33);
            this.tPath.TabIndex = 0;
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOk.Location = new System.Drawing.Point(40, 125);
            this.bOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(80, 39);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(222, 125);
            this.bCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(93, 39);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InputDialog
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(352, 181);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.tPath);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "InputDialog";
            this.Text = "InputDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tPath;
		private System.Windows.Forms.Button bOk;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Label label1;
	}
}