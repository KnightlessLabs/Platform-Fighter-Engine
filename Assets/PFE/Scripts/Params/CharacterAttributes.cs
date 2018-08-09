[System.Serializable]
public class CharacterAttributes {
    public int weight;
    public float gravity;
    public float walkInitialVelocity;
    public float walkAcceleration;
    public float minWalkSpeed;
    public float walkSpeed;
    public float dashInitialVelocity;
    public float dashRunAcceleration;
    public float dashRunDeceleration;
    public float dashSpeed; //Max velocity
    public float runMaxVelo; //Max velocity
    public float airSpeed; //Max velocity
    public float maxFallSpeed;
    public float fastFallSpeed;
    public float airAcceleration;
    public float airDeceleration;
    public float airFriction;
    public float jumpVelo;
    public float sHopVelo;
    public float groundToAir; //Modifies horizontal velocity when jumping
    public float dJumpMulti;
    public float dJumpMomentum;
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
    public float traction;
    public float shieldSize;
}