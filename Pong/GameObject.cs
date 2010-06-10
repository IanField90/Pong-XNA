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

        public GameObject(Texture2D loadedTexture)
        {
            position = Vector2.Zero;
            sprite = loadedTexture;
            velocity = Vector2.Zero;
            alive = false;
        }
    }
}
