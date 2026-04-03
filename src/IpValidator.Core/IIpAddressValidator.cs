namespace IpValidator.Core;

public interface IIpAddressValidator
{
    bool IsValidIPv4(string ip);
    byte[] GetOctets(string ip);
}