using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Factories.Factory;

namespace OpenAdm.Test.Infra.Test;

public class DomainFactoryTest
{
    [Fact]
    public void GetDomainParceiroIsNull()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);
        var factory = new DomainFactory(httpContextAccessorMock.Object);

        Assert.Throws<ExceptionApi>(factory.GetDomainParceiro);
    }

    [Fact]
    public void GetDomainParceiroReturnDomain()
    {
        const string dominio = "https://test.com";
        const string key = "Referer";
        var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { key,  dominio}
            });

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(x => x.Request.Headers).Returns(headers);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

        var factory = new DomainFactory(httpContextAccessorMock.Object);

        var result = factory.GetDomainParceiro();

        Assert.Equal(dominio, result);
    }
}
