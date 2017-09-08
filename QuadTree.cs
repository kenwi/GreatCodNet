using System.Collections.Generic;

namespace GreatCodNet
{
    using System;
    using SFML.Graphics;
    using SFML.System;

    public class QuadTree
    {
        public Vector2f Position => Bounds.Position;

        Color OutlineColor = Color.White;
        Color FillColor = Color.Black;
        RectangleShape Bounds;

        float OutlineThickness = 1;
        bool DrawBounds = true;
        int MaxObjects = 4;
        int MaxLevels = 10;
        int Level = 0;

        List<QuadTree> nodes = new List<QuadTree>(4);
        List<CircleShape> objects = new List<CircleShape>(4);

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
            if(objects.Count == 0)
            {
                int index = GetIndex(shape);
            }

            objects.Add(shape);
            if(objects.Count > MaxObjects)
            {
                if(nodes.Count == 0 )
                {
                    Split();
                }
            }
            
        }

        private void Split()
        {
            Console.WriteLine($"Splitting QuadTree at level {this.Level}");
        }

        private int GetIndex(CircleShape shape)
        {
            return -1;
        }
    }
}