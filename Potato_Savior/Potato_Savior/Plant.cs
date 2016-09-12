using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Potato_Savior
{
    class Plant
    {
        public Texture2D plantTexture;
        public Rectangle plantRectangle;

        public Vector2 PlantOriginalPosition;
        public Vector2 PlantPosition;
        public int health;

        public Plant(Texture2D newPlantTexture, Rectangle newPlantRectangle)
        {
            plantRectangle = newPlantRectangle;
            plantTexture = newPlantTexture;
            health = 3000;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(plantTexture, plantRectangle, Color.White);
        }
    }
}
