using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTS793Boxes;
using MTS793Rt;
using MTS793Units;
using MTS793Util;
using System.Diagnostics;

namespace WindowsFormsApplication_MTS_Test
{
    public partial class Form1 : Form
    {

        enum LoadChannel { Axial, Torsional };

        RtSystem MTSSystem;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MTSSystem = new RtSystem();

                MTSSystem.Connect("", "AppName", "Appname");

                IObjectCollection Names = MTSSystem.StationNames;

                foreach (var key in Names)
                {
                    INamed N = (INamed)key;

                    comboBox1.Items.Add(N.KeyName);
                }

                comboBox1.SelectedIndex = 0;

                string name = comboBox1.Items[0].ToString();

                Debug.WriteLine("Station name is: " + name);

                RtStation station = MTSSystem.FindStation(name, "AppName", "", oEnumAppType.oAppSummary);

                Debug.WriteLine(station.Channels.Count);

                ObjectCollection channels = station.Channels;

                RtChannel Axial = channels.Find(LoadChannel.Axial.ToString());

                IRtSegGen segGen = (IRtSegGen)Axial.SegGen;

                RtIntegerSig segCount = segGen.SegCountIntSignal;

                MessageBox.Show((segCount.Value / 2).ToString());

                     

                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
