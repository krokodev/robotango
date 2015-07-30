// Kolobok (c) 2015 Krokodev
// Kolobok.Core
// Owner.cs

using System.Collections.Generic;
using System.Linq;
using Kolobok.Core.Diagnostics;
using Kolobok.Core.Types;

namespace Kolobok.Core.Items
{
    internal class Owner : IOwner, IComponent
    {
        #region IComponent

        IComponent IComponent.Clone()
        {
            return new Owner {
                _properties = _properties.Select( p => p.Clone() ).ToList()
            };
        }

        #endregion


        #region IOwner

        IOwner IOwner { get { return this; } }
        void IOwner.Has( IProperty property )
        {
            Assert.That( property.Owner == null, "Property already belongs to other owner");
            _properties.Add( property );
            property.Owner = this;
        }

        T IOwner.GetFirst<T>()
        {
            return _properties.OfType< T >().First();
        }

        #endregion


        #region Fields

        private List< IProperty > _properties = new List< IProperty >();

        #endregion
    }
}