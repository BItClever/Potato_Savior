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
    class Angel
    {
        Random random = new Random();

        private Texture2D texture;
        private Rectangle rectangle;

        private Vector2 position;
        private Vector2 velocity;
        private Vector2 originalPosition;

        private float rotation;
        public bool isVisible;
        private float timer;
        private float interval = 50;  //интервал в миллисекундах между сменой кадров

        private int frameHeight;
        private int frameWidth;
        private int currentFrame;


        public Angel(Texture2D newTexture) //конструктор. Жук при создании направляется к кусту
        {
            texture = newTexture;
            frameHeight = 75;
            frameWidth = 113;
            position = new Vector2(-200,random.Next(35, 700));
            currentFrame = random.Next(8);
            rotation = 0f;
            velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 3.5f;

            isVisible = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //spritebatch.Draw(texture, position, rectangle, Color.White, rotation, originalPosition, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, position, rectangle, Color.White, rotation, originalPosition, 1f, SpriteEffects.None, 0);
        }


        public void Update(GameTime gameTime)
        {
            
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            originalPosition = new Vector2(rectangle.Width / 2, rectangle.Height / 2);


            position += velocity;
            if (position.X > 2000)
                isVisible = false;



            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }

        }

    }
}
