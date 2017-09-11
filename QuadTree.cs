using System.Collections.Generic;
using System.Linq;

namespace GreatCodNet
{
    using System;
    using SFML.Graphics;
    using SFML.System;

    public class QuadTree
    {
        public Vector2f Position => Bounds.Position;
        public Color FillColor = Color.Blue;
        
        Color OutlineColor = Color.White;
        RectangleShape Bounds;

        float OutlineThickness = 1;
        bool DrawBounds = true;
        int MaxObjects = 4;
        int MaxLevels = 10;
        int Level = 0;

        private List<Edge> edges;
        private List<QuadTree> nodes = new List<QuadTree>();

        public QuadTree(int level, RectangleShape bounds, Vector2f position)
        {
            Bounds = bounds;
            Bounds.OutlineThickness = OutlineThickness;
            Bounds.OutlineColor = OutlineColor;
            Bounds.FillColor = FillColor;
            Bounds.Position = position;
            Level = level;
            SetupEdges(bounds);
        }

        public void Draw(RenderWindow renderWindow)
        {
            if(DrawBounds)
                renderWindow.Draw(Bounds);
            
            foreach (var node in nodes)
            {
                node.Draw(renderWindow);
            }
        }

        public void SetupEdges(RectangleShape bounds)
        {
            edges = new List<Edge>
            {
                new Edge()
                {
                    name = "TopEdge",
                    x1 = bounds.Position.X,
                    x2 = bounds.Position.X + bounds.Size.X,
                    y1 = bounds.Position.Y,
                    y2 = bounds.Position.Y
                }, 
                new Edge()
                {
                    name = "LeftEdge",
                    x1 = Bounds.Position.X,
                    x2 = Bounds.Position.X,
                    y1 = Bounds.Position.Y + Bounds.Size.Y,
                    y2 = Bounds.Position.Y
                },
                new Edge()
                {
                    name = "BottomEdge",
                    x1 = Bounds.Position.X + Bounds.Size.X,
                    x2 = Bounds.Position.X,
                    y1 = Bounds.Position.Y + Bounds.Size.Y,
                    y2 = Bounds.Position.Y + Bounds.Size.Y
                }, 
                new Edge()
                {
                    name = "RightEdge",
                    x1 = Bounds.Position.X + Bounds.Size.X,
                    x2 = Bounds.Position.X + Bounds.Size.X,
                    y1 = Bounds.Position.Y,
                    y2 = Bounds.Position.Y + Bounds.Size.Y
                }
            };
        }

        public bool IsInside(Vector2f point)
        {
            var xp = point.X;
            var yp = point.Y;

            foreach (var edge in edges)
            {
                Console.WriteLine($"Checking {edge.name}");
                
                // A * x + B * y + C = 0
                var x1 = edge.x1;
                var x2 = edge.x2;
                var y1 = edge.y1;
                var y2 = edge.y2;

                var A = -(y2 - y1);
                var B = x2 - x1;
                var C = -(A * x1 + B * y1);
                
                var D = A * xp + B * yp + C;
                if (D < 0)
                    return false;
            }
            
            return true;
        }

        public void Split()
        {
            Console.WriteLine($"Splitting QuadTree at level {this.Level}");
            
            var newBounds = new RectangleShape(new Vector2f(Bounds.Size.X/2, Bounds.Size.Y/2));
            var center = GetCenter();

            var northEast = new QuadTree(Level+1, new RectangleShape(newBounds), new Vector2f(center.X, center.Y - newBounds.Size.Y));
            var northWest = new QuadTree(Level+1, new RectangleShape(newBounds), new Vector2f(center.X - newBounds.Size.X, center.Y - newBounds.Size.Y));
            var southWest = new QuadTree(Level+1, new RectangleShape(newBounds), new Vector2f(center.X - newBounds.Size.X, center.Y));
            var southEast = new QuadTree(Level+1, new RectangleShape(newBounds), new Vector2f(center.X, center.Y));
            nodes.AddRange(new List<QuadTree>{northEast, northWest, southWest, southEast});
        }

        public int GetIndex(Vector2f position)
        {
            int index = -1;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] != null && nodes[i].IsInside(position))
                {
                    index = i;
                }
            }
            
            return index;
        }

        public void SetPosition(Vector2f position)
        {
            Bounds.Position = position;
            SetupEdges(Bounds);
        }

        public Vector2f GetCenter()
        {
            return new Vector2f(this.Bounds.Position.X + this.Bounds.Size.X / 2, this.Bounds.Position.Y + this.Bounds.Size.Y / 2);
        }
    }
}