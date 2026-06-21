namespace SunamoWpf.UserControls;

public partial class LogUC : UserControl, IUserControl, IWindowOpener, IUserControlShared, IKeysHandler, ISaveWithoutArgWpf, IUserControlClosing
{
    #region Rewrite to pure cs. With xaml is often problems without building
    public LogUC()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
#if DEBUG
            Debugger.Break();
#endif
        }

        uc_Loaded(null, null);
    }

    public string Title => Translate.FromKey(XlfKeys.Logs);
    bool initialized = false;
    public WindowWithUserControl windowWithUserControl { get => windowOpenerMain.windowWithUserControl; set => windowOpenerMain.windowWithUserControl = value; }

    IKeysHandler keyHandlerMain = null;
    IEssentialMainWindow mainControl = null;
    IWindowOpener windowOpenerMain = null;

    public IEssentialMainWindow MainControl
    {
        get { return mainControl; }
        set
        {
            mainControl = value;

            if (value is IKeysHandler)
            {
                keyHandlerMain = (IKeysHandler)value;
            }
            if (value is IWindowOpener)
            {
                windowOpenerMain = (IWindowOpener)value;
            }
        }
    }

    public bool HandleKey(KeyEventArgs e)
    {
        if (keyHandlerMain != null)
        {
            if (keyHandlerMain.HandleKey(e))
            {
                //return true;
            }
        }

        return false;
    }

    public void Init()
    {
        if (!initialized)
        {
            initialized = true;


        }
    }

    public void uc_Loaded(object sender, RoutedEventArgs e)
    {
        WpfApp.sl("LogUC loaded");
    }

    public void Save(string path)
    {
        lbLogsErrors.Save(path);
        lbLogsOthers.Save(path);
    }

    public void OpenInCode(Action<string> PHWinCode)
    {
        PHWinCode(lbLogsErrors.fileToSave);
        PHWinCode(lbLogsOthers.fileToSave);

    }

    public void OnClosing()
    {

    }
    #endregion
}
