using DroidSolutions.Oss.AuthClaimBinder;

using Xunit;

namespace DroidSolutions.Oss.AuthClaimBinderTest;

public class FromClaimAttributeTests
{
  [Fact]
  public void ShouldSetBindingSourceToClaim()
  {
    FromClaimAttribute sut = new();
    Assert.Equal(ClaimBindingSource.Claim, sut.BindingSource);
  }
}
