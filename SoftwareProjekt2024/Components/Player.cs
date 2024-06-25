using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components;

internal class Player : Component
{


    public Player(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    { }

    public override void Update() //Update der Position
    {
        base.Update();
    }
    public override void draw(SpriteBatch _spriteBatch, AnimationManager _animationManager) // generalisierter Aufruf der Spritedraw Methode
    {
        _spriteBatch.Draw(
        this.texture,                                //texture 
        this.Rect,                                  //destinationRectangle
        _animationManager.GetFrame(),                   //sourceRectangle (frame) 
        Color.White,                                   //color
        0f,                                           //rotation 
        Vector2.Zero,                                //origin -> to place center texture correctly
        SpriteEffects.None,                        //effects
        1f);                                      //layer depth
    }
}
