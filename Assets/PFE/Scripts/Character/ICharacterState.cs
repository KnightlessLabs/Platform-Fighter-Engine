namespace PFE.Character {
    public interface ICharacterState {
        void OnStart();
        void OnUpdate();
        void OnLateUpdate();
        bool CheckInterrupt();
        void OnInterrupted();
        void Setup(CharInput cInput, CharController cCon);
        string StateName { get; }
        int StateDuration { get; }
        CharInput inputHandler { get; set; }
        CharController controller { get; set; }
    }
}
