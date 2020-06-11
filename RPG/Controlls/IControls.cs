namespace RPG.Controlls
{
    public interface IControls
    {
        bool Up(bool clickable = false);
        bool Down(bool clickable = false);
        bool Left(bool clickable = false);
        bool Right(bool clickable = false);
        bool R1(bool clickable = false);
        bool R2(bool clickable = false);
        bool R3();
        bool L1(bool clickable = false);
        bool L2(bool clickable = false);
        bool L3();
        bool Start();
        bool Option();
        bool Menu();
        bool Confirm();
        bool Cancel();
        bool AltConfirm();
    }
}
