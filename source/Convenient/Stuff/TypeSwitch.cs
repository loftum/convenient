using System;
using System.Collections.Generic;
using System.Linq;

namespace Convenient.Stuff
{
    public class TypeSwitch
    {
        private readonly object _item;

        public TypeSwitch(object item)
        {
            _item = item;
        }

        public static TypeSwitch OnA(object item)
        {
            return new TypeSwitch(item);
        }

        public TRet Return<TRet>(Action<TypeSwitch<TRet>> action)
        {
            var s = new TypeSwitch<TRet>();
            action(s);
            return s.Execute(_item);
        }

        public static TRet On<TRet>(object value, Action<TypeSwitch<TRet>> action)
        {
            var s = new TypeSwitch<TRet>();
            action(s);
            return s.Execute(value);
        }
    }

    public class TypeSwitch<TRet>
    {
        private readonly IList<TypeCase<TRet>> _cases = new List<TypeCase<TRet>>();
        private Func<TRet> _default = () => default(TRet);

        public TypeSwitch<TRet> Case<T>(Func<T, TRet> func)
        {
            _cases.Add(new TypeCase<T, TRet>(func));
            return this;
        }

        public TRet Execute(object item)
        {
            var typeCase = _cases.FirstOrDefault(c => c.CanExecute(item));
            return typeCase == null
                ? _default()
                : typeCase.Execute(item);
        }

        public TypeSwitch<TRet> Default(Func<TRet> func)
        {
            _default = func;
            return this;
        }

        public TypeSwitch<TRet> Default(TRet value)
        {
            _default = () => value;
            return this;
        }
    }
}