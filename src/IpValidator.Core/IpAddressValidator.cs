namespace IpValidator.Core;
using System;


public class IpAddressValidator : IIpAddressValidator
{
    public bool IsValidIPv4(string ip)
    {
        if (string.IsNullOrEmpty(ip)) return false;
        string[] segments = ip.Split('.');
        if (segments.Length != 4) return false;
        foreach (string s in segments)
        {
            if (s.Length == 0 || s.Length > 3) return false;
            foreach (char c in s) if (!char.IsDigit(c)) return false;
            if (!int.TryParse(s, out int val) || val < 0 || val > 255) return false;
            if (s.Length > 1 && s[0] == '0') return false; // Ведущие нули
        }
        return true;
    }

    public byte[] GetOctets(string ip)
    {
        if (!IsValidIPv4(ip)) throw new ArgumentException("IP address is not valid");
        string[] segments = ip.Split('.');
        return new byte[] { byte.Parse(segments[0]), byte.Parse(segments[1]), byte.Parse(segments[2]), byte.Parse(segments[3]) };
    }
}
