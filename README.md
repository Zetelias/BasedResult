# BasedResult, a Rust-like Result type in a Csharp-ier way.
## Description
This is a speed and memory optimized Result implementation in C#.
It is way faster than exceptions to handle errors.
The API is familiar to the Rust Result, but still has some
differences, as the two languages are different.


## Examples
### Example basic usage
```csharp
using BasedResult;

static Result<int, string> Divide(int x, int y)
{
    if (y == 0)
        return "Attempted to divide by 0";
    return x / y;
}

static void Main()
{
    var res = Divide(1, 2);
    res.Match(
        ok => Console.WriteLine(ok),
        err => Console.WriteLine(err)
    );
}
```
### Example chains
```cs
using BasedResult;

static Result<int, string> Divide(int x, int y)
{
    if (y == 0)
        return "Attempted to divide by 0";
    return x / y;
}

static Result<int, string> OverflowSafeMultiply(int x, int y)
{
    if ((y > int.MaxValue / x) || (x > int.MaxValue / y))
        return Result<int, string>.Err("Would overflow");
    return Result<int, string>.Ok(x * y);
}

static void Main()
{
    var res = Divide(0, 1)
        .AndThen(z => OverflowSafeMultiply(z, 1), "Division error")
        .AndThen(w => Divide(w, 4), "Multiplication error");

    // Now res is either an Error result with the first problematic result in the chain
    // or an Ok result with w * 4
}
```
### Example case where we return the same type in Ok and Err cases
```cs
static Result<string, string> NullableStringToResult(string? value)
{
    if (value == null)
        return Result<string, string>.Err("value was null");
    return Result<string, string>.Ok(value);
}
```


## Differences to Rust
- Obviously, no "?"
- Not all functions are implemented.
- UpperCamelCase is used instead of snake_case
- Matching is done with Result.Match()
- Calling .Unwrap() or .Expect() on an Err result will throw
  an exception, and not start a panic which makes it
  recoverable.
- There is implicit conversion from the OK type to an OK result
  and visa-versa for Err results. Optionally, implicit unwrapping.
  with `#define ENABLE_IMPLICIT_UNWRAPPING`

## Contributing
Contributions are welcomed

## License
[MIT](https://mit-license.org/)
