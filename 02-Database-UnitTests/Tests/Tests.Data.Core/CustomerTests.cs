using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Data.Core
{
  using System.Data.Entity;
  using System.Threading.Tasks;
  using global::Data.Core;

  [TestClass]
  public class CustomerTests
  {
    /// <summary>
    /// Checks if the current amount of customers in database matches 
    /// the expected one.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task GetCustomerCount()
    {
      // arrange
      const int ExpectedCount = 0;
      var realCount = -1;

      // act
      using (var ctx = new UnitTestSampleEntities())
      {
        realCount = await ctx.Customers.CountAsync();
      }

      // assert
      Assert.AreEqual(ExpectedCount, realCount);
    }
  }
}
