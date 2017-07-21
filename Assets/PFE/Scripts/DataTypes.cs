using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public enum GameState {
    MainMenu, CharacterSelection, InMatch, ResultsScreen
}

public enum MatchState {
    Countdown, MidMatch, End
}

public enum CSSPlayerType {
    None, Human, CPU
}

public enum HurtboxType{
    TwoD, ThreeD
}

public enum HurtboxShape {
    Square, Circle
}

//BC = Movement, opposing yellow hitbox cannot pass through, HitCollider = opposing green hitbox can overlap, ThrowCollider = detects opposing throw hurtbox
public enum CollisionType {
    BodyCollider, HitCollider, ThrowCollider, NoCollider
}

public class InputInfo {

}

[System.Serializable]
public class HurtboxSetupHolder {
    public HurtboxType TypeofHurtboxes;
    public List<HurtboxInfo> Hurtboxes = new List<HurtboxInfo>();
}

[System.Serializable]
public class HurtboxInfo {
    public HurtboxShape Shape;
    public Vector3 Size;
    public Vector3 Offset;
    public CollisionType CollisionT;

    //3D
    public Transform BoneLinkedTo; //The reference to the bone it's linked to
    public string BoneLinkedToName; //The bone this hitbox is linked to
}

[System.Serializable]
public class AttributesHolder {
    public int Weight;
    public float Gravity;
    public float WalkInitialVelocity;
    public float WalkAcceleration;
    public float minWalkSpeed;
    public float WalkSpeed;
    public float DashInitialVelocity;
    public float DashRunAcceleration;
    public float DashRunDeceleration;
    public float DashSpeed; //Max velocity
    public float RunMaxVelo; //Max velocity
    public float AirSpeed; //Max velocity
    public float MaxFallSpeed;
    public float FastFallSpeed;
    public float AirAcceleration;
    public float AirDeceleration;
    public float AirFriction;
    public FP JumpVelo;
    public FP GroundToAir; //Modifies horizontal velocity when jumping
    public int MaxJumps;
    public bool WallJump;
    public bool WallCling;
    public bool Crouch;
    public bool Crawl;
    public bool Tether;
    public int DashDanceWindow;
    public int DashFrames;
    public int JumpSquat;
    public int JumpFrameTime; //Length of the up part of the jump (in frames)
    public int SoftLandingLag;
    public int HardLandingLag;
    public float Traction;
    public float ShieldSize;
}

[System.Serializable]
public class CombatStanceHolder {
    public BasicMovesHolder BasicMoves;
    //Editor Data, ignore
    public bool BasicMovesFoldout;
}

[System.Serializable]
public class BasicMovesHolder {
    public BasicMovesInfo Idle = new BasicMovesInfo();
    public BasicMovesInfo Walk = new BasicMovesInfo();
    public BasicMovesInfo Dash = new BasicMovesInfo();
    public BasicMovesInfo Run = new BasicMovesInfo();
    public BasicMovesInfo RunBrake = new BasicMovesInfo();
    public BasicMovesInfo Turn = new BasicMovesInfo();
    public BasicMovesInfo RunTurn = new BasicMovesInfo();
    public BasicMovesInfo Crouch = new BasicMovesInfo();
    public BasicMovesInfo Jumpsquat = new BasicMovesInfo();
    public BasicMovesInfo JumpStraight = new BasicMovesInfo();
    public BasicMovesInfo JumpBack = new BasicMovesInfo();
    public BasicMovesInfo JumpForward = new BasicMovesInfo();
    public BasicMovesInfo FallStraight = new BasicMovesInfo();
    public BasicMovesInfo FallBack = new BasicMovesInfo();
    public BasicMovesInfo FallForward = new BasicMovesInfo();
    public BasicMovesInfo Landing = new BasicMovesInfo();
    public BasicMovesInfo GuardPose = new BasicMovesInfo();

}

[System.Serializable]
public class BasicMovesInfo {
    #region General
    public AnimationClip Animation;
    public float AnimationSpeed = 1;
    public WrapMode WMode;
    public float BlendInDuration = -1;
    public float BlendOutDuration = -1;
    #endregion

    #region Idle
    public AnimationClip[] IdleClips = new AnimationClip[5];
    public float IdleChangeInterval = 10;
    public bool IdleClipsFoldout;
    #endregion

    #region Jump
    public bool AutoSync = true;
    #endregion

    //Editor Data, ignore
    public bool BSFoldout;
}