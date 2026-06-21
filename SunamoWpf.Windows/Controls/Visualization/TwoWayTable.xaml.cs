namespace SunamoWpf.Controls.Visualization;

public partial class TwoWayTable : UserControl
{
    #region Rewrite to pure cs. With xaml is often problems without building
    const double marginInCell = 8;
    #region region for all code to easy transfer to another code
    /// <summary>
    /// Bez zapocitani top
    /// </summary>
    int rows = 0;
    /// <summary>
    /// Bez zapocitani left
    /// </summary>
    int columns = 0;
    /// <summary>
    /// Prvni rozmer jsou radky, druhy sloupce
    /// </summary>
    UIElement[,] controls = null;
    /// <summary>
    /// Prvni rozmer jsou radky, druhy sloupce
    /// Is initialized only when dataCellWrapper == AddBeforeControl.CheckBox
    /// </summary>
    CheckBox[,] checkBoxes = null;
    public void DoSave(string path)
    {
        foreach (var item in leftChbs)
        {
            if (displayEntity != string.Empty)
            {
                //if (Save != null)
                //{
                Twt_Save(this, displayEntity, item, path);
                //}
            }
        }
    }
    /// <summary>
    /// In A1 is displayEntity
    /// </summary>
    /// <param name="obj"></param>
    private void Twt_Save(TwoWayTable sender, string site, string page, string path)
    {
        //var path = AppData.ci.GetFile(AppFolders.Controls, string.Join("_", sender.Name, site, page));
        #region Get isChecked from row
        var ele = sender.GetCheckBoxesInRow(page);
        var isChecked = ele.Select(d => d.IsChecked.Value);
        var ints = new List<int>(isChecked.Count());
        foreach (var item in isChecked)
        {
            ints.Add(NewMethod(item));
        }
        #endregion
        if (site == "Lyr")
        {
        }
        var content = string.Join(',', ints);
        //Set(sender, checkBoxes, content);
        //SaveControl(sender);
        // better is save simpli and no use adc
        TF.WriteAllText(path, content).RunSynchronously();
    }
    private static int NewMethod(bool item)
    {
        return BTS.BoolToInt(item);
    }
    /// <summary>
    /// Prvni rozmer jsou radky, druhy sloupce
    /// Every cell data has own data
    /// </summary>
    object[,] data = null;
    public int Rows
    {
        get
        {
            return rows;
        }
    }
    public int Columns
    {
        get
        {
            return columns;
        }
    }
    public T GetDataAt<T>(int row, int column)
    {
        return (T)data[row, column];
    }
    public bool? IsCheckedAt(int row, int column)
    {
        return checkBoxes[row, column].IsChecked;
    }
    ILogger logger;
    public TwoWayTable(ILogger logger)
    {
        this.logger = logger;
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
        Loaded += TwoWayTable_Loaded;
    }
    private void TwoWayTable_Loaded(object sender, RoutedEventArgs e)
    {
#if DEBUG
        //if (grid.Children.Count > 0)
        //{
        //    var d = grid.Children[7];
        //    var fw = d as FrameworkElement;
        //    var s = fw.ActualHeight;
        //}
#endif
    }
    public void CreateGrid(int row, int column)
    {
        row++;
        column++;
        checkBoxes = null;
        data = null;
        if (dataCellWrapper == AddBeforeControl.CheckBox)
        {
            checkBoxes = new CheckBox[row, column];
        }
        controls = new UIElement[row, column];
        data = new object[row, column];
        rows = row;
        columns = column;
        ClearGridChildren();
        GridHelper.GetAutoSize(grid, column, row);
    }
    public void ClearGridChildren()
    {
        grid.Children.Clear();
    }
    /// <summary>
    /// key - twt_Geo etc
    /// value = key - name of aspx, value - checked
    /// </summary>
    /// <param name="twt"></param>
    /// <returns></returns>
    public
#if ASYNC
async Task<Dictionary<string, Dictionary<string, List<bool>>>>
#else
      Dictionary<string, Dictionary<string, List<bool>>>
#endif
GetRowsIsChecked(TwoWayTable twt, string folder)
    {
        var begin = twt.Name + "_";
        //var folder = AppData.ci.GetFolder(AppFolders.Controls);
        var files = FSGetFiles.GetFilesEveryFolder(logger, folder, begin + "*", System.IO.SearchOption.TopDirectoryOnly);
        Dictionary<string, Dictionary<string, List<bool>>> s = new Dictionary<string, Dictionary<string, List<bool>>>();
        // In web will be window always null
        // See comments in IWindowWithSettingsManager
        var window = (IWindowWithSettingsManager)WpfApp.mp;
        var Data = window.Data;
        var data = Data.data;
        foreach (var item in files)
        {
            string fn = FS.GetFileName(item);
            fn = fn.Substring(begin.Length);
            string key = begin + fn;
            var webPage = SHSplit.Split(fn, "_");
            var web = webPage[0];
            var page = webPage[1];
            var dKey = begin + web;
            string text =
#if ASYNC
await
#endif
TF.ReadAllText(item);
            #region MyRegion
            //ApplicationDataContainerList adcl = null;
            //// Automatically load
            //if (!data.ContainsKey(key))
            //{
            //    var v = new ApplicationDataContainerList(item);
            //    adcl = Data.AddFrameworkElement(key, v);
            //}
            //else
            //{
            //    adcl = data[key];
            //}
            // , for delimiting values in row, " " for entire new row
            //var text = adcl.GetString(ApplicationDataConsts.checkBoxes);
            #endregion
#if DEBUG
            if (item.Contains("Lyr"))
            {
            }
#endif
            List<string> cells = SHSplit.Split(text, ",");
            List<int> numbers = CAToNumber.ToNumber<int>(BTS.ParseInt, cells, int.MinValue);
            List<bool> bools = CA.ToBool(numbers);
            Dictionary<string, List<bool>> dict = null;
            if (s.ContainsKey(dKey))
            {
                dict = s[dKey];
            }
            else
            {
                dict = new Dictionary<string, List<bool>>();
                s.Add(dKey, dict);
            }
            DictionaryHelper.AddOrSet<string, List<bool>>(dict, page, bools);
        }
        return s;
    }
    public void ReRender()
    {
        base.OnRenderSizeChanged(new SizeChangedInfo(this, this.RenderSize, true, true));
    }
    public List<CheckBox> GetCheckBoxesInRow(string dex)
    {
        return GetCheckBoxesInRow(leftChbs.IndexOf(dex));
    }
    public List<CheckBox> GetCheckBoxesInRow(int dex)
    {
        var ele = GridHelper.GetControlsFrom<CheckBox>(grid, true, dex).ToList();
        ele.RemoveAt(ele.Count - 1);
        return ele;
    }
    AddBeforeControl dataCellWrapper = AddBeforeControl.None;
    string displayEntity = string.Empty;
    //public event Action<TwoWayTable, string, string> Save;
    public void GetDisplayEntity()
    {
    }
    /// <summary>
    /// For saving data for every table
    /// </summary>
    public
#if ASYNC
async Task
#else
    void
#endif
setDisplayEntity(string value, string folder)
    {
        displayEntity = value;
        var checkedCells =
#if ASYNC
await
#endif
GetRowsIsChecked(this, folder);
        var key = this.Name + "_" + displayEntity;
        if (checkedCells.ContainsKey(key))
        {
            Dictionary<string, List<bool>> s = checkedCells[key];
            foreach (var item in s)
            {
                int dex = GetIndexOfRow(item.Key);
                if (dex != -1)
                {
                    var ele = GetCheckBoxesInRow(dex);
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        var el = ele[i];
                        var isChecked = item.Value[i];
                        if (displayEntity == "Lyr")
                        {
                            if (isChecked)
                            {
                            }
                            el.IsChecked = isChecked;
                        }
                        else
                        {
                            el.IsChecked = isChecked;
                        }
                    }
                }
            }
        }
    }
    private int GetIndexOfRow(string key)
    {
        var result = leftChbs.IndexOf(key);
        return result;
    }
    public AddBeforeControl DataCellWrapper
    {
        get
        {
            return dataCellWrapper;
        }
        set
        {
            dataCellWrapper = value;
        }
    }
    public double maxHeightRowBorder = 30;
    /// <summary>
    /// Can be use AddColumn or AddRow in dependent how I have structured data
    /// A1 is columns in table
    /// In A2 is control and tick (if DataCellWrapper == AddBeforeControl.CheckBox). Can be null
    /// A3 = data to control, cant be null if element in A2 is not null
    /// </summary>
    /// <param name="dexCol"></param>
    /// <param name="uie"></param>
    /// <param name="o"></param>
    public void AddColumn(int dexCol, List<CheckBoxData<UIElement>> uie, List<object> o)
    {
        // 1,4,5,6,7,8,13
        for (int i = 0; i < uie.Count; i++)
        {
            UIElement item = null;
            if (uie[i] != null)
            {
                item = uie[i].t;
            }
            if (item != null)
            {
                Border b = new Border();
                b.Height = maxHeightRowBorder;
                b.Padding = new Thickness(5);
                b.Child = item;
                b.BorderBrush = Brushes.Transparent;
                b.BorderThickness = new Thickness(1);
                item = b;
            }
            controls[i, dexCol] = item;
            if (item != null)
            {
                data[i, dexCol] = o[i];
                if (DataCellWrapper == AddBeforeControl.CheckBox)
                {
                    CheckBox chb = new CheckBox();
                    chb.VerticalAlignment = VerticalAlignment.Center;
                    chb.VerticalContentAlignment = VerticalAlignment.Center;
                    chb.Content = item;
                    chb.IsChecked = uie[i].tick;
                    Grid.SetColumn(chb, dexCol + 1);
                    Grid.SetRow(chb, i + 1);
                    grid.Children.Add(chb);
                    checkBoxes[i, dexCol] = chb;
                }
                else
                {
                    Grid.SetColumn(item, dexCol + 1);
                    Grid.SetRow(item, i + 1);
                    grid.Children.Add(item);
                }
            }
        }
    }
    /// <summary>
    /// Can be use AddColumn or AddRow in dependent how I have structured data
    /// </summary>
    /// <param name="dexRow"></param>
    /// <param name="uie"></param>
    /// <param name="o"></param>
    public void AddRow(int dexRow, CheckBoxData<UIElement>[] uie, object[] o)
    {
        if (uie.Length + 1 != columns)
        {
            return;
        }
        for (int i = 0; i < uie.Length; i++)
        {
            UIElement item = null;
            if (uie[i] != null)
            {
                item = uie[i].t;
            }
            controls[dexRow, i] = item;
            if (item != null)
            {
                data[dexRow, i] = o[i];
                if (DataCellWrapper == AddBeforeControl.CheckBox)
                {
                    CheckBox chb = new CheckBox();
                    chb.Content = item;
                    chb.IsChecked = uie[i].tick;
                    FrameworkElementHelper.SetMargin(chb, marginInCell);
                    Grid.SetColumn(chb, i + 1);
                    Grid.SetRow(chb, dexRow + 1);
                    grid.Children.Add(chb);
                    checkBoxes[dexRow, i] = chb;
                }
                else
                {
                    FrameworkElementHelper.SetMargin(item, marginInCell);
                    Grid.SetColumn(item, i + 1);
                    Grid.SetRow(item, dexRow + 1);
                    grid.Children.Add(item);
                }
            }
        }
    }
    public void AddTop(params CheckBoxData<UIElement>[] uie)
    {
        AddTop(uie.ToList());
    }
    public void AddTop(IList<CheckBoxData<UIElement>> uie)
    {
        int i = 0;
        foreach (var item2 in uie)
        {
            UIElement item = item2.t;
            if (dataCellWrapper == AddBeforeControl.CheckBox)
            {
                CheckBox top = new CheckBox();
                top.Tag = i;
                top.Click += Top_Click;
                top.Content = item;
                item = top;
            }
            Grid.SetColumn(item, i + 1);
            Grid.SetRow(item, 0);
            grid.Children.Add(item);
            i++;
        }
    }
    private void Top_Click(object sender, RoutedEventArgs e)
    {
        CheckBox chb = (CheckBox)sender;
        int tagOfCheckBox = (int)chb.Tag;
        bool isChecked = ((bool)chb.IsChecked);
        for (int i = 0; i < checkBoxes.GetLength(0); i++)
        {
            var dr = checkBoxes[i, tagOfCheckBox];
            if (dr != null)
            {
                dr.IsChecked = isChecked;
            }
        }
    }
    public void AddLeft(params CheckBoxData<UIElement>[] uie)
    {
        AddLeft(uie.ToList());
    }
    List<string> leftChbs = new List<string>();
    public void AddLeft(IList<CheckBoxData<UIElement>> uie)
    {
        leftChbs.Clear();
        int i = 0;
        foreach (var item2 in uie)
        {
            UIElement item = item2.t;
            var s = item.GetContent();
            if (s != null)
            {
                leftChbs.Add(s.ToString());
            }
            else
            {
                leftChbs.Add(string.Empty);
            }
            if (dataCellWrapper == AddBeforeControl.CheckBox)
            {
                CheckBox left = new CheckBox();
                left.Tag = i;
                left.Click += Left_Click;
                left.Content = item;
                item = left;
            }
            Grid.SetColumn(item, 0);
            Grid.SetRow(item, i + 1);
            grid.Children.Add(item);
            i++;
        }
    }
    private void Left_Click(object sender, RoutedEventArgs e)
    {
        CheckBox chb = (CheckBox)sender;
        int tagOfCheckBox = (int)chb.Tag;
        bool isChecked = ((bool)chb.IsChecked);
        for (int i = 0; i < checkBoxes.GetLength(1); i++)
        {
            var dr = checkBoxes[tagOfCheckBox, i];
            if (dr != null)
            {
                dr.IsChecked = isChecked;
            }
        }
    }
    #endregion
    #endregion
}
