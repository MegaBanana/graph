using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace Maveric
{
    public class Graph
    {
        Dictionary<string,Node> nodes;
        Dictionary<Path, Tuple<Node, Node>> pathDic;



        public Graph()
        {
            nodes = new Dictionary<string, Node>();
            pathDic = new Dictionary<Path, Tuple<Node, Node>>();
        }

        internal Dictionary<string, Node> Nodes
        {
            get
            {
                return nodes;
            }

            set
            {
                nodes = value;
            }
        }

        internal Dictionary<Path, Tuple<Node, Node>> PathDic
        {
            get
            {
                return pathDic;
            }

            set
            {
                pathDic = value;
            }
        }

        /// <summary>
        /// Add node to the nodes dictionary.
        /// </summary>
        /// <param name="node"></param>
        public void addNode(Node node)
        {
            nodes.Add(node.Name,node);
        }

        public void addEdge(Node node1, Node node2, Path path)
        {
            pathDic.Add(path, Tuple.Create(node1, node2));
        }


        /// <summary>
        /// Return direction of the flow
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public int getFlowDirection(Node node1, Node node2)
        {
            if (node1.isOpen() & node2.isOpen())
                return node1.Weight - node2.Weight;
            else
                return int.MinValue;
        }

        public void clearWeight()
        {
            foreach (var node in nodes.Values)
                if(node.Weight > Node.STD_WEIGHT)
                    node.Weight = Node.STD_WEIGHT;
        }


        public void pushWeightOver(Node passed, Node to_node)
        {
           
            foreach (Node node in to_node.voisin)
            {
                if (!passed.Equals(node))
                {
                    if (node.isOpen())
                    {
                        if(node.pushFlux(to_node.Weight, to_node.FluxOrigin))
                            pushWeightOver(to_node, node);
                    }
                }
            }
        }

        override public string ToString()
        {
            string str = "";

            // Update weight 
            foreach (var node in this.Nodes.Values)
                if (node.Weight == Node.STD_WEIGHT - 1)
                    this.pushWeightOver(node, node);


            return str;
        }

    }

    public class Node
    {
        public static int STD_WEIGHT = 2;


        State state;
        int weight;
        bool isSource;
        string fluxOrigin;
        public List<Node> voisin;
        string name;

        public int Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string FluxOrigin
        {
            get
            {
                return fluxOrigin;
            }

            set
            {
                fluxOrigin = value;
            }
        }

        public bool IsSource
        {
            get
            {
                return isSource;
            }

            set
            {
                isSource = value;
            }
        }

        public State getState()
        {
            return state;
        }

        public Node(string p_n) : this(p_n, false) { }
        
        public Node(string p_n, bool p_isSource)
        {
            this.voisin = new List<Node>();
            this.name = p_n;
            this.state = Node.State.OPEN;
            this.weight = STD_WEIGHT;
            this.isSource = p_isSource;
            if (p_isSource)
                this.fluxOrigin = name;
        }
 
        
        override public string ToString()
        { 
            return "Name: " + name + " Weight: " + weight + " Origin: " + fluxOrigin + " Nb voisin: " + voisin.Count + " State: " + state;
        }


        public bool isOpen()
        {
            return this.state.Equals(State.OPEN);
        }

        public void setState(State p_s)
        {
            state = p_s;
        }

        public bool pushFlux(int p_w, string p_fluxOrigin)
        {
           
            if (this.weight == STD_WEIGHT)
            {
                //if (this.state == State.DEACTIVATED)
                //{
                    this.weight = p_w + 2;

                    if (!isSource)
                        this.fluxOrigin = p_fluxOrigin;
                //}

            }
            else if (this.weight > STD_WEIGHT)
                if ((p_w + 1) < this.weight)
                {
                    this.weight = p_w + 1;
                    if (!isSource)
                        this.fluxOrigin = p_fluxOrigin;
                }
                else
                {
                    return false;
                }
            return true;
        }



        public enum State
        {
            OPEN,
            CLOSE
        }

    }

}
