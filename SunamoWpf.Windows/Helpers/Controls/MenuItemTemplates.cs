namespace SunamoWpf.Helpers.Controls;

public class SuMenuItemTemplates
{
    public static SuMenuItem AvailableShortcut(Dictionary<string, string> dictionary2)
    {
        SuMenuItem miShowControls = new SuMenuItem();
        miShowControls.Click += delegate
        {
            WindowWithUserControl.AvailableShortcut(dictionary2);
        };
        miShowControls.Header = Translate.FromKey(XlfKeys.AvailableShortcuts) + "...";
        return miShowControls;
    }
}