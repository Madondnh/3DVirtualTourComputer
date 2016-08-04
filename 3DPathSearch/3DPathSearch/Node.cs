using System.Windows.Media.Media3D;

namespace _3DPathSearch
{
    public class Node
    {
        private Point3D pos;
        private int index;

        public string idx_String;

        //----------------------------------------------------------------------------------------
        public Node(int idx, Point3D n_Pos)
        {
            index = idx;
            pos = n_Pos;

            idx_String = idx.ToString();
        }

        //----------------------------------------------------------------------------------------

        public int getIndex()
        {
            return index;
        }

        //----------------------------------------------------------------------------------------

        public void setPos(Point3D n_Pos)
        {
            pos = n_Pos;
        }

        //----------------------------------------------------------------------------------------

        public Point3D getPos()
        {
            return pos;
        }

        //----------------------------------------------------------------------------------------

    }
}
