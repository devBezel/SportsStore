using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void IndicatesSelectedCategory()
        {
            string categoryToSelect = "Jablka";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Jablka"},
                new Product { ProductID = 4, Name = "P2", Category = "Pomarancze"},
            }).AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext { ViewContext = new ViewContext { RouteData = new RouteData() } };

            target.RouteData.Values["category"] = categoryToSelect;

            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            Assert.Equal(categoryToSelect, result);
        }

        [Fact]
        public void CanSelectCategories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Jablka"},
                new Product { ProductID = 2, Name = "P2", Category = "Jablka"},
                new Product { ProductID = 3, Name = "P3", Category = "Sliwki"},
                new Product { ProductID = 4, Name = "P4", Category = "Pomarancze"},
            }).AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            string[] result = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(new string[] { "Jablka", "Pomarancze", "Sliwki" }, result));
        }
    }
}
