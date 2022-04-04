using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteAnimation defaultAnimation;
    public bool autoPlay = true;
    public SpriteAnimation currentAnimation;

    bool active;

    SpriteRenderer renderer;

    public int currentFrame;
    public int startOnFrame = 7;

    float frameTime;
    float currentTime = 0f;

    bool reset = false;

    Action<bool> frameReachedCallback;
    int frameToCall;



    // Start is called before the first frame update
    void Start()
    {
        active = false;
        renderer = GetComponent<SpriteRenderer>();

        if (autoPlay && defaultAnimation != null)
            SetAnimation(defaultAnimation);
    }

    public void SetAnimationReset(SpriteAnimation animation)
    {
        reset = true;
        SetAnimation(animation);
    }
    public void SetAnimation(SpriteAnimation animation)
    {
        if (frameReachedCallback != null)
            frameReachedCallback(false);
        frameReachedCallback = null;
        if (animation.fps <= 0f || animation.sprites.Count == 0)
        {
            Debug.LogError("SpriteAnimation given with no frames or 0 fps!");
            return;
        }

        if (currentAnimation == animation && !reset)
            return;

        currentAnimation = animation;
        frameTime = 1f / (float)animation.fps;
        currentFrame = startOnFrame;
        active = true;
        reset = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        currentTime += Time.deltaTime;
        if (currentTime > frameTime)
        {
            currentTime = 0f;
            NextFrame();
        }
    }

    void NextFrame()
    {
        currentFrame++;

        if (currentAnimation == null)
            return;

        if (currentFrame > currentAnimation.sprites.Count - 1)
            currentFrame = 0;

        if (frameReachedCallback != null && currentFrame == frameToCall)
        {
            frameReachedCallback(true);
            frameReachedCallback = null;
        }

        renderer.sprite = currentAnimation.sprites[currentFrame];
    }

    public void RequestCallbackOnFrame(int frame, Action<bool> call)
    {
        frameReachedCallback = call;
        frameToCall = frame;
    }

    public void SetActive(bool active)
    {
        this.active = active;
        if (!active)
        {
            currentFrame = 8;
            NextFrame();
        }
    }
}

