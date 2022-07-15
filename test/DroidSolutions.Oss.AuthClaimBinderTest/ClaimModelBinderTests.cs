using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using DroidSolutions.Oss.AuthClaimBinder;
using DroidSolutions.Oss.AuthClaimBinder.Settings;
using DroidSolutions.Oss.AuthClaimBinderTest.Fixture;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace DroidSolutions.Oss.AuthClaimBinderTest;

public class ClaimModelBinderTests
{
  private readonly IMock<ILogger<ClaimModelBinder>> _logMock;

  public ClaimModelBinderTests()
  {
    _logMock = new Mock<ILogger<ClaimModelBinder>>();
  }

  [Fact]
  public async Task BindModelAsync_ShouldThrowInvalidOperationException_WhenClaimIsNull()
  {
    var claims = new Claim[]
     {
        new Claim(ClaimTypes.Name, "Affe"),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<ModelBindingContext>();
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, null);
    await Assert.ThrowsAsync<InvalidOperationException>(() => claimModelBinder.BindModelAsync(bindingContext.Object));
  }

  [Fact]
  public async Task BindModelAsync_ShouldThrowInvalidOperationException_WhenClaimIsNotAValidGuid()
  {
    var claims = new Claim[]
     {
        new Claim("Affe", "not-a-valid-uuid"),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<ModelBindingContext>();
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);
    bindingContext.Setup(bc => bc.FieldName).Returns("Affe");
    bindingContext.Setup(bc => bc.ModelType).Returns(typeof(Guid));

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, null);
    await Assert.ThrowsAsync<InvalidOperationException>(() => claimModelBinder.BindModelAsync(bindingContext.Object));
  }

  [Fact]
  public async Task BindModelAsync_ShouldNotFail_WhenAliasConfigIsNull()
  {
    var claims = new Claim[]
     {
        new Claim("user", "someuser"),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<DefaultModelBindingContext>() { CallBase = true };
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);
    bindingContext.Setup(bc => bc.FieldName).Returns("user");
    bindingContext.Setup(bc => bc.ModelType).Returns(typeof(string));

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, new ClaimBinderSettings());

    await claimModelBinder.BindModelAsync(bindingContext.Object);

    Assert.Equal("someuser", bindingContext.Object.Result.Model);
  }

  [Fact]
  public async Task BindModelAsync_ShouldNotFail_WhenSettingsIsNull()
  {
    var claims = new Claim[]
     {
        new Claim("user", "someuser"),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<DefaultModelBindingContext>() { CallBase = true };
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);
    bindingContext.Setup(bc => bc.FieldName).Returns("user");
    bindingContext.Setup(bc => bc.ModelType).Returns(typeof(string));

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, null);

    await claimModelBinder.BindModelAsync(bindingContext.Object);

    Assert.Equal("someuser", bindingContext.Object.Result.Model);
  }

  [Fact]
  public async Task BindModelAsync_ShouldTryAlias_WhenConfigured()
  {
    var claims = new Claim[]
     {
        new Claim(ClaimTypes.NameIdentifier, "someuser"),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<DefaultModelBindingContext>() { CallBase = true };
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);
    bindingContext.Setup(bc => bc.FieldName).Returns("user");
    bindingContext.Setup(bc => bc.ModelType).Returns(typeof(string));

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, new ClaimBinderSettings
    {
      AliasConfig = new Dictionary<string, List<string>> { { "user", new List<string> { "username", ClaimTypes.NameIdentifier } }, },
    });

    await claimModelBinder.BindModelAsync(bindingContext.Object);

    Assert.Equal("someuser", bindingContext.Object.Result.Model);
  }

  [Fact]
  public async Task BindModelAsync_ShouldNotFail_IfNoAliasIsFound()
  {
    var claims = new Claim[]
     {
        new Claim("catchme", "ifyoucan"),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<DefaultModelBindingContext>() { CallBase = true };
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);
    bindingContext.Setup(bc => bc.FieldName).Returns("user");
    bindingContext.Setup(bc => bc.ModelType).Returns(typeof(string));

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, new ClaimBinderSettings
    {
      AliasConfig = new Dictionary<string, List<string>> { { "user", new List<string> { "username", ClaimTypes.NameIdentifier } }, },
    });

    await Assert.ThrowsAsync<InvalidOperationException>(() => claimModelBinder.BindModelAsync(bindingContext.Object));
  }

  [Fact]
  public async Task BindingModelAsync_ShouldBeAbleToParseEnums()
  {
    var claims = new Claim[]
     {
        new Claim("role", BasicAuthRole.SomeUser.ToString()),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<DefaultModelBindingContext>() { CallBase = true };
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);
    bindingContext.Setup(bc => bc.FieldName).Returns("role");
    bindingContext.Setup(bc => bc.ModelType).Returns(typeof(BasicAuthRole));

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, null);

    await claimModelBinder.BindModelAsync(bindingContext.Object);

    Assert.Equal(BasicAuthRole.SomeUser, bindingContext.Object.Result.Model);
  }

  [Fact]
  public async Task BindingModelAsync_ShouldThrowInvalidOperationException_WhenEnumIsNotParsable()
  {
    var claims = new Claim[]
     {
        new Claim("role", "apo"),
     };
    var identity = new ClaimsIdentity(claims, "Scheme");
    var principal = new ClaimsPrincipal(identity);

    var httpContext = new Mock<HttpContext>();
    httpContext.Setup(hc => hc.User).Returns(principal);

    var bindingContext = new Mock<DefaultModelBindingContext>() { CallBase = true };
    bindingContext.Setup(bc => bc.HttpContext).Returns(httpContext.Object);
    bindingContext.Setup(bc => bc.FieldName).Returns("role");
    bindingContext.Setup(bc => bc.ModelType).Returns(typeof(BasicAuthRole));

    var claimModelBinder = new ClaimModelBinder(_logMock.Object, null);

    await Assert.ThrowsAsync<InvalidOperationException>(() => claimModelBinder.BindModelAsync(bindingContext.Object));
  }
}
