using System.Collections.Generic;
using System.Web.Mvc;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Repository;

    public class CateoryControllerFixture
    {
        private readonly Mock<ICategoryRepository> _repository;
        private readonly CategoryController _controller;

        public CateoryControllerFixture()
        {
            _repository = new Mock<ICategoryRepository>();
            _controller = new CategoryController(_repository.Object);
        }

        [Fact]
        public void Menu_Should_Render_Default_View()
        {
            var result = (ViewResult) _controller.Menu();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Menu_Should_Use_Category_Repository()
        {
            _repository.Expect(r => r.FindAll()).Returns((ICollection<ICategory>) null).Verifiable();
            _controller.Menu();

            _repository.Verify();
        }

        [Fact]
        public void RadioButtonList_Should_Render_Default_View()
        {
            var result = (ViewResult)_controller.Menu();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void RadioButtonList_Should_Use_Category_Repository()
        {
            _repository.Expect(r => r.FindAll()).Returns((ICollection<ICategory>) null).Verifiable();
            _controller.RadioButtonList();

            _repository.Verify();
        }
    }
}