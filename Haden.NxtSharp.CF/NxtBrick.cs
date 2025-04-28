using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using Haden.Utilities;

namespace Haden.NxtSharp {

	/// <summary>
	/// This class encapsulates core lego NXT functionality
	/// </summary>
	public partial class NxtBrick : Component {
		public NxtBrick() {
			InitializeComponent();
		}

		public NxtBrick(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}
		

		#region Connection

		[Browsable(false)]
		/// <summary>
		/// Are we connected to the Lego NXT Brick?
		/// </summary>
		public bool IsConnected {
			get {
				return _isConnected;
			}
			private set {
				_isConnected = value;
			}
		}
		bool _isConnected = false;

		/// <summary>
		/// Connects to the Lego NXT Brick
		/// </summary>
		public void Connect() {
			openSerialPorts();
			initSettings();
			Comm.KeepAlive();

			OnConnected(this);
			if(AutoPoll) {
				StartPolling();
			}
			keepAliveTimer.Enabled = true;
		}

		/// <summary>
		/// Disconnects the Lego NXT Brick
		/// </summary>
		public void Disconnect() {
			keepAliveTimer.Enabled = false;
			if(AutoPoll) {
				StopPolling();
			}

			closeSerialPorts();
			OnDisconnected(this);
		}

		/// <summary>
		/// Raises the Connected event
		/// </summary>
		/// <param name="sender"></param>
		protected virtual void OnConnected(NxtBrick sender) {
			IsConnected = true;
			if(Connected != null) {
				Connected(sender, new EventArgs());
			}
		}

		public event EventHandler Connected;

		/// <summary>
		/// Raises the Disconnected event
		/// </summary>
		/// <param name="sender"></param>
		protected virtual void OnDisconnected(NxtBrick sender) {
			IsConnected = false;
			if(Disconnected != null) {
				Disconnected(sender, new EventArgs());
			}
		}

		public event EventHandler Disconnected;


		/// <summary>
		/// Keep alive
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void keepAliveTimer_Tick(object sender, EventArgs e) {
			if(IsConnected) {
				Comm.KeepAlive();
			}
		}


		#endregion 

		#region Serial Communication


		/// <summary>
		/// Sets the COM port to use for communication
		/// </summary>
		[Category("Lego NXT"), Description("Sets the COM port to use for communication. Look for outgoing 'Dev B' ports in your Bluetooth manager to see which virtual COM Port is assigned to your NXT.")]
		public string COMPortName {
			get {
				return _comPortName;
			}
			set {
				_comPortName = value;
			}
		}
		string _comPortName = "COM8";

		[Browsable(false)]
		/// <summary>
		/// The underlying communications handler
		/// </summary>
		public NxtCommunicator Comm {
			get {
				return _comm;
			}
			private set {
				_comm = value;
			}
		}
		NxtCommunicator _comm = null;

		private void openSerialPorts() {
			Comm = new NxtCommunicator(COMPortName);
			Comm.Connect();
		}

		private void closeSerialPorts() {
			Comm.Disconnect();
		}

		#endregion

		#region Device settings and initialization

		private void initSettings() {
			foreach(NxtSensor sensor in ConnectedSensors()) {
				sensor.InitSensor();
			}
		}

		#endregion

		#region NXT Ports

		#region Motor ports

		/// <summary>
		/// The motor that is connected to Port A
		/// </summary>
		[Category("Lego NXT"), Description("The motor that is connected to Port A")]
		public NxtMotor MotorA {
			get {
				return _motorA;
			}
			set {
				AttachMotor(NxtMotorPort.PortA, value);
			}
		}
		NxtMotor _motorA = null;

		/// <summary>
		/// The motor that is connected to Port B
		/// </summary>
		[Category("Lego NXT"), Description("The motor that is connected to Port B")]
		public NxtMotor MotorB {
			get {
				return _motorB;
			}
			set {
				AttachMotor(NxtMotorPort.PortB, value);
			}
		}
		NxtMotor _motorB = null;

		/// <summary>
		/// The motor that is connected to Port C
		/// </summary>
		[Category("Lego NXT"), Description("The motor that is connected to Port C")]
		public NxtMotor MotorC {
			get {
				return _motorC;
			}
			set {
				AttachMotor(NxtMotorPort.PortC, value);
			}
		}
		NxtMotor _motorC = null;

