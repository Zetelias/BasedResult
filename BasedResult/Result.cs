using System;

namespace BasedResult
{
    public class Result<OkType, ErrType>
    {
        internal object Obj;
        
        public static Result<OkType, ErrType> Ok(OkType okType)
        {
            return new Result<OkType, ErrType> { Obj = okType };
        }

        public static Result<OkType, ErrType> Err(ErrType errType)
        {
            return new Result<OkType, ErrType> { Obj = errType };
        }
        
        public static implicit operator Result<OkType, ErrType>(OkType okType)
            => new Result<OkType, ErrType> { Obj = okType };
        
        public static implicit operator Result<OkType, ErrType>(ErrType errType)
            => new Result<OkType, ErrType> { Obj = errType };

        public static implicit operator bool(Result<OkType, ErrType> result)
            => result.IsOk();
        
        public bool IsOk()
            => Obj is OkType;

        public bool IsErr()
            => Obj is ErrType;

        public OkType Unwrap()
        {
            switch (Obj)
            {
                case OkType ok:
                    return ok;
                default:
                    throw new InvalidOperationException();
            }
        }
        
        public OkType UnwrapOr(OkType fallback)
        {
            switch (Obj)
            {
                case OkType ok:
                    return ok;
                default:
                    return fallback;
            }
        }
        
        public OkType UnwrapOrDefault()
        {
            switch (Obj)
            {
                case OkType ok:
                    return ok;
                default:
                    return default(OkType);
            }
        }
    }
}
