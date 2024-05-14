using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024
{
    internal class AnimationManager
    {
        int numFrames;
        int numColumns;
        Vector2 size;

        int counter;
        int activeFrame;
        int interval;

        int rowPos;
        int colPos;

        public AnimationManager(int numFrames, int numColumns, Vector2 size) {
            this.numFrames = numFrames;
            this.numColumns = numColumns;
            this.size = size;

            counter = 0;
            activeFrame = 0;
            interval = 30; //30 Frames Intervall 

            rowPos = 0;
            colPos = 0;
        }    

        public void Update()
        {
            //couter for animation 
            counter++;
            if (counter > interval) //animation changes every 30 frames 
            {
                counter = 0; //counter reset 
                NextFrame();
            }
        }

        private void NextFrame()
        {
            activeFrame++;
            colPos++;
            if(activeFrame >= numFrames) //reset active frames 
            {
                ResetAnimation();
            }

            if(colPos >= numColumns) //reset columns
            {
                colPos = 0;
                rowPos++;
            }
        }

        private void ResetAnimation()
        {
            activeFrame = 0;
            colPos = 0;
            rowPos = 0;
        }

        public Rectangle GetFrame()
        {
            return new Rectangle(
                colPos * (int)size.X, 
                rowPos * (int)size.Y, 
                (int)size.X, 
                (int)size.Y);
        }
    }
}
