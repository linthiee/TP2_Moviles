public interface IVibrationService : IService
{
    bool IsEnabled { get; set; }

    public void VibrateShort();
    public void VibrateLong();
}