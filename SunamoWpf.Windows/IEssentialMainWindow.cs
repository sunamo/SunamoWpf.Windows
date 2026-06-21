namespace SunamoWpf.Interfaces;

/// <summary>
/// IEssentialMainPage is in apps
/// Must be derived from IWindowOpener - without implement in MainWindow cant be shown exceptions window
/// </summary>
public interface IEssentialMainWindow : IEssentialMainWindowBase, IWindowOpener
{
    Control actual { get; set; }
}

public interface IEssentialMainWindowSelling : IEssentialMainWindowBase, IWindowOpener
{
    FrameworkElement actualR { get; set; }
}
