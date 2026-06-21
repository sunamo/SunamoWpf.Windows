namespace SunamoWpf.Controls.Input;

public class SelectMoreFoldersWithTemplateSets
{
    EnterOneValueWindow enterNameOfSet = null;
    ComboBoxHelper<string> cbSetsFoldersHelper = null;
    public ComboBox cbSetsFolders = new ComboBox();
    TextBlock tbSetsFolders = new TextBlock();
    public string tbSetsFoldersKey = XlfKeys.SavedFoldersSets;
    public TextBlock tbFolders = new TextBlock();
    public string tbFoldersKey = XlfKeys.Folders;
    public SelectMoreFolders txtFolders = new SelectMoreFolders();
    string pathFromCtor;
    /// <summary>
    /// A1,A3 can be zero, then wont add to grid and create content
    /// </summary>
    /// <param name="gridCbSets"></param>
    /// <param name="rowCbSets"></param>
    /// <param name="gridSmf"></param>
    /// <param name="rowSmf"></param>
    public SelectMoreFoldersWithTemplateSets(Grid gridCbSets, int rowCbSets, Grid gridSmf, int rowSmf, string path)
    {
        this.pathFromCtor = path;
        txtFolders.Name = "txtFoldersSelectMoreFoldersWithTemplateSets";
        #region CbSets
        if (gridCbSets != null)
        {
            Grid.SetRow(cbSetsFolders, rowCbSets);
            Grid.SetRow(tbSetsFolders, rowCbSets);
            Grid.SetColumn(cbSetsFolders, 1);
            tbSetsFolders.Text = Translate.FromKey(tbSetsFoldersKey);
            gridCbSets.Children.Add(tbSetsFolders);
            gridCbSets.Children.Add(cbSetsFolders);
        }
        #endregion
        #region Smf
        if (gridSmf != null)
        {
            Grid.SetRow(txtFolders, rowSmf);
            Grid.SetRow(tbFolders, rowSmf);
            Grid.SetColumn(txtFolders, 1);
            tbFolders.Text = Translate.FromKey(tbFoldersKey);
            gridSmf.Children.Add(tbFolders);
            gridSmf.Children.Add(txtFolders);
        }
        #endregion
        cbSetsFoldersHelper = new ComboBoxHelper<string>(cbSetsFolders);
        var lines = SF.GetAllElementsFile(path);
        foreach (var item in lines)
        {
            cbSetsFolders.Items.Add(item[0]);
        }
        cbSetsFoldersHelper.SelectionChanged += CbSetsFolders_SelectionChanged;
        txtFolders.SaveSetAsTemplate += this_SaveSetAsTemplate;
    }
    private void EnterOneValueUC_ChangeDialogResult(bool? b)
    {
        if (BTS.GetValueOfNullable(b))
        {
            string text = enterNameOfSet.enterOneValueUC.txtEnteredText.Text;
            if (cbSetsFolders.Items.Contains(text))
            {
                WpfLogger.Warning("Set " + Translate.FromKey(XlfKeys.withName) + " " + text + " " + Translate.FromKey(XlfKeys.alreadyExists));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    List<string> pass = new List<string>(txtFolders.SelectedFolders());
                    pass.Add(text);
                    var line = SF.PrepareToSerializationExplicit2(pass);
                    SF.AppendAllText(pathFromCtor, line).RunSynchronously();
                    cbSetsFolders.Items.Add(text);
                }
            }
        }
    }
    private void CbSetsFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selected = cbSetsFoldersHelper.SelectedS;
        List<List<string>> lines = SF.GetAllElementsFile(pathFromCtor);
        foreach (var item in lines)
        {
            if (item[0] == selected)
            {
                txtFolders.RemoveAllFolders();
                for (int i = 1; i < item.Count; i++)
                {
                    txtFolders.AddFolder(item[i]);
                }
            }
        }
    }
    private void this_SaveSetAsTemplate()
    {
        enterNameOfSet = new EnterOneValueWindow(Translate.FromKey(XlfKeys.nameOfSet));
        // TODO Replaced during repair 0xc0000374
        //enterNameOfSet.enterOneValueUC.ChangeDialogResult += EnterOneValueUC_ChangeDialogResult;
        //enterNameOfSet.enterOneValueUC.Accept("Websites");
        enterNameOfSet.ShowDialog();
        //SF.PrepareToSerialization()
    }
}
