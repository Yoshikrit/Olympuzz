﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    public class _GameObject
    {
        protected Texture2D _texture;//รูป
        protected Texture2D[] _allTextures;

        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;
        public Color color;
        public Vector2 Velocity;

        public string Name;

        public bool IsActive;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public _GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }

        public _GameObject(Texture2D[] allTextures)
        {
            _allTextures = allTextures;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch) {}

        public virtual void Update(GameTime gameTime, Bubble[,] bubbles, bool bossSkillActive) { }

        public virtual void Update(GameTime gameTime) { }
    }
}
