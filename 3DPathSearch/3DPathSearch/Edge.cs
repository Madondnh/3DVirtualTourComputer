using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace _3DPathSearch
{
    public class Edge
    {

        private int from;     //The index of the node from which this edge departs
        private int to;       //The index of the node from which this edge arrives
        private float cost;  //The cost of crossing through this node (i.e. the distance)

        //----------------------------------------------------------------------------------------

        public Edge(int n_From, int n_To, float n_Cost  = 1.0f)
        {
            from = n_From;
            to = n_To;
            cost = n_Cost;
        //    this.z = 1; ??????
        }

        //----------------------------------------------------------------------------------------

        public int getFrom()
        {
            return from;
        }

        //----------------------------------------------------------------------------------------

        public void setFrom( int n_From )
        {
            from=n_From;
        }

    //----------------------------------------------------------------------------------------

    public int getTo()
        {
            return to;
        }

        //----------------------------------------------------------------------------------------

        public void setTo(int n_To)
        {
            to = n_To;
        }

        //----------------------------------------------------------------------------------------
        public void setCost(float n_Cost)
        {
            cost = n_Cost;
        }

        //----------------------------------------------------------------------------------------

        public float getCost()
        {
            return cost;
        }

        //----------------------------------------------------------------------------------------

        /*
          Since the edge is just a line that connects the nodes,
          we will use this method to draw the edge, the style refers to how we will
          want the edge to be
        */
        public void drawEdge(Point3D fromPos, Point3D Vector3D, String style = "normal")
        {
            /*  switch (style)
              {
                  case "normal":
                      //If it is normal, create a gray line
                      this.graphics.clear();
                      this.graphics.lineStyle(1, 0x999999, .3);
                      this.graphics.moveTo(fromPos.x, fromPos.y);
                      this.graphics.lineTo(toPos.x, toPos.y);
                      break;
                  case "path":
                      //If it is a line from the path, create a black line
                      this.graphics.clear();
                      this.graphics.lineStyle(2, 0x000000, 1);
                      this.graphics.moveTo(fromPos.x, fromPos.y);
                      this.graphics.lineTo(toPos.x, toPos.y);
                      break;
                  case "visited":
                      //If it is a line used by the algorithm, create a red line
                      this.graphics.clear();
                      this.graphics.lineStyle(1, 0xFF0000, 1);
                      this.graphics.moveTo(fromPos.x, fromPos.y);
                      this.graphics.lineTo(toPos.x, toPos.y);
                      break;
              }*/
        }


    }
}
