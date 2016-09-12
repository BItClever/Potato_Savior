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
    class Bag
    {

        private Texture2D texture;
        private Rectangle rectangle;
        private Vector2 position;
        private Vector2 originalPosition;


        private float timer;
        private float interval = 50;  //интервал в миллисекундах между сменой кадров

        private int frameHeight = 169;
        private int frameWidth = 150;
        private int currentFrame = 0;

        private bool isAnimated;

        public Bag(Texture2D newTexture)
        {
            texture = newTexture;
            rectangle = new Rectangle(0, 0, 150, 169);
            position = new Vector2(1290, 660);
            originalPosition = new Vector2(75, 85);
            isAnimated = false;
        }




        public void Update(GameTime gameTime, List<Potato> potatos, MouseState mousePresent, MouseState mousePast, SoundEffect sound)
        {
            foreach (Potato potato in potatos)
            {
                Rectangle Rec = new Rectangle((int)position.X - texture.Width / 14, (int)position.Y - texture.Height / 2, texture.Width / 7, texture.Height); //прямоугольник содержащий жука, ужасная формула
                if (Rec.Contains(mousePresent.X, mousePresent.Y) && potato.isVisible && potato.isClicked) //убиение жука по нажатию клавиши
                {
                    sound.Play(0.4f, 0.0f, 0.0f);
                    isAnimated = true;
                    potato.isVisible = false;
                }
            }

            if(isAnimated)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
                if (timer > interval)
                {
                    currentFrame++;
                    timer = 0;
                    if (currentFrame >= 7)
                    {
                        currentFrame = 0;
                        isAnimated = false;
                    }

                }
            }
            

            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, originalPosition, 1f, SpriteEffects.None, 0);
        }

    }

    
}
