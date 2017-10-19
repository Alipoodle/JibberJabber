using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;


namespace RandomJibberJabber
{
    public partial class JibberJabber : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public string urla = "https://alipoodle.me/small.gif";
        string[] lines;
        Random random = new Random();
        string fileName = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\jibberjabber.txt";
        bool shouldTerminate = false;

        public JibberJabber()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            if (File.Exists(fileName))
            {
                lines = System.IO.File.ReadAllLines(fileName);
            }
            else {
                MessageBox.Show("Failed to find file! Creating file at location: " + fileName);
                byte[] array = Encoding.ASCII.GetBytes("https://google.co.uk/");
                using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                {
                    foreach (byte element in array)
                    {
                        fs.WriteByte(element);
                    }
                }
            }

            textBox1.Text = "Log:";
        }


        // -------------------------------------------------  Toggles -------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false) {
                timer1.Enabled = true;
                button1.Text = "Turn Off";
                label1.ForeColor = Color.Green;
                label1.Text = "Currently Jibber Jabbering";
            }
            else if (timer1.Enabled == true) {
                timer1.Enabled = false;
                button1.Text = "Turn On";
                label1.ForeColor = Color.DarkRed;
                label1.Text = "Click the button to enable Jibber Jabbering";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                shouldTerminate = false;
            }
            else
            {
                shouldTerminate = true;
            }
        }

        // -------------------------------------------------   Timer  -------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            var baseUrl = lines[random.Next(0, lines.Length)];
            // MessageBox.Show(lines[rndNum]);
            string[] urlSplit = baseUrl.Split('/');
            var startUrl = $"{urlSplit[0]}/{urlSplit[1]}/{urlSplit[2]}";
            var endUrl = baseUrl.Remove(0, startUrl.Length);

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                client.BaseAddress = new Uri(startUrl);

                HttpResponseMessage response = client.GetAsync(endUrl).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                // textBox1.Text = ("Result: " + result);
                textBox1.Text += ($"{Environment.NewLine}{DateTime.Now.ToString("h:mm:ss tt")} : {baseUrl}");
            }
            timer1.Interval = random.Next(10,60) * 1000;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(fileName);
        }



        // ------------------------------------------------- Open / Close -------------------------------------------------
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e) // Open
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            shouldTerminate = true;
            this.Close();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) // Close
        {
            shouldTerminate = true;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !shouldTerminate)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
            }
            base.OnFormClosing(e);
        }
    }
}
