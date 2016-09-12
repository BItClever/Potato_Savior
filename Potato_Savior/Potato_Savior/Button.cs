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
    class Button
    {

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
        public bool IsClicked;
        private bool mouseIn;

        public Button(Texture2D newTexture, Vector2 newPosition) //конструктор. Жук при создании направляется к кусту
        {
            texture = newTexture;
            frameHeight = 113;
            frameWidth = 300;
            position = newPosition;
            currentFrame = 0;
            rotation = 0f;
            IsClicked = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spritebatch.Draw(texture, position, rectangle, Color.White, rotation, originalPosition, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, position, rectangle, Color.White, rotation, originalPosition, 1f, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime, MouseState mousePresent, MouseState mousePast, SoundEffect click1, SoundEffect click2)
        {

            rectangle = new Rectangle(0, currentFrame * frameHeight, frameWidth, frameHeight);
            originalPosition = new Vector2(rectangle.Width / 2, rectangle.Height / 2);

            Rectangle Rec = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 4, texture.Width , texture.Height / 2); //прямоугольник содержащий кнопку

            if (Rec.Contains(mousePresent.X, mousePresent.Y)) //анимация кнопки по наведению мыши
            {
                currentFrame = 0;
                if(!mouseIn)
                {
                    click1.Play(0.6f, 0.0f, 0.0f);//звук наведения на клавишу
                    mouseIn = true;
                }
            }
            else
            {
                currentFrame = 1;
                mouseIn = false;
                IsClicked = false;
            }

            if (Rec.Contains(mousePresent.X, mousePresent.Y) && mousePresent.LeftButton == ButtonState.Pressed && mousePast.LeftButton == ButtonState.Released) //обработка нажатия
            {
                click2.Play(0.7f, 0.0f, 0.0f); //звук нажатия клавиши
                IsClicked = true;
            }


        }


    }
}
