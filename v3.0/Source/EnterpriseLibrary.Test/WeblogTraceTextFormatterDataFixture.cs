using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogTraceTextFormatterDataFixture
    {
        private const string Template = "Foo";
        private const string Name = "Bar";

        private WeblogTraceTextFormatterData _formatterData;

        [Fact]
        public void Template_Should_Be_Empty_String_When_No_Template_Is_Passed_In_Constructor()
        {
            _formatterData = new WeblogTraceTextFormatterData();

            Assert.Equal(string.Empty, _formatterData.Template);
        }

        [Fact]
        public void Template_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            _formatterData = new WeblogTraceTextFormatterData(Template);

            Assert.Equal(Template, _formatterData.Template);
        }

        [Fact]
        public void Name_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            _formatterData = new WeblogTraceTextFormatterData(Name, Template);

            Assert.Equal(Name, _formatterData.Name);
        }

        [Fact]
        public void Type_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            _formatterData = new WeblogTraceTextFormatterData(Name, GetType(), Template);

            Assert.Equal(GetType(), _formatterData.Type);
        }
    }
}