using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace HariFun_ComPortList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void updateList()
        {
            // Original code from StackOverflow. Thanks Humudu!
            // https://stackoverflow.com/questions/2837985/getting-serial-port-information
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => ports.FirstOrDefault(s => s.Contains(n))).ToList();

                // I want the port# to be in front of the description
                for (int i = 0; i < portList.Count; i++)
                {
                    var lastOpenParen = portList[i].LastIndexOf('(');
                    var lastCloseParen = portList[i].LastIndexOf(')');
                    var portName = portList[i].Substring(lastOpenParen + 1, lastCloseParen - lastOpenParen - 1);
                    var portDescription = portList[i].Substring(0, lastOpenParen - 1);
                    portList[i] = portName + " - " + portDescription;
                }

                output.DataSource = portList;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateList();
        }

        private void output_Click(object sender, EventArgs e)
        {
            updateList();
        }
    }
}
