using System.Diagnostics;

namespace BasedResult
{
    /// <summary>
    /// An optimized Rust like Result that can either be of type <typeparamref name="OkType"/> or of type <typeparamref name="ErrType"/>.
    /// A result of type <typeparamref name="OkType"/> represents a successfull operation while a result of type <typeparamref name="ErrType"/>
    /// represents an unsuccessfull operation.
    /// </summary>
    /// <typeparam name="OkType"> What we intend to return in a function if all goes according to plan </typeparam>
    /// <typeparam name="ErrType"> What we intend to return in a function if errors happen </typeparam>
    public class Result<OkType, ErrType>
    {

        /// <summary>
        /// The underlying owned data in the Result.
        /// It can either be <typeparamref name="OkType"/> or <typeparamref name="ErrType"/>.
        /// </summary>
        internal object Obj;

        /// <summary>
        /// Represents if the result is successful.
        /// If .IsOk is true, the result contains an OkType.
        /// Ex: if .Unwrap() is safe to call.
        /// </summary>
        public bool IsOk { get; internal init; }
        
        /// <summary>
        /// Represents if the result has failed.
        /// If .IsErr is true, the result contains an ErrType.
        /// Ex: if .UnwrapErr() is safe to call.
        /// </summary>
        public bool IsErr => !IsOk;

        /// <summary>
        /// Returns a Result that's Ok, with value <paramref name="okType"/>
        /// and given generics.
        /// </summary>
        /// <param name="okType">The Ok value to be held in the result</param>
        /// <returns>An Ok result with value <paramref name="okType"/></returns>
        public static Result<OkType, ErrType> Ok(OkType okType)
        {
            return new Result<OkType, ErrType> { Obj = okType, IsOk = true };
        }

        /// <summary>
        /// Returns a Result that's Err, with value <paramref name="errType"/>
        /// and given generics.
        /// </summary>
        /// <param name="errType">the Err value to be held in the result</param>
        /// <returns>An Err result with value <paramref name="errType"/></returns>
        public static Result<OkType, ErrType> Err(ErrType errType) 
        {
            return new Result<OkType, ErrType> { Obj = errType, IsOk = false };
        }
        
        public static implicit operator Result<OkType, ErrType>(OkType okType)
            => new Result<OkType, ErrType> { Obj = okType, IsOk = true };
        
        public static implicit operator Result<OkType, ErrType>(ErrType errType)
            => new Result<OkType, ErrType> { Obj = errType, IsOk = false };

        public static implicit operator bool(Result<OkType, ErrType> result)
            => result.IsOk;
        
        /// <summary>
        /// Executes the corresponding Action,
        /// with an <typeparamref name="OkType"/> or an <typeparamref name="ErrType"/>
        /// based on the state of the Result.
        /// </summary>
        /// <param name="okAction">The action to execute with an
        /// <typeparamref name="OkType"/> if the result is Ok. </param>
        /// <param name="errAction"> The action to execute with an
        /// <typeparamref name="ErrType"/> if the result is Err.
        /// </param>
        public void Match(
            Action<OkType> okAction,
            Action<ErrType> errAction)
        {
            if (IsOk)
            {
                okAction(Unwrap());
            }
            else
            {
                errAction(UnwrapErr());
            }
        }

        /// <summary>
        /// Returns the first Ok Result in <paramref name="results"/>,
        /// or an error message if none of the Result's were Ok.
        /// </summary>
        /// <param name="results">An array of result, we will take the first
        /// result that's Ok in this array.</param>
        /// <returns>The Ok value of the first Ok result in array <paramref name="results"/>
        /// or an error message if none of the results were Ok. </returns>
        public static Result<OkType, string> FirstOk(Result<OkType, ErrType>[] results)
        {
            // If there's an OK result in the array, this will resolve to an Ok result
            // else, it will be null
            var okResult = results
                .FirstOrDefault(result => result.IsOk, defaultValue: null);

            
            // So, if it's not null we return the unwrapped value
            // or, if it's null, we return an error message
            return okResult.Unwrap() ??
                   Result<OkType, string>.Err("No Ok result in array");
        }

        
        public static Result<OkType, string> FirstOk(List<Result<OkType, ErrType>> results)
            => FirstOk(results.ToArray());
        

