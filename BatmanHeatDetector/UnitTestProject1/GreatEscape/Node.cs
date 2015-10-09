using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.GreatEscape
{
    public class Node
    {
        private Node parentNode;

        /// <summary>
        /// The node's location in the grid
        /// </summary>
        public Point Location { get; private set; }

        /// <summary>
        /// True when the node may be traversed, otherwise false
        /// </summary>
        public bool IsWalkable { get; set; }

        /// <summary>
        /// Estimated cost from here to end
        /// </summary>

        public int EstimatedCostToEnd { get; set; }

        /// <summary>
        /// Cost from start to here
        /// </summary>
        public float CostFromStart { get; private set; }


        /// <summary>
        /// Estimated total cost (F = G + H)
        /// </summary>
        public float EstimatedTotalCost
        {
            get { return this.CostFromStart + this.EstimatedCostToEnd; }
        }
        /// <summary>
        /// Estimated cost from here to end
        /// </summary>

        /// <summary>
        /// Flags whether the node is open, closed or untested by the PathFinder
        /// </summary>
        public NodeState State { get; set; }


        public Node(int x, int y, int estimatedCostToEnd)
        {
            this.Location = new Point(x, y);
            this.State = NodeState.Untested;
            
            EstimatedCostToEnd = estimatedCostToEnd;

            CostFromStart = 0;
        }

        public Node ParentNode
        {
            get { return this.parentNode; }
            set
            {
             
                this.parentNode = value;
                this.CostFromStart = this.parentNode.CostFromStart + 1;
            }
        }

    }
}
