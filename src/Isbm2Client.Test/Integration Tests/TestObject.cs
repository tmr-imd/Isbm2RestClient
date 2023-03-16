namespace Isbm2Client.Test.Integration_Tests;

public class TestObject
{
    // TODO: Needs to be set back to int[] after issue on server has been resolved
    public double[] Numbers { get; set; } = null!;
    public string Text { get; set; } = "";
    public Dictionary<string, double> Weather { get; set; } = null!;
}
