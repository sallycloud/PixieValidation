using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace PixieValidation.PropCheckers;

public static class IpAddressCheckers
{
    public static PropChecker<string> IsValidIp() =>
        value => !IPAddress.TryParse(value, out _) ? "Must be a valid IP address." : null;

    public static PropChecker<string> IsIPv4() =>
        value => !(IPAddress.TryParse(value, out var address) &&
                   address.AddressFamily == AddressFamily.InterNetwork)
            ? "Must be a valid IPv4 address."
            : null;

    public static PropChecker<string> IsIPv6() =>
        value => !(IPAddress.TryParse(value, out var address) &&
                   address.AddressFamily == AddressFamily.InterNetworkV6)
            ? "Must be a valid IPv6 address."
            : null;
    
    /// <summary>Checks for an address range in CIDR notation, e.g. <c>10.0.0.0/8</c> or <c>2001:db8::/32</c>.
    /// The prefix length must be between 0 and 32 (IPv4) or 0 and 128 (IPv6).
    /// Host bits after the prefix are allowed, so <c>192.168.1.10/24</c> is valid as well.
    /// </summary>
    public static PropChecker<string> IsValidCidr() =>
        value => TryParseCidr(value, out _) ? null : "Must be a valid CIDR address range.";

    public static PropChecker<string> IsIPv4Cidr() =>
        value => TryParseCidr(value, out var family) && family == AddressFamily.InterNetwork
            ? null
            : "Must be a valid IPv4 CIDR address range.";

    public static PropChecker<string> IsIPv6Cidr() =>
        value => TryParseCidr(value, out var family) && family == AddressFamily.InterNetworkV6
            ? null
            : "Must be a valid IPv6 CIDR address range.";

    private static bool TryParseCidr(string value, out AddressFamily family)
    {
        family = AddressFamily.Unknown;

        var slash = value.IndexOf('/');
        if (slash < 0 || slash != value.LastIndexOf('/'))
            return false;

        if (!IPAddress.TryParse(value.AsSpan(0, slash), out var address))
            return false;

        // NumberStyles.None rejects signs, whitespace and empty prefixes ("10.0.0.0/", "10.0.0.0/+8").
        if (!int.TryParse(value.AsSpan(slash + 1), NumberStyles.None, CultureInfo.InvariantCulture, out var prefixLength))
            return false;

        var maxPrefixLength = address.AddressFamily == AddressFamily.InterNetwork ? 32 : 128;
        if (prefixLength > maxPrefixLength)
            return false;

        family = address.AddressFamily;
        return true;
    }
}
