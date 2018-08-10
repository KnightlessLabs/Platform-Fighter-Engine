using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Core;
using PFE.Helpers;
using FixedPointy;

namespace PFE.Character {
    public class CharController : MonoBehaviour {

        [HideInInspector]
        public Transform cacheTransform;
        [HideInInspector]
        public GameManager cacheGM;
        [HideInInspector]
        public CharInput cInput;
        [HideInInspector]
        public Rigidbody2D rBody;
        [HideInInspector]
        public GameObject topBone;
        [HideInInspector]
        public GameObject bottomBone;
        [Header("References")]
        public GameObject visualRoot;
        public PolygonCollider2D ecb;
        public ContactFilter2D ecbFilter;
        public ContactFilter2D isGroundedFilter;
        public CharacterInfo cInfo;
        public int selectedCostume;

        [Header("Variables")]
        public string ecbTopBone;
        public string ecbBottomBone;
        public KEnergies kineticEnergies;
        public CharacterVariables vars;
        #region States
        public Dictionary<string, ICharacterState> stateDictionary = new Dictionary<string, ICharacterState>();
        [SerializeField]
        public ICharacterState currentState;
        [Header("State")]
        public string cState;
        public int stateCurrentTime = 0;
        #endregion

        public virtual void Awake() {
            #region References
            cacheTransform = GetComponent<Transform>();
            cInput = GetComponent<CharInput>();
            rBody = GetComponent<Rigidbody2D>();
            cacheGM = GameManager.instance;
            #endregion
            
            #region ECB Setup
            GameObject gg = Instantiate(cInfo.costumes[selectedCostume], visualRoot.transform, false);
            if (!string.IsNullOrEmpty(ecbBottomBone)) {
                bottomBone = gg.transform.FindDeepChild(ecbBottomBone).gameObject;
            }
            if (!string.IsNullOrEmpty(ecbTopBone)) {
                topBone = gg.transform.FindDeepChild(ecbTopBone).gameObject;
            }
            #endregion

            #region States
            //States
            CharIdleState cis = new CharIdleState();
            cis.Setup(cInput, this);
            stateDictionary.Add(cis.StateName, cis);

            CharWalkState cws = new CharWalkState();
            cws.Setup(cInput, this);
            stateDictionary.Add(cws.StateName, cws);

            currentState = stateDictionary[cis.StateName];
            currentState.OnStart();
            cState = cis.StateName;
            #endregion

            vars.facingRight = true;
        }

        public virtual void CCUpdate() {
            if(vars.hitStop > 0) {
                vars.hitStop--;
                return;
            }

            vars.isGrounded = IsGroundedCheck();
            if (vars.isGrounded) {
                kineticEnergies.gravity = FixVec2.Zero;
            }
            if (vars.applyGravity) {
                HandleGravity();
            }

            if (currentState != null) {
                currentState.OnUpdate();
            }

            UpdateECB();
            HandleCharacerPushback();
        }

        public virtual void CCLateUpdate() {
            if (currentState != null) {
                currentState.OnLateUpdate();
            }
        }

        public virtual void ChangeState(string StateName) {
            if (stateDictionary.ContainsKey(StateName)) {
                cState = StateName;
                stateCurrentTime = 1;
                currentState.OnInterrupted();
                currentState = stateDictionary[StateName];
                currentState.OnStart();
            } else {
                Debug.Log("State " + StateName + " doesn't exist.");
            }
        }

        public virtual void UpdateECB() {
            Vector2[] temp = ecb.points;
            if (bottomBone != null) {
                temp[2].y = transform.InverseTransformPoint(bottomBone.transform.position).y-0.95f;
            }
            if(topBone != null) {
                temp[0].y = transform.InverseTransformPoint(topBone.transform.position).y-0.95f;
            }
            ecb.points = temp;
        }

        #region Forces
        public virtual void ApplyTraction() {
            if(kineticEnergies.horizontal.X == Fix.Zero) {
                return;
            }

            bool startedDir = FixMath.Sign(kineticEnergies.horizontal.X) == 1 ? true : false;
            if (startedDir) {
                kineticEnergies.horizontal._x -= cInfo.attributes.traction;
                if (kineticEnergies.horizontal.X < Fix.Zero) {
                    kineticEnergies.horizontal = FixVec2.Zero;
                }
            } else {
                kineticEnergies.horizontal._x += cInfo.attributes.traction;
                if(kineticEnergies.horizontal.X > Fix.Zero) {
                    kineticEnergies.horizontal = FixVec2.Zero;
                }
            }
        }

        public virtual void HandleGravity() {
            if (!vars.isGrounded) {
                kineticEnergies.gravity += new FixVec2(0, cInfo.attributes.gravity);
                if ((kineticEnergies.gravity.Y) < (cInfo.attributes.maxFallSpeed) ){
                    kineticEnergies.gravity =  new FixVec2(0, cInfo.attributes.maxFallSpeed);
                }
            }
        }

        public virtual void HandleForces() {
            KEnergies ke = kineticEnergies;
            Vector2 overall = (Vector2)(ke.gravity + ke.damage + ke.horizontal + ke.movingPlatforms + ke.opponentPushback + ke.secondaryMovement + ke.wind);
            rBody.velocity = overall;
        }

        public virtual void HandleCharacerPushback() {
            Collider2D[] results = new Collider2D[3];
            Physics2D.OverlapCollider(ecb, ecbFilter, results);

            //Get closest
            int closestResultIndex = -1;
            float minDist = 1000;
            for(int i = 0; i < results.Length; i++) {
                if(results[i] == null) {
                    continue;
                }
                float tm = Vector2.Distance(results[i].bounds.center, ecb.bounds.center);
                if (tm < minDist) {
                    minDist = tm;
                    closestResultIndex = i;
                }
            }
            if(closestResultIndex == -1) {
                kineticEnergies.opponentPushback = FixVec2.Zero;
                return;
            }

            //Pushback
            Vector2 pushDir = Mathf.Sign(results[closestResultIndex].bounds.center.x - ecb.bounds.center.x) == 1 ? Vector2.left : Vector2.right;
            kineticEnergies.opponentPushback = (FixVec2)pushDir*cacheGM.gameInfo.ecbPushback;
        }
        #endregion

        #region Checks
        public virtual bool CheckForAttack() {
            if (cInput.Attack().firstPress) {
                return true;
            }
            return false;
        }

        public virtual bool IsGroundedCheck() {
            Collider2D[] results = new Collider2D[1];
            Vector3 tm = ecb.points[2];
            Physics2D.OverlapPoint(transform.TransformPoint(tm + new Vector3(0, 0.899f, 0)), isGroundedFilter, results);

            if (results[0] != null) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
    }
}