using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Haden.Utilities;

namespace Haden.NxtSharp {
	
	/// <summary>
	/// This class allows communication with a single Lego NXT brick.
	/// </summary>
	public class NxtCommunicator {
		/// <summary>
		/// Constructs a LEGO communicator with the specified portname
		/// </summary>
		/// <param name="portname"></param>
		public NxtCommunicator(string portname) {
			_portName = portname;
		}
		private string _portName;

		#region Connection handling

		private SerialPort port = null;

		/// <summary>
		/// Connects to the COM port associated with this communicator
		/// </summary>
		public void Connect()
        {
			if(port != null)
            {
				Disconnect();
			}
			port = new SerialPort(_portName);
			port.Open();
			IsConnected = true;
		}

		/// <summary>
		/// Disconnects the communicator class
		/// </summary>
        public void Disconnect()
        {
            IsConnected = false;
            if (port.IsOpen)
            {
                port.Close();
                port.Dispose();
            }
            port = null;
        }

		
		/// <summary>
		/// Are we connected to the Lego NXT Brick?
		/// </summary>
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            private set
            {
                _isConnected = value;
            }
        }
        bool _isConnected = false;

		#endregion

		#region Communication primitives

		/// <summary>
		/// Sends the specified message to the port.
		/// </summary>
		/// <param name="message"></param>
		private byte[] sendMessage(byte[] message) {
			if(!IsConnected) {
				throw new InvalidOperationException("Can't send message - not connected");
			} else {
				lock(this) {
					int length = message.Length;
					byte[] btMessage = new byte[message.Length + 2];
					btMessage[0] = (byte)(length & 0xFF);
					btMessage[1] = (byte)((length & 0xFF00) >> 8);
					message.CopyTo(btMessage, 2);
					port.Write(btMessage, 0, btMessage.Length);

					if(message[0] < 0x80) {
						// A reply is expected. Check it.
						int lsb = port.ReadByte();
						int msb = port.ReadByte();
						int size = lsb + msb * 256;
						byte[] reply = new byte[size];
						port.Read(reply, 0, size);

						if(reply[0] != 0x02) {
							throw new Exception("Unexpected return message type: " + Util.Hex(reply[0]));
						}

						if(reply[1] != message[1]) {
							throw new Exception("Unexpected return command: " + Util.Hex(reply[2]));
						}
						if(reply[2] > 0) {
							NxtMessageResult error = (NxtMessageResult)reply[2];
							string errorMessage = "Nxt Error: " + error + " in reply to command " + Util.Hex(message);

							switch(error) {
								case NxtMessageResult.ChannelBusy:
									throw new NxtChannelBusyException(errorMessage);

								case NxtMessageResult.CommBusError:
									throw new NxtCommBusErrorException(errorMessage);

								default:
									throw new ApplicationException(errorMessage);

							}
						}

						return reply;
					} else {
						return null;
					}
				}
			}
		}


		#endregion

		#region System Commands

		/// <summary>
		/// Sets the name of the LEGO brick.
		/// </summary>
		/// <param name="name"></param>
		public void SetBrickName(string name) {
			if(name.Length > 14) {
				name = name.Substring(0, 14);
			}
			byte[] nameBytes = System.Text.Encoding.ASCII.GetBytes(name);
			byte[] message = new byte[18];
			message[0] = 0x01;	// Do not expect an answer
			message[1] = (byte)NxtCommand.SetBrickName;	// SetBrickName Command ID
			nameBytes.CopyTo(message, 2);
			sendMessage(message);
		}

		#endregion

		#region Direct Commands

		#region Motor Commands

		/// <summary>
		/// Sends a drive command to one of the motors
		/// </summary>
		/// <param name="port"></param>
		/// <param name="power"></param>
		/// <param name="mode"></param>
		/// <param name="regulationMode"></param>
		/// <param name="speed"></param>
		/// <param name="runState"></param>
		/// <param name="tachoLimit"></param>
		public void SetOutputState(NxtMotorPort port, sbyte power, NxtMotorMode mode, NxtMotorRegulationMode regulationMode, sbyte turnRatio, NxtMotorRunState runState, uint tachoLimit) {
			byte[] message = new byte[12];
			message[0] = 0x80;
			message[1] = (byte)NxtCommand.SetOutputState;
			message[2] = (byte)port;
			message[3] = (byte)power;
			message[4] = (byte)mode;
			message[5] = (byte)regulationMode;
			message[6] = (byte)turnRatio;
			message[7] = (byte)runState;
			Util.SetUInt32(message, 8, tachoLimit);
			sendMessage(message);
		}


