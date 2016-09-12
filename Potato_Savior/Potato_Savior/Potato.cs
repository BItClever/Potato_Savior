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
    class Potato
    {
        private Texture2D texture; //текстура
        private Rectangle rectangle; //управляющий прямоугольник 
        private Vector2 position; //позиция в данный момент
        private Vector2 originalPosition; //стартовая позиция, нужна только для отрисовки
        private Vector2 velocity; //скорость 
        private Vector2 acceleration; //ускорение
        private float rotation; //вращение в радианах
        public bool isClicked;
        public bool isVisible;

        Random random = new Random();

        public Potato(Texture2D newTexture, Plant kust)
        {
            isClicked = false;
            texture = newTexture;
            rectangle = new Rectangle(0,0,55,55);
            position =  new Vector2(kust.PlantPosition.X + kust.plantRectangle.Width/2, kust.PlantPosition.Y + kust.plantRectangle.Height /2);
            originalPosition = new Vector2(27, 27);
            rotation = random.Next(1001) / 20;
            velocity = new Vector2(random.Next(3, 8) * (int)Math.Pow(-1, random.Next(0,2)), random.Next(-20, -7)) *0.75f;
            isVisible = true;
            acceleration = new Vector2(0, 2) * 0.25f;
        }

        public void Update(MouseState mousePresent, MouseState mousePast)
        {

            Rectangle Rec = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, texture.Width , texture.Height); //прямоугольник содержащий картошку
            if (Rec.Contains(mousePresent.X, mousePresent.Y) && mousePresent.LeftButton == ButtonState.Pressed && mousePast.LeftButton == ButtonState.Released) //взятие картошки
            {
                isClicked = !isClicked;
            }

            if (isClicked)
            {
                velocity.Y = 0;
                velocity.X = 0;
                acceleration.X = 0;
                acceleration.Y = 0;
                position.X = mousePresent.X;
                position.Y = mousePresent.Y;
            }
            position += velocity;
            velocity += acceleration;
            if(velocity.Y >= 13)
            {
                velocity.Y = 0;
                velocity.X = 0;
                acceleration.X = 0;
                acceleration.Y = 0;
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, rotation, originalPosition, 1f, SpriteEffects.None, 0);
        }


    }
}
