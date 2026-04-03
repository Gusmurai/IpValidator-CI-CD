using NUnit.Framework;
using IpValidator.Core;

namespace Tests;

[TestFixture]
public class IpValidatorTests
{
    private IIpAddressValidator _validator = new IpAddressValidator();

    // ---------- ТЕСТЫ "ЧЕРНОГО ЯЩИКА" ----------

    [Test]
    [TestCase("127.0.0.1")]
    [TestCase("0.0.0.0")]
    [TestCase("255.255.255.255")]
    public void IsValidIPv4_ValidAddresses_ReturnsTrue(string ip)
    {
        Assert.That(_validator.IsValidIPv4(ip), Is.True);
    }

    [Test]
    [TestCase("192.168.1")]       // Мало октетов
    [TestCase("192.168.1.1.1")]   // Много октетов
    [TestCase("256.0.0.0")]       // Больше 255
    [TestCase("abc.def.ghi.jkl")] // Буквы
    [TestCase("")]                // Пустота
    public void IsValidIPv4_InvalidAddresses_ReturnsFalse(string ip)
    {
        Assert.That(_validator.IsValidIPv4(ip), Is.False);
    }

    [Test]
    public void GetOctets_ValidIp_ReturnsByteArray()
    {
        byte[] expected = { 192, 168, 0, 1 };
        var result = _validator.GetOctets("192.168.0.1");
        Assert.That(result, Is.EqualTo(expected));
    }

    // ---------- ТЕСТЫ "БЕЛОГО ЯЩИКА" ----------

    [Test]
    public void IsValidIPv4_LeadingZeros_ReturnsFalse()
    {
        // Проверка специфической обработки ведущих нулей (например, "01")
        // Некоторые парсеры могут трактовать это как восьмеричное число или ошибку
        Assert.That(_validator.IsValidIPv4("192.168.01.1"), Is.False, "Ведущие нули не должны допускаться для исключения неоднозначности");
    }

    [Test]
    public void GetOctets_NullInput_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _validator.GetOctets(null!));
    }
}
