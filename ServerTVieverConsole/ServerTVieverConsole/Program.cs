using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ServerTVieverConsole
{
    class Program
    {
        static Bitmap TakeScreenShot()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                return bmp;
            }
        }

        static void Main(string[] args)
        {
            var server = new TcpListener(
          IPAddress.Any,
          40001
         );

            server.Start();
            while (true)
            {
                var client = server.AcceptTcpClient();
                Task.Run(() =>
                {
                    while (true)
                    {
                        var stream = client.GetStream();
                        var bmp = TakeScreenShot();
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(stream, bmp);
                    }
                });
            }
        }
    }
}
