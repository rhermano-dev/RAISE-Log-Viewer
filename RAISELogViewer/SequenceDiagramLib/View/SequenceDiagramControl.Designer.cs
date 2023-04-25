using System;

namespace SequenceDiagramLib.View
{
	partial class SequenceDiagramControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.participantContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.enableBreakMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.participantContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// participantContextMenu
			// 
			this.participantContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.participantContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableBreakMenuItem});
			this.participantContextMenu.Name = "participantContextMenu";
			this.participantContextMenu.Size = new System.Drawing.Size(165, 28);
			// 
			// enableBreakMenuItem
			// 
			this.enableBreakMenuItem.Name = "enableBreakMenuItem";
			this.enableBreakMenuItem.Size = new System.Drawing.Size(164, 24);
			this.enableBreakMenuItem.Text = "Enable Break";
			this.enableBreakMenuItem.Click += new System.EventHandler(this.enableBreakMenuItem_Click);
			// 
			// SequenceDiagramControl
			// 
			this.Size = new System.Drawing.Size(448, 330);
			//this.Click += new System.EventHandler(this.SequenceDiagramControl_Click);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SequenceDiagramControl_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SequenceDiagramControl_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SequenceDiagramControl_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SequenceDiagramControl_MouseUp);
			this.Resize += new System.EventHandler(this.SequenceDiagramControl_Resize);
			this.participantContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip participantContextMenu;
		private System.Windows.Forms.ToolStripMenuItem enableBreakMenuItem;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
	}
}
