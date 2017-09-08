using System;

namespace GreatCodNet
{
    using SFML.Graphics;

    public class Application
    {
        private RenderWindow RenderWindow;

        public Application()
        {
            RenderWindow = new RenderWindow(new SFML.Window.VideoMode(1024, 768), "Application");
            RenderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
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

            RenderWindow.Display();
        }

        private void RenderWindow_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            System.Console.WriteLine("Keypress");
        }

        private void RenderWindow_MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            System.Console.WriteLine("Mouseclick");
        }
    }
}