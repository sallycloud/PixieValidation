using System.Net;

namespace PixieValidation.PropCheckers;

public static class IpAddressCheckers
{
    public static PropChecker<string> IsValid() =>
        value => !IPAddress.TryParse(value, out _) ? "Must be a valid IP address." : null;

    public static PropChecker<string> IsIPv4() =>
        value => !(IPAddress.TryParse(value, out var address) &&
                   address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            ? "Must be a valid IPv4 address."
            : null;

    public static PropChecker<string> IsIPv6() =>
        value => !(IPAddress.TryParse(value, out var address) &&
                   address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            ? "Must be a valid IPv6 address."
            : null;
}