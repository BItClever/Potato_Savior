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
    class BugTest
    {
        private Texture2D texture;
        private Rectangle rectangle;
        private Vector2 originalPosition;
        private Vector2 position;
        private Vector2 velocity;


        private int frameHeight;
        private int frameWidth;
        private int currentFrame;

        private float rotation;

        private float timer;
        private float interval = 50;


        public BugTest(Texture2D newTexture, Vector2 newPosition, int newFrameHeight, int newFrameWidth)
        {
            texture = newTexture;
            position = newPosition;
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
        }

        public void Right(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 2)
                    currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, rectangle, Color.White, rotation, originalPosition, 1.0f, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            originalPosition = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            //position = position + velocity;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rotation += 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rotation -= 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position.X += (int)(Math.Cos(rotation) * velocity.X);
                position.Y += (int)(Math.Sin(rotation) * velocity.X);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Right(gameTime);
                velocity.X = 3;
            }
            else
                velocity.X = 0;
        }
    }
}
