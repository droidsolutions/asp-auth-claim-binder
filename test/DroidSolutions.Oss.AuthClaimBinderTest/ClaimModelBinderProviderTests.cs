using DroidSolutions.Oss.AuthClaimBinder;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using Xunit;

namespace DroidSolutions.Oss.AuthClaimBinderTest;

public class ClaimModelBinderProviderTests
{
  [Fact]
  public void GetBinder_ShouldReturnNull_IfComingFromOtherBindingSource()
  {
    ClaimModelBinderProvider sut = new();
    BindingInfo info = new() { BindingSource = BindingSource.Query };

    var context = new Mock<ModelBinderProviderContext>();
    context.Setup(x => x.BindingInfo).Returns(info);

    Assert.Null(sut.GetBinder(context.Object));
  }

  [Fact]
  public void GetBinder_ShouldReturnNull_WhenBindingSourceIsNull()
  {
    ClaimModelBinderProvider sut = new();
    BindingInfo info = new() { BindingSource = null };

    var context = new Mock<ModelBinderProviderContext>();
    context.Setup(x => x.BindingInfo).Returns(info);

    Assert.Null(sut.GetBinder(context.Object));
  }

  [Fact]
  public void GetBinder_ShouldReturnClaimModelBinder_IfComingFromClaimBindingSource()
  {
    ClaimModelBinderProvider sut = new();
    BindingInfo info = new() { BindingSource = ClaimBindingSource.Claim, };
    IServiceCollection services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
    services.AddScoped<ILogger<ClaimModelBinder>>(x => NullLoggerFactory.Instance.CreateLogger<ClaimModelBinder>());

    var context = new Mock<ModelBinderProviderContext>();
    context.Setup(x => x.BindingInfo).Returns(info);
    context.Setup(x => x.Services).Returns(services.BuildServiceProvider());

    Assert.IsType<ClaimModelBinder>(sut.GetBinder(context.Object));
  }
}
