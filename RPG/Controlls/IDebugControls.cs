namespace RPG.Controlls
{
    public interface IDebugControls : IControls
    {
        bool EnableDebugMode();
        bool EnableDebugEconomyMode();
        bool EnableFreeCameraMode();
        bool EnableLogging();
        bool CameraFollowNext();
        bool DontRenderTilesOnX0OrY0();
    }
}
