using System;
using System.Drawing;
using System.Windows.Forms;
using SequenceDiagramLib.Model;
using System.Collections.Generic;
using SequenceDiagramLib.Presenter;
using System.IO;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SequenceDiagramLib.View
{
	public partial class SequenceDiagramControl : Panel, ISequenceDiagramView
	{
		private ISequenceDiagramPresenter presenter = null;
		public Sequence Sequence = null;

		public event TickDelegate OnTick;

		private Size size = new Size(0, 0);

		private int xMargin = 50;
		private int yMargin = 10;

		private int timestepHeight = 20;

		private int swimlaneWidth = 150;
		private int participantHeight = 40;
		private int activationWidth = 20;

		private bool drawGrid = true;
		private bool drawTextBounds = false;

		public List<Zone> zones = new List<Zone>();
        public List<Zone> timeZones = new List<Zone>();
        private Activation activeActivation = null;

		private int fontHeight = 0;
		private int boxNameHeight = 0;

		public SequenceDiagramControl()
		{
			InitializeComponent();

			this.BackColor = Color.White;
			this.DoubleBuffered = true;

			this.toolTip.ShowAlways = true;
			this.toolTip.SetToolTip(this, "-");

			this.presenter = new SequenceDiagramPresenter(this);
			this.Sequence = this.presenter.GetModel().GetSequence();
		}

		public void SetPresenter(ISequenceDiagramPresenter presenter)
		{
			this.presenter = presenter;
		}

		public int SwimlaneWidth
		{
			get
			{
				return this.swimlaneWidth;
			}
			set
			{
				this.swimlaneWidth = value;
			}
		}

		#region Resources
		//private Font font = null;
		private Font underlinedFont = null;
		private Font smallFont = null;

		private SolidBrush blackBrush = null;
		private SolidBrush darkGrayBrush = null;
		private SolidBrush lightGrayBrush = null;
		private SolidBrush redBrush = null;
		private SolidBrush yellowBrush = null;
		private SolidBrush whiteBrush = null;

		private Pen blackPen = null;
		private Pen bluePen = null;
		private Pen redPen = null;
		private Pen grayPen = null;
		private Pen dashedPen = null;

		private StringFormat stringFormat = null;

		private Font Font_
		{
			get
			{
				return this.Font;
				/*
				if (this.font == null)
					this.font = new Font("Arial", 10);

				return this.font;
				*/
			}
		}

		private Font SmallFont
		{
			get
			{
				if (this.smallFont == null)
					this.smallFont = new Font("Arial", 8);

				return this.smallFont;
			}
		}

		private Font UnderlinedFont
		{
			get
			{
				if (this.underlinedFont == null)
				{
					this.underlinedFont = new Font(
						this.Font.Name, this.Font.Size,
						FontStyle.Underline, this.Font.Unit,
						this.Font.GdiCharSet, this.Font.GdiVerticalFont);
				}
				return this.underlinedFont;
			}
		}

		private SolidBrush BlackBrush
		{
			get
			{
				if (this.blackBrush == null)
					this.blackBrush = new SolidBrush(Color.Black);

				return this.blackBrush;
			}
		}

		private SolidBrush DarkGrayBrush
		{
			get
			{
				if (this.darkGrayBrush == null)
					this.darkGrayBrush = new SolidBrush(Color.DimGray);

				return this.darkGrayBrush;
			}
		}

		private SolidBrush LightGrayBrush
		{
			get
			{
				if (this.lightGrayBrush == null)
					this.lightGrayBrush = new SolidBrush(Color.LightGray);

				return this.lightGrayBrush;
			}
		}

		private SolidBrush RedBrush
		{
			get
			{
				if (this.redBrush == null)
					this.redBrush = new SolidBrush(Color.Red);

				return this.redBrush;
			}
		}

		private SolidBrush YellowBrush
		{
			get
			{
				if (this.yellowBrush == null)
					this.yellowBrush = new SolidBrush(Color.Yellow);

				return this.yellowBrush;
			}
		}

		private SolidBrush WhiteBrush
		{
			get
			{
				if (this.whiteBrush == null)
					this.whiteBrush = new SolidBrush(Color.White);

				return this.whiteBrush;
			}
		}

		private Pen BlackPen
		{
			get
			{
				if (this.blackPen == null)
					this.blackPen = new Pen(Color.Black);

				return this.blackPen;
			}
		}

		private Pen BluePen
		{
			get
			{
				if (this.bluePen == null)
					this.bluePen = new Pen(Color.Blue);

				return this.bluePen;
			}
		}

		private Pen RedPen
		{
			get
			{
				if (this.redPen == null)
					this.redPen = new Pen(Color.Red);

				return this.redPen;
			}
		}

		private Pen GrayPen
		{
			get
			{
				if (this.grayPen == null)
					this.grayPen = new Pen(Color.Gainsboro);

				return this.grayPen;
			}
		}

		private Pen DashedPen
		{
			get
			{
				if (this.dashedPen == null)
				{
					this.dashedPen = new Pen(Color.Black);
					this.dashedPen.DashPattern = new float[] { 5.0F, 5.0F };
				}

				return this.dashedPen;
			}
		}

		private StringFormat StringFormat
		{
			get
			{
				if (this.stringFormat == null)
				{
					this.stringFormat = new StringFormat();
					this.stringFormat.Trimming = StringTrimming.EllipsisCharacter;
					this.stringFormat.LineAlignment = StringAlignment.Near;
					this.stringFormat.Alignment = StringAlignment.Center;
				}

				return this.stringFormat;
			}
		}
		#endregion

		public void Close()
		{

		}

		#region Callbacks
		public delegate void SelectActivationDelegate(Activation activation);

		public event SelectActivationDelegate OnSelectActivation;
		#endregion

		#region Thread-Safe Methods
		delegate void PaintCallback(PaintEventArgs e);
		delegate void RecalSizeCallback();

		private void PaintTs(PaintEventArgs e)
		{
			if (this.InvokeRequired)
			{
				PaintCallback d = new PaintCallback(Paint_);
				this.Invoke(d, new object[] { e });
			}
			else
				Paint_(e);
		}

		private void RecalcSizeTs()
		{
			if (this.InvokeRequired)
			{
				RecalSizeCallback d = new RecalSizeCallback(RecalcSize);
				this.Invoke(d, new object[] { });
			}
			else
				RecalcSize();
		}
		#endregion

		public void ToggleGrid()
		{
			this.drawGrid = !this.drawGrid;
			Invalidate();
		}

		public void Clear()
		{
			Sequence sequence = this.presenter.GetModel().GetSequence();
			RecalcSize();
		}

		private void Paint_(PaintEventArgs e)
		{
			if (this.presenter == null)
				return;

			this.fontHeight = this.Font.Height;
			this.boxNameHeight = 0;

			Sequence sequence = this.presenter.GetModel().GetSequence();

			lock (sequence.Lock)
			{
				e.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

				foreach (Box box in sequence.Boxes)
				{
					if (box.Name != "")
						this.boxNameHeight = 20;
				}

				foreach (Box box in sequence.Boxes)
					DrawBox(box, e);

                this.timeZones.Clear();
                if (this.drawGrid)
					DrawGridLines(e);

				this.zones.Clear();

                foreach (Participant participant in sequence.Participants)
					DrawParticipant(participant, e);

				foreach (Model.Message message in sequence.Messages)
					DrawMessage(message, e);
			}
		}

		public void DrawGridLines(PaintEventArgs e)
		{
			Sequence sequence = this.presenter.GetModel().GetSequence();

			Point p0 = new Point(0, this.yMargin + this.boxNameHeight + this.participantHeight + this.timestepHeight);

			for (int i = 0; i < sequence.Timestep; i++)
			{
				if (i > 0)
				{
					//TimeSpan timeSpan = sequence.Timesteps[i] - sequence.Timesteps[i - 1];

					string time = sequence.TimeLine[i];
					e.Graphics.DrawString(time, this.SmallFont, this.BlackBrush,
						p0.X, p0.Y + (i * this.timestepHeight) - 16);

                    Size size = new Size(this.swimlaneWidth, this.participantHeight);
                    Rectangle rect = new Rectangle(p0.X, p0.Y + (i * this.timestepHeight) - 16, size.Width, size.Height);

                    this.timeZones.Add(new Zone(rect, time, ""));
				}

				e.Graphics.DrawLine(this.GrayPen,
					p0.X, p0.Y + i * this.timestepHeight,
					this.size.Width, p0.Y + i * this.timestepHeight);
			}
			e.Graphics.FillRectangle(this.RedBrush,
				p0.X, p0.Y + ((sequence.Timestep) * this.timestepHeight),
				this.size.Width, 2);
		}

		private void DrawBox(Box box, PaintEventArgs e)
		{
			if (box.MinIndex == -1)
				return;

			Sequence sequence = box.Sequence;

			Point p0 = new Point(this.xMargin + (box.MinIndex * this.swimlaneWidth) - 5, 5);

			if (box.Name != "")
			{
				e.Graphics.DrawString(box.Name, this.Font_, this.BlackBrush,
					this.xMargin + (box.MinIndex * this.swimlaneWidth), 10);
			}
			p0.Y += this.boxNameHeight;

			int width = (box.MaxIndex - box.MinIndex + 1) * this.swimlaneWidth;
			int height = (this.yMargin + this.participantHeight + 20) + (sequence.Timestep * this.timestepHeight);

			SolidBrush brush = new SolidBrush(box.Color);
			e.Graphics.FillRectangle(brush, p0.X, p0.Y, width, height);

			e.Graphics.DrawRectangle(this.BlackPen, p0.X, p0.Y, width, height);
		}

		private void DrawParticipant(Participant participant, PaintEventArgs e)
		{
			Sequence sequence = participant.Sequence;

			Point p0 = new Point(
				this.xMargin + (participant.Index * this.swimlaneWidth) + (this.activationWidth / 2) + 15,
				this.yMargin + this.boxNameHeight + this.participantHeight);

			if (participant.CreationTimestep > 0)
				p0.Y += (this.timestepHeight / 2) + (participant.CreationTimestep + 1) * this.timestepHeight;

			{
				Point pLifeline = new Point(p0.X, p0.Y);

				int timestep = 0;
				int yCorrection = 0;

				if (participant.CreationTimestep == 0)
					timestep = sequence.Timestep + 1;
				else
				{
					timestep = sequence.Timestep - participant.CreationTimestep;
					yCorrection = 10;
				}

				e.Graphics.DrawLine(this.DashedPen,
					pLifeline.X, pLifeline.Y,
					pLifeline.X, pLifeline.Y + (timestep * this.timestepHeight) - yCorrection);
			}

			foreach (Activation activation in participant.Activations)
				DrawActivation(activation, e);

			{
				Point pTop = new Point(p0.X - (this.activationWidth / 2), p0.Y - this.participantHeight);
				Size size = new Size(this.swimlaneWidth, this.participantHeight);
				Rectangle rect = new Rectangle(pTop.X, pTop.Y, size.Width, size.Height);

				{
					if (this.drawTextBounds)
					{
						e.Graphics.FillRectangle(this.YellowBrush, rect);
						e.Graphics.DrawRectangle(this.BlackPen, rect);
					}
				}

				Brush brush = new SolidBrush(participant.Color);
				Brush textBrush = new SolidBrush(participant.TextColor);

				Font font = null;
				if (participant.Underlined)
					font = this.UnderlinedFont;
				else
					font = this.Font;

				if (participant.Type == EParticipantType.Box)
				{
					e.Graphics.FillRectangle(brush, new Rectangle(pTop.X, p0.Y - 30, size.Width - 10, 30));
					e.Graphics.DrawRectangle(this.BlackPen, new Rectangle(pTop.X, p0.Y - 30, size.Width - 10, 30));

					StringFormat stringFormat = new StringFormat();
					stringFormat.Alignment = StringAlignment.Near;
					stringFormat.LineAlignment = StringAlignment.Center;

					e.Graphics.DrawString(participant.Name, font, textBrush,
						new Rectangle(pTop.X + 3, p0.Y - 24, size.Width - 6, 24), stringFormat);
				}
				else if (participant.Type == EParticipantType.Actor)
				{
					e.Graphics.FillEllipse(brush, p0.X - 6, p0.Y - 30, 12, 12);
					e.Graphics.DrawEllipse(this.BlackPen, p0.X - 6, p0.Y - 30, 12, 12);

					e.Graphics.DrawLine(this.BlackPen, p0.X, p0.Y - 18, p0.X, p0.Y - 6);

					e.Graphics.DrawLine(this.BlackPen, p0.X - 6, p0.Y, p0.X, p0.Y - 6);
					e.Graphics.DrawLine(this.BlackPen, p0.X + 6, p0.Y, p0.X, p0.Y - 6);

					e.Graphics.DrawLine(this.BlackPen, p0.X - 6, p0.Y - 12, p0.X + 6, p0.Y - 12);

					e.Graphics.DrawString(participant.Name, font, this.BlackBrush, p0.X + 8, p0.Y - 16);
				}
				else if (participant.Type == EParticipantType.Boundary)
				{
					e.Graphics.DrawLine(this.BlackPen, p0.X - 10, p0.Y - 20, p0.X - 10, p0.Y);

					e.Graphics.DrawLine(this.BlackPen, p0.X - 10, p0.Y - 10, p0.X, p0.Y - 10);

					e.Graphics.FillEllipse(brush, p0.X, p0.Y - 20, 20, 20);
					e.Graphics.DrawEllipse(this.BlackPen, p0.X, p0.Y - 20, 20, 20);

					e.Graphics.DrawString(participant.Name, font, this.BlackBrush, p0.X + 24, p0.Y - 16);
				}
				else if (participant.Type == EParticipantType.Control)
				{
					e.Graphics.FillEllipse(brush, p0.X - 10, p0.Y - 20, 20, 20);
					e.Graphics.DrawEllipse(this.BlackPen, p0.X - 10, p0.Y - 20, 20, 20);

					e.Graphics.DrawLine(this.BlackPen, p0.X - 2, p0.Y - 20, p0.X + 2, p0.Y - 24);

					e.Graphics.DrawLine(this.BlackPen, p0.X - 2, p0.Y - 20, p0.X + 2, p0.Y - 16);

					e.Graphics.DrawString(participant.Name, font, this.BlackBrush, p0.X + 15, p0.Y - 16);
				}
				else if (participant.Type == EParticipantType.Entity)
				{
					e.Graphics.FillEllipse(brush, p0.X - 10, p0.Y - 20, 20, 20);
					e.Graphics.DrawEllipse(this.BlackPen, p0.X - 10, p0.Y - 20, 20, 20);

					e.Graphics.DrawLine(this.BlackPen, p0.X - 10, p0.Y, p0.X + 10, p0.Y);

					e.Graphics.DrawString(participant.Name, font, this.BlackBrush, p0.X + 15, p0.Y - 16);
				}
				else if (participant.Type == EParticipantType.Database)
				{
					e.Graphics.FillRectangle(brush, p0.X - 10, p0.Y - 20, 22, 15);
					e.Graphics.DrawRectangle(this.BlackPen, p0.X - 10, p0.Y - 20, 22, 15);

					e.Graphics.FillEllipse(brush, p0.X - 10, p0.Y - 25, 22, 10);
					e.Graphics.DrawEllipse(this.BlackPen, p0.X - 10, p0.Y - 25, 22, 10);

					e.Graphics.FillEllipse(brush, p0.X - 10, p0.Y - 10, 22, 10);
					e.Graphics.DrawEllipse(this.BlackPen, p0.X - 10, p0.Y - 10, 22, 10);

					e.Graphics.FillRectangle(brush, p0.X - 9, p0.Y - 10, 21, 5);

					e.Graphics.DrawString(participant.Name, font, this.BlackBrush, p0.X + 17, p0.Y - 16);
				}
				else if (participant.Type == EParticipantType.Collections)
				{
					e.Graphics.FillRectangle(brush, p0.X - 5, p0.Y - 25, 26, 20);
					e.Graphics.DrawRectangle(this.BlackPen, p0.X - 5, p0.Y - 25, 26, 20);

					e.Graphics.FillRectangle(brush, p0.X - 10, p0.Y - 20, 26, 20);
					e.Graphics.DrawRectangle(this.BlackPen, p0.X - 10, p0.Y - 20, 26, 20);

					e.Graphics.DrawString(participant.Name, font, this.BlackBrush, p0.X + 26, p0.Y - 16);
				}

				{
					string str = "PARTICIPANT:" + participant.Name + "\r\n"
						+ "Break:" + participant.Break;

					Zone zone = new Zone(rect, str, "");
					zone.Participant = participant;
					this.zones.Add(zone);
				}
			}

			if (this.drawTextBounds)
				e.Graphics.DrawEllipse(this.RedPen, p0.X - 5, p0.Y - 5, 10, 10);
		}

		private void DrawActivation(Activation activation, PaintEventArgs e)
		{
			Participant participant = activation.Participant;
			Sequence sequence = participant.Sequence;

			int maxIndex = -1;
			{
				if (activation.Closed == false)
					maxIndex = sequence.Timestep;
				else
					maxIndex = activation.EndTimestep;
			}

			Point p0 = new Point(
				this.xMargin + (participant.Index * this.swimlaneWidth) + (activation.Index * 10),
				(this.yMargin + this.boxNameHeight + this.participantHeight + 20) + (activation.BeginTimestep * this.timestepHeight));

			int width = this.activationWidth;
			int height = (maxIndex - activation.BeginTimestep) * this.timestepHeight;

			Rectangle rect = new Rectangle(p0.X, p0.Y, width, height);

			{
				if (this.drawTextBounds)
					e.Graphics.FillRectangle(this.YellowBrush, p0.X, p0.Y, width, height);
				e.Graphics.DrawString(activation.Name, this.Font_, this.BlackBrush, rect);
			}

			{
				Zone zone = new Zone(rect, "SPAN_ID:" + activation.Id, "");
				zone.ActivationBox = activation;
				this.zones.Add(zone);

				Brush brush = new SolidBrush(activation.Color);

				if (activation == this.activeActivation)
					brush = this.DarkGrayBrush;

				e.Graphics.FillRectangle(brush, rect);
			}

			if (activation.Closed)
				e.Graphics.DrawRectangle(this.BlackPen, p0.X, p0.Y, width, height);
			else
			{
				e.Graphics.DrawLine(this.RedPen, p0.X, p0.Y, p0.X + width, p0.Y);
				e.Graphics.DrawLine(this.RedPen, p0.X, p0.Y, p0.X, p0.Y + height);
				e.Graphics.DrawLine(this.RedPen, p0.X + width, p0.Y, p0.X + width, p0.Y + height);
			}
		}

		public void DrawMessage(Model.Message message, PaintEventArgs e)
		{
			Point p0 = new Point(0, this.yMargin + this.boxNameHeight + this.participantHeight + 20);

			SolidBrush brush = new SolidBrush(message.Color);
			Pen pen = new Pen(brush);
			pen.DashStyle = message.DashStyle;

			if (message.From == message.To)
			{
				Point tail = new Point(
					this.xMargin + (message.From.Index * this.swimlaneWidth),
					p0.Y + (message.Timestep * this.timestepHeight) - 10);

				{
					int level = GetLevel(message.From, message.Timestep, true);
					tail.X += (this.activationWidth / 2) + (level * (this.activationWidth / 2));
				}

				Point p1 = new Point(tail.X + 30, tail.Y);
				Point p2 = new Point(tail.X + 30, tail.Y + 10);

				Point head = new Point(
					this.xMargin + (message.From.Index * this.swimlaneWidth),
					tail.Y + 10);

				{
					int level = GetLevel(message.From, message.Timestep, false);
					head.X += (this.activationWidth / 2) + (level * (this.activationWidth / 2));
				}

				e.Graphics.DrawLine(pen, tail, p1);
				e.Graphics.DrawLine(pen, p1, p2);
				e.Graphics.DrawLine(pen, p2, head);

				{
					Point[] arrow = new Point[3];
					arrow[0] = head;
					arrow[1] = new Point(head.X + 8, head.Y - 6);
					arrow[2] = new Point(head.X + 8, head.Y + 6);
					e.Graphics.FillPolygon(brush, arrow);
				}

				{
					Rectangle textRect = new Rectangle(tail.X + 35, tail.Y - 3,
						this.swimlaneWidth - this.activationWidth - 25 - 10, 16);

					if (this.drawTextBounds)
						e.Graphics.FillRectangle(this.YellowBrush, textRect);

					e.Graphics.DrawString(message.Name, this.Font_, this.BlackBrush, textRect);
					this.zones.Add(new Zone(textRect, message.Name, message.FromTo));
				}
			}
			else
			{
				bool leftToRight = false;

				{
					if (message.From.Index < message.To.Index)
						leftToRight = true;
				}

				{
					{
						if (leftToRight)
						{
							Point tail = new Point(this.xMargin + (message.From.Index * this.swimlaneWidth) + 15,
								p0.Y + (message.Timestep * this.timestepHeight));

							{
								int level = GetMaxLevel(message.From, message.Timestep);
								tail.X += (this.activationWidth / 2) + (level * (this.activationWidth / 2));
							}

							Point head = new Point(this.xMargin + (message.To.Index * this.swimlaneWidth) + 15, tail.Y);

							if ((message.Timestep > 0) && (message.Timestep == message.To.CreationTimestep))
							{ }
							else
							{
								int level = GetMaxLevel(message.To, message.Timestep);
								if (level == 0)
									head.X += this.activationWidth / 2;
							}

							{
								e.Graphics.DrawLine(pen, tail, head);
							}

							{
								Point[] arrow = new Point[3];
								arrow[0] = head;
								arrow[1] = new Point(head.X - 8, head.Y - 6);
								arrow[2] = new Point(head.X - 8, head.Y + 6);
								e.Graphics.FillPolygon(brush, arrow);
							}

							{
								Rectangle textRect = new Rectangle(
									tail.X + 10, head.Y - this.fontHeight, head.X - tail.X - 20, this.fontHeight);

								if (this.drawTextBounds)
									e.Graphics.FillRectangle(this.YellowBrush, textRect);

								e.Graphics.DrawString(message.Name, this.Font_, this.BlackBrush, textRect, this.StringFormat);
								this.zones.Add(new Zone(textRect, message.Name, message.FromTo));
							}
						}
						else
						{
							Point tail = new Point(this.xMargin + (message.From.Index * this.swimlaneWidth) + 15,
								p0.Y + (message.Timestep * this.timestepHeight));

							{
								int level = GetMaxLevel(message.From, message.Timestep);
								if (level == 0)
									tail.X += this.activationWidth / 2;
							}

							Point head = new Point(this.xMargin + (message.To.Index * this.swimlaneWidth) + 15, tail.Y);

							{
								int level = GetMaxLevel(message.To, message.Timestep);
								head.X += (this.activationWidth / 2) + (level * (this.activationWidth / 2));
								/*
								if (level == 0)
									head.X += this.activationWidth / 2;
								else
									head.X += (this.activationWidth / 2) + (level * (this.activationWidth / 2));
								*/
							}

							{
								e.Graphics.DrawLine(pen, head, tail);
							}

							{
								Point[] arrow = new Point[3];
								arrow[0] = head;
								arrow[1] = new Point(head.X + 8, head.Y - 6);
								arrow[2] = new Point(head.X + 8, head.Y + 6);
								e.Graphics.FillPolygon(brush, arrow);
							}

							{
								Rectangle textRect = new Rectangle(
									head.X + 10, head.Y - this.fontHeight, tail.X - head.X - 20, this.fontHeight);

								if (this.drawTextBounds)
									e.Graphics.FillRectangle(this.YellowBrush, textRect);

								e.Graphics.DrawString(message.Name, this.Font_, this.BlackBrush, textRect, this.StringFormat);
								this.zones.Add(new Zone(textRect, message.Name, message.FromTo));
							}
						}
					}
				}
			}
		}

		private void RecalcSize()
		{
			if (this.presenter == null)
				return;

			Sequence sequence = this.presenter.GetModel().GetSequence();

			int width = this.xMargin + (sequence.Participants.Count * this.swimlaneWidth);
			if (width < this.ClientSize.Width)
				width = this.ClientSize.Width;

			int height = this.yMargin + this.boxNameHeight + this.participantHeight + 20 + ((sequence.Timestep + 1) * this.timestepHeight) + 20;

			this.size = new Size(width, height);

			this.AutoScrollMinSize = this.size;
			this.AutoScrollPosition = new Point(0, this.AutoScrollMinSize.Height);

			Invalidate();
		}

		private void SequenceDiagramControl_Paint(object sender, PaintEventArgs e)
		{
			PaintTs(e);
		}

		#region
		public void Tick()
		{
			RecalcSizeTs();
			Invalidate();

			if (this.OnTick != null)
				this.OnTick();
		}
		#endregion

		private void SequenceDiagramControl_Resize(object sender, EventArgs e)
		{
			RecalcSizeTs();
		}

		private Point grabPoint;

		private void SequenceDiagramControl_MouseDown(object sender, MouseEventArgs e)
		{
			this.grabPoint = e.Location;
		}

		private void SequenceDiagramControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				int deltaX = e.X - this.grabPoint.X;
				int deltaY = e.Y - this.grabPoint.Y;

				this.AutoScrollPosition = new Point(
					-this.AutoScrollPosition.X - deltaX,
					-this.AutoScrollPosition.Y - deltaY);

				this.grabPoint = e.Location;
			}
			else
			{
				string description = "";

				Point controlPoint = this.PointToClient(Cursor.Position);
				controlPoint.Offset(-this.AutoScrollPosition.X, -this.AutoScrollPosition.Y);

				for (int i = this.zones.Count - 1; i >= 0; i--)
				{
					Zone zone = this.zones[i];

					if (zone.Location.Contains(controlPoint))
					{
						description = zone.FromTo;
						break;
					}
				}
				this.toolTip.SetToolTip(this, description);
			}
		}

		private void SequenceDiagramControl_MouseUp(object sender, MouseEventArgs e)
		{
			this.grabPoint = Point.Empty;
		}

		public Point SequenceDiagramControl_Click(object sender, EventArgs e)
		{
            MouseEventArgs mouseEventArgs = (MouseEventArgs)e;

            Point controlPoint = new Point();
            
            if (mouseEventArgs.Button == MouseButtons.Left)
            {
                controlPoint = this.PointToClient(Cursor.Position);
                controlPoint.Offset(-this.AutoScrollPosition.X, -this.AutoScrollPosition.Y);

                /*for (int i = this.zones.Count - 1; i >= 0; i--)
                {
                    Zone zone = this.zones[i];

                    if (zone.Location.Contains(controlPoint))
                    {
                        if (zone.Participant == null)
                        {
							//string j = list.Where(x => x.Time == zone.Description).FirstOrDefault().JsonString;
							*//*string j = list[i - participantCount].JsonString;
							string jsonFormatted = JValue.Parse(j).ToString(Formatting.Indented);
							this.panel1.Controls.OfType<TextBox>().FirstOrDefault().Text = jsonFormatted;

							textBox3.Text = zone.Description;*//*
							break;
                        }
                    }
                }*/
                
            }
            return controlPoint;
            /*MouseEventArgs mouseEventArgs = (MouseEventArgs)e;

			if (mouseEventArgs.Button == MouseButtons.Left)
			{
				Point controlPoint = this.PointToClient(Cursor.Position);
				controlPoint.Offset(-this.AutoScrollPosition.X, -this.AutoScrollPosition.Y);

				for (int i = this.zones.Count - 1; i >= 0; i--)
				{
					Zone zone = this.zones[i];

					if (zone.Location.Contains(controlPoint))
					{
						if (zone.ActivationBox != null)
						{
							if (this.OnSelectActivation != null)
								this.OnSelectActivation(zone.ActivationBox);
							Invalidate();
							this.activeActivation = zone.ActivationBox;
						}
						break;
					}
				}
				return;
			}
			if (mouseEventArgs.Button == MouseButtons.Right)
			{
				Point controlPoint = this.PointToClient(Cursor.Position);
				controlPoint.Offset(-this.AutoScrollPosition.X, -this.AutoScrollPosition.Y);

				for (int i = this.zones.Count - 1; i >= 0; i--)
				{
					Zone zone = this.zones[i];

					if (zone.Location.Contains(controlPoint))
					{
						if (zone.Participant != null)
						{
							this.enableBreakMenuItem.Checked = zone.Participant.Break;
							this.participantContextMenu.Tag = zone.Participant;
							this.participantContextMenu.Show(Cursor.Position);
						}
						break;
					}
				}
				return;
			}*/
        }

		private void enableBreakMenuItem_Click(object sender, EventArgs e)
		{
			Participant participant = (Participant)this.participantContextMenu.Tag;
			participant.Break = !participant.Break;
			Invalidate();
		}

		public void Save()
		{
			this.saveFileDialog.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
			this.saveFileDialog.FilterIndex = 1;
			this.saveFileDialog.RestoreDirectory = true;

			if (this.saveFileDialog.ShowDialog() != DialogResult.OK)
				return;
			string path = this.saveFileDialog.FileName;

			using (Bitmap graphicSurface = new Bitmap(this.size.Width, this.size.Height))
			{
				using (StreamWriter bitmapWriter = new StreamWriter(path))
				{
					DrawToBitmap(graphicSurface, new Rectangle(0, 0, this.size.Width, this.size.Height));
					graphicSurface.Save(bitmapWriter.BaseStream, ImageFormat.Jpeg);
				}
			}
		}

		private int GetLevel(Participant participant, int timestep, bool begin)
		{
			if (begin)
			{
				if (!participant.BeginDepth.ContainsKey(timestep))
					return 0;

				return participant.BeginDepth[timestep];
			}
			else
			{
				if (!participant.EndDepth.ContainsKey(timestep))
					return 0;

				return participant.EndDepth[timestep];
			}
		}

		private int GetMaxLevel(Participant participant, int timestep)
		{
			int v0 = GetLevel(participant, timestep, true);
			int v1 = GetLevel(participant, timestep, false);
			return Math.Max(v0, v1);
		}
	}
}
