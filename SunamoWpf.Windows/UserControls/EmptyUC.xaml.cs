namespace SunamoWpf.UserControls;

public partial class EmptyUC : UserControl, IUserControl, IKeysHandler
{
    public EmptyUC()
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

        Loaded += uc_Loaded;
    }

    public string Title => "Empty";

    public WindowWithUserControl windowWithUserControl
    {
        get =>
            ((IWindowOpener)Application.Current.MainWindow).windowWithUserControl;
        set =>
             ((IWindowOpener)Application.Current.MainWindow).windowWithUserControl = value;
    }

    public bool HandleKey(KeyEventArgs e)
    {
        return false;
    }

    public void Init()
    {

    }

    public void uc_Loaded(object sender, RoutedEventArgs e)
    {

    }
}
