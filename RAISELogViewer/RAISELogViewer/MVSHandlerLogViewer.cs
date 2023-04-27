using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SequenceDiagramLib.Model;
using SequenceDiagramLib.View;
using SequenceDiagramTestApp.Class;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace SequenceDiagramTestApp
{
    public partial class MVSHandlerLogViewer : Form
    {

        List<Data> list = new List<Data>();
        private List<Zone> zones = new List<Zone>();
        private List<Zone> zzones = new List<Zone>();
        int participantCount = 0;

        string file;

        public MVSHandlerLogViewer()
        {
            InitializeComponent();
           
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                MainData.FilePath = openFileDialog1.FileName;
               
                SequenceLoad("");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SequenceLoad(textBox3.Text.ToString());

        }

        private void MVSHandlerLogViewer_Load(object sender, EventArgs e)
        {
            file = MainData.FilePath + "/MVSH/MVSH" + MainData.Time + ".txt";
            SequenceLoad("");
        }

        private void sequenceDiagram_Click(object sender, EventArgs e)
        {
            Point p = this.sequenceDiagram.SequenceDiagramControl_Click(sender, e);

            for (int i = this.zones.Count - 1; i >= 0; i--)
            {
                Zone zone = this.zones[i];

                if (zone.Location.Contains(p))
                {
                    if (zone.Participant == null)
                    {
                        string j = list.Where(x => x.Time == zone.Description).FirstOrDefault().JsonString;
                        //string j = list[i - participantCount].JsonString;
                        string jsonFormatted = JValue.Parse(j).ToString(Formatting.Indented);
                        this.panel1.Controls.OfType<TextBox>().FirstOrDefault().Text = jsonFormatted;

                        textBox3.Text = zone.Description;
                        break;
                    }
                }
            }
        }

        private void FillRegionRectangle(PaintEventArgs e)
        {

            // Create solid brush.
            SolidBrush blueBrush = new SolidBrush(Color.Blue);

            // Create rectangle for region.
            Rectangle fillRect = new Rectangle(100, 100, 200, 200);

            // Create region for fill.
            Region fillRegion = new Region(fillRect);

            // Fill region to screen.
            e.Graphics.FillRegion(blueBrush, fillRegion);
        }

        private void SequenceLoad(string time)
        {
            this.sequenceDiagram.Sequence.Clear();
            list.Clear();

            Sequence sequence = this.sequenceDiagram.Sequence;

            if (!string.IsNullOrEmpty(file))
            {
                sequence.Tick("");
                var sampe = File.ReadAllLines(file).ToList()
                .Select((value, index) => new { value, index })
                .Where(x => x.value.Substring(0, 12) == time)
                .Select(x => x.index)
                .Take(1)
                .ToList();

                File.ReadAllLines(file)
                    .ToList()
                    .Skip(sampe.FirstOrDefault() == 0 ? 0 : sampe.FirstOrDefault() - 1)
                    .ToList().ForEach(x => {

                        Data d = new Data();
                        string ss = x;

                        d.Time = ss.Substring(0, 12);

                        ss = ss.Substring(12);

                        d.LogType = ss.Substring(1, 3);

                        ss = ss.Substring(5);

                        d.MessageType = ss.Split(' ')[0];


                        if (d.MessageType == "send" || d.MessageType == "receive")
                        {
                            ss = ss.Substring(d.MessageType.Length + 9);

                            d.JsonString = ss;
                            JObject json = JObject.Parse(d.JsonString);

                            string sP1 = json?["hit"]?["label"]?["callFrom"]?.ToString();
                            string sP2 = json?["hit"]?["label"]?["callTo"]?.ToString();

                            if (sP1 != null || sP2 != null)
                            {
                                Participant p1 = sequence.Participants.CreateOrGet(sP1);
                                Participant p2 = sequence.Participants.CreateOrGet(sP2);
                                sequence.Messages.Add(json?["hit"]?["label"]?["scene"]?.ToString(), p1, p2, sP1 + " -> " + sP2);
                                sequence.Tick(d.Time);
                                sequence.Continue();

                                list.Add(d);
                            }
                        }

                    });

                this.zones = this.sequenceDiagram.timeZones;
                participantCount = this.sequenceDiagram.Sequence.Participants.Count();
            }
            
        }
    }
}
