// GENERATED AUTOMATICALLY FROM 'Assets/PFE/Example/GameControls.inputactions'

[System.Serializable]
public class GameControls : UnityEngine.Experimental.Input.InputActionWrapper
{
    private bool m_Initialized;
    private void Initialize()
    {
        // Gameplay
        m_Gameplay = asset.GetActionMap("Gameplay");
        m_Gameplay_Attack = m_Gameplay.GetAction("Attack");
        m_Gameplay_LeftStick = m_Gameplay.GetAction("LeftStick");
        m_Initialized = true;
    }
    // Gameplay
    private UnityEngine.Experimental.Input.InputActionMap m_Gameplay;
    private UnityEngine.Experimental.Input.InputAction m_Gameplay_Attack;
    private UnityEngine.Experimental.Input.InputAction m_Gameplay_LeftStick;
    public struct GameplayActions
    {
        private GameControls m_Wrapper;
        public GameplayActions(GameControls wrapper) { m_Wrapper = wrapper; }
        public UnityEngine.Experimental.Input.InputAction @Attack { get { return m_Wrapper.m_Gameplay_Attack; } }
        public UnityEngine.Experimental.Input.InputAction @LeftStick { get { return m_Wrapper.m_Gameplay_LeftStick; } }
        public UnityEngine.Experimental.Input.InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public UnityEngine.Experimental.Input.InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator UnityEngine.Experimental.Input.InputActionMap(GameplayActions set) { return set.Get(); }
    }
    public GameplayActions @Gameplay
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new GameplayActions(this);
        }
    }
}
