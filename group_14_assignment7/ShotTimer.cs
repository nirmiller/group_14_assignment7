namespace group_14_assignment7;

using Microsoft.Xna.Framework;

public class ShotTimer
{
    private float currentTime;
    private float interval;

    public bool IsRepeating;
    public bool IsPaused;
    public bool IsTriggered;

    private bool triggerOnStart;

    public ShotTimer(float interval, bool repeat = true)
    {
        this.interval = interval;
        this.IsRepeating = repeat;

        currentTime = 0f;
        IsPaused = false;
        IsTriggered = false;

        triggerOnStart = false;
    }

    public bool isTriggered()
    {
        return triggerOnStart;
    }

    public void Update(GameTime gameTime)
    {
        IsTriggered = false;

        if (IsPaused)
            return;

        currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (currentTime >= interval)
        {
            IsTriggered = true;

            if (triggerOnStart)
                triggerOnStart = false;

            if (IsRepeating)
                currentTime = 0f;
            else
                IsPaused = true;
        }
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void Start()
    {
        IsPaused = false;
        currentTime = 0f;
        triggerOnStart = true;
    }

    public void Restart()
    {
        currentTime = 0f;
        IsPaused = false;
        IsTriggered = false;
        triggerOnStart = true;
    }
}