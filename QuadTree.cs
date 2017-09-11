using System.Collections.Generic;

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

        Color OutlineColor = Color.White;
        Color FillColor = Color.Blue;
        RectangleShape Bounds;

        float OutlineThickness = 1;
        bool DrawBounds = true;
        int MaxObjects = 4;
        int MaxLevels = 10;
        int Level = 0;

        public QuadTree(int level, RectangleShape bounds, Vector2f position)
        {
            Bounds = bounds;
            Bounds.OutlineThickness = OutlineThickness;
            Bounds.OutlineColor = OutlineColor;
            Bounds.FillColor = FillColor;
            Bounds.Position = position;
            Level = level;
            
        }

        public void Draw(RenderWindow renderWindow)
        {
            if(DrawBounds)
                renderWindow.Draw(Bounds);
        }

        public void Insert(CircleShape shape)
        {
            
        }

        public bool Contains(Vector2f point, RenderWindow renderWindow)
        {
            var xp = point.X;
            var yp = point.Y;
            var edges = new List<Edge>
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

        private void Split()
        {
            Console.WriteLine($"Splitting QuadTree at level {this.Level}");
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