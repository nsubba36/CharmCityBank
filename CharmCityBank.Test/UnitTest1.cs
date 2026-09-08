namespace CharmCityBank.Test;

public class UnitTest1
{
    [Fact]
    public void Add_TwoNumbers_Test()
    {
        int a = 5, b = 10;
        var result = a + b;
        Assert.Equal(15, result);
    }
    
    [Theory]
    [InlineData(5,10,15)]
    public void Verify_Status_Test(int a, int b, int expected)
    {
        int result = a + b;
        Assert.Equal(expected, result);
    }
}