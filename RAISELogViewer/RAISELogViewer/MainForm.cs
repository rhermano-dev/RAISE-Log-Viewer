using SequenceDiagramTestApp;
using SequenceDiagramTestApp.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RAISELogViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MVSHandlerLogViewer mvsh = new MVSHandlerLogViewer();

            MainData.Time = this.dateTimePicker1.Text;
            MainData.FilePath = folderBrowserDialog1.SelectedPath.ToString();

            mvsh.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.dateTimePicker1.MaxDate = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TFLogViewer tf = new TFLogViewer();
            tf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MVSCLogViewer mvsc = new MVSCLogViewer();

            MainData.Time = this.dateTimePicker1.Text;
            MainData.FilePath = folderBrowserDialog1.SelectedPath.ToString();

            mvsc.Show();

        }
    }
}
