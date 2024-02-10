using ExpectedObjects;
using Moq;
using OpenAdm.Application.Models;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class BannerServiceTest
{
    [Fact]
    public async Task DeveRetornarOsBanners()
    {
        var banners = new List<Banner>()
        {
            new(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1 ,new byte[10], true)
        };

        var querable = banners.AsQueryable();

        var repositoryBannerMock = new Mock<IBannerRepository>();
        repositoryBannerMock.Setup(x => x.GetBannersAsync()).ReturnsAsync(querable);

        var service = new BannerService(repositoryBannerMock.Object);

        var result = await service.GetBannersAsync();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BannerViewModel>>(result);
        banners
            .First().Id
            .ToExpectedObject()
            .ShouldEqual(result.ToList().First().Id);
    }
}
