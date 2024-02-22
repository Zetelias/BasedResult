using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BasedResult
{
    /// <summary>
    /// A "struct" that's really an equivalent to C++'s union that can either be type OkType or type ErrType
    /// It is 
    /// </summary>
    /// <typeparam name="OkType"></typeparam>
    /// <typeparam name="ErrType"></typeparam>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ResultUnion<OkType, ErrType>
    {
        [FieldOffset(0)]
        internal OkType Ok;
        [FieldOffset(0)]
        internal ErrType Err;

        public bool IsOk()
            => !EqualityComparer<OkType>.Default.Equals(Ok, default);

        public bool IsErr()
            => !IsOk();

        public ResultUnion(OkType ok) : this()
        {
            Ok = ok;
        }

        public ResultUnion(ErrType err) : this()
        {
            Err = err;
        }

        public static implicit operator ResultUnion<OkType, ErrType>(OkType ok)
        {
            return new ResultUnion<OkType, ErrType> { Ok = ok };
        }

        public static implicit operator ResultUnion<OkType, ErrType>(ErrType err)
        {
            return new ResultUnion<OkType, ErrType> { Err = err };
        }
    }
}
