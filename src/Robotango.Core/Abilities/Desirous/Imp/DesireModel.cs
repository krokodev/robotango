// Robotango (c) 2015 Krokodev
// Robotango.Core
// DesireModel.cs

using System;
using Robotango.Core.System;

namespace Robotango.Core.Abilities.Desirous.Imp
{
    public class DesireModel<T> : IDesireModel< T >
    {
        #region Data

        private readonly string _name;
        private readonly Func< IReality, IAgent, T, bool > _predicate;

        #endregion


        #region Ctor

        public DesireModel( string name, Func< IReality, IAgent, T, bool > predicate )
        {
            _name = name;
            _predicate = predicate;
        }

        #endregion


        #region IDesireModel

        Func< IReality, IAgent, T, bool > IDesireModel< T >.Predicate {
            get { return _predicate; }
        }

        string IDesireModel< T >.Name {
            get { return _name; }
        }

        #endregion
    }
}