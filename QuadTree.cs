using System.Collections.Generic;
using System.Linq;

namespace GreatCodNet
{
    using System;
    using SFML.Graphics;
    using SFML.System;

    public class Edge
    {
        public float x1;
        public float x2;
        public float y1;
        public float y2;

        public string name;
    }
    
    public class QuadTree
    {
        public Vector2f Position => Bounds.Position;
        public Color FillColor;// = Color.Blue;
        
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
            SetupEdges();
        }

        public void Draw(RenderWindow renderWindow)
        {
            if(DrawBounds)
                renderWindow.Draw(Bounds);
            
            foreach (var node in nodes)
            {
                renderWindow.Draw(node.Bounds);
            }
        }

        public void SetupEdges()
        {
            edges = new List<Edge>
            {
                new Edge()
                {
                    name = "TopEdge",
                    x1 = Bounds.Position.X,
                    x2 = Bounds.Position.X + Bounds.Size.X,
                    y1 = Bounds.Position.Y,
                    y2 = Bounds.Position.Y
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
            
            var newBounds = new RectangleShape(new Vector2f(Bounds.Size.X/2, (float)Bounds.Size.Y/2));
            var center = GetCenter();

            var northEast = new QuadTree(Level+1, newBounds, new Vector2f(center.X, center.Y - newBounds.Size.Y));
            nodes.Add(northEast);
            
            var northWest = new QuadTree(Level+1, newBounds, new Vector2f(center.X - newBounds.Size.X, center.Y - newBounds.Size.Y));
            nodes.Add(northWest);
            
            var southWest = new QuadTree(Level+1, newBounds, new Vector2f(center.X - newBounds.Size.X, center.Y));
            nodes.Add(southWest);
            
            var southEast = new QuadTree(Level+1, newBounds, new Vector2f(center.X, center.Y));
            nodes.Add(southEast);
        }

        private int GetIndex(CircleShape shape)
        {
            return -1;
        }

        public void SetPosition(Vector2f position)
        {
            this.Bounds.Position = position;
        }

        public Vector2f GetCenter()
        {
            return new Vector2f(this.Bounds.Position.X + this.Bounds.Size.X / 2, this.Bounds.Position.Y + this.Bounds.Size.Y / 2);
        }
    }
}