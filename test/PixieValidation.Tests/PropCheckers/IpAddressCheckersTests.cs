using PixieValidation.PropCheckers;

namespace PixieValidation.Tests.PropCheckers;

public class IpAddressCheckersTests
{
    // IsValidIp

    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("10.0.0.5")]
    [InlineData("::1")]
    [InlineData("2001:db8::1")]
    public void IsValid_WithValidAddress_ReturnsNoError(string value) =>
        Assert.Null(IpAddressCheckers.IsValidIp()(value));

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("999.1.1.1")]
    [InlineData("1.2.3.4.5")]
    public void IsValid_WithInvalidAddress_ReturnsError(string value) =>
        Assert.Equal("Must be a valid IP address.", IpAddressCheckers.IsValidIp()(value));

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
    
    // IsValidCidr

    [Theory]
    [InlineData("10.0.0.0/8")]
    [InlineData("192.168.1.0/24")]
    [InlineData("192.168.1.10/24")]
    [InlineData("0.0.0.0/0")]
    [InlineData("127.0.0.1/32")]
    [InlineData("2001:db8::/32")]
    [InlineData("::/0")]
    [InlineData("::1/128")]
    public void IsValidCidr_WithValidRange_ReturnsNoError(string value) =>
        Assert.Null(IpAddressCheckers.IsValidCidr()(value));

    [Theory]
    [InlineData("")]
    [InlineData("10.0.0.0")]
    [InlineData("10.0.0.0/")]
    [InlineData("/8")]
    [InlineData("10.0.0.0/33")]
    [InlineData("2001:db8::/129")]
    [InlineData("10.0.0.0/-1")]
    [InlineData("10.0.0.0/+8")]
    [InlineData("10.0.0.0/ 8")]
    [InlineData("10.0.0.0/8/8")]
    [InlineData("10.0.0.0/abc")]
    [InlineData("999.0.0.0/8")]
    [InlineData("abc/8")]
    public void IsValidCidr_WithInvalidRange_ReturnsError(string value) =>
        Assert.Equal("Must be a valid CIDR address range.", IpAddressCheckers.IsValidCidr()(value)); 
    
    // IsIPv4Cidr

    [Theory]
    [InlineData("10.0.0.0/8")]
    [InlineData("0.0.0.0/0")]
    [InlineData("127.0.0.1/32")]
    public void IsIPv4Cidr_WithIPv4Range_ReturnsNoError(string value) =>
        Assert.Null(IpAddressCheckers.IsIPv4Cidr()(value));

    [Theory]
    [InlineData("2001:db8::/32")]
    [InlineData("10.0.0.0/33")]
    [InlineData("10.0.0.0")]
    [InlineData("")]
    public void IsIPv4Cidr_WithOtherValue_ReturnsError(string value) =>
        Assert.Equal("Must be a valid IPv4 CIDR address range.", IpAddressCheckers.IsIPv4Cidr()(value)); 
    
    // IsIPv6Cidr

    [Theory]
    [InlineData("2001:db8::/32")]
    [InlineData("::/0")]
    [InlineData("::1/128")]
    [InlineData("2001:db8::/64")]
    public void IsIPv6Cidr_WithIPv6Range_ReturnsNoError(string value) =>
        Assert.Null(IpAddressCheckers.IsIPv6Cidr()(value));

    [Theory]
    [InlineData("10.0.0.0/8")]
    [InlineData("2001:db8::/129")]
    [InlineData("2001:db8::")]
    [InlineData("")]
    public void IsIPv6Cidr_WithOtherValue_ReturnsError(string value) =>
        Assert.Equal("Must be a valid IPv6 CIDR address range.", IpAddressCheckers.IsIPv6Cidr()(value));
}