        #if ENABLE_IMPLICIT_UNWRAPPING // Locked behind a label as it can lead to unexpected results but can make life easier

        public static implicit operator OkType(Result<OkType, ErrType> result)
            => result.Unwrap();

        public static implicit operator ErrType(Result<OkType, ErrType> result)
            => result.UnwrapErr();

        #endif

        /// <summary>
        /// Returns the Ok value held by the Result,
        /// or an Exception if the Result is not Ok.
        /// </summary>
        /// <returns>The Ok value held by the Result if it's Ok</returns>
        /// <exception cref="InvalidOperationException">If the result is Err</exception>
        public OkType Unwrap()
        {
            if (IsOk)
            {
                switch (Obj)
                {
                    case OkType ok:
                        return ok;
                    default:
                        throw new UnreachableException("Called Result.Unwrap() on an Ok value " +
                                                       "that held the wrong type.\n" +
                                                       "Should never happen, please open an issue."); // should never happen
                }
            }
            throw new InvalidOperationException("Called Result.Unwrap() on an Err value");
        }
        
        public OkType Expect(string errorMessage) 
        {
            if (IsOk)
            {
                switch (Obj)
                {
                    case OkType ok:
                        return ok;
                    default:
                        throw new UnreachableException("Called Result.Expect() on an Ok value " +
                                                       "that held the wrong type.\n" +
                                                       "Should never happen, please open an issue."); // should never happen
                }
            }
            throw new InvalidOperationException(errorMessage);
        }

        public OkType UnwrapOr(OkType fallback)
        {
            if (IsOk)
            {
                switch (Obj)
                {
                    case OkType ok:
                        return ok;
                    default:
                        return fallback; // should never happen
                }
            }
            return fallback;
        }

        public OkType UnwrapOrElse(Func<OkType> fn)
        {
            if (IsOk)
            {
                switch (Obj)
                {
                    case OkType ok:
                        return ok;
                    default:
                        return fn(); // should never happen
                }
            }
            return fn();
        }

        public OkType? UnwrapOrDefault()
        {
            if (IsOk)
            {
                switch (Obj)
                {
                    case OkType ok:
                        return ok;
                    default:
                        return default; // should never happen
                }
            }
            return default;
        }

        public ErrType UnwrapErr()
        {
            if (!IsOk)
            {
                switch (Obj)
                {
                    case ErrType err:
                        return err;
                    default:
                        throw new UnreachableException("Called Result.UnwrapErr() on an Err " +
                                                       "Result but that held the wrong type.\n" +
                                                       "Should never happen, please open an issue.");
                }
            }
            throw new InvalidOperationException("Called Result.UnwrapErr() on an Ok Result");
        }

        public Result<NewOk, NewErr> AndThen<NewOk, NewErr>(Func<OkType, Result<NewOk, NewErr>> fn, NewErr error = default)
        {
            if (IsOk)
            {
                switch (Obj)
                {
                    case OkType ok:
                        return fn(ok);
                    default:
                        return error;
                }
            }

            return error;
        }

        public ErrType UnwrapOr(ErrType fallback)
        {
            if (!IsOk)
            {
                switch (Obj)
                {
                    case ErrType err:
                        return err;
                    default:
                        return fallback;
                }
            }
            return fallback;
        }

        public ErrType? UnwrapErrOrDefault()
        {
            if (!IsOk)
            {
                switch (Obj)
                {
                    case ErrType err:
                        return err;
                    default:
                        return default;
                }
            }
            return default;
        }
    }
}
