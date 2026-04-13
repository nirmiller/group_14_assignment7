using System.Collections.Generic;

namespace group_14_assignment7;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Asteroid
{
    private Texture2D _bodyTexture;
    private Texture2D _tailTexture;
    public float radius;
    private Vector2 _bodyPosition;
    private float _bodyRotation;
    private float _size;
    private float _tailWiggle;
    private float _tailWiggleSpeed;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private float _startSize;
    private float _spin;
    private float _t;
    public float score;

    public bool isAlive;

    private Vector2 _dir;
    private Vector2 _velocity;
    private float _speed;

    private bool _useRandomTrajectory;
    private float _curveAmount;
    private float _curveFrequency;
    private float _travelTime;

    public Asteroid(Texture2D bodyTexture, Texture2D tailTexture, float size, Vector2 bodyPosition, float _score, Vector2 endPosition, bool useRandomTrajectory)
    {
        
        _bodyTexture = bodyTexture;
        _tailTexture = tailTexture;
        
        
       
        _size = size;
        _startSize = size;
        _t = 0f;
        _travelTime = 0f;
        _bodyPosition = bodyPosition;
        _startPosition = bodyPosition;
        _endPosition = endPosition;
        _tailWiggleSpeed = 2f;
        score = _score;
        isAlive = true;
        _useRandomTrajectory = useRandomTrajectory;

        Vector2 v = _endPosition - _startPosition;
        _dir = v.LengthSquared() > 0f ? Vector2.Normalize(v) : Vector2.UnitX;

        _speed = Random.Shared.Next(50, 100);       // originally (120, 220)
        _velocity = _dir * _speed;

        _curveAmount = Random.Shared.Next(20, 60) / 100f;
        _curveFrequency = Random.Shared.Next(10, 20) / 10f;

        _bodyRotation = (float)Math.Atan2(_dir.Y, _dir.X);
        radius = (_bodyTexture.Width * _size) * 0.3f;
    }

    public void ResetAnimation()
    {
        _t = 0f;
        _travelTime = 0f;
        _spin = 0f;
        _bodyPosition = _startPosition;
        _size = _startSize;
        _tailWiggle = 0f;
        isAlive = true;

        Vector2 v = _endPosition - _startPosition;
        _dir = v.LengthSquared() > 0f ? Vector2.Normalize(v) : Vector2.UnitX;

        _speed = Random.Shared.Next(120, 220);
        _velocity = _dir * _speed;

        _curveAmount = Random.Shared.Next(20, 60) / 100f;
        _curveFrequency = Random.Shared.Next(10, 20) / 10f;

        _bodyRotation = (float)Math.Atan2(_dir.Y, _dir.X);
    }
    
    public bool IsColliding(Vector2 otherPosition, float otherRadius)
    {
        float r = radius + otherRadius;
        return Vector2.DistanceSquared(_bodyPosition, otherPosition) <= r * r;
    }

    public bool CheckCollisions(List<(Vector2 pos, float radius)> collisionPoints)
    {
        if (collisionPoints == null || collisionPoints.Count == 0)
            return false;

        for (int i = 0; i < collisionPoints.Count; i++)
        {
            var (pos, otherRadius) = collisionPoints[i];

            if (IsColliding(pos, otherRadius))
            {
                return true;
            }
        }

        return false;
    }
    public void SetPath(Vector2 start, Vector2 end, bool useRandomTrajectory)
    {
        _startPosition = start;
        _endPosition = end;
        _bodyPosition = start;
        _useRandomTrajectory = useRandomTrajectory;
        ResetAnimation();
    }
    
    

    public void Animate(GameTime gameTime, List<(Vector2 pos, float radius)> collisionPoints)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _t += dt;
        _travelTime += dt;

        _tailWiggle = 0.1f * (float)Math.Sin(_tailWiggleSpeed * _t);
        _spin += dt * 3f;

        Vector2 moveDir = _dir;

        // ONLY check first collision point (the ship)
        if (isAlive && collisionPoints.Count > 0 &&
            IsColliding(collisionPoints[0].pos, collisionPoints[0].radius))
        {
            isAlive = false;
            return;
        }

        if (_useRandomTrajectory)
        {
            Vector2 normal = new Vector2(-_dir.Y, _dir.X);
            float bend = _curveAmount * (float)Math.Sin(_curveFrequency * _travelTime);
            moveDir = _dir + bend * normal;

            if (moveDir.LengthSquared() > 0.0001f)
                moveDir.Normalize();
            else
                moveDir = _dir;
        }

        _velocity = moveDir * _speed;
        _bodyPosition += _velocity * dt;

        if (_velocity.LengthSquared() > 0.0001f)
        {
            Vector2 facing = Vector2.Normalize(_velocity);
            _bodyRotation = (float)Math.Atan2(facing.Y, facing.X);
        }

        if (_bodyPosition.X < -250f || _bodyPosition.X > 1250f ||
            _bodyPosition.Y < -250f || _bodyPosition.Y > 1050f)
        {
            isAlive = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 parentOrigin = new Vector2(_bodyTexture.Width / 2f, _bodyTexture.Height / 2f);

        Matrix M_parent =
            Matrix.CreateTranslation(-parentOrigin.X, -parentOrigin.Y, 0f) *
            Matrix.CreateScale(_size, _size, 1f) *
            Matrix.CreateRotationZ(_bodyRotation) *
            Matrix.CreateTranslation(_bodyPosition.X, _bodyPosition.Y, 0f);


        /*
            Vector2 tailBaseOrigin = new Vector2(_tailTexture.Width, _tailTexture.Height / 2f);
            Vector2 socketLocal = new Vector2(-_bodyTexture.Width / 20f, _bodyTexture.Height / 5f);
            float tailArtOffset = -MathHelper.Pi / 3;

            Matrix M_childLocal =
                Matrix.CreateTranslation(-tailBaseOrigin.X, -tailBaseOrigin.Y, 0f) *
                Matrix.CreateScale(_size / 2f, _size / 2f, 1f) *
                Matrix.CreateRotationZ(tailArtOffset + _tailWiggle) *
                Matrix.CreateTranslation(socketLocal.X, socketLocal.Y, 0f);

            Matrix M_childWorld = M_childLocal * M_parent;

            spriteBatch.Begin(transformMatrix: M_childWorld);
            spriteBatch.Draw(_tailTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

        */
            
        Matrix M_body =
            Matrix.CreateTranslation(-parentOrigin.X, -parentOrigin.Y, 0f) *
            Matrix.CreateScale(_size, _size, 1f) *
            Matrix.CreateRotationZ(_bodyRotation + _spin) *
            Matrix.CreateTranslation(_bodyPosition.X, _bodyPosition.Y, 0f);

        spriteBatch.Begin(transformMatrix: M_body);
        spriteBatch.Draw(_bodyTexture, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
    
    public void Kill()
    {
        isAlive = false;
    }
    
}