namespace SunamoWpf.Interfaces;

/// <summary>
/// Uz to neimplementovat do kazde tridy ale pouzivat __ kde jsou tytez promenne staticke
/// Bude to setrit pamet
/// </summary>
public interface IMainWindowBasicControls
{
    ICheckBoxListUC checkBoxListUC
    {
        get; set;
    }


    SelectOneValue selectOneValue
    {
        get; set;
    }
    RadioButtonsList radioButtonsList
    {
        get; set;
    }
    EnterOneValueUC enterOneValueUC
    {
        get; set;
    }
    ShowTextResult showTextResult
    {
        get; set;
    }
    WindowWithUserControl windowWithUserControl
    {
        get; set;
    }
    // public

    ShowTextResultWindow showTextResultWindow
    {
        get; set;
    }

    ICompareInCheckBoxListUC compareInCheckBoxListUC
    {
        get; set;
    }

    SelectTwoValues selectTwoValues
    {
        get; set;
    }
}
