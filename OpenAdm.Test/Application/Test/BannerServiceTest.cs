using OpenAdm.Domain.Entities;
using ExpectedObjects;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Azure.Interfaces;

namespace OpenAdm.Test.Application.Test;

public class BannerServiceTest
{
    [Fact]
    public async Task DeveRetornarOsBanners()
    {
        var banners = new List<Banner>()
        {
            new(Guid.NewGuid(), DateTime.Now, DateTime.Now, 1 ,"teste", true, "nome foto")
        };

        var repositoryBannerMock = new Mock<IBannerRepository>();
        var uploadMock = new Mock<IUploadImageBlobClient>();
        repositoryBannerMock.Setup(x => x.GetBannersAsync()).ReturnsAsync(banners);

        var service = new BannerService(repositoryBannerMock.Object, uploadMock.Object);

        var result = await service.GetBannersAsync();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BannerViewModel>>(result);
        banners
            .First().Id
            .ToExpectedObject()
            .ShouldEqual(result.First().Id);
    }
}
