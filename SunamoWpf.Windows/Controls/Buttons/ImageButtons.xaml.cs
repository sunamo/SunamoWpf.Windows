namespace SunamoWpf.Controls.Buttons;

public partial class ImageButtons : UserControl
{
    static Type type = typeof(ImageButtons);
    List<Button> allButtons = null;
    public event VoidString Added;
    EnterOneValueWindow eov = null;
    public event Action CopyToClipboard;
    public event Action ClearAll;
    public event Action SelectAll;
    public event Action UnselectAll;

    async Task SetAwesomeIcons()
    {
        // In serie how is written in xaml
        await AwesomeFontControls.SetAwesomeFontSymbol(btnCopyToClipboard, "");
        await AwesomeFontControls.SetAwesomeFontSymbol(btnClear, "");
        await AwesomeFontControls.SetAwesomeFontSymbol(btnAdd, "");
        await AwesomeFontControls.SetAwesomeFontSymbol(btnSelectAll, "");
        await AwesomeFontControls.SetAwesomeFontSymbol(btnUnselectAll, "");
    }

    /// <summary>
    /// Private constructor - use CreateAsync instead
    /// A1 can be Action or bool(Visibility).
    /// </summary>
    /// <param name="copyToClipboard"></param>
    /// <param name="clear"></param>
    private ImageButtons()
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
        Loaded += ImageButtons_Loaded;
    }

    /// <summary>
    /// Factory method for creating ImageButtons with async initialization
    /// </summary>
    /// <returns>Fully initialized ImageButtons instance</returns>
    public static async Task<ImageButtons> CreateAsync()
    {
        var imageButtons = new ImageButtons();
        await imageButtons.SetAwesomeIcons();
        return imageButtons;
    }

    private void BtnClear_Click(object sender, RoutedEventArgs e)
    {
        ClearAll();
    }
    private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
    {
        CopyToClipboard();
    }
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        eov = new EnterOneValueWindow("item to insert (one on line)");
        // TODO Replaced during repair 0xc0000374
        //eov.enterOneValueUC.ChangeDialogResult += EnterOneValueUC_ChangeDialogResult;
        eov.ValidatorBeforeAdding = ValidatorBeforeAdding;
        eov.ValidatorBeforeAddingMessage = ValidatorBeforeAddingMessage;
        eov.IsMultiline = true;
        eov.ShowDialog();
    }
    private void EnterOneValueUC_ChangeDialogResult(bool? b)
    {
        if (b.HasValue && b.Value)
        {
            data = eov.enterOneValueUC.txtEnteredText.Text;
            Handler(btnAdd, null);
        }
    }
    private void ImageButtons_Loaded(object sender, RoutedEventArgs e)
    {
    }
    public double HeightOfFirstVisibleButton()
    {
        foreach (var item in allButtons)
        {
            if (item.ActualHeight != 0)
            {
                return item.ActualHeight;
            }
        }
        return 0;
    }
    /// <summary>
    /// 1. handlers RoutedEventHandler,VoidString directly to A1. or object - then will be use default action
    /// 2. bool and set handlers, elements has public FieldModifier
    /// </summary>
    /// <param name="copyToClipboard"></param>
    /// <param name="clear"></param>
    public void Init(ImageButtonsInit i)
    {
        SetToolTip(btnCopyToClipboard, XlfKeys.CopyTextToClipboard);
        SetToolTip(btnClear, XlfKeys.Clear);
        SetToolTip(btnAdd, XlfKeys.Add);
        SetToolTip(btnSelectAll, XlfKeys.CheckAll);
        SetToolTip(btnUnselectAll, XlfKeys.UncheckAll);
        SetVisibility(btnCopyToClipboard, i.copyToClipboard);
        SetVisibility(btnClear, i.clear);
        SetVisibility(btnAdd, i.add);
        SetVisibility(btnSelectAll, i.selectAll);
        SetVisibility(btnUnselectAll, i.deselectAll);
        allButtons = CAG.ToList<Button>(btnCopyToClipboard, btnClear, btnAdd, btnSelectAll, btnUnselectAll);
        this.Visibility = this.IsAllCollapsed() ? Visibility.Collapsed : Visibility.Visible;
        ResourceDictionaryStyles.Margin10(allButtons);
        foreach (var item in allButtons)
        {
            item.HorizontalAlignment = HorizontalAlignment.Center;
            item.VerticalAlignment = VerticalAlignment.Center;
        }
    }
    private void SetToolTip(Button btnCopyToClipboard, string copyTextToClipboard)
    {
        FrameworkElementHelper.SetToolTip(btnCopyToClipboard, copyTextToClipboard);
    }
    public bool IsAllCollapsed()
    {
        foreach (var item in allButtons)
        {
            if (item.Visibility != Visibility.Collapsed)
            {
                return false;
            }
        }
        return true;
    }
    object data;
    public Func<string, bool> ValidatorBeforeAdding = null;
    public string ValidatorBeforeAddingMessage = null;
    private void SetVisibility(Button btn, object copyToClipboard)
    {
        string methodName = "SetVisibility";
        if (copyToClipboard == null)
        {
            btn.Visibility = Visibility.Collapsed;
        }
        else
        {
            btn.Visibility = Visibility.Visible;
            var t = copyToClipboard.GetType();
            if (t == typeof(bool))
            {
                UIElementHelper.SetVisibility((bool)copyToClipboard, btn);
            }
            else if (t == TypesDesktop.tRoutedEventHandler)
            {
                var d = (RoutedEventHandler)copyToClipboard;
                btn.Click += d;
            }
            else if (t == TypesD.tVoidString)
            {
                var voidString = (VoidString)copyToClipboard;
                btn.Tag = voidString;
                // If is not RoutedEventHandler, button have own handler which will call Handler()
                //btn.Click += Handler;
            }
            else
            {
                ThrowEx.NotImplementedCase(t);
            }
        }
    }
    void Handler(object o, RoutedEventArgs e)
    {
        string methodName = Translate.FromKey(XlfKeys.Handler);
        Button btn = (Button)o;
        var t = btn.Tag.GetType();
        if (t == TypesD.tVoidString)
        {
            var voidString = (VoidString)btn.Tag;
            voidString.Invoke(data.ToString());
        }
        else
        {
            ThrowEx.NotImplementedCase(t);
        }
    }
    private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
    {
        SelectAll();
    }
    private void BtnUnselectAll_Click(object sender, RoutedEventArgs e)
    {
        UnselectAll();
    }
}
