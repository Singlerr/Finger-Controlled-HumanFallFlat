using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace ArduinoCon
{

    public partial class Form1 : Form
    {
        
        double anglePrev = 0;
        double time, timePrev = 0;
        private SerialPort leftPort, rightPort;
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]

        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);



      
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public Form1()
        {
            InitializeComponent();
        }
        public static double Current()
        {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
            return javaSpan.TotalMilliseconds;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Left_TextChanged(object sender, EventArgs e)
        {

        }
        private void PerformLeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }
        private void PerformRightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            leftPort = new SerialPort();
            rightPort = new SerialPort();
            leftPort.PortName = left.Text;
            rightPort.PortName = right.Text;
            leftPort.BaudRate = 115200;
            rightPort.BaudRate = 115200;
            leftPort.DataReceived += DataReceivedL;
            rightPort.DataReceived += DataReceivedR;
            leftPort.Open();
            rightPort.Open();
            status.Text = "Connected";
        }
        public static int Map(int value, int fromSource, int toSource, int fromTarget, int toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
        private void DataReceivedR(object sender, SerialDataReceivedEventArgs e)
        {
            double angle = 0;
            bool flag = double.TryParse(rightPort.ReadExisting(), out angle);
            if (flag)
            {
                rstat.Text = "Right: " + angle;
                int k = 0;
                if (angle < 0)
                {
                    k = 0;
                }
                else
                if (angle > 180)
                {
                    k = 180;
                }
                else
                {
                    k = (int)angle;
                }
                if(k > 160)
                {
                    mouse_event(MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                }
                else if(30 < k)
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                    timer2.Interval = Map(k, 0, 160, 31, 100);
                    timer2.Start();
                }
                else
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                }

            }
        }
        private double Abs(double x)
        {
            if(x < 0)
            {
                return -x;
            }
            else
            {
                return x;
            }
            
        }
       
        private void button1_Click_1(object sender, EventArgs e)
        {
            leftPort = new SerialPort();
            rightPort = new SerialPort();
            leftPort.PortName = left.Text;
            rightPort.PortName = right.Text;
            leftPort.BaudRate = 115200;
            rightPort.BaudRate = 115200;
            leftPort.DataReceived += DataReceivedL;
            rightPort.DataReceived += DataReceivedR;
            leftPort.Open();
            rightPort.Open();
            status.Text = "Connected";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
            timer2.Stop();
        }

        private void Calculate(double angle)
        {
            time = Current();
            double k = (angle - anglePrev) / (time - timePrev);
            anglePrev = angle;
            timePrev = time;
            if(Abs(k)> 10)
            {
                
            }
        }
        private void DataReceivedL(object sender, SerialDataReceivedEventArgs e)
        {
            double angle = 0;
            bool flag = double.TryParse(leftPort.ReadExisting(), out angle);
            if (flag)
            {
                lstat.Text = "Left: " + angle;
                int k = 0;
                if (angle < 0)
                {
                    k = 0;
                }
                else
                if (angle > 180)
                {
                    k = 180;
                }
                else
                {
                    k = (int)angle;
                }
                if (k > 160)
                {
                    mouse_event(MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                }
                else if (30 < k)
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                    timer2.Interval = Map(k, 0, 160, 31, 100);
                    timer2.Start();
                }
                else
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                }

            }
        }
    }
}