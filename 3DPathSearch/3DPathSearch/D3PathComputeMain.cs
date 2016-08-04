using System.Collections;
using System.IO;

namespace _3DPathSearch
{
    public class D3PathComputeMain
    {
        Graph vertEdgeGraph;
        private const string shortestPath = "C:\\Users\\Siphesihle\\Documents\\Visual Studio 2015\\Projects\\PathComputingProjectDemo\\STLReaderLibraryWrapper\\ShortestPath.txt";
        private const string triListPath = "C:\\Users\\Siphesihle\\Documents\\Visual Studio 2015\\Projects\\PathComputingProjectDemo\\STLReaderLibraryWrapper\\TriangleCompare.txt";

        //----------------------------------------------------------------------------------------

        // Add the model to the Model3DGroup.
        private void DefineModel()
        {
            string vertpath = "C:\\Users\\Siphesihle\\Documents\\Visual Studio 2015\\Projects\\PathComputingProjectDemo.txt";
            string tripath = "C:\\Users\\Siphesihle\\Documents\\Visual Studio 2015\\Projects\\Triangles.txt";

            ReadMapGeneratedByUnity VMap = new ReadMapGeneratedByUnity();
            VMap.ReadFileVerts(vertpath);
            VMap.ReadFileTris(tripath);

            vertEdgeGraph = new Graph(VMap.VERTS, VMap.TRIANGLES);

            //We create a new Astar search that will look a path from node 24 to node 35
            int seachStartNode = VMap.START;
            int searchEndNode = VMap.END;
            vertEdgeGraph.GenSearchStartAndEndNode(out seachStartNode, out searchEndNode);
            AStar astar = new AStar(vertEdgeGraph, seachStartNode, searchEndNode);

            //  ArrayList ausgabeListe = astar.getPath();
            WritePath(new ArrayList(vertEdgeGraph.TRIANGLES), triListPath);
            WritePath(astar.getPath(), shortestPath);

        }

        //----------------------------------------------------------------------------------------

        private void WritePath(ArrayList ausgabeListe, string writeFile)
        {
            StreamWriter file = new System.IO.StreamWriter(writeFile);
            foreach (var line in ausgabeListe)
                file.WriteLine(line);
            file.Close();

        }

        //----------------------------------------------------------------------------------------

        public int Test( ) { return 5; }

        //----------------------------------------------------------------------------------------

        private void RecomputePath()
        {
            int seachStartNode;
            int searchEndNode;
            vertEdgeGraph.GenSearchStartAndEndNode(out seachStartNode, out searchEndNode);

            AStar astar = new AStar(vertEdgeGraph, seachStartNode, searchEndNode);
            ArrayList ausgabeListe = astar.getPath();

            WritePath(ausgabeListe, shortestPath);

        }

        //----------------------------------------------------------------------------------------

    }
}
