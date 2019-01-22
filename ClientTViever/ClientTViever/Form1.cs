using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTViever
{
    public partial class Form1 : Form
    {
        Task screen = null;
        CancellationTokenSource tokenSource2 = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e) //Connect
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            var client = new TcpClient();
            client.Connect(IPAddress.Parse(textBox1.Text), 40001);

            if (client.Connected)
            {
                var stream = client.GetStream();
                var formatter = new BinaryFormatter();
                button2.Enabled = true;
                button1.Enabled = false;

                 tokenSource2 = new CancellationTokenSource();
                CancellationToken ct = tokenSource2.Token;


                screen = Task.Run(() =>
                {
                    while (true)
                    {
                        ct.ThrowIfCancellationRequested();
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }
                        var image = formatter.Deserialize(stream) as Bitmap;
                        pictureBox1.Image = image;
                    }
                }, tokenSource2.Token);

            }
        }

        private void button2_Click(object sender, EventArgs e) //Disconnect
        {
            button1.Enabled = true;
            button2.Enabled=false;
            tokenSource2.Cancel();                    
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
