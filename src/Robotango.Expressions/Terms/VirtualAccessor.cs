// Robotango (c) 2015 Krokodev
// Robotango.Expressions
// VirtualAccessor.cs

using Robotango.Common.Types.Expressions;
using Robotango.Common.Types.Types;
using Robotango.Core.Abilities.Virtual;
using Robotango.Core.Abilities.Virtual.Imp;
using Robotango.Core.System;

namespace Robotango.Expressions.Terms
{
    public class VirtualAccessor : AgentProxy< IVirtual >, IVirtualAccessor
    {
        #region IVirtualAccessor

        IPropertyAccessor< IAgent, IPosition > IVirtualAccessor.Position {
            get { return new PropertyAccessor< IAgent, IPosition >( GetPosition, SetPosition ); }
        }

        IPropertyAccessor< IAgent, ILocation > IVirtualAccessor.Location {
            get { return new PropertyAccessor< IAgent, ILocation >( GetLocation, SetLocation ); }
        }

        #endregion


        #region Ctor

        public VirtualAccessor()
            : base( Terms.Convert.Agent.To.Virtual ) {}

        #endregion


        #region Routines

        private void SetPosition( IAgent agent, IPosition position )
        {
            Convert( agent ).AddAttribute( position );
        }

        private IPosition GetPosition( IAgent agent )
        {
            return Convert( agent ).GetAttribute< IPosition >();
        }

        private ILocation GetLocation( IAgent agent )
        {
            return Convert( agent ).GetAttribute< IPosition >().Location;
        }

        private void SetLocation( IAgent agent, ILocation location )
        {
            Convert( agent ).SetAttributeTo< Position, ILocation >(
                ( p, l ) => SetLocation( p, l ),
                location
                );
        }

        private static ILocation SetLocation( Position p, ILocation l )
        {
            return p.IPosition.Location = l;
        }

        #endregion
    }
}