using System;
using System.Collections.Generic;
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
    public Vector2 _position;
    private float _direction = 1; // up = 1, right = 2, down = 3, left = 4 
    private float _shipSpeed; // speed will be the same in each direction with no acceleration 
    private Vector2 _shipVelocity;
    private Boolean _isAlive = true;

    // turning 
    private KeyboardState _prevKeyState;

    // shot 
    private Texture2D _shot;
    private float _shotSpeed;
    private List<(Vector2 pos, Vector2 vel)> _shots = new();
    
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
        _position = new Vector2(_screenWidth / 2, _screenHeight / 2);
        _shipSpeed = shipSpeed;
        _shipVelocity = VeclocityDirection(_direction, _shipSpeed);

        // shot
        _shot = shot;
        _shotSpeed = shotSpeed;
    }

    public List<Vector2> GetShotPositions()
    {
        List<Vector2> positions = new List<Vector2>();

        foreach (var (pos, _) in _shots)
        {
            positions.Add(pos);
        }

        return positions;
    }

    public Vector2 getPosition() => _position;
    
    // helper- Velocity direction 
    private static Vector2 VeclocityDirection(float direction, float speed) => direction switch
    {
        1 => new Vector2(0, -speed), // up
        2 => new Vector2(speed, 0), // right
        3 => new Vector2(0, speed), // down
        4 => new Vector2(-speed, 0), // left 
    };
    
    // helper- direction to an angle 
    private static float DirectionToAngle(float direction) => direction switch
    {
        1 => 0f,                // up
        2 => MathF.PI / 2f,     // right
        3 => MathF.PI,          // down
        4 => -MathF.PI / 2f,    // left 
    };
    
    // helper- shooting start point offset 
    private Vector2 ShotOffset(Vector2 offset)
    {
        float angle = DirectionToAngle(_direction);
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Vector2(
            offset.X * cos - offset.Y * sin,
            offset.X * sin + offset.Y * cos);
    }

    // traveling and turning 
    private void Turning(KeyboardState keys, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        bool w = keys.IsKeyDown(Keys.W);
        bool a = keys.IsKeyDown(Keys.A);
        bool s = keys.IsKeyDown(Keys.S);
        bool d = keys.IsKeyDown(Keys.D);

        bool wPressed = w && !_prevKeyState.IsKeyDown(Keys.W);
        bool aPressed = a && !_prevKeyState.IsKeyDown(Keys.A);
        bool sPressed = s && !_prevKeyState.IsKeyDown(Keys.S);
        bool dPressed = d && !_prevKeyState.IsKeyDown(Keys.D);

        // setting direction 
        if (wPressed) _direction = 1;
        else if (aPressed) _direction = 4;
        else if (sPressed) _direction = 3;
        else if (dPressed) _direction = 2;

        // move if the key for the current direction is held
        if ((w && _direction == 1) ||
            (d && _direction == 2) ||
            (s && _direction == 3) ||
            (a && _direction == 4))
        {
            _position += VeclocityDirection(_direction, _shipSpeed) * dt;

            // wrap horizontally
            if (_position.X < 0)             _position.X = _screenWidth;
            if (_position.X > _screenWidth)  _position.X = 0;

            // wrap vertically
            if (_position.Y < 0)             _position.Y = _screenHeight;
            if (_position.Y > _screenHeight) _position.Y = 0;
        }
    }
    
    // shooting
    private void Shooting(KeyboardState keys, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 leftOffset  = new Vector2(-26, -30);
        Vector2 rightOffset = new Vector2( 26, -30);

        bool spacePressed = keys.IsKeyDown(Keys.Space)
                            && !_prevKeyState.IsKeyDown(Keys.Space);
        if (spacePressed)
        {
            Vector2 vel = VeclocityDirection(_direction, _shotSpeed);
            _shots.Add((_position + ShotOffset(leftOffset),  vel));
            _shots.Add((_position + ShotOffset(rightOffset), vel));
        }

        for (int i = _shots.Count - 1; i >= 0; i--)
        {
            var (pos, vel) = _shots[i];
            pos += vel * dt;

            bool offScreen = pos.X < 0 || pos.X > _screenWidth
                                       || pos.Y < 0 || pos.Y > _screenHeight;
            if (offScreen)
                _shots.RemoveAt(i);
            else
                _shots[i] = (pos, vel);
        }
    }

    public void Kill()
    {
        _isAlive = false;
    }

    public void Reset()
    {
        _isAlive = true;
        _position = new Vector2(_screenWidth / 2, _screenHeight / 2);
        _direction = 1;
        _shipVelocity = VeclocityDirection(_direction, _shipSpeed);
        _shots.Clear();
    }

    public bool IsAlive() => _isAlive;

    public void Update(GameTime gameTime)
    {
        if (!_isAlive) return;
        
        KeyboardState keys = Keyboard.GetState();
        
        Turning(keys, gameTime);
        
        Shooting(keys, gameTime);
        
        _prevKeyState = keys;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        spriteBatch.Draw(
            texture: _ship,
            position: _position,
            sourceRectangle: null,
            color: Color.White,
            rotation: DirectionToAngle(_direction),
            origin: new (_ship.Width/2f, _ship.Height/2f),
            scale: 1f,
            effects: SpriteEffects.None,
            layerDepth: 0f);
        
        foreach (var (pos, _) in _shots)
        {
            spriteBatch.Draw(
                texture: _shot,
                position: pos,
                sourceRectangle: null,
                color: Color.Yellow,
                rotation: DirectionToAngle(_direction),
                origin: new Vector2(_shot.Width / 2f, _shot.Height / 2f),
                scale: 2f,
                effects: SpriteEffects.None,
                layerDepth: 0f);
        }
        
        spriteBatch.End();
    }
}