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
using Ivi.Visa.Interop;

namespace WindowsFormsApplication_MTS_Test
{
    public partial class Form1 : Form
    {
        enum LoadChannel { Axial, Torsional };

        RtSystem MTSSystem;
        RtStation Station;

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

                Station = MTSSystem.FindStation(name, "AppName", "", oEnumAppType.oAppSummary);

                Debug.WriteLine(Station.Channels.Count);
                                   
          
            }
            catch (Exception err)
            {
                MessageBox.Show("DANGER WILL ROBINSON--" + err.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    ResourceManager InstronRM = new ResourceManager();

            //    FormattedIO488 Instron = new FormattedIO488();

            //    Instron.IO = (IMessage)InstronRM.Open("GPIB0::3::INSTR", AccessMode.NO_LOCK, 20000, "");

            //    MessageBox.Show("Connection established.");
            //}
            //catch (Exception error)
            //{
            //    MessageBox.Show("INSTRON ERROR: " + error.ToString());
            //}

            comboBox2.SelectedIndex = 0;
           
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }


        private void Get_Cycle_Count()
        {
            ObjectCollection channels = Station.Channels;

            RtChannel Axial = channels.Find(comboBox2.SelectedItem.ToString());

            IRtSegGen segGen = (IRtSegGen)Axial.SegGen;

            RtIntegerSig segCount = segGen.SegCountIntSignal;

            textBox_MTS_CycleCounts.Text = (segCount.Value / 2).ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {

            System.Timers.Timer Refresh_Timer = new System.Timers.Timer(5000);

            Refresh_Timer.Elapsed += Refresh_Timer_Elapsed;

            
        }

        void Refresh_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Get_Cycle_Count();
        }

    }
}
