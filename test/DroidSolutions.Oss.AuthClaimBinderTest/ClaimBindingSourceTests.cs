using DroidSolutions.Oss.AuthClaimBinder;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Xunit;

namespace DroidSolutions.Oss.AuthClaimBinderTest;

public class ClaimBindingSourceTests
{
  [Fact]
  public void ShouldAcceptDataFromItself()
  {
    ClaimBindingSource sut = new("claim", "claim", true, true);
    Assert.True(sut.CanAcceptDataFrom(sut));
  }

  [Fact]
  public void ShouldAcceptDataFromCustom()
  {
    ClaimBindingSource sut = new("claim", "claim", true, true);
    Assert.True(sut.CanAcceptDataFrom(BindingSource.Custom));
  }
}
