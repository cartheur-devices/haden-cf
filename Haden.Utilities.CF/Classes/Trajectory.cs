using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Haden.Utilities.CF.Geometry {
	public interface ITrajectory {
		/// <summary>
		/// Projects the specified point on this
		/// piece. Returns the parameter 't'.
		/// 
		/// t=0 being the start, t=1 being 
		/// the end.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		double Project(Vector3 point);

		/// <summary>
		/// returns the length of this trajectory
		/// </summary>
		double Length {
			get;
		}

		/// <summary>
		/// returns a point on this piece
		/// </summary>
		/// <param name="t">t=0 being the start and t=1 being the end.</param>
		/// <returns></returns>
		Vector3 GetPositionByT(double t);

		/// <summary>
		/// returns a direction on this piece
		/// </summary>
		/// <param name="t">t=[0,1], 0 being the start and 1 being the end.</param>
		/// <returns></returns>
		Vector3 GetDirectionByT(double t);

		/// <summary>
		/// Follows the specified distance along this
		/// trajectory
		/// </summary>
		/// <param name="position"></param>
		/// <param name="direction"></param>
		/// <param name="distance"></param>
		/// <param name="newPosition"></param>
		/// <param name="newDirection"></param>
		/// <param name="newDistance"></param>
		void Follow(Vector3 position, Vector3 direction, double distance, out Vector3 newPosition, out Vector3 newDirection, out double distanceLeft);

		/// <summary>
		/// Projects the specified point on this trajectory
		/// and returns the closest point _still on this track_
		/// </summary>
		/// <param name="point"></param>
		/// <param name="projection"></param>
		/// <param name="distance"></param>
		void GetClosestPointOnTrajectory(Vector3 point, out Vector3 projection);
	}

	public abstract class BaseTrajectory : ITrajectory {
		public abstract double Project(Vector3 point);

		public abstract double Length {
			get;
		}

		public abstract Vector3 GetPositionByT(double t);

		public abstract Vector3 GetDirectionByT(double t);

		public double GetDistanceByT(double t) {
			Debug.Assert(t >= 0.0 && t <= 1.0, "t must be within [0,1]");
			return Length * t;
		}

		public void GetClosestPointOnTrajectory(Vector3 point, out Vector3 projection) {
			double t = Project(point);
			t = Math.Max(0.0, Math.Min(1.0, t));
			projection = GetPositionByT(t);
		}


		public void Follow(Vector3 position, Vector3 direction, double distance, out Vector3 newPosition, out Vector3 newDirection, out double newDistance) {
			double t = Project(position);
			Vector3 pPosition = GetPositionByT(t);
			Vector3 pDirection = GetDirectionByT(t);
			double pDistance = GetDistanceByT(t);
			double dot = direction.Normalize() * pDirection.Normalize();
			Debug.Assert(pPosition.DistanceTo(position) < 0.1, "Can follow from position " + position.ToString() + "; too far from trajectory");
			Debug.Assert(Math.Abs(dot) > 0.9, "dot product should be close to |1|");
			if(dot > 0) {
				// traveling along the direction of the piece
				pDistance += distance;
				if(pDistance > Length) {
					newDistance = pDistance - Length;
					t = 1.0;
				} else {
					newDistance = 0;
					t = pDistance / Length;
				}
			} else {
				// traveling against the direction of the piece
				pDistance -= distance;
				if(pDistance < 0) {
					newDistance = -pDistance;
					t = 0.0;
				} else {
					newDistance = 0;
					t = pDistance / Length;
				}
			}
			newDirection = GetDirectionByT(t);
			newPosition = GetPositionByT(t);
		}
	}

	public class LineTrajectory : BaseTrajectory {
		private Vector3 start, end, delta, direction;

		public LineTrajectory(Vector3 start, Vector3 end) {
			this.start = start;
			this.end = end;
			delta = end - start;
			direction = delta.Normalize();
		}

		public override double Project(Vector3 point) {
			Vector3 v = point - start;
			return delta.Project(v);
		}

		public override double Length {
			get {
				return delta.Length;
			}
		}

		public override Vector3 GetPositionByT(double t) {
			return start + delta * t;
		}

		public override Vector3 GetDirectionByT(double t) {
			return direction;
		}
	}

	public class SemiCircleTrajectory : BaseTrajectory {
		private Vector3 center, normal;
		private double radius, angle1, angle2, dAngle;


		public SemiCircleTrajectory(Vector3 center, double radius, double angle1, double angle2, Vector3 normal) {
			this.center = center;
			this.normal = normal.Normalize();
			this.radius = radius;
			this.angle1 = angle1;
			this.angle2 = angle2;
			this.dAngle = angle2 - angle1;
		}

		public SemiCircleTrajectory(Vector3 center, double radius, double angle1, double angle2) : this(center, radius, angle1, angle2, Vector3.UnitZ) { }

		public override double Project(Vector3 point) {
			return 0;
		}

		public override double Length {
			get {
				return Math.Abs(dAngle * radius);
			}
		}

		public override Vector3 GetPositionByT(double t) {
			double angle = ((angle2 - angle1) * t) + angle1;
			Vector3 p = new Vector3(Math.Cos(angle), Math.Sin(angle), 0) * radius;
			return center + p;
		}

		public override Vector3 GetDirectionByT(double t) {
			double angle = ((angle2 - angle1) * t) + angle1;
			Vector3 p = new Vector3(-Math.Sin(angle), Math.Cos(angle), 0);
			return p;
		}
	}

}
