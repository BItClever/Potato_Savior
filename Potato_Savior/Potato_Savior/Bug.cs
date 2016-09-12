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
    class Bug
    {
        Random random = new Random();

        private Texture2D bugTexture;
        private Rectangle bugRectangle;

        private Vector2 bugPosition;
        private Vector2 bugVelocity;
        private Vector2 bugOriginalPosition;

        private float bugRotation;
        public bool isVisible;
        private float timer;
        private float interval = 50;  //интервал в миллисекундах между сменой кадров

        private int frameHeight;
        private int frameWidth;
        private int currentFrame;


        public Bug(Texture2D newBugTexture, int newFrameHeigth, int newFrameWidth, Plant Kust) //конструктор. Жук при создании направляется к кусту
        {
            bugTexture = newBugTexture;
            frameHeight = newFrameHeigth;
            frameWidth = newFrameWidth;
            //генерация жуков строго по рамке экрана
            int gran;
            gran = random.Next(4);
            switch (gran)
            {
                case 0:
                    bugPosition = new Vector2(random.Next(0, 1300), 0);
                    break;

                case 1:
                    bugPosition = new Vector2(random.Next(0, 1300), 720);                   
                    break;

                case 2:
                    bugPosition = new Vector2(0, random.Next(0, 720));
                    break;

                case 3:
                    bugPosition = new Vector2(1300, random.Next(0, 720));
                    break;
            }
            
            bugRotation = (float)Math.Atan2(bugPosition.Y - Kust.PlantPosition.Y - Kust.plantRectangle.Height, bugPosition.X - Kust.PlantPosition.X - Kust.plantRectangle.Height) + (float)Math.PI;
            bugVelocity = new Vector2((float)Math.Cos(bugRotation), (float)Math.Sin(bugRotation)) * 5f;

            isVisible = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //spritebatch.Draw(texture, position, rectangle, Color.White, rotation, originalPosition, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(bugTexture, bugPosition, bugRectangle, Color.White, bugRotation, bugOriginalPosition, 1f, SpriteEffects.None, 0);
        }


        public void Update(GameTime gameTime, Plant Kust, MouseState mousePresent, MouseState mousePast, SoundEffect sound)
        {


            Rectangle Rec = new Rectangle((int)bugPosition.X - bugTexture.Width /8, (int)bugPosition.Y - bugTexture.Height /2, bugTexture.Width / 4, bugTexture.Height); //прямоугольник содержащий жука, ужасная формула
            if (Rec.Contains(mousePresent.X, mousePresent.Y) && mousePresent.LeftButton == ButtonState.Pressed && mousePast.LeftButton == ButtonState.Released) //убиение жука по нажатию клавиши
            {
                sound.Play(0.4f, 0.0f, 0.0f);
                isVisible = false;
            }

            /*
            if(Rec.Intersects(Kust.plantRectangle)) //пересечение куста с жуками, где-то здесь нужно прописывать атаку куста  
            {
                bugPosition -= bugVelocity;
            }
            */
            float distance = (float)Math.Sqrt(Math.Abs(bugPosition.X - (Kust.PlantPosition.X + Kust.plantRectangle.Width / 2)) //здесь начинается подсчёт расстояния от жука до центра куста. То есть распределение жуков идёт по окружности а не по прямоугольнику
                * Math.Abs(bugPosition.X - (Kust.PlantPosition.X + Kust.plantRectangle.Width / 2))
                + Math.Abs(bugPosition.Y - (Kust.PlantPosition.Y + Kust.plantRectangle.Height/2)) * Math.Abs(bugPosition.Y - (Kust.PlantPosition.Y + Kust.plantRectangle.Height/2)));
            
            if (distance < 77) //пересечение куста с жуками, где-то здесь нужно прописывать атаку куста
            {
                bugPosition -= bugVelocity;
                Kust.health -= 1;
            }

            bugRectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
           // bugOriginalPosition = new Vector2(bugRectangle.Width / 2, bugRectangle.Height / 2);
            bugRectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            bugOriginalPosition = new Vector2(bugRectangle.Width / 2, bugRectangle.Height / 2);

            
                bugPosition += bugVelocity;
                if (Vector2.Distance(bugPosition, Kust.PlantPosition) > 2000)
                   isVisible = false;
            
            

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 3)
                    currentFrame = 0;
            }

            bugRotation = (float)Math.Atan2(bugPosition.Y - Kust.PlantPosition.Y - Kust.plantRectangle.Height/2, bugPosition.X - Kust.PlantPosition.X - Kust.plantRectangle.Width/2) + (float)Math.PI;
            bugVelocity = new Vector2((float)Math.Cos(bugRotation), (float)Math.Sin(bugRotation)) * 1f;


        }

    }
}