		/// <summary>
		/// Attaches the specified motor to the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="motor"></param>
		public void AttachMotor(NxtMotorPort port, NxtMotor motor) {
			foreach(NxtMotorPort p in MotorPorts()) {
				if(GetMotor(p) == motor) {
					SetMotor(p, null);
				}
			}
			if(port != NxtMotorPort.None) {
				SetMotor(port, motor);
			}
		}

		/// <summary>
		/// Returns the motor connected to the specified port
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public NxtMotor GetMotor(NxtMotorPort port) {
			if(port == NxtMotorPort.PortA) {
				return MotorA;
			}
			if(port == NxtMotorPort.PortB) {
				return MotorB;
			}
			if(port == NxtMotorPort.PortC) {
				return MotorC;
			}
			return null;
		}


		/// <summary>
		/// Sets the port for the specified motor, without any
		/// checks to see if two motors are attached to the same port
		/// </summary>
		/// <param name="port"></param>
		/// <param name="motor"></param>
		private void SetMotor(NxtMotorPort port, NxtMotor motor) {
			if(port == NxtMotorPort.PortA) {
				_motorA = motor;
			}
			if(port == NxtMotorPort.PortB) {
				_motorB = motor;
			}
			if(port == NxtMotorPort.PortC) {
				_motorC = motor;
			}
		}


		/// <summary>
		/// Enumerates all motor ports connected to the NXT
		/// </summary>
		/// <returns></returns>
		public IEnumerable<NxtMotorPort> MotorPorts() {
			yield return NxtMotorPort.PortA;
			yield return NxtMotorPort.PortB;
			yield return NxtMotorPort.PortC;
		}

		/// <summary>
		/// Enumerates all motors connected to the NXT
		/// </summary>
		/// <returns></returns>
		public IEnumerable<NxtMotor> ConnectedMotors() {
			if(MotorA != null) {
				yield return MotorA;
			}
			if(MotorB != null) {
				yield return MotorB;
			}
			if(MotorC != null) {
				yield return MotorC;
			}
		}

		#endregion


		#region Sensor Ports

		/// <summary>
		/// The sensor that is connected to Port 1 (usually the pressure sensor)
		/// </summary>
		[Category("Lego NXT"), Description("The sensor that is connected to Port 1 (usually the pressure sensor)")]
		public NxtSensor Sensor1 {
			get {
				return _sensor1;
			}
			set {
				AttachSensor(NxtSensorPort.Port1, value);
			}
		}
		NxtSensor _sensor1 = null;


		/// <summary>
		/// The sensor that is connected to Port 2 (usually the sound sensor)
		/// </summary>
		[Category("Lego NXT"), Description("The sensor that is connected to Port 2 (usually the sound sensor)")]
		public NxtSensor Sensor2 {
			get {
				return _sensor2;
			}
			set {
				AttachSensor(NxtSensorPort.Port2, value);
			}
		}
		NxtSensor _sensor2 = null;


		/// <summary>
		/// The sensor that is connected to Port 3 (usually the light sensor)
		/// </summary>
		[Category("Lego NXT"), Description("The sensor that is connected to Port 3 (usually the light sensor)")]
		public NxtSensor Sensor3 {
			get {
				return _sensor3;
			}
			set {
				AttachSensor(NxtSensorPort.Port3, value);
			}
		}
		NxtSensor _sensor3 = null;


		/// <summary>
		/// The sensor that is connected to Port 4 (usually the sonar)
		/// </summary>
		[Category("Lego NXT"), Description("The sensor that is connected to Port 4 (usually the sonar)")]
		public NxtSensor Sensor4 {
			get {
				return _sensor4;
			}
			set {
				AttachSensor(NxtSensorPort.Port4, value);
			}
		}
		NxtSensor _sensor4 = null;


		/// <summary>
		/// Attaches the specified sensor to the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="sensor"></param>
		public void AttachSensor(NxtSensorPort port, NxtSensor sensor) {
			foreach(NxtSensorPort p in SensorPorts()) {
				if(GetSensor(p) == sensor) {
					SetSensor(p, null);
				}
			}
			if(port != NxtSensorPort.None) {
				SetSensor(port, sensor);
			}
		}

