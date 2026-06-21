namespace SunamoWpf;

public sealed partial class ErrorListing : UserControl
{
    #region Rewrite to pure cs. With xaml is often problems without building
    public event VoidT<object> ClickOK;
    public string Title
    {
        set
        {
            tbTitle.Text = value;
        }
    }
    public Size MaxContentSize
    {
        get
        {
            //return maxContentSize;
            return FrameworkElementHelper.GetMaxContentSize(this);
        }
        set
        {
            //maxContentSize = value;
            FrameworkElementHelper.SetMaxContentSize(this, value);
        }
    }
    /// <summary>
    /// Musíš nastavit i Visible
    /// </summary>
    public string Collapse
    {
        set
        {
            ttCollapse.Content = value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                tbCollapse.Visibility = Visibility.Visible;
            }
            else
            {
                tbCollapse.Visibility = Visibility.Collapsed;
            }
        }
    }
    /// <summary>
    /// Musíš nastavit i Collapsee
    /// </summary>
    public string Visible
    {
        set
        {
            tbChybovaZprava.Text = value;
        }
    }
    public ErrorListing()
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
    }
    private void OnClickOK(object sender, RoutedEventArgs e)
    {
        ClickOK(null);
    }
    public Brush PopupBorderBrush
    {
        set { border.BorderBrush = value; }
    }
    public event VoidT<object> ClickCancel;

    #endregion
}