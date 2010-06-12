using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Pong
{
    class GameObject
    {
        public Texture2D sprite;
        public Vector2 position;
        public Vector2 velocity;
        public bool alive; // only used for ball
        private int width, height;

        public GameObject(Texture2D loadedTexture, int x, int y)
        {
            position = Vector2.Zero;
            sprite = loadedTexture;
            velocity = Vector2.Zero;
            width = x;
            height = y;
            alive = true;
        }
        public int getHeight(){ return height; }
        public int getWidth() { return width; }
    }
}
