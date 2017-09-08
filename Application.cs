using System;

namespace GreatCodNet
{
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;

    public class Application
    {
        private RenderWindow RenderWindow;
        private QuadTree QuadTree = new QuadTree(0, new RectangleShape(new Vector2f(100, 100)),  new Vector2f(0, 0));

        public Application()
        {
            RenderWindow = new RenderWindow(new VideoMode(1024, 768), "Application");
            RenderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
            RenderWindow.MouseWheelScrolled += RenderWindow_MouseWheelScrolled;
            RenderWindow.KeyPressed += RenderWindow_KeyPressed;
            RenderWindow.Closed += (sender, e) => RenderWindow.Close();
        }

        public void Run()
        {
            while (RenderWindow.IsOpen)
            {
                Render();
                RenderWindow.WaitAndDispatchEvents();
            }            
        }

        private void Render()
        {
            RenderWindow.Clear();

            QuadTree.Draw(RenderWindow);

            RenderWindow.Display();
        }

        private void RenderWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"Keypress {e.Code}");
            if(e.Code == Keyboard.Key.Escape)
            {
                RenderWindow.Close();
            }
            if(e.Code == Keyboard.Key.I)
            {
                //Console.WriteLine($"{QuadTree.}");
                Console.WriteLine($"Quadtree Position x:{QuadTree.Position.X}, y:{QuadTree.Position.Y}");
            }
        }

        private void RenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine($"Mouseclick {e.Button} x:{e.X}, y:{e.Y}");
        }

        private void RenderWindow_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            Console.WriteLine($"Mouse {e.Wheel} {e.Delta} x:{e.X}, y:{e.Y}");
        }
    }
}