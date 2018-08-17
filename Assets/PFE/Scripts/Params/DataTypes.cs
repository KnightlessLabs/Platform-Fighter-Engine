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

    public enum NetworkType {
        Offline, Online
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

    public enum PlayerType {
        None, Player, CPU
    }

    [System.Serializable]
    public class PlayerDefinition {
        public PlayerType playerType;
        public CharacterInfo selectedChar;
    }

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

}