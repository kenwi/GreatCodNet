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
        public int Id => id;        
        
        private Color OutlineColor = Color.White;
        private RectangleShape Bounds;
        private float OutlineThickness = 1;
        private bool DrawBounds = true;
        private int MaxObjects = 4;
        private int MaxLevels = 10;
        private int Level = 0;
        private int id = 0;

        private List<Edge> _edges;
        private readonly List<QuadTree> _nodes = new List<QuadTree>(4);

        public QuadTree(int id, int level, RectangleShape bounds, Vector2f position, bool center = false)
        {
            Bounds = bounds;
            Bounds.OutlineThickness = OutlineThickness;
            Bounds.OutlineColor = OutlineColor;
            Bounds.FillColor = FillColor;
            Bounds.Position = center ? position - GetCenter() : position;
            Level = level;
            SetupEdges(bounds);
            this.id = id;
        }

        public void Draw(RenderWindow renderWindow)
        {
            if(DrawBounds)
                renderWindow.Draw(Bounds);

            _nodes.ForEach(node => node.Draw(renderWindow));
        }

        public void SetupEdges(RectangleShape bounds)
        {
            _edges = new List<Edge>
            {
                new Edge()
                {
                    Name = "TopEdge",
                    X1 = bounds.Position.X,
                    X2 = bounds.Position.X + bounds.Size.X,
                    Y1 = bounds.Position.Y,
                    Y2 = bounds.Position.Y
                }, 
                new Edge()
                {
                    Name = "LeftEdge",
                    X1 = Bounds.Position.X,
                    X2 = Bounds.Position.X,
                    Y1 = Bounds.Position.Y + Bounds.Size.Y,
                    Y2 = Bounds.Position.Y
                },
                new Edge()
                {
                    Name = "BottomEdge",
                    X1 = Bounds.Position.X + Bounds.Size.X,
                    X2 = Bounds.Position.X,
                    Y1 = Bounds.Position.Y + Bounds.Size.Y,
                    Y2 = Bounds.Position.Y + Bounds.Size.Y
                }, 
                new Edge()
                {
                    Name = "RightEdge",
                    X1 = Bounds.Position.X + Bounds.Size.X,
                    X2 = Bounds.Position.X + Bounds.Size.X,
                    Y1 = Bounds.Position.Y,
                    Y2 = Bounds.Position.Y + Bounds.Size.Y
                }
            };
        }

        public QuadTree GetQuadTreeForPoint(Vector2f point)
        {
            if (!IsInside(point)) 
                return null;
            
            if(_nodes.Count > 0)
            {
                foreach(var node in _nodes)
                {
                    var subNode = node.GetQuadTreeForPoint(point);
                    if (subNode == null) 
                        continue;
                    
                    Console.WriteLine("Found node");
                    return subNode;
                }
            }
            return this;
        }

        public bool IsInside(Vector2f point)
        {
            var xp = point.X;
            var yp = point.Y;
            
            foreach (var edge in _edges)
            {
                Console.WriteLine($"Checking {edge.Name}");                
                var a = -(edge.Y2 - edge.Y1);
                var b = edge.X2 - edge.X1;
                var c = -(a * edge.X1 + b * edge.Y1);
                // A * x + B * y + C = 0
                var d = a * xp + b * yp + c;
                if (d < 0)
                    return false;
            }
            return true;
        }

        public void Split()
        {
            if (_nodes.Count > 0)
                return;
            
            Console.WriteLine($"Splitting QuadTree at level {Level}");
            
            var newBounds = new RectangleShape(new Vector2f(Bounds.Size.X/2, Bounds.Size.Y/2));
            var eastBounds = new RectangleShape(new Vector2f(Bounds.Size.X/2-1, Bounds.Size.Y/2));
            
            var center = GetCenter();

            var northEast = new QuadTree(id++, Level+1, new RectangleShape(eastBounds), new Vector2f(center.X + 1, center.Y - newBounds.Size.Y));
            var northWest = new QuadTree(id++, Level+1, new RectangleShape(newBounds), new Vector2f(center.X - newBounds.Size.X, center.Y - newBounds.Size.Y));
            var southWest = new QuadTree(id++, Level+1, new RectangleShape(newBounds), new Vector2f(center.X - newBounds.Size.X, center.Y));
            var southEast = new QuadTree(id++, Level+1, new RectangleShape(eastBounds), new Vector2f(center.X + 1, center.Y));
            _nodes.AddRange(new List<QuadTree>{northEast, northWest, southWest, southEast});
        }

        public int GetIndex(Vector2f position)
        {
            var index = -1;
            for (var i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] != null && _nodes[i].IsInside(position))
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
            return new Vector2f(Position.X + Bounds.Size.X / 2, Position.Y + Bounds.Size.Y / 2);
        }
    }
}