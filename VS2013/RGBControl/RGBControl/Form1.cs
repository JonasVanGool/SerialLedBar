

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace RGBControl
{
    public partial class RGBControlForm : Form
    {


        private enum COMMAND
        {
            SLIDER_MASTER = 101,
            PROGRAM,
            FADE,
            SLIDER_1,
            SLIDER_2,
            SLIDER_3,
            HP,
            SLIDER_MASTER_HP,
            PROGRAM_HP,
            FADE_HP,
            SLIDER_1_HP,
            BOOM,
            BOOM_HP,
            ZERO,
            ZERO_HP,
            RED_M,
            GREEN_M,
            BLUE_M,
            RED_M_HP,
            GREEN_M_HP,
            BLUE_M_HP
        };

        private static SerialPort _serialPort1;
        private static SerialPort _serialPort2;
        private static Boolean _serialPort1_IsConnected;
        private static Boolean _serialPort2_IsConnected;
        public Dictionary<int, int> dictionary = new Dictionary<int, int>();

        public RGBControlForm()
        {

            this.KeyPreview = true;

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);

            InitializeComponent();

            // set enabel main
            setMainEnable(false);

            // init checkbox
            checkBoxEnable1.Enabled = false;
            checkBoxEnable2.Enabled = false;


            // comboBox1 initialize
            foreach (string comPortName in SerialPort.GetPortNames())
            {
                this.comboBox1.Items.Add(comPortName);
            }
            this.comboBox1.SelectedIndex = SerialPort.GetPortNames().Count() - 1;

            // comboBox2 initialize
            foreach (string comPortName in SerialPort.GetPortNames())
            {
                this.comboBox2.Items.Add(comPortName);
            }
            this.comboBox2.SelectedIndex = SerialPort.GetPortNames().Count() - 1;

            // comboBox3 initialize
            this.comboBox3.SelectedIndex = 0;

            // comboBox4 initialize
            this.comboBoxHP.SelectedIndex = 0;

        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyValue);
            if (!dictionary.ContainsKey(e.KeyValue))
            {
                switch(e.KeyValue){
                    case 96:
                        dictionary.Add(e.KeyValue, trackBar5.Value);
                        trackBar5.Value = 100;
                        writeCommand(COMMAND.SLIDER_MASTER, (byte)trackBar5.Value);
                        break;
                    case 97: 
                        dictionary.Add(e.KeyValue, trackBar1.Value);
                        trackBar1.Value = 100;
                        writeCommand(COMMAND.SLIDER_1, (byte)trackBar1.Value);
                        break;
                    case 98: 
                        dictionary.Add(e.KeyValue, trackBar2.Value);
                        trackBar2.Value = 100;
                        writeCommand(COMMAND.SLIDER_2, (byte)trackBar2.Value);
                        break;
                    case 99:
                        dictionary.Add(e.KeyValue, trackBar3.Value);
                        trackBar3.Value = 100;
                        writeCommand(COMMAND.SLIDER_3, (byte)trackBar3.Value);
                        break;
                    case 100:
                        dictionary.Add(e.KeyValue, trackBarHP.Value);
                        trackBarHP.Value = 100;
                        writeCommand(COMMAND.SLIDER_MASTER_HP, (byte)trackBarHP.Value);
                        break;
                    case 101:
                        dictionary.Add(e.KeyValue, trackBar4.Value);
                        trackBar4.Value = 100;
                        writeCommand(COMMAND.SLIDER_1_HP, (byte)trackBar4.Value);
                        break;
                    case 65:
                        dictionary.Add(e.KeyValue, 0);
                        writeCommand(COMMAND.RED_M, (byte)1);
                        break;
                    case 83:
                        dictionary.Add(e.KeyValue, 0);
                        writeCommand(COMMAND.GREEN_M, (byte)1);
                        break;
                    case 68:
                        dictionary.Add(e.KeyValue, 0);
                        writeCommand(COMMAND.BLUE_M, (byte)1);
                        break;
                    case 81:
                        dictionary.Add(e.KeyValue, 0);
                        writeCommand(COMMAND.RED_M_HP, (byte)1);
                        break;
                    case 87:
                        dictionary.Add(e.KeyValue, 0);
                        writeCommand(COMMAND.GREEN_M_HP, (byte)1);
                        break;
                    case 69:
                        dictionary.Add(e.KeyValue, 0);
                        writeCommand(COMMAND.BLUE_M_HP, (byte)1);
                        break;
                    default: break;
                }
            }
        }

        void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 96:
                    trackBar5.Value = dictionary[e.KeyValue];
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.SLIDER_MASTER, (byte)trackBar5.Value);
                    break;
                case 97: 
                    trackBar1.Value = dictionary[e.KeyValue];
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.SLIDER_1, (byte)trackBar1.Value);
                    break;
                case 98:
                    trackBar2.Value = dictionary[e.KeyValue];
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.SLIDER_2, (byte)trackBar2.Value);
                    break;
                case 99: 
                    trackBar3.Value = dictionary[e.KeyValue];
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.SLIDER_3, (byte)trackBar3.Value);
                    break;
                case 100:
                    trackBarHP.Value = dictionary[e.KeyValue];
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.SLIDER_MASTER_HP, (byte)trackBarHP.Value);
                    break;
                case 101:
                    trackBar4.Value = dictionary[e.KeyValue];
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.SLIDER_1_HP, (byte)trackBar4.Value);
                    break;
                case 65:
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.RED_M, (byte)0);
                    break;
                case 83:
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.GREEN_M, (byte)0);
                    break;
                case 68:
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.BLUE_M, (byte)0);
                    break;
                case 81:
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.RED_M_HP, (byte)0);
                    break;
                case 87:
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.GREEN_M_HP, (byte)0);
                    break;
                case 69:
                    dictionary.Remove(e.KeyValue);
                    writeCommand(COMMAND.BLUE_M_HP, (byte)0);
                    break;
                default: break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_serialPort1_IsConnected)
            {
                try
                {
                    _serialPort1 = new SerialPort(comboBox1.SelectedItem.ToString(),9600, Parity.None, 8, StopBits.One);
                    //_serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    // Set the read/write timeouts
                    _serialPort1.ReadTimeout = 500;
                    _serialPort1.WriteTimeout = 500;
                    _serialPort1.Open();
                    

                }
                catch (Exception e1)
                {
                    //MessageBox.Show(e1.Message);
                    MessageBox.Show("Unable to connect");
                    checkBoxEnable1.Enabled = false;
                    updateMainVisibility();
                    return;
                }
                _serialPort1_IsConnected = true;
                checkBoxEnable1.Enabled = true;
                updateMainVisibility();
            }
            else
            {
                _serialPort1 = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                try
                {
                    _serialPort1 = new SerialPort(comboBox1.SelectedItem.ToString(), 9600, Parity.None, 8, StopBits.One);
                    //_serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    // Set the read/write timeouts
                    _serialPort1.ReadTimeout = 500;
                    _serialPort1.WriteTimeout = 500;
                    _serialPort1.Open();
                }
                catch (Exception e1)
                {
                    //MessageBox.Show(e1.Message);
                    MessageBox.Show("Unable to connect");
                    checkBoxEnable1.Enabled = false;
                    updateMainVisibility();
                    return;
                }
                checkBoxEnable1.Enabled = true;
                updateMainVisibility();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_serialPort2_IsConnected)
            {
                try
                {
                    _serialPort2 = new SerialPort(comboBox2.SelectedItem.ToString(), 9600, Parity.None, 8, StopBits.One);
                    //_serialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    // Set the read/write timeouts
                    _serialPort2.ReadTimeout = 500;
                    _serialPort2.WriteTimeout = 500;
                    _serialPort2.Open();
                }
                catch (Exception e1)
                {
                    //MessageBox.Show(e1.Message);
                    MessageBox.Show("Unable to connect");
                    checkBoxEnable2.Enabled = false;
                    updateMainVisibility();
                    return;
                }
                _serialPort2_IsConnected = true;
                checkBoxEnable2.Enabled = true;
                updateMainVisibility();
            }
            else
            {
                _serialPort2 = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                try
                {
                    _serialPort2 = new SerialPort(comboBox2.SelectedItem.ToString(), 9600, Parity.None, 8, StopBits.One);
                    //_serialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    // Set the read/write timeouts
                    _serialPort2.ReadTimeout = 500;
                    _serialPort2.WriteTimeout = 500;
                    _serialPort2.Open();
                }
                catch (Exception e1)
                {
                    //MessageBox.Show(e1.Message);
                    MessageBox.Show("Unable to connect");
                    checkBoxEnable2.Enabled = false;
                    updateMainVisibility();
                    return;  
                }
                checkBoxEnable2.Enabled = true;
                updateMainVisibility();
            }
        }

        private void setMainEnable(Boolean enable)
        {
            trackBar5.Enabled = enable;
            trackBar1.Enabled = enable;
            trackBar2.Enabled = enable;
            trackBar3.Enabled = enable;
            checkBox3.Enabled = enable;
            comboBox3.Enabled = enable;
            checkBox4.Enabled = enable;
            checkBox1.Enabled = enable;
            checkBox2.Enabled = enable;
            checkBox5.Enabled = enable;
            checkBox6.Enabled = enable;
            trackBarHP.Enabled = checkBox4.Checked && checkBox4.Enabled;
            checkBoxFadeHP.Enabled = checkBox4.Checked && checkBox4.Enabled;
            comboBoxHP.Enabled = checkBox4.Checked && checkBox4.Enabled;
        }

        private void updateMainVisibility()
        {
            setMainEnable(checkBoxEnable1.Enabled || checkBoxEnable2.Enabled);
        }

        private void checkBoxEnable1_CheckedChanged(object sender, EventArgs e)
        {
            updateMainVisibility();
            if (checkBoxEnable1.Checked)
            {
                writeAllCommands();
            }
        }

        private void checkBoxEnable2_CheckedChanged(object sender, EventArgs e)
        {
            updateMainVisibility();
            if (checkBoxEnable2.Checked)
            {
                writeAllCommands();
            }
        }

        private void writeAllCommands()
        {
            writeCommand(COMMAND.SLIDER_MASTER, (byte)trackBar5.Value);  
            
            writeCommand(COMMAND.PROGRAM, (byte)comboBox3.SelectedIndex);
            writeCommand(COMMAND.FADE, (byte)(checkBox3.Checked ? 1 : 0));
            writeCommand(COMMAND.SLIDER_1, (byte)trackBar1.Value);
            writeCommand(COMMAND.SLIDER_2, (byte)trackBar2.Value);
            writeCommand(COMMAND.SLIDER_3, (byte)trackBar3.Value);
            writeCommand(COMMAND.HP, (byte)(checkBox4.Checked ? 1 : 0));
            writeCommand(COMMAND.FADE_HP, (byte)(checkBox4.Checked ? 1 : 0));
            writeCommand(COMMAND.SLIDER_MASTER_HP, (byte)trackBarHP.Value);
            
            writeCommand(COMMAND.PROGRAM_HP, (byte)comboBoxHP.SelectedIndex);
            
            writeCommand(COMMAND.FADE_HP, (byte)(checkBoxFadeHP.Checked ? 1 : 0));
            writeCommand(COMMAND.SLIDER_1_HP, (byte)trackBar4.Value);

            writeCommand(COMMAND.BOOM, (byte)(checkBox1.Checked ? 1 : 0));
            writeCommand(COMMAND.BOOM_HP, (byte)(checkBox2.Checked ? 1 : 0));
            writeCommand(COMMAND.ZERO, (byte)(checkBox5.Checked ? 1 : 0));
            writeCommand(COMMAND.ZERO_HP, (byte)(checkBox6.Checked ? 1 : 0));
        }

        private void writeCommand(COMMAND command, byte value)
        {
            byte[] message = new byte[2];
            message[0] = (byte)command;
            message[1] = value;
            try
            {
                if (_serialPort1 != null)
                {
                    if (checkBoxEnable1.Checked && checkBoxEnable1.Enabled)
                        _serialPort1.Write(message, 0, 2);
                }
                if (_serialPort2 != null)
                {
                    if (checkBoxEnable2.Checked && checkBoxEnable2.Enabled)
                        _serialPort2.Write(message, 0, 2);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Invalid operation!");
            }
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            writeCommand(COMMAND.SLIDER_MASTER,(byte)trackBar5.Value);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.PROGRAM, (byte)comboBox3.SelectedIndex);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.FADE, (byte)(checkBox3.Checked ? 1 : 0));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            writeCommand(COMMAND.SLIDER_1, (byte)trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            writeCommand(COMMAND.SLIDER_2, (byte)trackBar2.Value);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            writeCommand(COMMAND.SLIDER_3, (byte)trackBar3.Value);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.HP, (byte)(checkBox4.Checked ? 1 : 0));
            trackBarHP.Enabled = checkBox4.Checked;
            checkBoxFadeHP.Enabled = checkBox4.Checked;
            comboBoxHP.Enabled = checkBox4.Checked;
        }

        private void trackBarHP_Scroll(object sender, EventArgs e)
        {
            writeCommand(COMMAND.SLIDER_MASTER_HP, (byte)trackBarHP.Value);
        }

        private void comboBoxHP_SelectedIndexChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.PROGRAM_HP, (byte)comboBoxHP.SelectedIndex);
        }

        private void checkBoxFadeHP_CheckedChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.FADE_HP, (byte)(checkBoxFadeHP.Checked ? 1 : 0));
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            writeCommand(COMMAND.SLIDER_1_HP, (byte)trackBar4.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBoxEnable1.Checked = true;
            checkBoxEnable2.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBoxEnable1.Checked = false;
            checkBoxEnable2.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.BOOM, (byte)(checkBox1.Checked ? 1 : 0));
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.BOOM_HP, (byte)(checkBox2.Checked ? 1 : 0));
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.ZERO, (byte)(checkBox5.Checked ? 1 : 0));
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            writeCommand(COMMAND.ZERO_HP, (byte)(checkBox6.Checked ? 1 : 0));
        }
    }
}
