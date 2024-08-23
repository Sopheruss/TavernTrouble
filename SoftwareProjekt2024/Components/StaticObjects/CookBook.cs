using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Screens;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class CookBook : StaticObject
{
    static AnimationManager _cookBookAnimationManager;
    public static Texture2D _cookBookAnimation;
    public static Texture2D _cookBookClose;

    public static bool _playCookBookAnimation = false;

    public CookBook(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        _cookBookAnimationManager = new AnimationManager(4, 4, new Vector2(32, 64));
        _cookBookAnimationManager.RowPos = 0;
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public static void HandleInteraction()
    {
        /* what esle should happen: 
            - before opening new scene, play cookBook animation 
            - after closing scene, close book again
        */

        _playCookBookAnimation = true;
    }

    public static void Update()
    {
        _cookBookAnimationManager.Update();

        //Debug.WriteLine("activeFrame:" + _cookBookAnimationManager.activeFrame);
        //Debug.WriteLine("numFrame:" + _cookBookAnimationManager.numFrames);

        if (_cookBookAnimationManager.activeFrame == 3)
        {
            _playCookBookAnimation = false;
            _cookBookAnimationManager.PlayAnimation = false;
            _cookBookAnimationManager.ResetAnimation();
            Game1.activeScene = Scenes.COOKBOOKSCREEN;
            GamePlay._timer.Stop();
        }
    }

    public override void draw(SpriteBatch spriteBatch)
    {
        if (_playCookBookAnimation == false)
        {
            spriteBatch.Draw(texture, dest, src, Color.White);
        }
        else
        {
            spriteBatch.Draw(
            _cookBookAnimation,                          //texture 
            dest,                                       //destinationRectangle
            _cookBookAnimationManager.GetFrame(),      //sourceRectangle (frame) 
            Color.White,                              //color
            0f,                                      //rotation 
            Vector2.Zero,                           //origin
            SpriteEffects.None,                    //effects
            1f);                                  //layer depth
        }
    }
}
