using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment7;

public class Spaceship
{
    // ship
    private Texture2D _spaceship;
    private Vector2 _direction;     // can be up, down left or right 
    private float _shipSpeed;       // speed will be the same in each direction with no acceleration 
    private Vector2 _shipVelocity;
    private Boolean _isShooting = false;
    private Boolean _isAlive = true;
    
    // shot 
    private Texture2D _shot;
    private float _shotSpeed;
    private Vector2 _shotVelocity;      // direction will be the same as spaceship's direction 

    public Spaceship(Texture2D spaceshipTexture,
        Vector2 direction,
        float shipSpeed,
        Texture2D shot,
        float shotSpeed)
    {
        // ship
        _spaceship = spaceshipTexture;
        _direction = direction;
        _shipSpeed = shipSpeed;
        
        // shot
        _shot = shot;
        _shotSpeed = shotSpeed;
        
    }
}