		/// <summary>
		/// Gets the state of one of the motors
		/// </summary>
		/// <param name="port"></param>
		/// <param name="power"></param>
		/// <param name="mode"></param>
		/// <param name="regulationMode"></param>
		/// <param name="turnRatio"></param>
		/// <param name="runState"></param>
		/// <param name="tachoLimit"></param>
		/// <param name="tachoCount"></param>
		/// <param name="blockTachoCount"></param>
		/// <param name="rotationCount"></param>
		public NxtGetOutputState GetOutputState(NxtMotorPort port) {
			byte[] message = new byte[3];
			message[0] = 0x00;
			message[1] = (byte)NxtCommand.GetOutputState;
			message[2] = (byte)port;
			byte[] reply = sendMessage(message);
			NxtGetOutputState result = new NxtGetOutputState();
			result.Power = (sbyte)reply[4];
			result.Mode = (NxtMotorMode)reply[5];
			result.RegulationMode = (NxtMotorRegulationMode)reply[6];
			result.TurnRatio = (sbyte)reply[7];
			result.RunState = (NxtMotorRunState)reply[8];
			result.TachoLimit = Util.GetUInt32(reply, 9);
			result.TachoCount = Util.GetInt32(reply, 13);
			result.BlockTachoCount = Util.GetInt32(reply, 17);
			result.RotationCount = Util.GetInt32(reply, 21);
			return result;
		}

		public void ResetMotorPosition(NxtMotorPort port, bool relative) {
			byte[] message = new byte[4];
			message[0] = 0x80; // do not expect an answer
			message[1] = (byte)NxtCommand.ResetMotorPosition;
			message[2] = (byte)port;
			if(relative) {
				message[3] = 1;
			} else {
				message[3] = 0;
			}
			sendMessage(message);

		}

		#endregion

		#region Sensor commands

