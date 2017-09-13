using System;
using System.Collections.Generic;

namespace GreatCodNet
{
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;

    public class Application
    {
        private readonly RenderWindow _renderWindow;
        private readonly QuadTree _quadTree;
        private QuadTree _selectedQuadTree;
        private Vector2f _selectedObject;
        
        private int _numPoints;
        private Vertex[] _points = new Vertex[100];

        public Application()
        {
            var windowSize = new Vector2f(1024, 768);
            _renderWindow = new RenderWindow(new VideoMode((uint)windowSize.X, (uint)windowSize.Y), "Application");
            _renderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
            _renderWindow.MouseWheelScrolled += RenderWindow_MouseWheelScrolled;
            _renderWindow.KeyPressed += RenderWindow_KeyPressed;
            _renderWindow.Closed += (sender, e) => _renderWindow.Close();

            var screenCenter = new Vector2f(_renderWindow.Size.X / 2, _renderWindow.Size.Y / 2);
            AddPoint(screenCenter);

            var startId = 0;
            _quadTree = new QuadTree(ref startId, 0, new RectangleShape(new Vector2f(1000, 700)), screenCenter, center:true);
        }

        private void AddPoint(Vector2f point)
        {
            _points[_numPoints++] = new Vertex(point);
        }

        private void DrawPoints()
        {
            _renderWindow.Draw(_points, PrimitiveType.Points);
        }

        public void Run()
        {
            while (_renderWindow.IsOpen)
            {
                Render();
                _renderWindow.WaitAndDispatchEvents();
            }            
        }

        private void Render()
        {
            _renderWindow.Clear();

            _quadTree.Draw(_renderWindow);
            DrawPoints();

            _renderWindow.Display();
        }

        private void RenderWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"Keypress {e.Code}");
            if(e.Code == Keyboard.Key.Escape)
            {
                _renderWindow.Close();
            }
            if(e.Code == Keyboard.Key.I)
            {
                Console.WriteLine($"Quadtree Position x:{_quadTree.Position.X}, y:{_quadTree.Position.Y}");
            }
            if (e.Code == Keyboard.Key.S)
            {
                var quadId = _selectedQuadTree?.Id.ToString() ?? "None";
                Console.WriteLine($"Selected QuadTree id : {quadId}");
                _selectedQuadTree?.Split();
            }
            if (e.Code == Keyboard.Key.Space)
            {
                var objects = new List<Vector2f>();
                var list = _selectedQuadTree?.Retrieve(ref objects, _selectedObject);
                objects.ForEach(point => Console.WriteLine($"Point x: {point.X}, y: {point.Y}"));
            }
        }

        private void RenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine($"Mouseclick {e.Button} x:{e.X}, y:{e.Y}");
            if (e.Button == Mouse.Button.Right)
            {
                var index = _quadTree.GetIndex(new Vector2f(e.X, e.Y));
                Console.WriteLine($"Index: {index}");
            }
            if (e.Button == Mouse.Button.Left)
            {
                var point = new Vector2f(e.X, e.Y);
                var quadTree = _quadTree.GetQuadTreeForPoint(point);
                if(quadTree == null)
                    Console.WriteLine("No quadtree selected");
                quadTree?.Insert(point);
                _selectedObject = point;

            }
            if(e.Button == Mouse.Button.Middle)
            {
                _selectedQuadTree = _quadTree.GetQuadTreeForPoint(new Vector2f(e.X, e.Y));
                var quadId = _selectedQuadTree?.Id.ToString() ?? "None";
                var quadlevel = _selectedQuadTree?.Level.ToString() ?? "None";
                var quadObjects = _selectedQuadTree?.Objects.Count.ToString() ?? "None";
                //_selectedQuadTree?.Split();
                
                Console.WriteLine($"Selected QuadTree id : {quadId}, level : {quadlevel}, object count : {quadObjects}");
            }
            
        }

        private void RenderWindow_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            Console.WriteLine($"Mouse {e.Wheel} {e.Delta} x:{e.X}, y:{e.Y}");
        }
    }
}