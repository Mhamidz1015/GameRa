using FluentAssertions;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.UnitTests.Abstractions;

namespace GameRa.Modules.Distribution.UnitTests.GameVersions;

public class VersionNumberTests : BaseTest
{
    [Theory]
    [InlineData("0.0.0", 0, 0, 0)]
    [InlineData("1.2.3", 1, 2, 3)]
    [InlineData("1.10.0", 1, 10, 0)]
    [InlineData("10.0.20", 10, 0, 20)]
    public void TryParse_ShouldSucceed_WhenValueIsValid(string value, int major, int minor, int patch)
    {
        bool parsed = VersionNumber.TryParse(value, out VersionNumber version);

        parsed.Should().BeTrue();
        version.Major.Should().Be(major);
        version.Minor.Should().Be(minor);
        version.Patch.Should().Be(patch);
        version.ToString().Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1.2")]
    [InlineData("1.2.3.4")]
    [InlineData("a.b.c")]
    [InlineData("-1.0.0")]
    [InlineData("01.2.3")]
    [InlineData("1.02.3")]
    [InlineData("1..3")]
    [InlineData(" 1.2.3")]
    [InlineData("99999999999.0.0")]
    public void TryParse_ShouldFail_WhenValueIsInvalid(string? value)
    {
        VersionNumber.TryParse(value, out _).Should().BeFalse();
    }

    [Fact]
    public void Compare_ShouldBeNumeric()
    {
        Version("1.10.0").Should().BeGreaterThan(Version("1.9.0"));
        (Version("1.10.0") > Version("1.9.0")).Should().BeTrue();
        (Version("2.0.0") > Version("1.99.99")).Should().BeTrue();
        (Version("1.0.1") < Version("1.0.2")).Should().BeTrue();
    }

    [Fact]
    public void Equality_ShouldCompareComponents()
    {
        Version("1.2.3").Should().Be(Version("1.2.3"));
        (Version("1.2.3") == Version("1.2.3")).Should().BeTrue();
        (Version("1.2.3") >= Version("1.2.3")).Should().BeTrue();
        Version("1.2.3").Should().NotBe(Version("1.2.4"));
    }
}
