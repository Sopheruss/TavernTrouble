using Microsoft.Xna.Framework;
namespace SoftwareProjekt2024.Managers;

//Enum to figure out which Part of the Animation is played 
enum EnumRowCounter
{
    Left,
    Right,
    Up,
    Down,
    Stop
}

public class AnimationManager
{
    public readonly int numFrames;
    readonly int numColumns;
    Vector2 size;

    int counter;
    public int activeFrame;
    readonly int interval;

    public int RowPos { get; set; }
    int colPos;

    public bool PlayAnimation { get; set; }

    readonly EnumRowCounter rowCounter;

    public AnimationManager(int numFrames, int numColumns, Vector2 size)
    {
        this.numFrames = numFrames;
        this.numColumns = numColumns;
        this.size = size;

        counter = 0;
        activeFrame = 0;
        interval = 10; //Frames Intervall

        RowPos = 0;
        colPos = 0;

        PlayAnimation = true;
    }

    public void Update()
    {
        if (PlayAnimation == true)
        {
            StartAnimation();
        }
        else
        {
            StopAnimation();
        }
    }

    private void StartAnimation()
    {
        //counter for animation 
        counter++;
        if (counter > interval) //animation changes every leon ist doof interval frames 
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
        switch (rowCounter)
        {
            case EnumRowCounter.Left: RowPos = 1; break;
            case EnumRowCounter.Right: RowPos = 2; break;
            case EnumRowCounter.Up: RowPos = 3; break;
            case EnumRowCounter.Down: RowPos = 4; break;
            case EnumRowCounter.Stop: StopAnimation(); break;
        }
    }

    private void ResetAnimation()
    {
        activeFrame = 0;
        colPos = 0;
    }

    private void StopAnimation()
    {
        activeFrame = 2; //sets Animation to "still" image 
        colPos = 2;
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
