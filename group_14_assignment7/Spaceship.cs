using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment7;

public class Spaceship
{
    // screen info 
    private float _screenWidth;
    private float _screenHeight;
    
    // ship
    private Texture2D _ship;
    private Vector2 _postition;
    private float _direction = 1;     // up = 1, right = 2, down = 3, left = 4 
    private float _shipSpeed;       // speed will be the same in each direction with no acceleration 
    private Vector2 _shipVelocity;      // (x,y); up = (0, shipSpeed);
                                        // right = (shipSpeed, 0);
                                        // down = (0, -shipSpeed);
                                        // left = (-shipSpeed, 0)
    private Boolean _isShooting = false;
    private Boolean _isAlive = true;
    
    // shot 
    private Texture2D _shot;
    private float _shotSpeed;
    private Vector2 _shotVelocity;      // direction will be the same as spaceship's direction 

    public Spaceship(float screenWidth,
        float screenHeight,
        Texture2D spaceshipTexture,
        //float direction,
        float shipSpeed,
        Texture2D shot,
        float shotSpeed)
    {
        // screen info 
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        
        // ship
        _ship = spaceshipTexture;
        _postition = new Vector2(_screenWidth / 2, _screenHeight / 2);
        //_direction = direction;     
        _shipSpeed = shipSpeed;
        
        // shot
        _shot = shot;
        _shotSpeed = shotSpeed;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(
            texture: _ship,
            position: _postition,
            sourceRectangle: null,
            color: Color.White,
            rotation: 0f,
            origin: new (_ship.Width/2f, _ship.Height/2f),
            scale: 3f,
            effects: SpriteEffects.None,
            layerDepth: 0f);
        spriteBatch.End();
    }
}