namespace BasedResult;

public class Example
{
    public static Result<int, string> Divide(int x)
    {
        return x;
    }
    
    public static Result<string, int> Caca()
    
    public static void Test()
    {
        var result = Divide(5)
            .AndThen(x => Divide(x), "wow")
            .AndThen(x => Divide(x), "wow");
    }
}