using System.Collections.Generic;
using System.IO;

namespace _3DPathSearch
{
    public class ReadMapGeneratedByUnity
    {
        List<float> verts;
        List<int> tris;
        string file_path;

        int startNode = 0;
        int endNode = 0;
        public ReadMapGeneratedByUnity()
        {
            verts = new List<float>();
            tris = new List<int>();
        }

        public List<float> VERTS   // the Name property
        {
            get
            {
                return verts;
            }
        }


        public List<int> TRIANGLES   // the Name property
        {
            get
            {
                return tris;
            }
        }
        

        public int START// the Name property
        {
            get
            {
                return startNode;
            }
        }


        public int END// the Name property
        {
            get
            {
                return endNode;
            }
        }

        public void ReadFileTris(string filePlath)
        {
            char[] whitespace = new char[] { ' ', '\t' };
            StreamReader inp_stm = new StreamReader(@filePlath);

            string inp_ln;
            while (!inp_stm.EndOfStream)
            {
                inp_ln = inp_stm.ReadLine();
                tris.Add(int.Parse(inp_ln));
            }
        }

        public void ReadFileVerts( string filePlath )
        {
            char[] whitespace = new char[] { ' ', '\t' };

            file_path = filePlath;
            StreamReader inp_stm = new StreamReader(@file_path);
            // the first 2 lines are  start and end seach
            string inp_ln = inp_stm.ReadLine();
            var pt = inp_ln.Split(whitespace);


            startNode = int.Parse(pt[0]);
            int temp  = int.Parse(pt[1]);
            endNode = temp;
            if (temp < startNode)
            {
                endNode = startNode;
                startNode = temp;
            }

            while (!inp_stm.EndOfStream)
            {
                inp_ln = inp_stm.ReadLine();
                pt = inp_ln.Split(whitespace);
                foreach (string ind in pt)
                {
                    verts.Add(float.Parse(ind));
                }
            }
        }
    }
}
