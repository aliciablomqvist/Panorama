using Moq;
using System.Collections.Generic;
using System.Linq;

namespace PanoramaApp.Tests.Helpers
{
public static class DbSetMockExtensions
{
    public static Mock<DbSet<T>> CreateMockDbSet<T>(this IEnumerable<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        return mockSet;
    }

    public static DbSet<T> ReturnsDbSet<T>(this Mock<DbSet<T>> dbSet, List<T> data) where T : class
    {
        return CreateMockDbSet(data).Object;
    }
}
}