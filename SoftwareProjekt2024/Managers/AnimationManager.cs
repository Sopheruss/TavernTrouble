using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SoftwareProjekt2024.Managers;

//Enum to figure out which Part of the Animation is played 
enum RowCounter
{
    Left,
    Right, 
    Up,
    Down
}

public class AnimationManager
{
    readonly int numFrames;
    readonly int numColumns;
    Vector2 size;

    int counter;
    int activeFrame;
    readonly int interval;

    public int RowPos {  get; set; }
    int colPos;

    readonly RowCounter rowCounter;

    public AnimationManager(int numFrames, int numColumns, Vector2 size)
    {
        this.numFrames = numFrames;
        this.numColumns = numColumns;
        this.size = size;

        counter = 0;
        activeFrame = 0;
        interval = 15; //30 Frames Intervall 

        RowPos = 0;
        colPos = 0;
    }

    public void Update()
    {
        //couter for animation 
        counter++;
        if (counter > interval) //animation changes every interval frames 
        {
            counter = 0; //counter reset 
            NextFrame();
        }
    }

    private void NextFrame()
    {
        activeFrame++;
        colPos++;

        if (activeFrame >= numFrames) //reset active frames 
        {
            ResetAnimation();
        }

        if (colPos >= numColumns) //reset columns
        {
            colPos = 0;
            //rowPos is changing based RowCounter
            WhichRow();
        }
    }

    private void WhichRow()
    {
        //sets which Animation Playes based on which Row takes Place 
        if (rowCounter == RowCounter.Left)
        {
            RowPos = 1;
        }

        if (rowCounter == RowCounter.Right)
        {
            RowPos = 2;
        }

        if(rowCounter == RowCounter.Up)
        {
            RowPos = 3; 
        }

        if(rowCounter == RowCounter.Down)
        {
            RowPos = 4;
        }
    }

    

    private void ResetAnimation()
    {
        activeFrame = 0;
        colPos = 0;
    }

    public Rectangle GetFrame()
    {
        return new Rectangle(
            colPos * (int)size.X,
            RowPos * (int)size.Y,
            (int)size.X,
            (int)size.Y);
    }
}
