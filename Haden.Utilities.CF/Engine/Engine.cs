using System;
using System.Collections.Generic;
using System.Text;

namespace Haden.Utilities.CF.Engine {
	public class Engine {
		List<ITickable> objects = new List<ITickable>();
		List<ITickable> addedObjects = new List<ITickable>();
		List<ITickable> deletedObjects = new List<ITickable>();

		/// <summary>
		/// Add a object to the simulation
		/// </summary>
		/// <param name="obj"></param>
		public void Add(ITickable obj) {
			addedObjects.Add(obj);
			obj.NotifyAdded(this);
		}

		private void addObjects() {
			objects.AddRange(addedObjects);
			addedObjects.Clear();
		}

		/// <summary>
		/// Remove an object from the simulation
		/// </summary>
		/// <param name="obj"></param>
		public void Delete(ITickable obj) {
			deletedObjects.Remove(obj);
			obj.NotifyRemoved(this);
		}

		private void deleteObjects() {
			foreach(ITickable obj in deletedObjects) {
				objects.Remove(obj);
			}
			deletedObjects.Clear();
		}


		public void Tick() {
			deleteObjects();
			addObjects();

			long currentFrameStart = Util.MilliSeconds();
			double deltaTime = 0;
			if(previousFrameStart > 0) {
				long delta = currentFrameStart - previousFrameStart;
				deltaTime = delta / 1000.0f;
			}
						
			foreach(ITickable obj in objects) {
				obj.Tick(deltaTime);	
			}
		}

		long previousFrameStart = 0;

		
	}
}
