using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Haden.Utilities.CF.WireScene {
	public interface IRenderable {
		void Render(Graphics g, Projection p);
	}

	public class Scene {
		public Scene() {
			Graphics g = null;
			g.Transform = null;
			Matrix m = new Matrix();
		}

		List<Model> models = new List<Model>();

		public void Add(Model model) {
			models.Add(model);
		}

		public IEnumerable<Model> Models() {
			foreach(Model model in models) {
				yield return model;
			}
		}
	}
}
