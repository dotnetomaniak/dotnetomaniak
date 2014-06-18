using System;
using Kigg.DomainObjects;
using Xunit;
using Xunit.Extensions;

namespace Kigg.Core.Test
{
    using DomainObjects;

    public class StringExtensionFixture
    {
        [Theory]
        [InlineData("http://dotnetshoutout.com", true)]
        [InlineData("htp://dotnetshoutout.com", false)]
        [InlineData("http://www.dotnetshoutout.com", true)]
        [InlineData("www.dotnetshoutout.com", false)]
        [InlineData("", false)]
        public void IsWebUrl_Should_Return_Correct_Result(string target, bool result)
        {
            Assert.Equal(result, target.IsWebUrl());
        }

        [Theory]
        [InlineData("admin@dotnetshoutout.com", true)]
        [InlineData("admin@dotnetshoutout.com.bd", true)]
        [InlineData("admin@dotnetshoutoutcom", false)]
        [InlineData("admin", false)]
        [InlineData("", false)]
        public void IsEmail_Should_Return_Correct_Result(string target, bool result)
        {
            Assert.Equal(result, target.IsEmail());
        }

        [Fact]
        public void NullSafe_Should_Return_Empty_String_When_Null_String_Is_Passed()
        {
            const string nullString = null;

            Assert.Equal(string.Empty, nullString.NullSafe());
        }

        [Fact]
        public void FormatWith_Should_Replace_Place_Holder_Tokens_With_Provided_Value()
        {
            Assert.Equal("A-B-C-D", "{0}-{1}-{2}-{3}".FormatWith("A", "B", "C", "D"));
        }

        [Theory]
        [InlineData('x', 2048, "XkLq9Yfgp4/OzryaEaAl8Q==")]
        public void Hash_Should_Return_Hashed_Value(char input, int length, string result)
        {
            var plain = new string(input, length);
            var hash = plain.Hash();

            Assert.Equal(result, hash);
        }

        [Theory]
        [InlineData("abcd")]
        [InlineData("a dummy string")]
        [InlineData("another dummy string")]
        [InlineData("x")]
        public void Hash_Should_Always_Return_Twenty_Four_Character_String(string target)
        {
            var hash = target.Hash();

            Assert.Equal(24, hash.Length);
        }

        [Fact]
        public void WrapAt_Should_Returns_String_Which_Ends_With_Three_Dots()
        {
            Assert.Equal("a du...", "a dummy string".WrapAt(7));
        }

        [Fact]
        public void StripHtml_Should_Return_Only_Text()
        {
            const string Html = "<div style=\"border:1px #000\">This is a div</div>";

            Assert.Equal("This is a div", Html.StripHtml());
        }

        [Theory]
        [InlineData("JKgxzYZ2dEeRQz_D7XlRDw", "cd31a824-7686-4774-9143-3fc3ed79510f")]
        public void ToGuid_Should_Return_Correct_Guid_When_Previously_Shrinked_String_Is_Passed(string input, string result)
        {
            Assert.Equal(new Guid(result), input.ToGuid());
        }

        [Theory]
        [InlineData("0P#x!=?dl;)x~cxza1@&Z$")]
        [InlineData("jdgndbaxhkgsaghdngtdhas")]
        public void ToGuid_Should_Return_Empty_Guid_When_Incorrect_String_Is_Passed(string invalidGuid)
        {
            Assert.Equal(Guid.Empty, invalidGuid.ToGuid());
        }

        [Theory]
        [InlineData("Administrator", Roles.Administrator)]
        [InlineData("foo", Roles.User)]
        public void ToEnum_Should_Be_Able_To_Convert_From_String(string input, Roles output)
        {
            Assert.Equal(output, input.ToEnum(output));
        }

        [Theory]
        [InlineData(" abcd", "abcd")]
        [InlineData("abcd", "abcd")]
        [InlineData("ab;cd", "abcd")]
        [InlineData("abc/d", "abcd")]
        [InlineData("abc+d", "abcd")]
        [InlineData("abc<d", "abcd")]
        [InlineData("a&bcd", "abcd")]
        [InlineData("ab   cd", "ab-cd")]
        [InlineData("<>,+$", "")]
        [InlineData("", "")]
        public void ToLegalUrl_Should_Return_String_Which_Does_Not_Contain_Any_Illegal_Url_Character(string target, string result)
        {
            Assert.Equal(result, target.ToLegalUrl());
        }

        [Fact]
        public void UrlEncode_Should_Return_Url_Encoded_String()
        {
            Assert.Equal("A%3fString", "A?String".UrlEncode());
        }

        [Fact]
        public void UrlDecode_Should_Return_Url_Decoded_String()
        {
            Assert.Equal("A?String", "A%3fString".UrlDecode());
        }

        [Fact]
        public void AttributeEncode_Should_Return_Attribute_Encoded_String()
        {
            Assert.Equal("&quot;quotes", "\"quotes".AttributeEncode());
        }

        [Fact]
        public void HtmlEncode_Should_Return_Html_Encoded_String()
        {
            Assert.Equal("&lt;A?String/&gt;", "<A?String/>".HtmlEncode());
        }

        [Fact]
        public void HtmlDecode_Should_Return_Html_Decoded_String()
        {
            Assert.Equal("<A?String/>", "&lt;A?String/&gt;".HtmlDecode());
        }
    }
}