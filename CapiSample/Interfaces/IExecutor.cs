namespace CapiSample.Interfaces
{
    internal interface IExecutor
    {
        IControl Control
        {
            get;
        }

        string ControlNumber
        {
            set;
        }

        void ShowControlsList();
    }
}
