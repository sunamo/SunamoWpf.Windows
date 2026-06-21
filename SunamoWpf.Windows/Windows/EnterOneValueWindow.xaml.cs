namespace SunamoWpf;

/// <summary>
/// Select Value - more from selector
/// EnterOneValueUC - single,  EnterOneValueUC - fwElemements 
/// </summary>
public partial class EnterOneValueWindow : Window
{
    //public EnterOneValueUC enterOneValueUC = null;

    #region MyRegion
    public Func<string, bool> ValidatorBeforeAdding
    {
        set
        {
            enterOneValueUC.ValidatorBeforeAdding = value;
        }
    }

    public string ValidatorBeforeAddingMessage
    {
        set
        {
            enterOneValueUC.ValidatorBeforeAddingMessage = value;
        }
    }



    /// <summary>
    /// access to everything via enterOneValueUC
    /// </summary>
    /// <param name="whatEnter"></param>
    public EnterOneValueWindow(string whatEnter)
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
        enterOneValueUC.Init(whatEnter);
        // TODO Replaced during repair 0xc0000374
        //enterOneValueUC.ChangeDialogResult += EnterOneValueUC_ChangeDialogResult;
    }

    public bool IsMultiline
    {
        set
        {
            if (value)
            {
                enterOneValueUC.IsMultiline = value;
            }
        }
    }
    private void EnterOneValueUC_ChangeDialogResult(bool? b)
    {
        // Close() + DialogResult = b - Dialog result can be only set when is show as the dialog
        // Only DialogResult = b - works rightly with attach ChangeDialogResult or ShowDialog()
        DialogResult = b;
    }
    #endregion
}
