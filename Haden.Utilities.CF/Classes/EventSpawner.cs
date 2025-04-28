#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#endregion

namespace Haden.Utilities.CF {


	public class EventSpawner<T> {

		#region Public properties
		/// <summary>
		/// Returns the time of the first event
		/// </summary>
		/// <value></value>
		public DateTime FirstEventTime {
			get {
				return list.First.Value.Time;
			}
		}

		/// <summary>
		/// Returns the number of milliseconds to the first upcoming event
		/// </summary>
		/// <value></value>
		public int MilliSecondsToFirstEvent {
			get {
				if(list.Count == 0) {
					return Timeout.Infinite;
				} else {
					return (int)Util.MilliSecondsTo(FirstEventTime);
				}
			}
		}
		#endregion


		#region Private parts
		private AutoResetEvent signalNewEvent = new AutoResetEvent(false);
		private LinkedList<Event> list = new LinkedList<Event>();
		private Thread thread;
		#endregion


		/// <summary>
		/// Constructs a new EventSpawner
		/// </summary>
		/// <param name="recipient">Object that will receive these events</param>
		public EventSpawner(IEventRecipient<T> recipient) {
			Spawned += new EventSpawner<T>.SpawnedEvent(recipient.Received);

			// Start thread
			thread = new Thread(new ThreadStart(work));
			thread.Name = GetType().ToString();
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// Schedules an event
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="time"></param>
		public void Schedule(T obj, DateTime time) {
			Event e;
			e.obj = obj;
			e.Time = time;

			lock(list) {
				if(list.Count == 0 || time < FirstEventTime) {
					list.AddFirst(e);
					signalNewEvent.Set();
				} else {
					LinkedListNode<Event> node = list.First;

					while(node != list.Last) {
						if(node.Next.Value.Time > time)
							break;

						node = node.Next;
					}

					list.AddAfter(node, e);
				}
			}
		}

		/// <summary>
		/// Schedules an event
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="milliseconds"></param>
		public void Schedule(T obj, int milliseconds) {
			Schedule(obj, DateTime.Now.AddMilliseconds(milliseconds));
		}

		
		public void work() {
			while(true) {
				// Determine time to wait
				int timeOut = MilliSecondsToFirstEvent;

				// Wait
				signalNewEvent.WaitOne(timeOut, false);

				// Fire all current events
				lock(list) {
					while(list.Count > 0 && DateTime.Now >= list.First.Value.Time) {
						Event e = list.First.Value;

						// fire event
						if(Spawned != null) {
							Spawned(e.obj);
						}

						list.RemoveFirst();
					}
				}
			}
		}

		public event SpawnedEvent Spawned;

		#region Types

		/// <summary>
		/// A single event
		/// </summary>
		struct Event {
			/// <summary>
			/// The time at which the event should be spawned
			/// </summary>
			public DateTime Time;

			/// <summary>
			/// The associated object
			/// </summary>
			public T obj;
		}

		public delegate void SpawnedEvent(T o);
		#endregion

	}

	public interface IEventRecipient<T> {
		void Received(T o);
	}

}
