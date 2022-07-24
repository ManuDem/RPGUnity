using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> surfSprites;

    [SerializeField] List<Sprite> runDownSprites;
    [SerializeField] List<Sprite> runUpSprites;
    [SerializeField] List<Sprite> runRightSprites;
    [SerializeField] List<Sprite> runLeftSprites;

    [SerializeField] List<Sprite> bikeDownSprites;
    [SerializeField] List<Sprite> bikeUpSprites;
    [SerializeField] List<Sprite> bikeRightSprites;
    [SerializeField] List<Sprite> bikeLeftSprites;

    [SerializeField] List<Sprite> surfDownSprites;
    [SerializeField] List<Sprite> surfUpSprites;
    [SerializeField] List<Sprite> surfRightSprites;
    [SerializeField] List<Sprite> surfLeftSprites;

    [SerializeField] private bool isPlayer;

    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool IsJumping { get; set; }
    public bool IsSurfing { get; set; }
    public bool IsBiking { get; set; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }

    // States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator runDownAnim;
    SpriteAnimator runUpAnim;
    SpriteAnimator runRightAnim;
    SpriteAnimator runLeftAnim;

    SpriteAnimator bikeDownAnim;
    SpriteAnimator bikeUpAnim;
    SpriteAnimator bikeRightAnim;
    SpriteAnimator bikeLeftAnim;

    SpriteAnimator surfDownAnim;
    SpriteAnimator surfUpAnim;
    SpriteAnimator surfRightAnim;
    SpriteAnimator surfLeftAnim;

    SpriteAnimator currentAnim;
    bool wasPreviouslyMoving;

    // Refrences
    SpriteRenderer spriteRenderer;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        runDownAnim = new SpriteAnimator(runDownSprites, spriteRenderer);
        runUpAnim = new SpriteAnimator(runUpSprites, spriteRenderer);
        runRightAnim = new SpriteAnimator(runRightSprites, spriteRenderer);
        runLeftAnim = new SpriteAnimator(runLeftSprites, spriteRenderer);

        bikeDownAnim = new SpriteAnimator(bikeDownSprites, spriteRenderer);
        bikeUpAnim = new SpriteAnimator(bikeUpSprites, spriteRenderer);
        bikeRightAnim = new SpriteAnimator(bikeRightSprites, spriteRenderer);
        bikeLeftAnim = new SpriteAnimator(bikeLeftSprites, spriteRenderer);

        surfDownAnim = new SpriteAnimator(surfDownSprites, spriteRenderer);
        surfUpAnim = new SpriteAnimator(surfUpSprites, spriteRenderer);
        surfRightAnim = new SpriteAnimator(surfRightSprites, spriteRenderer);
        surfLeftAnim = new SpriteAnimator(surfLeftSprites, spriteRenderer);

        SetFacingDirection(defaultDirection);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;

        if ((Input.GetKeyDown(KeyCode.B)) && IsPlayer)
        {
            //if(!isOnBike)
            //AudioManager.
            IsBiking = !IsBiking;
        }

        if (IsBiking && IsPlayer)
        {
            if (MoveX == 1)
                currentAnim = bikeRightAnim;
            else if (MoveX == -1)
                currentAnim = bikeLeftAnim;
            else if (MoveY == 1)
                currentAnim = bikeUpAnim;
            else if (MoveY == -1)
                currentAnim = bikeDownAnim;
        }
        else if (IsSurfing && IsPlayer)
        {
            if (MoveX == 1)
                currentAnim = surfRightAnim;
            else if (MoveX == -1)
                currentAnim = surfLeftAnim;
            else if (MoveY == 1)
                currentAnim = surfUpAnim;
            else if (MoveY == -1)
                currentAnim = surfDownAnim;

        }
        else if (!IsBiking && !IsSurfing && IsPlayer)
        {

            if ((Input.GetKey(KeyCode.LeftShift)) && IsPlayer)
            {
                if (MoveX == 1)
                    currentAnim = runRightAnim;
                else if (MoveX == -1)
                    currentAnim = runLeftAnim;
                else if (MoveY == 1)
                    currentAnim = runUpAnim;
                else if (MoveY == -1)
                    currentAnim = runDownAnim;

            }
            else
            {
                if (MoveX == 1)
                    currentAnim = walkRightAnim;
                else if (MoveX == -1)
                    currentAnim = walkLeftAnim;
                else if (MoveY == 1)
                    currentAnim = walkUpAnim;
                else if (MoveY == -1)
                    currentAnim = walkDownAnim;
            }
        }
        else
        {
            if (MoveX == 1)
                currentAnim = walkRightAnim;
            else if (MoveX == -1)
                currentAnim = walkLeftAnim;
            else if (MoveY == 1)
                currentAnim = walkUpAnim;
            else if (MoveY == -1)
                currentAnim = walkDownAnim;
        }

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if (IsJumping)
            spriteRenderer.sprite = currentAnim.Frames[currentAnim.Frames.Count - 1];
        else if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[0];

        wasPreviouslyMoving = IsMoving;
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
            MoveX = 1;
        else if (dir == FacingDirection.Left)
            MoveX = -1;
        else if (dir == FacingDirection.Down)
            MoveY = -1;
        else if (dir == FacingDirection.Up)
            MoveY = 1;
    }

    public FacingDirection DefaultDirection
    {
        get => defaultDirection;
    }

}


public enum FacingDirection { Up, Down, Left, Right }
