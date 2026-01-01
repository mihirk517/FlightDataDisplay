using GenericsBasics.Domain;
using Microsoft.Extensions.Options;
using Xunit.Sdk;
namespace GenericsBasics.Tests;

public class BaggageInfoTest
{
    [Fact]
    public void Test1()
    {
        int expectedflight = 841;
        string expectedfrom = "Pandora";
        int expectedCarousel = 0;
        var actual = new BaggageInfo()
        {
            flight = 841,
            from = expectedfrom,
            carousel = expectedCarousel
        };
        Assert.Equal(expectedflight,actual.flight);
        Assert.Equal(expectedfrom,actual.from);

    }
}