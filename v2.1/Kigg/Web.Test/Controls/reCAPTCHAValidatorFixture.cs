using System.Collections.Specialized;
using System.Net;
using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class reCAPTCHAValidatorFixture : BaseFixture
    {
        public const string VerifyUrl = "http://api-verify.recaptcha.net/verify";
        public const string InsecureHost = "http://api.recaptcha.net";
        public const string SecureHost = "https://api-secure.recaptcha.net";
        public const string PrivateKey = "6Lf1EAQAAAAAAKIyPloGISsXZV7t6O8p72pxdL2F";
        public const string PublicKey = "6Lf1EAQAAAAAAAzjqvg-oUY868Np7V7vLrBv2D8e";
        public const string ChallengeInputName = "recaptcha_challenge_field";
        public const string ResponseInputName = "recaptcha_response_field";

        private readonly Mock<IHttpForm> _httpForm;
        private readonly reCAPTCHAValidator _validator;

        public reCAPTCHAValidatorFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _validator = new reCAPTCHAValidator(VerifyUrl, InsecureHost, SecureHost, PrivateKey, PublicKey, ChallengeInputName, ResponseInputName, _httpForm.Object);
        }

        [Fact]
        public void InsecureHost_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(InsecureHost, _validator.InsecureHost);
        }

        [Fact]
        public void SecureHost_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(SecureHost, _validator.SecureHost);
        }

        [Fact]
        public void PublicKey_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(PublicKey, _validator.PublicKey);
        }

        [Fact]
        public void ChallengeInputName_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(ChallengeInputName, _validator.ChallengeInputName);
        }

        [Fact]
        public void ResponseInputName_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(ResponseInputName, _validator.ResponseInputName);
        }

        [Fact]
        public void Validate_Should_Use_HttpForm()
        {
            Validate("foobar");

            _httpForm.Verify();
        }

        [Fact]
        public void Validate_Should_Return_True_When_Response_Contains_True_In_String()
        {
            bool result = Validate("true");

            Assert.True(result);
        }

        [Fact]
        public void Validate_Should_Return_False_When_Response_Contains_False_In_String()
        {
            bool result = Validate("false");

            Assert.False(result);
        }

        [Fact]
        public void Validate_Should_Return_False_When_Response_Contains_Unknown_Value()
        {
            bool result = Validate("foobar");

            Assert.False(result);
        }

        [Fact]
        public void Validate_Should_Return_True_When_Exception_Occurrs()
        {
            var result = ValidateOccurrsException();

            Assert.True(result);
        }

        [Fact]
        public void Validate_Should_Log_Exception_When_Exception_Occurrs()
        {
            ValidateOccurrsException();

            log.Verify();
        }

        private bool Validate(string response)
        {
            _httpForm.Expect(h => h.Post(It.IsAny<string>(), It.IsAny<NameValueCollection>())).Returns(response).Verifiable();

            return _validator.Validate("192.168.0.1", "foo", "bar");
        }

        private bool ValidateOccurrsException()
        {
            _httpForm.Expect(h => h.Post(It.IsAny<string>(), It.IsAny<NameValueCollection>())).Throws<WebException>();

            return _validator.Validate("192.168.0.1", "foo", "bar");
        }
    }
}