using BasedResult;

namespace BasedResultTest
{
    public class ResultUnionTests
    {
        [Test]
        public void Test1()
        {
            ResultUnion<int, int> res = new ResultUnion<int, int>();
            res.Ok = 5;
            Assert.Equals(5, res.Ok);
            res.Err = 6;
            Assert.Equals(6, res.Err);
        }
    }
}