		/// <summary>
		/// Returns the sensor connected to the specified port
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public NxtSensor GetSensor(NxtSensorPort port) {
			if(port == NxtSensorPort.Port1) {
				return Sensor1;
			}
			if(port == NxtSensorPort.Port2) {
				return Sensor2;
			}
			if(port == NxtSensorPort.Port3) {
				return Sensor3;
			}
			if(port == NxtSensorPort.Port4) {
				return Sensor4;
			}
			return null;
		}


		/// <summary>
		/// Sets the port for the specified sensor, without any
		/// checks to see if two sensors are attached to the same port
		/// </summary>
		/// <param name="port"></param>
		/// <param name="sensor"></param>
		private void SetSensor(NxtSensorPort port, NxtSensor sensor) {
			if(port == NxtSensorPort.Port1) {
				_sensor1 = sensor;
			}
			if(port == NxtSensorPort.Port2) {
				_sensor2 = sensor;
			}
			if(port == NxtSensorPort.Port3) {
				_sensor3 = sensor;
			}
			if(port == NxtSensorPort.Port4) {
				_sensor4 = sensor;
			}
		}


		/// <summary>
		/// Enumerates all sensor ports on the NXT
		/// </summary>
		/// <returns></returns>
		public IEnumerable<NxtSensorPort> SensorPorts() {
			yield return NxtSensorPort.Port1;
			yield return NxtSensorPort.Port2;
			yield return NxtSensorPort.Port3;
			yield return NxtSensorPort.Port4;
		}

		/// <summary>
		/// Enumerates all sensors connected to the NXT
		/// </summary>
		/// <returns></returns>
		public IEnumerable<NxtSensor> ConnectedSensors() {
			if(Sensor1 != null) {
				yield return Sensor1;
			}
			if(Sensor2 != null) {
				yield return Sensor2;
			}
			if(Sensor3 != null) {
				yield return Sensor3;
			}
			if(Sensor4 != null) {
				yield return Sensor4;
			}
		}


		#endregion


		#endregion

		#region Sensor Polling

		/// <summary>
		/// When AutoPoll is set, the NxtBrick will continuously 
		/// poll the state of all sensors which are connected to 
		/// the Brick and have the AutoPoll property set.
		/// </summary>
		[Category("Lego NXT"), Description("When AutoPoll is set, the NxtBrick will continuously poll the state of all sensors which are connected to the Brick and have the AutoPoll property set.")]
		public bool AutoPoll {
			get {
				return _autoPoll;
			}
			set {
				_autoPoll = value;
			}
		}
		bool _autoPoll = false;
		
		private Thread pollThread = null;
		private List<NxtSensor> pollList = null;

		/// <summary>
		/// Starts polling the sensors continuously
		/// </summary>
		public void StartPolling() {
			lock(this) {
				if(pollThread == null) {
					pollList = new List<NxtSensor>();
					foreach(NxtSensor sensor in ConnectedSensors()) {
						if(sensor.AutoPoll) {
							pollList.Add(sensor);
						}
					}

					if(pollList.Count > 0) {
						pollThread = new Thread(new ThreadStart(pollLoop));
						pollThread.Name = "SensorPoller";
						pollThread.IsBackground = true;
						pollThread.Start();
					}
				}
			}
		}

		/// <summary>
		/// Terminates the thread poll queue
		/// </summary>
		public void StopPolling() {
			lock(this) {
				if(pollThread != null) {
					pollThread.Abort();
					// Wait until polling thread is stopped
					while(pollThread.IsAlive) {
						Thread.Sleep(100);
					}
					pollThread = null;
				}
			}
		}

		/// <summary>
		/// Continuously polls sensors
		/// </summary>
		private void pollLoop() {
			try {
				while(true) {
					foreach(NxtSensor sensor in pollList) {
						if(sensor.LastPollTimestamp + sensor.AutoPollDelay <= Util.MilliSeconds()) {
							sensor.Poll();
							Thread.Sleep(0);
						}
					}
					Thread.Sleep(0); // Allow context switch
				}
			}
			catch(ThreadAbortException) {
			}
		}



		#endregion
	}
}
