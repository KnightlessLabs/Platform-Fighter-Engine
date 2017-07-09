using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class PFEWindow : EditorWindow {

    public CharacterInfo CS;
    public GameObject SceneRef;

    public bool UIElementsFoldout = false;
    public bool HurtboxSetupFoldout = false;
    public bool HurtboxesFoldout = false;
    public bool AttributesFoldout = false;
    public bool CombatStancesFoldout = false;

    [MenuItem("Window/PFE Character Editor")]
    public static void ShowEditor () {
        EditorWindow.GetWindow(typeof(PFEWindow));
    }

    public void OnGUI () {
        GUI.changed = false;

        CS = (CharacterInfo)EditorGUILayout.ObjectField("Currently Editing:", CS, typeof(CharacterInfo), false);

        if (CS) {

            if(SceneRef == null && CS.Costumes.Count > 0) {
                if(CS.Costumes[0] != null) {
                    SceneRef = (GameObject)Instantiate(CS.Costumes[0], Vector3.zero, Quaternion.identity);
                    SceneRef.transform.eulerAngles = new Vector3(0, 90, 0);
                }
            }

            ShowUIElements();
            ShowHitboxSetup();
            ShowAttributes();
            ShowCombatStances();
        }

        //Make sure all the changes are saved.
        if (GUI.changed == true) {
            EditorUtility.SetDirty(CS);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            GUI.changed = false;
        }
    }

    public void ShowUIElements () {
        UIElementsFoldout = EditorGUILayout.Foldout(UIElementsFoldout, "UI Elements");

        if (UIElementsFoldout) {
            EditorGUI.indentLevel++;

            //UI
            CS.BigPortrait = (Sprite)EditorGUILayout.ObjectField("Big Portrait:", CS.BigPortrait, typeof(Sprite), false);
            CS.SmallPortrait = (Sprite)EditorGUILayout.ObjectField("Small Portrait:", CS.SmallPortrait, typeof(Sprite), false);
            CS.CSSPortrait = (Sprite)EditorGUILayout.ObjectField("CSS Portrait:", CS.CSSPortrait, typeof(Sprite), false);
            CS.StockIcon = (Sprite)EditorGUILayout.ObjectField("Stock Icon:", CS.StockIcon, typeof(Sprite), false);

            //Costumes
            if (GUILayout.Button("Add Costume")) {
                CS.Costumes.Add(null);
            }
            for(int i = 0; i < CS.Costumes.Count; i++) {
                EditorGUILayout.BeginHorizontal();
                CS.Costumes[i] = (GameObject)EditorGUILayout.ObjectField(CS.Costumes[i], typeof(GameObject), false);
                if (GUILayout.Button("-", GUILayout.MaxWidth(25))) {
                    CS.Costumes.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }


            EditorGUI.indentLevel--;
        }
    }

    public void ShowHitboxSetup () {
        if (CS.Costumes.Count > 0) {
            HurtboxSetupFoldout = EditorGUILayout.Foldout(HurtboxSetupFoldout, "Hurtbox Setup");
            if (HurtboxSetupFoldout) {
                EditorGUI.indentLevel++;

                CS.HurtboxSetupInfo.TypeofHurtboxes = (HurtboxType)EditorGUILayout.EnumPopup("Type of Hurtboxes:", CS.HurtboxSetupInfo.TypeofHurtboxes);

                HurtboxesFoldout = EditorGUILayout.Foldout(HurtboxesFoldout, "Hurtboxes");

                if (HurtboxesFoldout) {
                    EditorGUI.indentLevel++;


                    //This setup of hitboxes only happens if we're working in 3D space. In 2D space, we want to handle htiboxes/hurtboxes/etc. per animation.
                    if (CS.HurtboxSetupInfo.TypeofHurtboxes == HurtboxType.ThreeD) {
                        if (GUILayout.Button("Add Hurtbox")) {
                            CS.HurtboxSetupInfo.Hurtboxes.Add(new HurtboxInfo());
                        }

                    EditorGUILayout.Space();

                        for (int i = 0; i < CS.HurtboxSetupInfo.Hurtboxes.Count; i++) {
                            HurtboxInfo HI = CS.HurtboxSetupInfo.Hurtboxes[i];
                            HI.Shape = (HurtboxShape)EditorGUILayout.EnumPopup("Shape:", HI.Shape);
                            HI.Size = EditorGUILayout.Vector3Field("Size:", HI.Size);
                            HI.Offset = EditorGUILayout.Vector3Field("Offset:", HI.Offset);
                            if (CS.HurtboxSetupInfo.TypeofHurtboxes == HurtboxType.ThreeD) {
                                HI.BoneLinkedToName = EditorGUILayout.TextField("Bone Linked To: ", HI.BoneLinkedToName);
                                HI.BoneLinkedTo = (Transform)EditorGUILayout.ObjectField(HI.BoneLinkedTo, typeof(Transform), true);
                            }
                            HI.CollisionT = (CollisionType)EditorGUILayout.EnumPopup("Collision Type:", HI.CollisionT);
                            if (GUILayout.Button("Remove")) {
                                CS.HurtboxSetupInfo.Hurtboxes.RemoveAt(i);
                            }
                            EditorGUILayout.Space();
                        }
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }
        }
    }

    public void ShowAttributes () {
        AttributesFoldout = EditorGUILayout.Foldout(AttributesFoldout, "Attributes");

        if (AttributesFoldout) {
            EditorGUI.indentLevel++;

            AttributesHolder AH = CS.AttributesInfo;

            EditorGUILayout.LabelField("Grounded", EditorStyles.boldLabel);
            AH.WalkSpeed = EditorGUILayout.FloatField("Walk Speed:", AH.WalkSpeed);
            AH.RunMaxVelo = EditorGUILayout.FloatField("Max Run Velocity:", AH.RunMaxVelo);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Aerial", EditorStyles.boldLabel);
            AH.MaxJumps = EditorGUILayout.IntField(new GUIContent("Max Jumps:", "How many times the character can jump."), AH.MaxJumps);
            AH.AirSpeed = EditorGUILayout.FloatField(new GUIContent("Air Speed:", "Indicated the max air speed."), AH.AirSpeed);
            AH.AirAcceleration = EditorGUILayout.FloatField(new GUIContent("Air Acceleration:", "How fast the character reaches max air speed"), AH.AirAcceleration);
            AH.AirDeceleration = EditorGUILayout.FloatField(new GUIContent("Air Deceleration:", "How fast the character can reverse momentum."), AH.AirDeceleration);
            AH.AirFriction = EditorGUILayout.FloatField(new GUIContent("Air Friction:", "Speed character loses air momentum when stick is in netrual."), AH.AirFriction);
            AH.MaxFallSpeed = EditorGUILayout.FloatField(new GUIContent("Fall Speed:", "Indicated max fall speed."), AH.MaxFallSpeed);
            AH.FastFallSpeed = EditorGUILayout.FloatField(new GUIContent("Fast Fall Speed:", "Indicates fast fall speed."), AH.FastFallSpeed);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Physics", EditorStyles.boldLabel);
            AH.Weight = EditorGUILayout.IntField(new GUIContent("Weight:", "How much the character can resist knockback."), AH.Weight);
            AH.Gravity = EditorGUILayout.FloatField(new GUIContent("Gravity:", "Makes the character reach their top fall speed faster."), AH.Gravity);
            AH.Traction = EditorGUILayout.FloatField(new GUIContent("Traction:", "How long it takes for the character to stop moving."), AH.Traction);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
            AH.WallJump = EditorGUILayout.Toggle(new GUIContent("Wall Jump:", "If the character has a wall jump."), AH.WallJump);
            AH.WallCling = EditorGUILayout.Toggle(new GUIContent("Wall Cling:", "If the character has a wall cling."), AH.WallCling);
            AH.SoftLandingLag = EditorGUILayout.IntField(new GUIContent("Soft Landing Lag:", "Soft landing lag (in frames)."), AH.SoftLandingLag);
            AH.HardLandingLag = EditorGUILayout.IntField(new GUIContent("Hard Landing Lag:", "Hard landing lag (in frames)."), AH.HardLandingLag);
            AH.ShieldSize = EditorGUILayout.FloatField(new GUIContent("Shield Size:", "Size of the character's shield."), AH.ShieldSize);


            EditorGUI.indentLevel--;
        }
    }

    public void ShowCombatStances () {
        CombatStancesFoldout = EditorGUILayout.Foldout(CombatStancesFoldout, "Combat Stances");

        if (CombatStancesFoldout) {
            EditorGUI.indentLevel++;

            CS.ExecutionTiming = EditorGUILayout.FloatField(new GUIContent("Execution Timing:", "How fast a button sequence has to be completed"), CS.ExecutionTiming);
            CS.BlendingDuration = EditorGUILayout.FloatField(new GUIContent("Blending Duration:", "How smooth the transition between basic moves are."), CS.BlendingDuration);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stances:", EditorStyles.boldLabel);
            if(GUILayout.Button("Add Stance")) {
                CS.CombatStances.Add(new CombatStanceHolder());
            }
            for (int i = 0; i < CS.CombatStances.Count; i++) {
                EditorGUI.indentLevel++;

                CombatStanceHolder CSH = CS.CombatStances[i];

                EditorGUILayout.LabelField("Stance" + (i+1).ToString(),EditorStyles.boldLabel);

                CSH.BasicMovesFoldout = EditorGUILayout.Foldout(CSH.BasicMovesFoldout, "Basic Moves");

                if (CSH.BasicMovesFoldout) {
                    EditorGUI.indentLevel++;

                    #region Basic Moves
                    EditorGUILayout.LabelField("Grounded", EditorStyles.boldLabel);
                    //Idle
                    CSH.BasicMoves.Idle.BSFoldout = EditorGUILayout.Foldout(CSH.BasicMoves.Idle.BSFoldout, "Idle(*)");
                    if (CSH.BasicMoves.Idle.BSFoldout) {
                        EditorGUI.indentLevel++;

                        BasicMovesInfo BMI = CSH.BasicMoves.Idle;
                        BMI.Animation = (AnimationClip)EditorGUILayout.ObjectField("Animation:", BMI.Animation, typeof(AnimationClip), false);
                        BMI.AnimationSpeed = EditorGUILayout.FloatField("Animation Speed:", BMI.AnimationSpeed);
                        BMI.WMode = (WrapMode)EditorGUILayout.EnumPopup("Wrap Mode:", BMI.WMode);
                        BMI.BlendInDuration = EditorGUILayout.FloatField("Blend In Override:", BMI.BlendInDuration);
                        BMI.BlendOutDuration = EditorGUILayout.FloatField("Blend Out Duration:", BMI.BlendOutDuration);

                        BMI.IdleChangeInterval = EditorGUILayout.FloatField("Idle Change Interval:", BMI.IdleChangeInterval);
                        BMI.IdleClipsFoldout = EditorGUILayout.Foldout(BMI.IdleClipsFoldout, 
                            new GUIContent("Idle Clips:", "Optional clips that can play in place of the regular idle."));

                        if (BMI.IdleClipsFoldout) {
                            EditorGUI.indentLevel++;

                            for(int s = 0; s < BMI.IdleClips.Length; s++) {
                                BMI.IdleClips[s] = (AnimationClip)EditorGUILayout.ObjectField(BMI.IdleClips[s], typeof(AnimationClip), false);
                            }

                            EditorGUI.indentLevel--;
                        }

                        EditorGUI.indentLevel--;
                    }
                    
                    //Walk
                    CSH.BasicMoves.Walk.BSFoldout = EditorGUILayout.Foldout(CSH.BasicMoves.Walk.BSFoldout, "Walk(*)");
                    if (CSH.BasicMoves.Walk.BSFoldout) {
                        EditorGUI.indentLevel++;

                        BasicMovesInfo BMI = CSH.BasicMoves.Walk;
                        BMI.Animation = (AnimationClip)EditorGUILayout.ObjectField("Animation:", BMI.Animation, typeof(AnimationClip), false);
                        BMI.AnimationSpeed = EditorGUILayout.FloatField("Animation Speed:", BMI.AnimationSpeed);
                        BMI.WMode = (WrapMode)EditorGUILayout.EnumPopup("Wrap Mode:", BMI.WMode);
                        BMI.BlendInDuration = EditorGUILayout.FloatField("Blend In Override:", BMI.BlendInDuration);
                        BMI.BlendOutDuration = EditorGUILayout.FloatField("Blend Out Duration:", BMI.BlendOutDuration);

                        EditorGUI.indentLevel--;
                    }

                    //Run
                    CSH.BasicMoves.Run.BSFoldout = EditorGUILayout.Foldout(CSH.BasicMoves.Walk.BSFoldout, "Run(*)");
                    if (CSH.BasicMoves.Run.BSFoldout) {
                        EditorGUI.indentLevel++;

                        BasicMovesInfo BMI = CSH.BasicMoves.Run;
                        BMI.Animation = (AnimationClip)EditorGUILayout.ObjectField("Animation:", BMI.Animation, typeof(AnimationClip), false);
                        BMI.AnimationSpeed = EditorGUILayout.FloatField("Animation Speed:", BMI.AnimationSpeed);
                        BMI.WMode = (WrapMode)EditorGUILayout.EnumPopup("Wrap Mode:", BMI.WMode);
                        BMI.BlendInDuration = EditorGUILayout.FloatField("Blend In Override:", BMI.BlendInDuration);
                        BMI.BlendOutDuration = EditorGUILayout.FloatField("Blend Out Duration:", BMI.BlendOutDuration);

                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.LabelField("Jumping", EditorStyles.boldLabel);
                    #endregion

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
    }
}
