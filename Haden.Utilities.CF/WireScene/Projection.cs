using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Haden.Utilities.CF.Geometry;

namespace Haden.Utilities.CF.WireScene {
	public abstract class Projection {
		public abstract Point Transform(Vector3 p);
	}

	public class ProjectionXY : Projection {
		public override Point Transform(Vector3 p) {
			return p.ToPointXY();
		}
	}

	public class ProjectionXZ : Projection {
		public override Point Transform(Vector3 p) {
			return p.ToPointXZ();
		}
	}

	public class ProjectionYZ : Projection {
		public override Point Transform(Vector3 p) {
			return p.ToPointYZ();
		}
	}
}
