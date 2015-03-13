using System;

namespace Convenient.Stuff
{
    public abstract class TypeCase<TRet>
    {
        public abstract bool CanExecute(object item);
        public abstract TRet Execute(object item);
    }

    public class TypeCase<T, TRet> : TypeCase<TRet>
    {
        private readonly Func<T, TRet> _func;

        public TypeCase(Func<T, TRet> func)
        {
            _func = func;
        }

        public override bool CanExecute(object item)
        {
            return item is T;
        }

        public override TRet Execute(object item)
        {
            return Execute((T) item);
        }

        public TRet Execute(T item)
        {
            return _func(item);
        }
    }
}