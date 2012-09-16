using Xunit;

namespace Kigg.Web.Test
{
    public class AssetElementCollectionFixture
    {
        private readonly AssetElementCollectionTestDouble _collection;

        public AssetElementCollectionFixture()
        {
            _collection = new AssetElementCollectionTestDouble();
        }

        [Fact]
        public void CreateNewElement_Should_Return_New_Element()
        {
            var element = _collection.CreateNewElement();

            Assert.NotNull(element);
        }

        [Fact]
        public void GetElementKey_Should_Return_Element_With_The_Specified_Name()
        {
            var element = CreateElement();

            Assert.Equal(element.Name, _collection.GetElementKey(element));
        }

        [Fact]
        public void Add_Should_Increases_The_Collection()
        {
            var element = CreateElement();
            _collection.Add(element);

            Assert.Equal(1, _collection.Count);
        }

        [Fact]
        public void Index_Should_Return_Element()
        {
            AssetElement element = CreateElement();
            _collection.Add(element);

            var result = _collection["foo"];

            Assert.Same(element, result);
        }

        private static AssetElement CreateElement()
        {
            return new AssetElement{Name = "foo", ContentType = "bar"};
        }
    }

    public class AssetElementCollectionTestDouble : AssetElementCollection
    {
        public string GetElementKey(AssetElement element)
        {
            return base.GetElementKey(element) as string;
        }

        public new AssetElement CreateNewElement()
        {
            return base.CreateNewElement() as AssetElement;
        }
    }
}