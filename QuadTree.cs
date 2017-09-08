using System.Collections.Generic;

namespace GreatCodNet
{
    using SFML.Graphics;
    using SFML.System;

    public class QuadTree
    {
        public QuadTree(Vector2f Position)
        {
            bounds = new RectangleShape(new Vector2f(10, 10));
            bounds.OutlineThickness = OutlineThickness;
            bounds.OutlineColor = OutlineColor;
            bounds.FillColor = FillColor;
            this.Position = new Vector2f(0, 0);
        }

        public RectangleShape Bounds => bounds;
        private RectangleShape bounds;
        private Color OutlineColor = Color.White;
        private Color FillColor = Color.Black;
        private Vector2f Position;

        private float OutlineThickness = 1;
        private bool DrawBounds = true;
        private int MaxObjects = 4;
        private int MaxLevels = 10;
        private int CurrentLevel = 0;

        List<QuadTree> nodes = new List<QuadTree>(4);
        List<CircleShape> objects = new List<CircleShape>(4);

        
    }
}