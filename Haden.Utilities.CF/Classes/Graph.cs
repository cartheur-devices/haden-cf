#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Haden.Utilities.CF.Graph {
	#region Base classes

	/// <summary>
	/// Base class for all graphes
	/// </summary>
	/// <typeparam name="NODETYPE"></typeparam>
	/// <typeparam name="EDGETYPE"></typeparam>
	public abstract class BaseGraph<NODETYPE, EDGETYPE> {
		#region Public properties
		public bool IsDirected {
			get {
				return _directed;
			}
		}
		private bool _directed;
		#endregion

		public BaseGraph(bool directed) {
			_directed = directed;
		}

		#region Public abstract interface
		public abstract Node<NODETYPE, EDGETYPE> AddNode(NODETYPE node);

		public abstract Edge<NODETYPE, EDGETYPE> AddEdge(NODETYPE from, NODETYPE to, double weight, EDGETYPE edge);

		public abstract IEnumerable<Node<NODETYPE, EDGETYPE>> AllNodes();

		public abstract IEnumerable<Edge<NODETYPE, EDGETYPE>> AllEdges();

		public abstract Node<NODETYPE, EDGETYPE> NodeByValue(NODETYPE value);
		#endregion

		#region Public methods
		/// <summary>
		/// Returns a string describing this graph
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			StringBuilder builder = new StringBuilder();

			foreach(Node<NODETYPE, EDGETYPE> node in AllNodes()) {
				builder.Append(node.ToString() + ": \r\n");
				foreach(Edge<NODETYPE, EDGETYPE> edge in node.ExitingEdges()) {

					builder.Append("  --> " + edge.OtherNode(node).ToString() + " (" + edge.Weight + ")\r\n");
				}
			}

			return builder.ToString();
		}

		#endregion

		#region Algorithms

		#region Depth First Search
		public IEnumerable<Node<NODETYPE, EDGETYPE>> DFS(Node<NODETYPE, EDGETYPE> start) {
			initDFS();
			foreach(Node<NODETYPE, EDGETYPE> node in start.DFS()) {
				yield return node;
			}
		}

		private void initDFS() {
			foreach(Node<NODETYPE, EDGETYPE> node in AllNodes()) {
				node.Visited = false;
			}
		}

		#endregion

		#region Dijkstra

		private LinkedList<Node<NODETYPE, EDGETYPE>> fringe;
		private LinkedList<Node<NODETYPE, EDGETYPE>> todo;

		/// <summary>
		/// Finds the shortest path from a node to another node using Dijkstra-Moore
		/// </summary>
		/// <param name="from">departure</param>
		/// <param name="to">destination</param>
		/// <param name="route">OUT parameter with the route, including from and to</param>
		/// <returns>the length of the shortest path</returns>
		public double ShortestPath(Node<NODETYPE, EDGETYPE> from, Node<NODETYPE, EDGETYPE> to, out LinkedList<Node<NODETYPE, EDGETYPE>> route) {
			if(from == to) {
				route = new LinkedList<Node<NODETYPE, EDGETYPE>>();
				route.AddLast(from);
				return 0;
			}

			initShortestPath(from);

			while(todo.Count > 0) {
				Node<NODETYPE, EDGETYPE> node = currentNearestNode();
				fringe.AddLast(node);
				todo.Remove(node);
				relax();
			}

			route = constructRoute(from, to);

			return to.Shortest;
		}

		/// <summary>
		/// Initializes the shortest path
		/// </summary>
		private void initShortestPath(Node<NODETYPE, EDGETYPE> from) {
			fringe = new LinkedList<Node<NODETYPE, EDGETYPE>>();
			todo = new LinkedList<Node<NODETYPE, EDGETYPE>>();


			foreach(Node<NODETYPE, EDGETYPE> node in AllNodes()) {
				node.Shortest = double.PositiveInfinity;
				todo.AddLast(node);
			}

			from.Shortest = 0;
		}

		private Node<NODETYPE, EDGETYPE> currentNearestNode() {
			Node<NODETYPE, EDGETYPE> current = null;
			double currentShortest = double.PositiveInfinity;
			foreach(Node<NODETYPE, EDGETYPE> node in todo) {
				if(node.Shortest < currentShortest)
					current = node;
			}
			return current;
		}

		private void relax() {
			foreach(Node<NODETYPE, EDGETYPE> node in todo) {
				foreach(Edge<NODETYPE, EDGETYPE> edge in node.ExitingEdges()) {
					Node<NODETYPE, EDGETYPE> other = edge.OtherNode(node);
					if(node.Shortest > other.Shortest + edge.Weight) {
						node.Shortest = other.Shortest + edge.Weight;
						node.Predecessor = other;
					}
				}
			}
		}

		private LinkedList<Node<NODETYPE, EDGETYPE>> constructRoute(Node<NODETYPE, EDGETYPE> from, Node<NODETYPE, EDGETYPE> to) {
			LinkedList<Node<NODETYPE, EDGETYPE>> route = new LinkedList<Node<NODETYPE, EDGETYPE>>();
			while(from != to) {
				route.AddFirst(to);
				to = to.Predecessor;
			}
			route.AddFirst(from);
			return route;
		}


		#endregion

		#endregion
	}

	/// <summary>
	/// Represents a node in the graph
	/// </summary>
	/// <typeparam name="NODEVALUETYPE"></typeparam>
	public abstract class Node<NODETYPE, EDGETYPE> {

		/// <summary>
		/// The value attached to this node
		/// </summary>
		/// <value></value>
		public NODETYPE Value {
			get {
				return _value;
			}
		}
		private NODETYPE _value;

		public double Shortest;							// For Dijkstra
		public Node<NODETYPE, EDGETYPE> Predecessor;	// For Dijkstra
		public bool Visited;							// For DFS, BFS

		public Node(NODETYPE value) {
			_value = value;
		}

		public abstract IEnumerable<Edge<NODETYPE, EDGETYPE>> ExitingEdges();

		public IEnumerable<Node<NODETYPE, EDGETYPE>> NeighbouringNodes() {
			foreach(Edge<NODETYPE, EDGETYPE> edge in ExitingEdges()) {
				yield return edge.OtherNode(this);
			}
		}

		public abstract bool IsConnectedTo(Node<NODETYPE, EDGETYPE> to);

		public abstract double DistanceTo(Node<NODETYPE, EDGETYPE> to);

		/// <summary>
		/// Returns a string representation of this Edge
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Value.ToString();
		}

		public IEnumerable<Node<NODETYPE, EDGETYPE>> DFS() {
			if(!Visited) {
				Visited = true;
				yield return this;
				foreach(Node<NODETYPE, EDGETYPE> neighbour in NeighbouringNodes()) {
					foreach(Node<NODETYPE, EDGETYPE> node in neighbour.DFS()) {
						yield return node;
					}
				}
			}
		}
	}


	/// <summary>
	/// Represents an edge in the graph
	/// </summary>
	/// <typeparam name="NODEVALUETYPE">Type of the object associated with a node</typeparam>
	/// <typeparam name="EDGEVALUETYPE">Type of the object associated with an edge</typeparam>
	public class Edge<NODETYPE, EDGETYPE> {
		/// <summary>
		/// Returns the weight of the edge
		/// </summary>
		/// <value></value>
		public double Weight {
			get {
				return _weight;
			}
		}
		private double _weight;

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public Node<NODETYPE, EDGETYPE> From {
			get {
				return _from;
			}
		}
		private Node<NODETYPE, EDGETYPE> _from;

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		public Node<NODETYPE, EDGETYPE> To {
			get {
				return _to;
			}
		}
		private Node<NODETYPE, EDGETYPE> _to;

		/// <summary>
		/// The value attached to this edge
		/// </summary>
		/// <value></value>
		public EDGETYPE Value {
			get {
				return _value;
			}
		}
		private EDGETYPE _value;

		public Edge(double weight, EDGETYPE value, Node<NODETYPE, EDGETYPE> from, Node<NODETYPE, EDGETYPE> to) {
			_weight = weight;
			_value = value;
			_from = from;
			_to = to;
		}

		/// <summary>
		/// Returns the other end of this edge
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public Node<NODETYPE, EDGETYPE> OtherNode(Node<NODETYPE, EDGETYPE> source) {
			if(source == From) {
				return To;
			} else {
				return From;
			}
		}

		/// <summary>
		/// Returns a string representation of this Edge
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Value.ToString();
		}
	}

	#endregion

	#region Adjacency matrix implementation

	public abstract class AdjacencyGraph<NODETYPE, EDGETYPE> : BaseGraph<NODETYPE, EDGETYPE> {
		/// <summary>
		/// returs the node list
		/// </summary>
		/// <value></value>
		public Node<NODETYPE, EDGETYPE>[] Node {
			get {
				return _node;
			}
		}
		private Node<NODETYPE, EDGETYPE>[] _node;


		/// <summary>
		/// Returns the number of nodes in this graph
		/// </summary>
		/// <value></value>
		public int Nodes {
			get {
				return _nodes;
			}
		}
		private int _nodes;

		/// <summary>
		/// Returns the edge matrix
		/// </summary>
		/// <value></value>
		public Edge<NODETYPE, EDGETYPE>[,] Edge {
			get {
				return _edge;
			}
		}
		private Edge<NODETYPE, EDGETYPE>[,] _edge;

		private Dictionary<NODETYPE, int> nodeMap = new Dictionary<NODETYPE, int>();

		public AdjacencyGraph(bool directed, int size) : base(directed) {
			_node = new Node<NODETYPE, EDGETYPE>[size];
			_edge = new Edge<NODETYPE, EDGETYPE>[size, size];
		}

		/// <summary>
		/// Adds an edge between node 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="weight"></param>
		/// <param name="edge"></param>
		public override Edge<NODETYPE, EDGETYPE> AddEdge(NODETYPE from, NODETYPE to, double weight, EDGETYPE edge) {
			int f = ((AdjacencyNode<NODETYPE, EDGETYPE>)NodeByValue(from)).ID;
			int t = ((AdjacencyNode<NODETYPE, EDGETYPE>)NodeByValue(to)).ID;


			if(!IsDirected) {
				if(t < f) {
					int tmp = t;
					t = f;
					f = tmp;
				}
			}

			Edge[f, t] = new AdjacencyEdge<NODETYPE, EDGETYPE>(weight, edge, Node[f], Node[t]);
			return Edge[f, t];
		}
		
		/// <summary>
		/// Adds a node
		/// </summary>
		/// <returns></returns>
		public override Node<NODETYPE, EDGETYPE> AddNode(NODETYPE value) {
			int current = _nodes;
			_nodes++;

			_node[current] = new AdjacencyNode<NODETYPE, EDGETYPE>(this, value, current);
			nodeMap.Add(value, current);

			return _node[current];
		}

		/// <summary>
		/// Returns all nodes
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Node<NODETYPE, EDGETYPE>> AllNodes() {
			for(int i = 0; i < Nodes; i++) {
				yield return Node[i];
			}
		}

		/// <summary>
		/// Returns all edges
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Edge<NODETYPE, EDGETYPE>> AllEdges() {
			for(int y = 0; y < Nodes; y++) {
				for(int x = 0; x < Nodes; x++) {
					if(Edge[x, y] != null) {
						yield return Edge[x, y];
					}
				}
			}
		}

		/// <summary>
		/// Returns the node belonging to the specified value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override Node<NODETYPE, EDGETYPE> NodeByValue(NODETYPE value) {
			return Node[nodeMap[value]];
		}


		/// <summary>
		/// Returns the edge between from and to, if it exists, null otherwise
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public Edge<NODETYPE, EDGETYPE> GetEdge(int from, int to) {
			if(!IsDirected) {
				if(to < from) {
					int tmp = to;
					to = from;
					from = tmp;
				}
			}

			return Edge[from, to];
		}

		#region Algorithms

		/// <summary>
		/// Finds the shortest path in this graph
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="route"></param>
		/// <returns></returns>
		public double ShortestPath(int from, int to, out LinkedList<Node<NODETYPE, EDGETYPE>> route) {
			return base.ShortestPath(Node[from], Node[to], out route);
		}

		#endregion

	}

	public class AdjacencyNode<NODETYPE, EDGETYPE> : Node<NODETYPE, EDGETYPE> {
		/// <summary>
		/// Returns the graph
		/// </summary>
		/// <value></value>
		public AdjacencyGraph<NODETYPE, EDGETYPE> Graph {
			get {
				return _graph;
			}
		}
		private AdjacencyGraph<NODETYPE, EDGETYPE> _graph;

		/// <summary>
		/// The ID of this node
		/// </summary>
		/// <value></value>
		public int ID {
			get {
				return _id;
			}
		}
		private int _id;

		public AdjacencyNode(AdjacencyGraph<NODETYPE, EDGETYPE> graph, NODETYPE value, int id) : base(value) {
			_graph = graph;
			_id = id;
		}

		/// <summary>
		/// Returns all edges neighbouring this one
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Edge<NODETYPE, EDGETYPE>> ExitingEdges() {
			foreach(Node<NODETYPE, EDGETYPE> node in Graph.AllNodes()) {
				if(node != this) {
					Edge<NODETYPE, EDGETYPE> edge = Graph.GetEdge(this.ID, ((AdjacencyNode<NODETYPE, EDGETYPE>)node).ID);
					if(edge != null) {
						yield return edge;
					}
				}
			}
		}

		/// <summary>
		/// Returns the distance to the specified edge, positive infinity if 
		/// </summary>
		/// <param name="to"></param>
		/// <returns></returns>
		public override double DistanceTo(Node<NODETYPE, EDGETYPE> to) {
			Edge<NODETYPE, EDGETYPE> edge = Graph.GetEdge(ID, ((AdjacencyNode<NODETYPE, EDGETYPE>)to).ID);
			if(edge == null) {
				return double.PositiveInfinity;
			} else {
				return edge.Weight;
			}
		}

		/// <summary>
		/// Returns whether this node is directly connected to the specified node
		/// </summary>
		/// <param name="to"></param>
		/// <returns></returns>
		public override bool IsConnectedTo(Node<NODETYPE, EDGETYPE> to) {
			return DistanceTo(to) != double.PositiveInfinity;
		}


		public override string ToString() {
			return "" + Value;
		}
	}

	public class AdjacencyEdge<NODETYPE, EDGETYPE> : Edge<NODETYPE, EDGETYPE> {
		public AdjacencyEdge(double weight, EDGETYPE value, Node<NODETYPE, EDGETYPE> from, Node<NODETYPE, EDGETYPE> to) : base(weight, value, from, to) {
		}
	}



	#endregion


	/*
	#region Adjacency List implementation
	public class ListGraph {
		public ListGraph(bool directed) {
			_isDirected = directed;
		}

		public Edge AddEdge(Node from, Node to, double weight) {
		}

		public Edge AddUndirectedEdge(Node n1, Node n2, double weight) {
		}
	}

	public class ListNode {
		public bool IsConnectedTo(ListNode other) {
		}

		public bool IsReachableFrom(ListNode other) {
		}

		public IEnumerable<ListEdge> EnteringEdges() {
		}

		public IEnumerable<ListEdge> ExitingEdges() {
		}
	}

	public class ListEdge {
		public bool IsDirected {
			get {
				return _isDirected;
			}
			set {
				_isDirected = value;
			}
		}
		bool _isDirected;
	}

	#endregion

	public interface INode {
		ListNode Node {
			get;
			set;
		}
	}

	public interface IEdge {
		ListEdge Edge {
			get;
			set;
		}
	}
	*/
}
