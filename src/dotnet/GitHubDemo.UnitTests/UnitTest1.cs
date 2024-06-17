namespace GitHubDemo.UnitTests
{
    public class UnitTest1
    {
        // this trait allows us to execute this unit tests through our ci pipeline
        // run: dotnet test --no-restore --verbosity normal --filter Category=Unit
        [Trait("Category", "Unit")]
        [Fact]
        public void Test1()
        {
            int a = 2;
            int b = 3;
            int sum = a + b;
            Assert.True(sum == 5);
        }
    }
}