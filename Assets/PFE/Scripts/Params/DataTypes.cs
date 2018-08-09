using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Game State
public enum GameState {
    MainMenu, CharacterSelection, InMatch, ResultsScreen
}

public enum GameModeEnum {
    Training, Versus
}

public enum MatchState {
    Countdown, MidMatch, End
}
#endregion

[System.Serializable]
public struct KEnergies {
    public Vector2 gravity;
    public Vector2 horizontal;
    public Vector2 damage;
    public Vector2 wind;
    public Vector2 movingPlatforms;
    public Vector2 opponentPushback;
    public Vector2 secondaryMovement;
}

[System.Serializable]
public struct PlayerInputs{
    public bool Grab, Jump, Attack;
    public FPVector2 LeftStick, RightStick;
}

public struct PlayerInputDefinition{
    public FPVector2 axis;
    public bool isDown;
    public bool firstPress;
    public bool released;
    public bool usedInBuffer;
}

public struct LocalPlayerInputs{
    public PlayerInputDefinition grab, jump, attack, special, shield;
    public PlayerInputDefinition tauntUp, tauntDown, tauntLeft, tauntRight;
    public PlayerInputDefinition leftStick, rightStick;

    public LocalPlayerInputs(FPVector2 lStick, FPVector2 rStick, bool Grab, bool Jump, bool Attack, bool Special, bool Shield, bool TauntUp, bool TauntDown,
            bool TauntLeft, bool TauntRight){
        leftStick = new PlayerInputDefinition();
        leftStick.axis = lStick;
        rightStick = new PlayerInputDefinition();
        rightStick.axis = rStick;

        grab = new PlayerInputDefinition();
        grab.isDown = Grab;
        jump = new PlayerInputDefinition();
        jump.isDown = Jump;
        attack = new PlayerInputDefinition();
        attack.isDown = Attack;
        special = new PlayerInputDefinition();
        special.isDown = Special;
        shield = new PlayerInputDefinition();
        shield.isDown = Shield;

        tauntUp = new PlayerInputDefinition();
        tauntUp.isDown = TauntUp;
        tauntDown = new PlayerInputDefinition();
        tauntDown.isDown = TauntDown;
        tauntLeft = new PlayerInputDefinition();
        tauntLeft.isDown = TauntLeft;
        tauntRight = new PlayerInputDefinition();
        tauntRight.isDown = TauntRight;
    }
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

/// <summary>
/// https://answers.unity.com/questions/799429/transformfindstring-no-longer-finds-grandchild.html
/// </summary>
public static class TransformDeepChildExtension {
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName) {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent) {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }
}