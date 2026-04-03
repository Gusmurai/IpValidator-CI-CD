namespace IpValidator.Core;
using System;


public class IpAddressValidator : IIpAddressValidator
{
    public bool IsValidIPv4(string ip)
    {
        if (ip == null) return false;
        string[] parts = ip.Split('.');
        if (parts.Length != 4) return false;
        foreach (var part in parts)
        {
            if (!int.TryParse(part, out int val) || val < 0 || val > 255) return false;
        }
        return true;
    }

    public byte[] GetOctets(string ip)
    {
        if (!IsValidIPv4(ip)) throw new ArgumentException("Invalid IP");
        string[] parts = ip.Split('.');
        byte[] result = new byte[4];
        for (int i = 0; i < 4; i++) result[i] = byte.Parse(parts[i]);
        return result;
    }
}
