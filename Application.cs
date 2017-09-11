﻿using System;

namespace GreatCodNet
{
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;

    public class Application
    {
        private static Vector2f _windowSize;
        private readonly RenderWindow _renderWindow;
        private readonly QuadTree _quadTree;

        int numPoints = 0;
        Vertex[] points = new Vertex[100];

        public Application()
        {
            _windowSize = new Vector2f(1024, 768);

            _renderWindow = new RenderWindow(new VideoMode((uint)_windowSize.X, (uint)_windowSize.Y), "Application");
            _renderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
            _renderWindow.MouseWheelScrolled += RenderWindow_MouseWheelScrolled;
            _renderWindow.KeyPressed += RenderWindow_KeyPressed;
            _renderWindow.Closed += (sender, e) => _renderWindow.Close();

            var screenCenter = new Vector2f(_renderWindow.Size.X / 2, _renderWindow.Size.Y / 2);
            AddPoint(screenCenter);

            _quadTree = new QuadTree(0, new RectangleShape(new Vector2f(1000, 700)), screenCenter, center:true);
        }

        private void AddPoint(Vector2f point)
        {
            points[numPoints] = new Vertex(point);
        }

        private void DrawPoints()
        {
            _renderWindow.Draw(points, PrimitiveType.Points);
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
                _quadTree.Split();
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
                var contains = _quadTree.IsInside(new Vector2f(e.X, e.Y));
                Console.WriteLine(contains ? "Inside" : "Outside");                
            }
            
        }

        private void RenderWindow_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            Console.WriteLine($"Mouse {e.Wheel} {e.Delta} x:{e.X}, y:{e.Y}");
        }
    }
}