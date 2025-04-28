using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Haden.NxtSharp;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;

namespace Haden.NxtRemote.CF
{
    public partial class RemoteForm : Form
    {
        public RemoteForm()
        {
            InitializeComponent();
            InitializeBrick();
            InitializeSensors();
            //RemoteForm_Load();
        }

        const int MAX_MESSAGE_SIZE = 128;
        const int MAX_TRIES = 3;

        private Guid ServiceName = new Guid("{E075D486-E23D-4887-8AF5-DAA1F6A5B172}");

        BluetoothClient btClient = new BluetoothClient();
        BluetoothListener btListener;

        private bool listening = true;
        string str = "";

        public void RemoteForm_Load(object sender, EventArgs e)
        {
            System.Threading.Thread t1;
            t1 = new System.Threading.Thread(receiveLoop);
            t1.Start();

            btClient = new BluetoothClient();

            BluetoothDeviceInfo[] bdi = btClient.DiscoverDevices();

            cboDevices.DataSource = bdi;
            cboDevices.DisplayMember = "DeviceName";
        }

        protected void InitializeBrick()
        {
            try
            {
                nxtBrick.MotorA = nxtMotorA;
                nxtMotorA.Brick = nxtBrick;
                nxtBrick.MotorB = nxtMotorB;
                nxtMotorB.Brick = nxtBrick;
                nxtBrick.MotorC = nxtMotorC;
                nxtMotorC.Brick = nxtBrick;
                nxtBrick.COMPortName = "COM0";
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }

        protected void InitializeSensors()
        {
            try
            {
                this.nxtBrick.Sensor1 = this.nxtPressureSensor;
                this.nxtPressureSensor.Brick = this.nxtBrick;
                this.nxtPressureSensor.AutoPoll = true;
                this.nxtPressureSensor.AutoPollDelay = 100;

                this.nxtBrick.Sensor2 = this.nxtSoundSensor;
                this.nxtSoundSensor.Brick = this.nxtBrick;
                this.nxtSoundSensor.AdjustForHumanEar = true;
                this.nxtSoundSensor.AutoPoll = true;
                this.nxtSoundSensor.AutoPollDelay = 0;

                this.nxtBrick.Sensor3 = this.nxtLightSensor;
                this.nxtLightSensor.Brick = this.nxtBrick;
                this.nxtLightSensor.Active = false;
                this.nxtLightSensor.AutoPoll = true;
                this.nxtLightSensor.AutoPollDelay = 100;

                this.nxtBrick.Sensor4 = this.nxtSonar;
                this.nxtSonar.Brick = this.nxtBrick;
                this.nxtSonar.AutoPoll = true;
                this.nxtSonar.AutoPollDelay = 0;
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public void receiveLoop()
        {
            string strReceived;
            btListener = new BluetoothListener(ServiceName);
            btListener.Start();

            strReceived = receiveMessage(MAX_MESSAGE_SIZE);
            while (listening)
            {
                //---keep on listening for new message
                if (strReceived != "")
                {
                    this.Invoke(new EventHandler(UpdateTextBox));

                    strReceived = receiveMessage(MAX_MESSAGE_SIZE);
                }
            }
        }

        private void UpdateTextBox(object sender, EventArgs e)
        {
            //---delegate to update the textbox control
            lblStatus.Text += str;
        }

        private string receiveMessage(int BufferLen)
        {
            int bytesRead = 0;
            BluetoothClient client = null;
            System.IO.Stream stream = null;
            string Constants_vbCrLf = " ";
            byte[] Buffer = new byte[MAX_MESSAGE_SIZE + 1];

            try
            {
                client = btListener.AcceptBluetoothClient();
                // blocking call
                stream = client.GetStream();
                bytesRead = stream.Read(Buffer, 0, BufferLen);

                str = client.RemoteMachineName + " --> " + System.Text.Encoding.Unicode.GetString(Buffer, 0, bytesRead) + Constants_vbCrLf;
            }
            catch (Exception)
            {
                //dont display error if we are ending the listener
                if (listening)
                {
                    MessageBox.Show("Error listening to incoming message.");
                }
            }
            finally
            {
                if (((stream != null)))
                {
                    stream.Close();
                }
                if (((client != null)))
                {
                    client.Close();
                }
            }
            return str;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //nxtBrick.Connect();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //nxtBrick.Disconnect();
            base.OnClosing(e);
        }

        //put the stuff that doesn't work.

        private void connectBrick_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            this.nxtBrick.Connect(btClient);
            lblStatus.Text = "Brick connected.";
        }

        private void disconnectBrick_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Disconnecting brick...";
            nxtBrick.Disconnect();
            nxtBrick.Dispose();
            lblStatus.Text = "Brick disconnected.";
        }

        private void mnuSearch_Click(object sender, EventArgs e)
        {
            //add the chat code here.
        }
    }
}