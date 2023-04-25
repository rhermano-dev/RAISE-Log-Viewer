using System.Drawing;
using System.Windows.Forms;
using SequenceDiagramLib.Model;

namespace SequenceDiagramLib
{
	public partial class ActivationControl : UserControl
	{
		public ActivationControl()
		{
			InitializeComponent();
		}

		public void SetActivation(Activation activation)
		{
			this.Controls.Clear();

			int y = 10;
			foreach (Tag tag in activation.Tags)
			{
				Label label = new Label();
				label.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
				label.Text = tag.Key;
				label.Location = new Point(10, y);
				label.Size = new System.Drawing.Size(this.Size.Width - 20, 17);
				this.Controls.Add(label);
				y += 20;

				if (tag.Value.IndexOf('\n') > -1)
				{
					TextBox textBox = new TextBox();
					textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
					textBox.Text = tag.Value.Replace("\n", "\r\n");
					textBox.Location = new Point(10, y);
					textBox.Multiline = true;
					textBox.Size = new System.Drawing.Size(this.Size.Width - 20, 17 * 20);
					textBox.ScrollBars = ScrollBars.Both;
					textBox.WordWrap = false;
					this.Controls.Add(textBox);
					y += (17 * 20) + 13;
				}
				else
				{
					TextBox textBox = new TextBox();
					textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
					textBox.Text = tag.Value;
					textBox.Location = new Point(10, y);
					textBox.Size = new System.Drawing.Size(this.Size.Width - 20, 17);
					this.Controls.Add(textBox);
					y += 17 + 13;
				}
			}
		}
	}
}
