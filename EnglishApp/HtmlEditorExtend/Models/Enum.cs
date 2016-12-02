namespace HtmlEditorExtend.Models
{
    public enum HtmlDocumentState
    {
        Uninitialized,
        Loading,
        Loaded,
        Interactive,
        Complete
    }

    public enum EditMode
    {
        Visual,
        Source
    }
}
