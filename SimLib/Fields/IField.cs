using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SimLib.Nodes;

namespace SimLib.Fields
{
    public interface IField
    {
        //Coordinates stuff
        Point getEmptyRandomCoordinates();
        Point getRandomCoordinates();
        Point getCloseCoordinates(Point point);

        //Node stuff
        void addNode(INode node);
        void removeNode(INode node);
        void removeNode(int id);
        INode getNode(int id);
        INode getNode(Point Coordinates);
        List<INode> getNodes();
        Dictionary<Point, INode> getNodesByCoordinates();
        void setupNeighbors();
        List<int> getNeighbors(int ID);

        //DrawData stuff
        double[][] Data { get; set; }
    }
}
