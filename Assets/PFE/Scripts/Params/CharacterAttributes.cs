using FixedPointy;

[System.Serializable]
public class CharacterAttributes {
    public Fix weight;
    public Fix gravity;
    public Fix walkInitialVelocity;
    public Fix walkAcceleration;
    public Fix minWalkSpeed;
    public Fix walkSpeed;
    public Fix dashInitialVelocity;
    public Fix dashRunAcceleration;
    public Fix dashRunDeceleration;
    public Fix dashSpeed; //Max velocity
    public Fix runMaxVelo; //Max velocity
    public Fix airSpeed; //Max velocity
    public Fix maxFallSpeed;
    public Fix fastFallSpeed;
    public Fix airAcceleration;
    public Fix airDeceleration;
    public Fix airFriction;
    public Fix jumpVelo;
    public Fix sHopVelo;
    public Fix groundToAir; //Modifies horizontal velocity when jumping
    public Fix dJumpMulti;
    public Fix dJumpMomentum;
    public int maxJumps;
    public bool wallJump;
    public bool wallCling;
    public bool crouch;
    public bool crawl;
    public bool tether;
    public int dashDanceWindow;
    public int dashFrames;
    public int jumpSquat;
    public int jumpFrameTime; //Length of the up part of the jump (in frames)
    public int softLandingLag;
    public int hardLandingLag;
    public Fix traction;
    public Fix shieldSize;
}