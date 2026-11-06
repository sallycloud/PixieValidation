using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class IpAddressCheckersTests
{
    // IsValid

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("10.0.0.5")]
    [InlineData("::1")]
    [InlineData("2001:db8::1")]
    public void IsValid_WithValidAddress_ReturnsNoError(string value) =>
        Assert.Null(IpAddressCheckers.IsValid()(value));

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("999.1.1.1")]
    [InlineData("1.2.3.4.5")]
    public void IsValid_WithInvalidAddress_ReturnsError(string value) =>
        Assert.Equal("Must be a valid IP address.", IpAddressCheckers.IsValid()(value));

    // IsIPv4

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("192.168.1.10")]
    public void IsIPv4_WithIPv4Address_ReturnsNoError(string value) =>
        Assert.Null(IpAddressCheckers.IsIPv4()(value));

    [Theory]
    [InlineData("::1")]
    [InlineData("2001:db8::1")]
    [InlineData("abc")]
    [InlineData("")]
    public void IsIPv4_WithOtherValue_ReturnsError(string value) =>
        Assert.Equal("Must be a valid IPv4 address.", IpAddressCheckers.IsIPv4()(value));

    // IsIPv6

    [Theory]
    [InlineData("::1")]
    [InlineData("2001:db8::1")]
    public void IsIPv6_WithIPv6Address_ReturnsNoError(string value) =>
        Assert.Null(IpAddressCheckers.IsIPv6()(value));

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("abc")]
    [InlineData("")]
    public void IsIPv6_WithOtherValue_ReturnsError(string value) =>
        Assert.Equal("Must be a valid IPv6 address.", IpAddressCheckers.IsIPv6()(value));
}
