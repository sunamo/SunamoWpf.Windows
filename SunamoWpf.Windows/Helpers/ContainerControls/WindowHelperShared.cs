namespace SunamoWpf.Helpers.ContainerControls;

public partial class WindowHelper
{
    public static void SizeOfWindowToTitle(Window w, RowDefinition growingRow, FrameworkElement c)
    {
        SizeOfWindowToTitle(w, growingRow.ActualHeight, c);
    }
    public static void SizeOfWindowToTitle(Window w, double growingRow, FrameworkElement c)
    {
        w.Title = $"Window: {w.ActualWidth}x{w.ActualHeight} growingRow: {growingRow} Content: {c.ActualWidth}x{c.ActualHeight}";
    }
    static IWindowOpener windowOpener = null;
    public static void ShowExceptionWindow2(object e)
    {
        ShowExceptionWindow(e, Environment.NewLine);
    }
    private static void Result_ChangeDialogResult(bool? b)
    {
        windowOpener.windowWithUserControl.Close();
    }
#if MB
    static void ShowMb(string s)
    {
        WpfApp.ShowMb(s);
    }
#endif
    static string lastError = null;
    static Action<string> sl => PD.WriteToStartupLogRelease;
    /// <summary>
    /// Return dump A1
    /// </summary>
    /// <param name="e"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public static string ShowExceptionWindow(object e, string methodName = "", bool isTerminanting = false)
    {
        if (methodName != string.Empty)
        {
            methodName += " ";
        }
        string dump = null;
        //dump = YamlHelper.DumpAsYaml(e);
        //dump = SunamoJsonHelper.SerializeObject(e, true);
        //dump = JsonParser.Serialize<>
        dump = RH.DumpAsString(new DumpAsStringArgs { o = e, d = DumpProvider.Reflection });
        if (dump == lastError)
        {
            return dump;
        }
        lastError = dump;
        StringBuilder sb = new StringBuilder();
        if (isTerminanting)
        {
            sb.AppendLine("Is terminating: YES");
            sb.AppendLine();
        }
        sb.AppendLine(methodName);
        sb.Append(dump);
        var result = new ShowTextResult(sb.ToString());
        result.ChangeDialogResult += Result_ChangeDialogResult;
        if (isTerminanting)
        {
            //result.txtResult.Background = Brushes.OrangeRed;
        }
        var mw = Application.Current.MainWindow;
        windowOpener = mw as IWindowOpener;
        if (windowOpener == null)
        {
            string windowOpenerIsNull = "windowOpener == null";
#if MB
            ShowMb(windowOpenerIsNull);
#endif
            sl(windowOpenerIsNull);
            var d = Translate.FromKey(XlfKeys.MainWindowMustBeIWindowOpenerDueToShowExceptions) + $"Is Application.Current.MainWindow null: {mw == null}";
            if (mw != null)
            {
                d += $"Type: {mw.GetType()}";
            }
            MessageBox.Show(d);
        }
        else
        {
            var _a = "windowOpener.windowWithUserControl.ShowDialog";
#if MB
            ShowMb(_a);
#endif
            sl(_a);
            windowOpener.windowWithUserControl = new WindowWithUserControl(result, ResizeMode.CanResizeWithGrip, false);
            windowOpener.windowWithUserControl.ShowDialog();
        }
        return dump;
    }
    public static void Close(Window w)
    {
        try
        {
            w.Close();
        }
        catch (Exception ex)
        {
        }
    }
}