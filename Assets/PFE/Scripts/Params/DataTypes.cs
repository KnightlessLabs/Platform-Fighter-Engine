using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixedPointy;

namespace PFE {
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

    #region Variables
    [System.Serializable]
    public struct CharacterVariables {
        public int hitStun;
        public int hitStop;
        public bool applyGravity;
        public bool facingRight;
        public bool isGrounded;
    }

    [System.Serializable]
    public struct KEnergies {
        public FixVec2 gravity;
        public FixVec2 horizontal;
        public FixVec2 damage;
        public FixVec2 wind;
        public FixVec2 movingPlatforms;
        public FixVec2 opponentPushback;
        public FixVec2 secondaryMovement;
    }
    #endregion

    #region Inputs
    [System.Serializable]
    public struct PlayerInputs {
        public bool Grab, Jump, Attack;
        public FixVec2 LeftStick, RightStick;
    }

    public struct PlayerInputDefinition {
        public FixVec2 axis;
        public bool isDown;
        public bool firstPress;
        public bool released;
        public bool usedInBuffer;
    }
    #endregion

    public enum HurtboxType {
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

    public class CombatStance {

    }
}