using System.Collections;
using System.Collections.Generic;

namespace _3DPathSearch
{
    public class AStar
    {
        private Graph graph;
        private List<Edge> SPT;
        private List<float> G_Cost;  //This vector will store the G cost of each node
        private List<float> F_Cost;    //This vector will store the F cost of each node
        private List<Edge> SF;
        private int source;
        private int target;

        //-----------------------------------------------------------------------------------

        public AStar(Graph n_graph, int src, int tar)
        {
            graph = n_graph;
            source = src;
            target = tar;

            SPT = new List<Edge>(graph.numNodes());
            G_Cost = new List<float>(graph.numNodes());
            F_Cost = new List<float>(graph.numNodes());
            SF = new List<Edge>(graph.numNodes());

            for (int index = 0; index < graph.numNodes(); index++)
            {
                SPT.Add(null);
                G_Cost.Add(0);// = new List<float>(graph.numNodes());
                F_Cost.Add(0);// = new List<float>(graph.numNodes());
                SF.Add(null);// = new List<Edge>(graph.numNodes());
            }

            search();
        }

        //-----------------------------------------------------------------------------------

        private void search()
        {
            int costOfStartNode = 0; /// ??????
            //The pq is now sorted depending on the F cost vector
            PriorityQueue<int> pq = new PriorityQueue<int>();
            pq.Enqueue(source, costOfStartNode);

            while (pq.Count > 0)
            {
                int NCN = pq.Dequeue();

                SPT[NCN] = SF[NCN];

                if (SF[NCN] == null)
                {
                    SF[NCN]= SF[NCN];
                }

                if (SPT[NCN] != null)
                {
                    SPT[NCN].drawEdge(graph.getNode(SPT[NCN].getFrom()).getPos(),
                                       graph.getNode(SPT[NCN].getTo()).getPos(),
                                      "visited");
                }
                if (NCN == target)
                {
                    return;
                }
                var edges = graph.getEdges(NCN);
                foreach (Edge edge in edges)
                {
                    //The H cost is obtained by the distance between the target node, and the arrival node of the edge being analyzed
                    float Hcost = Graph.Distance(graph.getNode(edge.getTo()).getPos(), graph.getNode(target).getPos());

                    float Gcost = G_Cost[NCN] + edge.getCost();
                    int to = edge.getTo();
                    if (SF[edge.getTo()] == null)
                    {
                        F_Cost[edge.getTo()] = Gcost + Hcost;
                        G_Cost[edge.getTo()] = Gcost;
                        pq.Enqueue(edge.getTo(), F_Cost[edge.getTo()]);
                        SF[edge.getTo()] = edge;
                    }
                    else if ((Gcost < G_Cost[edge.getTo()]) && (SPT[edge.getTo()] == null))
                    {
                        F_Cost[edge.getTo()] = Gcost + Hcost;
                        G_Cost[edge.getTo()] = Gcost;

                        //UpdateItems === pq.reorderUp()
                        pq.UpdateItems(edge.getTo(), F_Cost[edge.getTo()]);
                        SF[edge.getTo()] = edge;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------

        public ArrayList getPath()
        {
            ArrayList path = new ArrayList();
            if (target < 0) return path;
            int nd = target;
            path.Add(nd);
            while ((nd != source) && (SPT[nd] != null))
            {
                SPT[nd].drawEdge( graph.getNode(SPT[nd].getFrom()).getPos(),
                                  graph.getNode(SPT[nd].getTo()).getPos(),"path");

                nd = SPT[nd].getFrom();
                path.Add(nd);
            }
            path.Reverse();
            return path;
        }
    }
}
