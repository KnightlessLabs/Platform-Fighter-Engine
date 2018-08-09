using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Core;

namespace PFE.Character {
    public class CharController : MonoBehaviour {

        [HideInInspector]
        public Transform cacheTransform;
        [HideInInspector]
        public GameManager cacheGM;
        [HideInInspector]
        public CharInput cInput;
        [Header("References")]
        public GameObject visualRoot;
        public Rigidbody2D rBody;
        public PolygonCollider2D ecb;
        public ContactFilter2D ecbFilter;
        public ContactFilter2D isGroundedFilter;
        public CharacterInfo cInfo;
        public int selectedCostume;
        [HideInInspector]
        public GameObject topBone;
        [HideInInspector]
        public GameObject bottomBone;

        [Header("Variables")]
        public string ecbTopBone;
        public string ecbBottomBone;
        public KEnergies kineticEnergies;
        public int hitStop;
        public int hitStun;
        public bool applyGravity = true;
        public bool facingRight = true;
        public bool isGrounded = true;
        #region States
        public Dictionary<string, ICharacterState> stateDictionary = new Dictionary<string, ICharacterState>();
        [SerializeField]
        public ICharacterState currentState;
        [Header("State")]
        public string cState;
        public int stateCurrentTime = 0;
        #endregion

        private void Awake() {
            cacheTransform = GetComponent<Transform>();
            cInput = GetComponent<CharInput>();
            cacheGM = GameManager.instance;
            GameObject gg = Instantiate(cInfo.costumes[selectedCostume], visualRoot.transform, false);
            if (!string.IsNullOrEmpty(ecbBottomBone)) {
                bottomBone = gg.transform.FindDeepChild(ecbBottomBone).gameObject;
            }
            if (!string.IsNullOrEmpty(ecbTopBone)) {
                topBone = gg.transform.FindDeepChild(ecbTopBone).gameObject;
            }

            //States
            CharIdleState cis = new CharIdleState();
            cis.cInput = cInput;
            cis.cController = this;
            stateDictionary.Add(cis.StateName, cis);

            currentState = stateDictionary[cis.StateName];
            currentState.OnStart();
        }

        public virtual void CCUpdate() {
            if(hitStop > 0) {
                hitStop--;
                return;
            }

            isGrounded = IsGroundedCheck();
            if (isGrounded) {
                kineticEnergies.gravity.y = 0;
            }
            if (applyGravity) {
                HandleGravity();
            }

            if (currentState != null) {
                currentState.OnUpdate();
            }

            UpdateECB();
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
        public void HandleGravity() {

        }

        public void HandleCharacerPushback() {
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
                return;
            }

            //Pushback
            Vector2 pushDir = Mathf.Sign(results[closestResultIndex].bounds.center.x - ecb.bounds.center.x) == 1 ? Vector2.right : Vector2.left;
        }

        public bool IsGroundedCheck() {
            Collider2D[] results = new Collider2D[1];
            Physics2D.OverlapCollider(ecb, isGroundedFilter, results);

            if(results[0] != null) {
                return true;
            } else {
                return false;
            }
        }
        #endregion

        #region Checks
        public bool CheckForAttack() {
            if (cInput.Attack().firstPress) {
                return true;
            }
            return false;
        }

        public void ApplyForces() {
            KEnergies ke = kineticEnergies;
            Vector2 overall = ke.gravity+ke.damage+ke.horizontal+ke.movingPlatforms+ke.opponentPushback+ke.secondaryMovement+ke.wind;
            rBody.velocity = overall;
        }
        #endregion
    }
}