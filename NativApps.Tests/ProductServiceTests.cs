using Moq;
using NativApps.Core.Models;
using NativApps.Core.Services;
using ProductDto = NativApps.Core.Models.Dto.ProductDto;

namespace NativApps.Tests;

public class ProductServiceTests
{
    private static Product GetFullProduct(int id)
    {
        return new Product()
        {
            ProductId = id,
            Category = $"CategoryValue {id}",
            Description = $"DescriptionValue {id}",
            InitialQuantity = 1,
            Name = $"NameValue {id}",
            Price = id * 100,
        };
    }

    private static IEnumerable<ProductDto> PartialProductDtos()
    {
        yield return new ProductDto()
        {
            Name = "Test",
        };
        yield return new ProductDto()
        {
            Category = "Test",
        };
        yield return new ProductDto()
        {
            Description = "Test",
        };
        yield return new ProductDto()
        {
            InitialQuantity = 1,
        };
        yield return new ProductDto()
        {
            Price = 1m
        };
        yield return new ProductDto()
        {
            Name = "Test",
            Description = "Test",
            InitialQuantity = 1,
            Price = 1m
        };
        yield return new ProductDto()
        {
            Name = "Test",
            Category = "Test",
            InitialQuantity = 1,
            Price = 1m
        };
        yield return new ProductDto()
        {
            Name = "Test",
            Category = "Test",
            Description = "Test",
            Price = 1m
        };
        yield return new ProductDto()
        {
            Name = "Test",
            Category = "Test",
            Description = "Test",
            InitialQuantity = 1,
        };
    }

    [Test]
    public void Service_can_create_products()
    {
        var newProduct = new ProductDto()
        {
            Name = "Test",
            Category = "Test",
            Description = "Test",
            InitialQuantity = 1,
            Price = 1m
        };        
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Create(It.IsAny<Product>())).Returns(1).Verifiable(Times.Once());

        var service = new ProductService(repo.Object);
        Assert.Multiple(() =>
        {
            Assert.That(service.Create(newProduct).Success, Is.True);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [TestCaseSource(nameof(PartialProductDtos))]
    public void Create_fails_if_data_is_missing(ProductDto failingDto)
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Create(It.IsAny<Product>())).Verifiable(Times.Never());

        var service = new ProductService(repo.Object);
        Assert.Multiple(() =>
        {
            Assert.That(service.Create(failingDto).Success, Is.False);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [Test]
    public void Create_fails_on_repo_error()
    {
        var newProduct = new ProductDto()
        {
            Name = "Test",
            Category = "Test",
            Description = "Test",
            InitialQuantity = 1,
            Price = 1m
        };
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Create(It.IsAny<Product>())).Throws<IOException>().Verifiable(Times.Once());

        var service = new ProductService(repo.Object);
        Assert.Multiple(() =>
        {
            Assert.That(service.Create(newProduct).Success, Is.False);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [Test]
    public void Service_can_update_products()
    {
        var newData = new ProductDto() { Name = "Test2" };
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Read(1)).Returns(GetFullProduct(1)).Verifiable(Times.Once());
        repo.Setup(r => r.Update(It.IsAny<Product>())).Verifiable(Times.Once());

        var service = new ProductService(repo.Object);
        var result = service.Update(1, newData);
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.Null);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [Test]
    public void Service_skips_update_if_no_changes_declared()
    {
        var newData = new ProductDto();
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Read(1)).Returns(GetFullProduct(1));
        repo.Setup(r => r.Update(It.IsAny<Product>())).Verifiable(Times.Never());
        var service = new ProductService(repo.Object);
        var result = service.Update(1, newData);
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
        repo.Verify();
    }

    [Test]
    public void Service_skips_update_if_no_actual_changes()
    {
        var newData = new ProductDto() { Name = "NameValue 1" };
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Read(1)).Returns(GetFullProduct(1));
        repo.Setup(r => r.Update(It.IsAny<Product>())).Verifiable(Times.Never());
        var service = new ProductService(repo.Object);
        var result = service.Update(1, newData);
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
        repo.Verify();
    }

    [Test]
    public void Update_fails_if_product_does_not_exists()
    {
        var newData = new ProductDto() { Name = "Test2" };
        var repo = new Mock<IProductRepository>();
        var product = GetFullProduct(1);
        repo.Setup(r => r.Read(1)).Returns((Product?)null).Verifiable(Times.Once());
        repo.Setup(r => r.Update(product)).Verifiable(Times.Never());

        var service = new ProductService(repo.Object);
        var result = service.Update(1, newData);
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [Test]
    public void Service_can_read_products()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Read(1))
            .Returns(GetFullProduct(1))
            .Verifiable(Times.Once());
        var service = new ProductService(repo.Object);
        var result = service.Read(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Result, Is.InstanceOf<Product>());
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [Test]
    public void Read_fails_if_product_does_not_exist()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Read(1))
            .Returns((Product?)null)
            .Verifiable(Times.Once());
        var service = new ProductService(repo.Object);
        var result = service.Read(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Result, Is.Null);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [Test]
    public void Service_can_delete_products()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.Read(1)).Returns(GetFullProduct(1)).Verifiable(Times.Once());
        repo.Setup(r => r.Delete(1)).Verifiable(Times.Once());

        var service = new ProductService(repo.Object);
        Assert.Multiple(() =>
        {
            Assert.That(service.Delete(1).Success, Is.True);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }

    [Test]
    public void Querying_by_name_returns_data()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.GetAll())
            .Returns(Enumerable.Range(1, 10).Select(GetFullProduct).AsQueryable())
            .Verifiable(Times.Exactly(3));
        var service = new ProductService(repo.Object);
        Assert.Multiple(() =>
        {
            Assert.That(service.GetByName("1").ToArray(), Has.Length.EqualTo(2));
            Assert.That(service.GetByName("2").ToArray(), Has.Length.EqualTo(1));
            Assert.That(service.GetByName("XXX").ToArray(), Is.Empty);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }


    [Test]
    public void Querying_by_price_range_returns_data()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.GetAll())
            .Returns(Enumerable.Range(1, 10).Select(GetFullProduct).AsQueryable())
            .Verifiable(Times.Exactly(3));
        var service = new ProductService(repo.Object);
        Assert.Multiple(() =>
        {
            Assert.That(service.GetByPriceRange(150, 350).ToArray(), Has.Length.EqualTo(2));
            Assert.That(service.GetByPriceRange(450, 550).ToArray(), Has.Length.EqualTo(1));
            Assert.That(service.GetByPriceRange(0, 50).ToArray(), Is.Empty);
            Assert.That(() => repo.Verify(), Throws.Nothing);
        });
    }
}