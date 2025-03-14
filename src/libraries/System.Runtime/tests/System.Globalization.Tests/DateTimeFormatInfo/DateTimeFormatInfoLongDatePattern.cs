// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit;

namespace System.Globalization.Tests
{
    public class DateTimeFormatInfoLongDatePattern
    {
        [Fact]
        public void LongDatePattern_InvariantInfo_ReturnsExpected()
        {
            Assert.Equal("dddd, dd MMMM yyyy", DateTimeFormatInfo.InvariantInfo.LongDatePattern);
        }

        public static IEnumerable<object[]> LongDatePattern_Set_TestData()
        {
            yield return new object[] { string.Empty };
            yield return new object[] { "garbage" };
            yield return new object[] { "dddd, dd MMMM yyyy HH:mm:ss" };
            yield return new object[] { "dddd" };
            yield return new object[] { "D" };
            yield return new object[] { "HH:mm:ss dddd, dd MMMM yyyy" };
            yield return new object[] { "dddd, dd MMMM yyyy" };
        }

        public static IEnumerable<object[]> LongDatePattern_Get_TestData_ICU()
        {
            yield return new object[] { CultureInfo.GetCultureInfo("en-US").DateTimeFormat, "dddd, MMMM d, yyyy" };
            yield return new object[] { CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat,  "dddd d MMMM yyyy" };
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsIcuGlobalization))]
        [MemberData(nameof(LongDatePattern_Get_TestData_ICU))]
        public void LongDatePattern_Get_ReturnsExpected_ICU(DateTimeFormatInfo format, string expected)
        {
            Assert.Equal(expected, format.LongDatePattern);
        }

        [Theory]
        [MemberData(nameof(LongDatePattern_Set_TestData))]
        public void LongDatePattern_Set_GetReturnsExpected(string value)
        {
            var format = new DateTimeFormatInfo();
            format.LongDatePattern = value;
            Assert.Equal(value, format.LongDatePattern);
        }

        [Fact]
        public void LongDatePattern_Set_InvalidatesDerivedPattern()
        {
            const string Pattern = "#$";
            var format = new DateTimeFormatInfo();
            var d = DateTimeOffset.Now;
            d.ToString("F", format); // FullDateTimePattern
            format.LongDatePattern = Pattern;
            Assert.Contains(Pattern, d.ToString("F", format));
        }

        [Fact]
        public void LongDatePattern_SetNullValue_ThrowsArgumentNullException()
        {
            var format = new DateTimeFormatInfo();
            AssertExtensions.Throws<ArgumentNullException>("value", () => format.LongDatePattern = null);
        }

        [Fact]
        public void LongDatePattern_SetReadOnly_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => DateTimeFormatInfo.InvariantInfo.LongDatePattern = "dddd, dd MMMM yyyy");
        }
    }
}
