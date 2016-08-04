using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace _3DPathSearch
{
    public class Graph
    {
       private static int nextIndex = 0;
       private List<Node> nodes;
       private List<List<Edge>> edges;
       private List<int> triangularNodeIDS;

       private int searchStart = 0;
       private int seacchEnd = 0;

        //----------------------------------------------------------------------------------------
 
        public Graph()
        {
            nodes = new List< Node > ();
            edges = new List< List < Edge > > ();
        }

        //----------------------------------------------------------------------------------------

        public List<int>  TRIANGLES
        {
            get { return triangularNodeIDS; }
        }
        
        //----------------------------------------------------------------------------------------

        public void GenSearchStartAndEndNode(out int start, out int end)
        {
            //default
            start = 0;
            end = 0;

            if (nodes.Count == 0)
            {

                return;
            }
            int a, b;
            Random rInt = new Random();
            do
            {
                a = rInt.Next(0, nodes.Count - 1);
                b = rInt.Next(0, nodes.Count - 1);
            } while (a == b);

            searchStart = a;
            seacchEnd = b;
            if (a > b)
            {
                searchStart = b;
                seacchEnd = a;
            }

            start = searchStart;
            end = seacchEnd;

            // temp setting
            //start = searchStart;
            if(seacchEnd - start > 20 )
            end = start + 20 ;
        }

        //----------------------------------------------------------------------------------------

        public static float Distance( Point3D from, Point3D to)
        {
            float deltaX = (float)( from.X - to.X );
            float deltaY = (float)(from.Y - to.Y);
            float deltaZ = (float)( from.Z - to.Z );

          return  (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }


        //----------------------------------------------------------------------------------------

        private void GenNodes(List<float> uniqueVertList)
        {
            for (int index = 0; index < uniqueVertList.Count; index += 3)
            {

                var newNode = new Node(Graph.getNextIndex(),
                                        new Point3D((double)uniqueVertList[index],
                                                   (double)uniqueVertList[index + 1],
                                                   (double)uniqueVertList[index + 2]));
                this.addNode(newNode);
            }
        }

        //----------------------------------------------------------------------------------------

        private void SetEdges( List<Node> nodes, List<int> nodetri )
        {
            int tri = 0;
            for (int index = 0; index < nodetri.Count; index++)
            {
                int from = nodetri[index];
                int to = 0;

                if (tri != 0 && tri % 2 == 0)
                {
                    to = nodetri[index - 2];
                    tri = 0;
                }
                else
                {
                    from = nodetri[index];
                    to = nodetri[index + 1];
                    tri += 1;
                }

                var Cost = Distance(nodes[from].getPos(), nodes[to].getPos());
                var edge = new Edge(from, to, Cost);
                this.addEdge(edge);
            }
        }

        //----------------------------------------------------------------------------------------

        public Graph( List<float> uniqueVertList, List<int> nodetri)
        {
            nextIndex = 0;
            nodes = new List<Node>();
            edges = new List<List<Edge>>();

            // this logics needs to move to a new  class like process / prepair STL mah format

            // ( 1) read  in the ordering of nodes for msh triangular settings
            GenNodes(uniqueVertList);
            SetEdges(nodes, nodetri);

            triangularNodeIDS = nodetri;
        }

        //----------------------------------------------------------------------------------------

        public Graph(List<float> vertList)
        {
            nextIndex = 0;
            nodes = new List<Node>();
            edges = new List<List<Edge>>();

            // this logics needs to move to a new  class like process / prepair STL mah format

            // ( 1) read  in the ordering of nodes for msh triangular settings
            var nodetri = new List<Node>();
            for (int index = 0; index < vertList.Count; index += 3)
            {

                var newNode = new Node(Graph.getNextIndex(),
                                        new Point3D((double)vertList[index],
                                                   (double)vertList[index + 1],
                                                   (double)vertList[index + 2]));
                nodetri.Add(newNode);
            }

            //(2) find the unique nodes a and IDs for triange nodes
            var nodeTriIDs = new List<int>();
            var uniqueNodes = new List<Node>();
            foreach (var node in nodetri)
            {
                bool found = false;
                int nodeID = 0;
                foreach (var uniqueNode in uniqueNodes)
                {
                    if (uniqueNode.getPos().Equals(node.getPos())) // same node
                    {
                        found = true;
                        break;
                    }
                    nodeID++;
                }

                if (found)
                {
                    nodeTriIDs.Add(nodeID);
                }
                else
                {
                    uniqueNodes.Add(node);
                    nodeTriIDs.Add(uniqueNodes.Count - 1);
                }
            }

            triangularNodeIDS = nodeTriIDs;

            // (3) add all the unique nodes to the graph nodes
            foreach (var uniqueNode2 in uniqueNodes)
            {
                var nodeNew = new Node(Graph.getNextIndex(), uniqueNode2.getPos());
                this.addNode(nodeNew);
            }

            // (4) add all the edges to the graph
            int tri = 0;
            for (int index = 0; index < nodeTriIDs.Count; index++)
            {
                int from = nodeTriIDs[index];
                int to = 0;

                if (tri != 0 && tri % 2 == 0)
                {
                    to = nodeTriIDs[index - 2];
                    tri = 0;
                }
                else
                {
                    from = nodeTriIDs[index];
                    to = nodeTriIDs[index + 1];
                    tri += 1;
                }

                var Cost = Distance(nodes[from].getPos(), nodes[to].getPos());
                var edge = new Edge(from, to, Cost);
                this.addEdge(edge);
            }
        }

        //----------------------------------------------------------------------------------------

        //In order to get the node, we just ask for the index of it, and access the nodes vector with that key
        public Node getNode(int idx )
        {
            return nodes.ElementAt(idx); 
        }

        //----------------------------------------------------------------------------------------

        //To get an edge, we ask for the two nodes that it connects,
        //then we retrieve all the edges of the from node and search if one of them
        //goes to the same node as the edge we are looking for, if it does, that's our edge.
        public Edge getEdge(int from, int to)
        {
            List<Edge> from_Edges = edges.ElementAt(from);
            for (int a = 0; a < from_Edges.Count; a++)
            {
                if (from_Edges[a].getTo() == to)
                {
                    return from_Edges[a];
                }
            }
            return null;
        }

        //----------------------------------------------------------------------------------------

        //To add a node to the graph, we first look if it already exist on it,
        //if it doesn't, then we add it to the nodes vector, and add an array to the
        //edges vector where we will store the edges of that node, finally we increase
        //the next valid index int in order to give the next available index in the graph
        public int addNode(Node node)
        {
            if (validIndex(node.getIndex()))
            {
                nodes.Add(node);
                edges.Add(new List<Edge>());
                nextIndex++;
            }
            return 0;
        }

        //----------------------------------------------------------------------------------------

        //To add an edge we must first look if both nodes it connects actually exist,
        //then we must see if this edge already exist on the graph, finally we add it
        //to the array of edges of the node from where it comes
        public void addEdge(Edge edge)
        {
            if (validIndex(edge.getTo()) && validIndex(edge.getFrom()))
            {
                if (getEdge(edge.getFrom(), edge.getTo()) == null)
                {
                    (edges[edge.getFrom()]).Add(edge);
                }
            }
        }

        //----------------------------------------------------------------------------------------

        //To get the edges of a node, just return the array given by the edges vector
        //at node's index position
        public List<Edge> getEdges(int node)
        {
            return edges[node];
        }

        //----------------------------------------------------------------------------------------

        //This function checks if the node index is between the range of already added nodes
        //which is form 0 to the next valid index of the graph
        public bool validIndex(int idx)
        {
            bool results = (idx >= 0 && idx <= nextIndex);
            return results;
        }

        //----------------------------------------------------------------------------------------

        //Just returns the amount of nodes already added to the graph
        public int numNodes()
        {
            return nodes.Count;
        }

        //----------------------------------------------------------------------------------------

        //This is to redraw all the edges on the graph to get them to the normal style
        public void redraw()
        {
            foreach (var a_edges in edges)

            {
                foreach (var edge in a_edges)
                {
                    edge.drawEdge(getNode(edge.getFrom()).getPos(),
                                   getNode(edge.getTo()).getPos());
                }
            }
        }

        //----------------------------------------------------------------------------------------

        // Add the triangle's three segments.
        private static void AddTriangleSegment(MeshGeometry3D mesh,
                                                MeshGeometry3D wireframe, Dictionary<int, int> already_drawn,
                                                int index1, int index2, double thickness)
        {
            // Get a unique ID for a segment connecting the two points.
            if (index1 > index2)
            {
                int temp = index1;
                index1 = index2;
                index2 = temp;
            }
            int segment_id = index1 * mesh.Positions.Count * index2;

            // If we've already added this segment for
            // another triangle, do nothing.
            if (already_drawn.ContainsKey(segment_id)) return;
            already_drawn.Add(segment_id, segment_id);

            // Create the segment.
            AddSegment(wireframe, mesh.Positions[index1],
                mesh.Positions[index2], thickness);
        }


        //----------------------------------------------------------------------------------------

        public static void AddSegment(MeshGeometry3D mesh,
                                      Point3D point1, Point3D point2,
                                      double thickness, bool extend = false)
        {
            // Find an up vector that is not colinear with the segment.
            // Start with a vector parallel to the Y axis.
            Vector3D up = new Vector3D(0, 1, 0);

            // If the segment and up vector point in more or less the
            // same direction, use an up vector parallel to the X axis.
            Vector3D segment = point2 - point1;
            segment.Normalize();
            if (Math.Abs(Vector3D.DotProduct(up, segment)) > 0.9)
                up = new Vector3D(1, 0, 0);

        }

       //----------------------------------------------------------------------------------------

        // Set the vector's length.
        private static Vector3D ScaleVector(Vector3D vector, double length)
        {
            double scale = length / vector.Length;
            return new Vector3D(
                vector.X * scale,
                vector.Y * scale,
                vector.Z * scale);
        }

        //      //----------------------------------------------------------------------------------------

        // Add a triangle to the indicated mesh.
        // Do not reuse points so triangles don't share normals.
        public static void AddTriangle( MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            // Create the points.
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(point1);
            mesh.Positions.Add(point2);
            mesh.Positions.Add(point3);

            // Create the triangle.
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);
        }

        //--------------------------------------------------------------------------------------

        private float setMin( float a,float b)
        {
            if (a < b)
                return a;
            return b;
        }

        //--------------------------------------------------------------------------------------

        private float setMax(float a, float b)
        {
            if (a < b)
                return b;
            return a;
        }

        //----------------------------------------------------------------------------------------

        public void SetMesh(ref MeshGeometry3D mesh )
        {
            float min_x = 0.0f;
            float max_x = 0.0f;

            float min_y = 0.0f;
            float max_y = 0.0f;

            float min_z = 0.0f;
            float max_z = 0.0f;

            for (int index = 0; index < nodes.Count; index++)
            {
                min_x = setMin(min_x, (float)nodes[index].getPos().X);
                min_y = setMin(min_y, (float)nodes[index].getPos().Y);
                min_z = setMin(min_z, (float)nodes[index].getPos().Z);

                max_x = setMax(max_x, (float)nodes[index].getPos().X);
                max_y = setMax(max_y, (float)nodes[index].getPos().Y);
                max_z = setMax(max_z, (float)nodes[index].getPos().Z);
            }

            float c_x = 65;
            float c_y = -max_y;
            float c_z = -max_z;

            float c_screenX = 0;
            float c_screenY = 0;

            //Final Ofsets
            Point3D CenterNew = new Point3D(c_screenX+ c_x, c_screenY + c_y, c_z);
            System.Windows.Point offSet = new System.Windows.Point(CenterNew.X, CenterNew.Y);

            SetMeshs(ref mesh, offSet);

    }

        //----------------------------------------------------------------------------------------

        public void SetMeshs(ref MeshGeometry3D mesh, System.Windows.Point pointMouse )
        {
 
            float c_x =(float) pointMouse.X;
            float c_y = (float)pointMouse.Y;
            float c_z = (float)0;

            float c_screenX = 0;
            float c_screenY = 0;
            Point3D CenterNew = new Point3D(c_screenX + c_x, c_screenY + c_y, c_z);
            for (int index = 0; index < triangularNodeIDS.Count; index += 3)
            {
                int node1Id = triangularNodeIDS[index];
                int node2Id = triangularNodeIDS[index+1];
                int node3Id = triangularNodeIDS[index+2];

                AddTriangle(mesh, Add3DPoints(nodes[node1Id].getPos(), CenterNew ),
                                  Add3DPoints(nodes[node2Id].getPos(), CenterNew ),
                                  Add3DPoints(nodes[node3Id].getPos(), CenterNew));
            }

        }

        //---------------------------------------------------------------------------------------

        private Point3D Add3DPoints(Point3D a, Point3D b )
        {

            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        //----------------------------------------------------------------------------------------

        // Return a MeshGeometry3D representing this mesh's wireframe.
        public static MeshGeometry3D ToWireframe(ref MeshGeometry3D mesh, double thickness)
        {
            // Make a dictionary in case triangles share segments
            // so we don't draw the same segment twice.
            Dictionary<int, int> already_drawn =
                new Dictionary<int, int>();

            // Make a mesh to hold the wireframe.
            MeshGeometry3D wireframe = new MeshGeometry3D();

            // Loop through the mesh's triangles.
            for (int triangle = 0;
                triangle < mesh.TriangleIndices.Count;
                triangle += 3)
            {
                // Get the triangle's corner indices.
                int index1 = mesh.TriangleIndices[triangle];
                int index2 = mesh.TriangleIndices[triangle + 1];
                int index3 = mesh.TriangleIndices[triangle + 2];

                // Make the triangle's three segments.
                AddTriangleSegment(mesh, wireframe, already_drawn,
                    index1, index2, thickness);
                AddTriangleSegment(mesh, wireframe, already_drawn,
                    index2, index3, thickness);
                AddTriangleSegment(mesh, wireframe, already_drawn,
                    index3, index1, thickness);
            }

            return wireframe;
        }

        //----------------------------------------------------------------------------------------

        //This function return the next valid node index to be added
        public static int getNextIndex()
        {
            return nextIndex;
        }

        //----------------------------------------------------------------------------------------

    }
}
