namespace YMouseButtonControl.DataAccess.Models;

public class NothingMapping : IButtonMapping
{
    public int Index { get; } = 0;
    public bool Enabled { get; } = false;
    public string Description { get; } = "** No Change (Don't Intercept) **";

    public void Run()
    {
        throw new System.NotImplementedException();
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return Description;
    }
}