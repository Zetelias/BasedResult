using System;

namespace BasedResult
{
    public class Result<OkType, ErrType>
    {
        internal ResultUnion<OkType, ErrType> union;
        public bool IsOk { get; internal set; }
        public bool IsErr { get => !IsOk; }

        public static implicit operator Result<OkType, ErrType>(OkType ok)
            => Ok(ok);

        public static implicit operator Result<OkType, ErrType>(ErrType err)
            => Err(err);

        public static Result<OkType, ErrType> Ok(OkType ok)
            => new Result<OkType, ErrType> { union = ok, IsOk = true };

        public static Result<OkType, ErrType> Err(ErrType err)
            => new Result<OkType, ErrType> { union = err, IsOk = false };

        public OkType Unwrap()
            => IsOk ? union.Ok : throw new InvalidOperationException();

        public OkType UnwrapOr(OkType or)
            => IsOk ? union.Ok : or;

        public OkType? UnwrapOrDefault()
            => IsOk ? union.Ok : default;

        public ErrType UnwrapErr()
            => IsErr ? union.Err : throw new InvalidOperationException();

        public ErrType UnwrapErrOr(ErrType or)
            => IsErr ? union.Err : or;

        public ErrType? UnwrapErrOrDefault()
            => IsErr ? union.Err : default;

        public static implicit operator bool(Result<OkType, ErrType> result)
            => result.IsOk;
    }
}
