using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Haden.Utilities.CF.Geometry;

namespace Haden.Utilities.CF.Engine {
	public interface ITickable {
		void Tick(double deltaTime);

		void NotifyAdded(Engine engine);

		void NotifyRemoved(Engine engine);
	}

	public interface IMovable {
		Vector3 Location {
			get;
			set;
		}

		Vector3 Velocity {
			get;
			set;
		}

		Vector3 Acceleration {
			get;
			set;
		}
	}

	public interface IDrawable {
		void Draw(Graphics g);
	}
}
