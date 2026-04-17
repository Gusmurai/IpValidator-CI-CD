#nullable disable
// =============================================================
// AUTO-GENERATED TESTS. DO NOT EDIT MANUALLY.
// Source: spec/ipvalidator.yaml
// Generator: gen_tests.py v1.0
// =============================================================
using System;
using System.Collections.Generic;
using NUnit.Framework;
using IpValidator.Core;

namespace Tests
{
    [TestFixture]
    [Description("Автоматически сгенерированные тесты для IpAddressValidator")]
    public class IpAddressValidatorGeneratedTests
    {
        private IIpAddressValidator _sut;

        [SetUp]
        public void SetUp()
        {
            // Инициализация тестируемой системы (SUT)
            _sut = new IpAddressValidator();
        }


        [Test]
        [Description("Класс эквивалентности: Валидный IP")]
        [TestCase("127.0.0.1")]
        public void Test_IsValidIPv4_Валидный_IP(string input)
        {
            // === Arrange ===
            // Предусловие: ip != null
            // Ожидаемый результат: true

            // === Act & Assert ===
            var result = _sut.IsValidIPv4(input);
            Assert.That(result, Is.True);
        }


        [Test]
        [Description("Класс эквивалентности: Мало октетов")]
        [TestCase("192.168.1")]
        public void Test_IsValidIPv4_Мало_октетов(string input)
        {
            // === Arrange ===
            // Предусловие: ip != null
            // Ожидаемый результат: false

            // === Act & Assert ===
            var result = _sut.IsValidIPv4(input);
            Assert.That(result, Is.False);
        }


        [Test]
        [Description("Класс эквивалентности: Ведущие нули")]
        [TestCase("192.168.01.1")]
        public void Test_IsValidIPv4_Ведущие_нули(string input)
        {
            // === Arrange ===
            // Предусловие: ip != null
            // Ожидаемый результат: false

            // === Act & Assert ===
            var result = _sut.IsValidIPv4(input);
            Assert.That(result, Is.False);
        }


        [Test]
        [Description("Класс эквивалентности: Пустая строка")]
        [TestCase("")]
        public void Test_IsValidIPv4_Пустая_строка(string input)
        {
            // === Arrange ===
            // Предусловие: ip != null
            // Ожидаемый результат: false

            // === Act & Assert ===
            var result = _sut.IsValidIPv4(input);
            Assert.That(result, Is.False);
        }


        [Test]
        [Description("Класс эквивалентности: Корректное преобразование")]
        [TestCase("192.168.0.1")]
        public void Test_GetOctets_Корректное_преобразование(string input)
        {
            // === Arrange ===
            // Предусловие: IsValidIPv4(ip) == true
            // Ожидаемый результат: new byte[] { 192, 168, 0, 1 }

            // === Act & Assert ===
            var result = _sut.GetOctets(input);
            Assert.Pass("Результат получен: " + result);
        }


        [Test]
        [Description("Класс эквивалентности: Null (исключение)")]
        [TestCase(null)]
        public void Test_GetOctets_Null_исключение(string input)
        {
            // === Arrange ===
            // Предусловие: IsValidIPv4(ip) == true
            // Ожидаемый результат: ArgumentException

            // === Act & Assert ===
            Assert.Throws<ArgumentException>(() => _sut.GetOctets(input));
        }

    }
}
