using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Haden.Utilities.CF.Geometry;

namespace Haden.Utilities.CF.Engine {
	/// <summary>
	/// An  actor represents a first-class citizen of the
	/// Engine world
	/// </summary>
	public class Actor : ITickable, IDrawable, IMovable {
		/// <summary>
		/// Draws this pawn to the specified graphics
		/// </summary>
		/// <param name="g"></param>
		public virtual void Draw(Graphics g) {

		}

		public void NotifyAdded() { }

		/// <summary>
		/// Ticks this actor
		/// </summary>
		/// <param name="deltaTime"></param>
		public virtual void Tick(double deltaTime) {
			Velocity += deltaTime * Acceleration;
			Location += deltaTime * Velocity;
		}

		public void NotifyAdded(Engine engine) {
		}

		public void NotifyRemoved(Engine engine) {
		}

		#region Location, Velocity, Acceleration

		/// <summary>
		/// 
		/// </summary>
		public Vector3 Location {
			get {
				return _location;
			}
			set {
				_location = value;
			}
		}
		Vector3 _location;

		/// <summary>
		/// 
		/// </summary>
		public Vector3 Velocity {
			get {
				return _velocity;
			}
			set {
				_velocity = value;
			}
		}
		Vector3 _velocity;

		/// <summary>
		/// 
		/// </summary>
		public Vector3 Acceleration {
			get {
				return _acceleration;
			}
			set {
				_acceleration = value;
			}
		}
		Vector3 _acceleration;

		#endregion
	}
}
