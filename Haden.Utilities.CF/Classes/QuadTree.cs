using System;
using System.Collections.Generic;
using System.Text;

namespace Haden.Utilities.CF.Geometry {
	/// <summary>
	/// A QuadTree 
	/// </summary>
	public class Quadtree<T> : QuadNode<T> where T: IQuadTreeMember {
		public Quadtree(Rectangle2D region) : base(region, 8) {
		}

		#region Searching

		private int searchGeneration;

		public IEnumerable<T> Find(IIntersectsRectangle2D r) {
			foreach(T item in Find(r, searchGeneration++)) {
				yield return item;
			}
		}
		#endregion

		#region Adding

		#endregion

	}

	public class QuadNode<T> where T : IQuadTreeMember {
		protected Rectangle2D Region {
			get {
				return _region;
			}
		}
		Rectangle2D _region;
		int _depthLeft;

		public QuadNode(Rectangle2D region, int depthLeft) {
			_region = region;
			_depthLeft = depthLeft;
		}

		protected QuadNode<T>[,] nodes;
		protected List<T> members = new List<T>();

		protected bool IsLeaf {
			get {
				return nodes == null;
			}
		}

		protected IEnumerable<T> Find(IIntersectsRectangle2D r, int generation) {
			if(r.Intersects(Region)) {
				if(IsLeaf) {
					foreach(T member in members) {
						if(generation > member.LastSearchHit) {
							member.LastSearchHit = generation;
							yield return member;
						}
					}
				} else {
					foreach(QuadNode<T> node in nodes) {
						foreach(T member in node.Find(r, generation)) {
							yield return member;
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks if this node should split into 4 nodes
		/// </summary>
		protected void CheckSplit() {
			if(_depthLeft > 0 && members != null && members.Count > 20) {
				// There are too many nodes
				Rectangle2D[, ] rectangles = Region.Quarter();
				nodes = new QuadNode<T>[2, 2];
				for(int y = 0; y < 2; y++) {
					for(int x = 0; x < 2; x++) {
						nodes[x, y] = new QuadNode<T>(rectangles[x, y], _depthLeft - 1);
						foreach(T member in members) {
							if(member.BoundingBox.Intersects(nodes[x, y].Region)) {
								nodes[x, y].Add(member);
							}
						}
					}
				}
				members = null;
			}
		}

		protected void Add(T member) {
			if(IsLeaf) {
				if(member.BoundingBox.Intersects(Region)) {
					members.Add(member);
					CheckSplit();
				}
			} else {
				foreach(QuadNode<T> node in nodes) {
					node.Add(member);
				}
			}
		}
	}

	public interface IQuadTreeMember {
		/// <summary>
		/// This is used to prevent 
		/// </summary>
		int LastSearchHit {
			get;
			set;
		}

		IIntersectsRectangle2D BoundingBox {
			get;
		}
	}

	public interface IIntersectsRectangle2D {
		bool Intersects(Rectangle2D rectangle);
	}


	public struct Rectangle2D : IIntersectsRectangle2D {
		public Rectangle2D(double x1, double y1, double x2, double y2) {
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}

		private double x1, x2, y1, y2;
		public double MinX {
			get {
				return Math.Min(x1, x2);
			}
		}
		public double MaxX {
			get {
				return Math.Max(x1, x2);
			}
		}
		public double MinY {
			get {
				return Math.Min(y1, y2);
			}
		}
		public double MaxY {
			get {
				return Math.Max(y1, y2);
			}
		}

		public bool Intersects(Rectangle2D rectangle) {
			if(rectangle.MaxX < MinX) {
				return false;
			}
			if(rectangle.MinX > MaxX) {
				return false;
			}
			if(rectangle.MaxY < MinY) {
				return false;
			}
			if(rectangle.MinY > MaxY) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Divides this rectangle in 4 equal-sized rectangles
		/// </summary>
		/// <returns></returns>
		public Rectangle2D[, ] Quarter() {
			Rectangle2D[, ] result = new Rectangle2D[2,2];
			double hMiddle = (MaxY + MinY) / 2;
			double vMiddle = (MaxX + MinX) / 2;
			result[0, 0] = new Rectangle2D(MinX, MinY, hMiddle, vMiddle);
			result[0, 1] = new Rectangle2D(MinX, vMiddle, hMiddle, MaxY);
			result[1, 0] = new Rectangle2D(hMiddle, MinY, MaxX, vMiddle);
			result[1, 1] = new Rectangle2D(hMiddle, vMiddle, MaxX, MaxY);
			return result;
		}
	}
}