		/// <summary>
		/// 
		/// </summary>
		/// <param name="port"></param>
		/// <param name="type"></param>
		/// <param name="mode"></param>
		public void SetInputMode(NxtSensorPort port, NxtSensorType type, NxtSensorMode mode) {
			byte[] message = new byte[5];
			message[0] = (byte)NxtCommandType.DirectCommandWithResponse;
			message[1] = (byte)NxtCommand.SetInputMode;
			message[2] = (byte)port;
			message[3] = (byte)type;
			message[4] = (byte)mode;
			sendMessage(message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public NxtGetInputValues GetInputValues(NxtSensorPort port) {
			NxtGetInputValues result = new NxtGetInputValues();
			byte[] message = new byte[3];
			message[0] = 0x00; // Expect an answer
			message[1] = (byte)NxtCommand.GetInputValues;
			message[2] = (byte)port;
			byte[] reply = sendMessage(message);
			result.Valid = (reply[4] == 1) ? true : false;
			result.Calibrated = (reply[5] == 1) ? true : false;
			result.Type = (NxtSensorType)reply[6];
			result.Mode = (NxtSensorMode)reply[7];
			result.RawAD = Util.GetUInt16(reply, 8);
			result.NormalizedAD = Util.GetUInt16(reply, 10);
			result.ScaledValue = Util.GetInt16(reply, 12);
			result.CalibratedValue = Util.GetInt16(reply, 14);
			return result;
		}

		#endregion

		#region I2C (Low speed bus) Commands

		/// <summary>
		/// Reads the amount of bytes ready at a port on the I2C (Low speed)
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public int LSGetStatus(NxtSensorPort port) {
			byte[] message = new byte[3];
			message[0] = (byte)NxtCommandType.DirectCommandWithResponse;
			message[1] = (byte)NxtCommand.LSGetStatus;
			message[2] = (byte)port;
			byte[] reply = sendMessage(message);
			return reply[3];
		}


		/// <summary>
		/// Writes a message to a port on the I2C (Low speed)
		/// </summary>
		/// <param name="port"></param>
		/// <param name="data"></param>
		/// <param name="returnMessageLength">the length of the return message (you must specify this)</param>
		public void LSWrite(NxtSensorPort port, byte[] data, int returnMessageLength) {
			if(data.Length < 1 || data.Length > 16) {
				throw new InvalidOperationException("data length must be between 1 and 16 bytes");
			}
			
			byte[] message = new byte[5 + data.Length];
			message[0] = (byte)NxtCommandType.DirectCommandWithResponse;
			message[1] = (byte)NxtCommand.LSWrite;
			message[2] = (byte)port;
			message[3] = (byte)data.Length;
			message[4] = (byte)returnMessageLength;
			Array.Copy(data, 0, message, 5, data.Length);
			sendMessage(message);
		}

		/// <summary>
		/// Reads all bytes from a sensor port using the I2C (low speed) bus
		/// </summary>
		/// <returns></returns>
		public byte[] LSRead(NxtSensorPort port) {
			byte[] message = new byte[3];
			message[0] = (byte)NxtCommandType.DirectCommandWithResponse;
			message[1] = (byte)NxtCommand.LSRead;
			message[2] = (byte)port;
			byte[] reply = sendMessage(message);
			int length = (int)reply[3];
			byte[] result = new byte[length];
			Array.Copy(reply, 4, result, 0, length);
			return result;
		}


		#endregion

		#region Misc Commands

		/// <summary>
		/// Send keepalive signal to Lego Brick
		/// </summary>
		/// <param name="name"></param>
		public void KeepAlive() {
			byte[] message = new byte[2];
			message[0] = 0x80;	// Do not expect an answer
			message[1] = (byte)NxtCommand.KeepAlive;	// KeepAlive Command ID
			sendMessage(message);
		}

		/// <summary>
		/// Retrieves the battery level
		/// </summary>
		/// <returns>the battery level, in millivolts</returns>
		public int GetBatteryLevel() {
			byte[] message = new byte[2];
			message[0] = 0x00;	// Expect an answer
			message[1] = (byte)NxtCommand.GetBatteryLevel;	// GetBatteryLevel Command ID
			byte[] reply = sendMessage(message);
			return Util.GetUInt16(reply, 3);
		}

		#endregion


		#region Bluetooth messages
		
		#region Sending

		/// <summary>
		/// Sends a message to a bluetooth mailbox
		/// </summary>
		/// <param name="mailbox">The mailbox number [0-9]</param>
		/// <param name="data"></param>
		public void MessageWrite(byte mailbox, byte[] data) {
			if(data.Length > 57) {
				throw new ArgumentException("Data size must be less than 58 bytes");
			}
			byte[] message = new byte[5 + data.Length];
			message[0] = 0x80; // do not expect an answer
			message[1] = (byte)NxtCommand.MessageWrite;
			message[2] = mailbox;
			message[3] = (byte)(data.Length + 1);
			for(int i = 0; i < data.Length; i++) {
				message[i + 4] = data[i];
			}
			sendMessage(message);
		}

		/// <summary>
		/// Sends a string to a bluetooth mailbox
		/// </summary>
		/// <param name="mailbox"></param>
		/// <param name="data"></param>
		public void MessageWrite(byte mailbox, string value) {
			MessageWrite(mailbox, Encoding.ASCII.GetBytes(value));
		}

		/// <summary>
		/// Sends a number to a bluetooth mailbox
		/// </summary>
		/// <param name="mailbox"></param>
		/// <param name="data"></param>
		public void MessageWrite(byte mailbox, int value) {
			byte[] data = new byte[4];
			Util.SetInt32(data, 0, value);
			MessageWrite(mailbox, data);
		}

		/// <summary>
		/// Sends a boolean to a bluetooth mailbox
		/// </summary>
		/// <param name="mailbox"></param>
		/// <param name="data"></param>
		public void MessageWrite(byte mailbox, bool value) {
			byte[] data = new byte[1];
			data[0] = (byte)(value ? 0x01 : 0x00);
			MessageWrite(mailbox, data);
		}

		#endregion

		#region Receiving

		/// <summary>
		/// Reads data from the specified mailbox
		/// </summary>
		/// <param name="mailbox">mailbox on the NXT [0-9]</param>
		/// <returns></returns>
		public byte[] MessageRead(byte mailbox) {
			byte[] message = new byte[5];
			message[0] = 0x00; // Expect an answer
			message[1] = (byte)NxtCommand.MessageRead;
			message[2] = (byte)(mailbox + 10);
			message[3] = (byte)(mailbox + 10);
			message[4] = 0x01;
			byte[] reply = sendMessage(message);
			int size = reply[4];
			byte[] result = new byte[size];
			Array.Copy(reply, 5, result, 0, size);
			return result;
		}

		/// <summary>
		/// Reads a string from the specified mailbox
		/// </summary>
		/// <param name="mailbox">The mailbox [0-9]</param>
		/// <returns></returns>
		public string MessageReadString(byte mailbox) {
			byte[] data = MessageRead(mailbox);
			return Encoding.ASCII.GetString(data, 0, data.Length - 1);
		}

		/// <summary>
		/// Reads an int from the specified mailbox
		/// </summary>
		/// <param name="mailbox">The mailbox [0-9]</param>
		/// <returns></returns>
		public int MessageReadInt(byte mailbox) {
			byte[] data = MessageRead(mailbox);
			return Util.GetInt32(data, 0);
		}

		/// <summary>
		/// Reads a bool from the specified mailbox
		/// </summary>
		/// <param name="mailbox">The mailbox [0-9]</param>
		/// <returns></returns>
		public bool MessageReadBool(byte mailbox) {
			byte[] data = MessageRead(mailbox);
			return data[0] != 0;
		}

		#endregion

		#endregion



		#endregion

		#region I2C Command Helpers

		/// <summary>
		/// Writes a byte on the I2C (Low Speed) interface
		/// </summary>
		/// <param name="port"></param>
		/// <param name="address"></param>
		/// <param name="value"></param>
		public void I2CSetByte(NxtSensorPort port, byte address, byte value) {
			byte[] i2cCmd = new byte[3];
			i2cCmd[0] = 0x02;
			i2cCmd[1] = address;
			i2cCmd[2] = value;
			LSWrite(port, i2cCmd, 0);
		}

		/// <summary>
		/// Reads a byte from the I2C (low speed) interface
		/// </summary>
		/// <param name="port"></param>
		/// <param name="address"></param>
		/// <returns></returns>
		public byte I2CGetByte(NxtSensorPort port, byte address) {
			byte[] i2cCmd = new byte[2];
			i2cCmd[0] = 0x02;
			i2cCmd[1] = address;

			int bytesRead = 0;
			do {
				LSWrite(port, i2cCmd, 1);
				try {
					bytesRead = LSGetStatus(port);
				}
				catch(NxtCommBusErrorException) {
					bytesRead = 0;
				}
			} while(bytesRead < 1);

			return LSRead(port)[0];
		}

		#endregion


	}

	public struct NxtGetOutputState {
		public sbyte Power;
		public NxtMotorMode Mode;
		public NxtMotorRegulationMode RegulationMode;
		public sbyte TurnRatio;
		public NxtMotorRunState RunState;
		public uint TachoLimit;
		public int TachoCount;
		public int BlockTachoCount;
		public int RotationCount;
	}

	public struct NxtGetInputValues {
		public bool Valid;
		public bool Calibrated;
		public NxtSensorType Type;
		public NxtSensorMode Mode;
		public UInt16 RawAD;
		public UInt16 NormalizedAD;
		public Int16 ScaledValue;
		public Int16 CalibratedValue;
	}


	#region Protocol Enumerations and Constants

	public enum NxtCommand : byte {
		StartProgram = 0x00,
		StopProgram = 0x01,
		PlaySoundFile = 0x02,
		PlayTone = 0x03,
		SetOutputState = 0x04,
		SetInputMode = 0x05,
		GetOutputState = 0x06,
		GetInputValues = 0x07,
		MessageWrite = 0x09,
		ResetMotorPosition = 0x0A,
		GetBatteryLevel = 0x0B,
		KeepAlive = 0x0D,
		LSGetStatus = 0x0E,
		LSWrite = 0x0F,
		LSRead = 0x10,
		MessageRead = 0x13,
		OpenRead = 0x80,
		OpenWrite = 0x81,
		Read = 0x82,
		Write = 0x83,
		SetBrickName = 0x98
	}

	public enum NxtMessageResult : byte {
		OK = 0x00,
		PendingCommunicationInProgress = 0x20,
		SpecifiedMailBoxEmpty = 0x40,
		RequestFailed = 0xBD,
		UnknownCommand = 0xBE,
		InsanePacket = 0xBF,
		publicOfRangeData = 0xC0,
		CommBusError = 0xDD,
		publicOfMemoryInCommunicationBuffer = 0xDE,
		ChannelInvalid = 0xDF,
		ChannelBusy = 0xE0,
		NoActiveProgram = 0xEC,
		IllegalSizeSpecified = 0xED,
		IllegalMailboxID = 0xEE,
		InvalidFieldAccess = 0xEF,
		BadData = 0xF0,
		publicOfMemory = 0xFB,
		BadArguments = 0xFF
	}

	[Flags()]
	public enum NxtMotorMode : byte {
		None = 0x00,
		/// <summary>
		/// Should the motor be turned on?
		/// </summary>
		MotorOn = 0x01,
		/// <summary>
		/// Should the motor break after the action is completed?
		/// </summary>
		Brake = 0x02,
		/// <summary>
		/// Should motor regulation be used? (use 
		/// NxtMotorRegulationMode to specify which one)
		/// </summary>
		Regulated = 0x04
	}

	[Flags]
	public enum NxtMotorRegulationMode : byte {
		/// <summary>
		/// Use when motor usage is not regulated.
		/// </summary>
		Idle = 0x00,
		/// <summary>
		/// Use this to regulate speed, whatever that may be.
		/// </summary>
		MotorSpeed = 0x01,
		/// <summary>
		/// Use this to synchronize two motors
		/// </summary>
		MotorSynchronization = 0x02
	}

	[Flags]
	public enum NxtMotorRunState : byte {
		Idle = 0x00,
		RampUp = 0x10,
		Running = 0x20,
		Rampdown = 0x40
	}

	public enum NxtMotorPort {
		/// <summary>
		/// Port A (Auxiliary motor)
		/// </summary>
		PortA = 0x00,
		/// <summary>
		/// Port B (Drive motor 1)
		/// </summary>
		PortB = 0x01,
		/// <summary>
		/// Port C (Drive motor 2)
		/// </summary>
		PortC = 0x02,
		/// <summary>
		/// No port
		/// </summary>
		None = 0xFE,
		/// <summary>
		/// All motors
		/// </summary>
		All = 0xFF
	}

	public enum NxtSensorPort {
		/// <summary>
		/// Port 1 (Push sensor)
		/// </summary>
		Port1 = 0x00,
		Port2 = 0x01,
		Port3 = 0x02,
		Port4 = 0x03,
		None = 0xFE
	}


	public enum NxtSensorType {
		/// <summary>
		/// No sensor
		/// </summary>
		None = 0x00,
		/// <summary>
		/// Pressure switch, like the one supplied in the Mindstorms NXT set
		/// </summary>
		Switch = 0x01,
		Temperature = 0x02,
		/// <summary>
		/// Sonar???
		/// </summary>
		Reflection = 0x03,
		Angle = 0x04,
		/// <summary>
		/// Light sensor, the sensor also generates light
		/// </summary>
		LightActive = 0x05,
		/// <summary>
		/// Light sensor, the sensor relies on external light sources
		/// </summary>
		LightInactive = 0x06,
		/// <summary>
		/// Sound (Decibel)
		/// </summary>
		SoundDB = 0x07,
		/// <summary>
		/// Sound (Decibel, adjusted for the human ear)
		/// </summary>
		SoundDBA = 0x08,
		Custom = 0x09,
		LowSpeed = 0x0A,
		LowSpeed_9V = 0x0B
	}

	public enum NxtSensorMode {
		Raw = 0x00,
		Boolean = 0x20,
		TransitionCounter = 0x40,
		PeriodCounter = 0x60,
		Percentage = 0x80,
		Celsius = 0xA0,
		Fahrenheit = 0xC0,
		AngleStep = 0xE0
	}

	public enum NxtCommandType {
		/// <summary>
		/// Direct command, the NXT will send a response
		/// </summary>
		DirectCommandWithResponse = 0x00,
		/// <summary>
		/// System command, the NXT will send a response
		/// </summary>
		SystemCommandWithResponse = 0x01,
		/// <summary>
		/// Reply
		/// </summary>
		Reply,
		/// <summary>
		/// Direct command, the NXT will NOT send a response
		/// </summary>
		DirectCommandNoResponse = 0x80,
		/// <summary>
		/// System command, the NXT will NOT send a response
		/// </summary>
		SystemCommandNoResponse = 0x81
	}


	#endregion

	#region Exceptions
	
	public class NxtChannelBusyException : ApplicationException {
		public NxtChannelBusyException(string message)
			: base(message) {
		}
	}

	public class NxtCommBusErrorException : ApplicationException {
		public NxtCommBusErrorException(string message)
			: base(message) {
		}
	}

	#endregion

}
