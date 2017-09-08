using System.Collections.Generic;

namespace GreatCodNet
{
    using SFML.Graphics;
    using SFML.System;

    public class QuadTree
    {
        public RectangleShape Bounds;

        public QuadTree(int Level, RectangleShape Bounds, Vector2f Position)
        {
            this.Bounds = new RectangleShape(new Vector2f(10, 10));
            this.Bounds.OutlineThickness = OutlineThickness;
            this.Bounds.OutlineColor = OutlineColor;
            this.Bounds.FillColor = FillColor;

            this.Position = new Vector2f(0, 0);
            this.Level = Level;
            this.Bounds = Bounds;
        }
        
        private Color OutlineColor = Color.White;
        private Color FillColor = Color.Black;
        private Vector2f Position;

        private float OutlineThickness = 1;
        private bool DrawBounds = true;
        private int MaxObjects = 4;
        private int MaxLevels = 10;
        private int Level = 0;

        List<QuadTree> nodes = new List<QuadTree>(4);
        List<CircleShape> objects = new List<CircleShape>(4);

        
    }
}