using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace RTSPrototype
{
    class Structure
    {
        public Vector2 Position;

        public void Draw(SpriteBatch sb, Game1 getGame1)
        {
                    Rectangle Destination = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
                    sb.Draw(getGame1.TreeTexture, Destination, Color.White);
        }
    }
}
