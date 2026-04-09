namespace group_14_assignment7;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Asteroid
{
    private Texture2D _bodyTexture;
    private Texture2D _tailTexture;

    private Vector2 _bodyPosition;
    private float _bodyRotation;
    private float _size;
    private float _tailWiggle;
    private float _tailWiggleSpeed;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private float _startSize;
    private float _progress;
    private float _spin;
    private float _t;
    public float score;

    public bool isAlive;

    private Vector2 _dir;

    private bool _useRandomTrajectory;
    private Vector2 _curveOffset;

    public Asteroid(Texture2D bodyTexture, Texture2D tailTexture, float size, Vector2 bodyPosition, float _score, Vector2 endPosition, bool useRandomTrajectory)
    {
        _bodyTexture = bodyTexture;
        _tailTexture = tailTexture;
        _size = size;
        _startSize = size;
        _t = 0f;
        _bodyPosition = bodyPosition;
        _startPosition = bodyPosition;
        _endPosition = endPosition;
        _progress = 0f;
        _tailWiggleSpeed = 2f;
        score = _score;
        isAlive = true;
        _useRandomTrajectory = useRandomTrajectory;
        _curveOffset = useRandomTrajectory ? GenerateCurveOffset(bodyPosition, endPosition) : Vector2.Zero;

        Vector2 v = _endPosition - _startPosition;
        _dir = v.LengthSquared() > 0f ? Vector2.Normalize(v) : Vector2.UnitX;
        _bodyRotation = (float)Math.Atan2(_dir.Y, _dir.X);
    }

    public void ResetAnimation()
    {
        _t = 0f;
        _progress = 0f;
        _spin = 0f;
        _bodyPosition = _startPosition;
        _size = _startSize;
        _tailWiggle = 0f;
        _curveOffset = _useRandomTrajectory ? GenerateCurveOffset(_startPosition, _endPosition) : Vector2.Zero;
        isAlive = true;
        Vector2 v = _endPosition - _startPosition;
        _dir = v.LengthSquared() > 0f ? Vector2.Normalize(v) : Vector2.UnitX;
        _bodyRotation = (float)Math.Atan2(_dir.Y, _dir.X);
    }

    public void SetPath(Vector2 start, Vector2 end, bool useRandomTrajectory)
    {
        _startPosition = start;
        _endPosition = end;
        _bodyPosition = start;
        _useRandomTrajectory = useRandomTrajectory;
        _curveOffset = useRandomTrajectory ? GenerateCurveOffset(start, end) : Vector2.Zero;
        ResetAnimation();
    }

    public void Animate(GameTime gameTime, float duration)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _t += dt;

        if (duration <= 0f)
            duration = 1f;

        _progress += dt / duration;
        if (_progress > 1f)
            _progress = 1f;

        _tailWiggle = 0.1f * (float)Math.Sin(_tailWiggleSpeed * _t);
        _spin += dt * 3f;

        Vector2 previousPosition = _bodyPosition;

        if (_useRandomTrajectory)
        {
            Vector2 control = (_startPosition + _endPosition) * 0.5f + _curveOffset;
            float u = 1f - _progress;
            _bodyPosition =
                u * u * _startPosition +
                2f * u * _progress * control +
                _progress * _progress * _endPosition;
        }
        else
        {
            _bodyPosition.X = MathHelper.Lerp(_startPosition.X, _endPosition.X, _progress);
            _bodyPosition.Y = MathHelper.Lerp(_startPosition.Y, _endPosition.Y, _progress);
        }

        Vector2 delta = _bodyPosition - previousPosition;
        if (delta.LengthSquared() > 0.0001f)
        {
            _dir = Vector2.Normalize(delta);
            _bodyRotation = (float)Math.Atan2(_dir.Y, _dir.X);
        }

        if (_progress >= 1f)
        {
            ResetAnimation();
        }
    }

    private Vector2 GenerateCurveOffset(Vector2 start, Vector2 end)
    {
        Vector2 path = end - start;
        if (path.LengthSquared() <= 0.0001f)
            return Vector2.Zero;

        Vector2 normal = new Vector2(-path.Y, path.X);
        normal.Normalize();

        float magnitude = Random.Shared.Next(80, 220);
        float sign = Random.Shared.Next(2) == 0 ? -1f : 1f;

        return normal * magnitude * sign;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 parentOrigin = new Vector2(_bodyTexture.Width / 2f, _bodyTexture.Height / 2f);

        Matrix M_parent =
            Matrix.CreateTranslation(-parentOrigin.X, -parentOrigin.Y, 0f) *
            Matrix.CreateScale(_size, _size, 1f) *
            Matrix.CreateRotationZ(_bodyRotation) *
            Matrix.CreateTranslation(_bodyPosition.X, _bodyPosition.Y, 0f);

        Vector2 tailBaseOrigin = new Vector2(_tailTexture.Width, _tailTexture.Height);
        Vector2 socketLocal = new Vector2((_bodyTexture.Width / 2f), (_bodyTexture.Height / 2f));
        float tailArtOffset = -MathHelper.Pi / 3;

        Matrix M_childLocal =
            Matrix.CreateTranslation(-tailBaseOrigin.X, -tailBaseOrigin.Y, 0f) *
            Matrix.CreateRotationZ(tailArtOffset + _tailWiggle) *
            Matrix.CreateTranslation(socketLocal.X, socketLocal.Y, 0f) *
            Matrix.CreateScale(_size / 2f, _size / 2f, 1f);

        Matrix M_childWorld = M_childLocal * M_parent;

        spriteBatch.Begin(transformMatrix: M_childWorld);
        spriteBatch.Draw(_tailTexture, Vector2.Zero, Color.White);
        spriteBatch.End();

        Matrix M_body =
            Matrix.CreateTranslation(-parentOrigin.X, -parentOrigin.Y, 0f) *
            Matrix.CreateScale(_size, _size, 1f) *
            Matrix.CreateRotationZ(_bodyRotation + _spin) *
            Matrix.CreateTranslation(_bodyPosition.X, _bodyPosition.Y, 0f);

        spriteBatch.Begin(transformMatrix: M_body);
        spriteBatch.Draw(_bodyTexture, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
}