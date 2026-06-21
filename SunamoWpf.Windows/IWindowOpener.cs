/// <summary>
/// For open in one time one window which is defined in MainWindow
/// </summary>
public interface IWindowOpener
{
    /// <summary>
    /// MainWindow = { get; set; } 
    /// Other = { get => MainWindow.Instance.windowWithUserControl; set => MainWindow.Instance.windowWithUserControl = value; }
    /// </summary>
    WindowWithUserControl windowWithUserControl { get; set; }
    
}
