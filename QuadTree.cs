using System.Collections.Generic;
using System.Linq;

namespace GreatCodNet
{
    using System;
    using SFML.Graphics;
    using SFML.System;

    public class QuadTree
    {
        public int Id => _id;
        public int Level => _level;
        public Vector2f Position => _bounds.Position;
        public List<Vector2f> Objects => _objects;
        public Color FillColor { get => _fillColor; set => _fillColor = value; }
    
        private int _id;
        private int _level;
        private int _maxObjects = 4;
        private int _maxLevels = 10;
        private float _outlineThickness = 1;
        private bool _drawBounds = true;

        private Color _outlineColor = Color.White;
        private Color _fillColor = Color.Blue;
        private RectangleShape _bounds;

        private List<Edge> _edges;
        //private readonly List<QuadTree> _nodes;
        //private readonly QuadTree[] _nodes;
        private readonly Dictionary<int, QuadTree> _nodes;
        private readonly List<Vector2f> _objects;
        
        public QuadTree(ref int id, int level, RectangleShape bounds, Vector2f position, bool center = false)
        {
            _id = id++;
            _level = level;
            _bounds = bounds;
            _bounds.Position = center ? position - GetCenter() : position;
            _bounds.OutlineThickness = _outlineThickness;
            _bounds.OutlineColor = _outlineColor;
            _bounds.FillColor = _fillColor;

            //_nodes = new List<QuadTree>();
            //_nodes = new QuadTree[4];
            _nodes = new Dictionary<int, QuadTree>(4);
            _objects = new List<Vector2f>();
            SetupEdges(_bounds);
        }

        public void Draw(RenderWindow renderWindow)
        {
            if(_drawBounds)
                renderWindow.Draw(_bounds);
            
            //_nodes.Where(node => node != null).ToList().ForEach(node => node.Draw(renderWindow));
            _nodes.Values.ToList().ForEach(node => node.Draw(renderWindow));
            
            _objects.ForEach(obj =>
            {
                var sphere = new CircleShape(5);
                sphere.Position = obj - new Vector2f(sphere.Radius, sphere.Radius);
                renderWindow.Draw(sphere);
            });
        }

        public void SetupEdges(RectangleShape bounds)
        {
            _edges = new List<Edge>
            {
                new Edge()
                {
                    Name = "Top Edge",
                    X1 = bounds.Position.X,
                    X2 = bounds.Position.X + bounds.Size.X,
                    Y1 = bounds.Position.Y,
                    Y2 = bounds.Position.Y
                }, 
                new Edge()
                {
                    Name = "Left Edge",
                    X1 = _bounds.Position.X,
                    X2 = _bounds.Position.X,
                    Y1 = _bounds.Position.Y + _bounds.Size.Y,
                    Y2 = _bounds.Position.Y
                },
                new Edge()
                {
                    Name = "Bottom Edge",
                    X1 = _bounds.Position.X + _bounds.Size.X,
                    X2 = _bounds.Position.X,
                    Y1 = _bounds.Position.Y + _bounds.Size.Y,
                    Y2 = _bounds.Position.Y + _bounds.Size.Y
                }, 
                new Edge()
                {
                    Name = "Right Edge",
                    X1 = _bounds.Position.X + _bounds.Size.X,
                    X2 = _bounds.Position.X + _bounds.Size.X,
                    Y1 = _bounds.Position.Y,
                    Y2 = _bounds.Position.Y + _bounds.Size.Y
                }
            };
        }

        public QuadTree GetQuadTreeForPoint(Vector2f point)
        {
            if (!IsInside(point)) 
                return null;
            
            if(_nodes.ToList().Count > 0)
            {
                foreach(var node in _nodes.Values)
                {
                    var subNode = node.GetQuadTreeForPoint(point);
                    if (subNode == null) 
                        continue;
                    
                    Console.WriteLine($"Found node id : {subNode.Id} in level {_level}");
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

        public void Insert(Vector2f obj)
        {
            Console.WriteLine($"Adding object to QuadTree id : {_id}, level : {Level}");
            _objects.Add(obj);
            if (_objects.Count > _maxObjects && Level < _maxLevels)
            {
                if(_nodes.Count == 0)
                    Split();

                int i = 0;
                while (i < _objects.Count)
                {
                    int index = GetIndex(_objects[i]);
                    if (index != -1)
                    {
                        _nodes[index].Insert(_objects[i]);
                        if (_objects.Remove(_objects[i]))
                        {
                            Console.WriteLine("Removed node");
                        }
                    }
                    else
                        i++;
                }
            }
        }

        public List<Vector2f> Retrieve(ref List<Vector2f> objects, Vector2f shape)
        {
            var index = GetIndex(shape);
            if (index != -1 && _nodes.Count > 0)
            {
                _nodes[index].Retrieve(ref objects, shape);
            }
            objects.AddRange(_objects);
            return objects;
        }
        
        public void Split()
        {
            if (_nodes.Count > 0)
                return;
            
            Console.WriteLine($"Splitting QuadTree at level {Level}");
            
            var westBounds = new RectangleShape(new Vector2f(_bounds.Size.X/2, _bounds.Size.Y/2));
            var eastBounds = new RectangleShape(new Vector2f(_bounds.Size.X/2-1, _bounds.Size.Y/2));
            
            var center = GetCenter();
            var northEast = new QuadTree(ref _id, Level+1, new RectangleShape(eastBounds), new Vector2f(center.X + 1, center.Y - westBounds.Size.Y));
            var northWest = new QuadTree(ref _id, Level+1, new RectangleShape(westBounds), new Vector2f(center.X - westBounds.Size.X, center.Y - westBounds.Size.Y));
            var southWest = new QuadTree(ref _id, Level+1, new RectangleShape(westBounds), new Vector2f(center.X - westBounds.Size.X, center.Y));
            var southEast = new QuadTree(ref _id, Level+1, new RectangleShape(eastBounds), new Vector2f(center.X + 1, center.Y));
            _nodes[0] = northEast;
            _nodes[1] = northWest;
            _nodes[2] = southWest;
            _nodes[3] = southEast;
            //_nodes.AddRange(new List<QuadTree>{northEast, northWest, southWest, southEast});
        }

        public int GetIndex(Vector2f position)
        {
            var index = -1;
            for (var i = 0; i < _nodes.ToList().Count; i++)
            {
                if (_nodes[i] != null && _nodes[i].IsInside(position))
                {
                    index = i;
                    break;
                }
            }
            
            return index;
        }

        public void SetPosition(Vector2f position)
        {
            _bounds.Position = position;
            SetupEdges(_bounds);
        }

        public Vector2f GetCenter()
        {
            return new Vector2f(Position.X + _bounds.Size.X / 2, Position.Y + _bounds.Size.Y / 2);
        }
    }
}