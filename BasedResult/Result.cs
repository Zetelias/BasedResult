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

        public bool IsOk { get; internal set; }
        public bool IsErr { get => !IsOk; }
        
        public static Result<OkType, ErrType> Ok(OkType okType)
        {
            return new Result<OkType, ErrType> { Obj = okType, IsOk = true };
        }

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

        #if ENABLE_IMPLICIT_UNWRAPPING // Locked behind a label as it can lead to unexpected results but can make life easier

        public static implicit operator OkType(Result<OkType, ErrType> result)
            => result.Unwrap();

        public static implicit operator ErrType(Result<OkType, ErrType> result)
            => result.UnwrapErr();

        #endif

        public OkType Unwrap()
        {
            if (IsOk)
            {
                switch (Obj)
                {
                    case OkType ok:
                        return ok;
                    default:
                        throw new InvalidOperationException(); // should never happen
                }
            }
            throw new InvalidOperationException();
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
                        throw new InvalidOperationException();
                }
            }
            throw new InvalidOperationException();
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
