// Kolobok (c) 2015 Krokodev
// Kolobok.Core
// World.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kolobok.Core.Common;
using Kolobok.Core.Diagnostics;
using Kolobok.Core.Types;
using Kolobok.Core.Utils;
using MoreLinq;

namespace Kolobok.Core.Enteties
{
    internal class World : IWorld, IComponent
    {
        #region IWorld

        private IWorld IWorld
        {
            get { return this; }
        }

        IAgent IWorld.Agent( IAgent agent )
        {
            try {
                return _agents.First( a => a.Id == agent.Id );
            }
            catch {
                throw new KolobokUnknownAgentException( "World {0} does not contain agent {1}", IWorld, agent );
            }
        }

        void IWorld.Add( params IAgent[] agents )
        {
            agents.ForEach( AddAgent );
        }

        IWorld IWorld.Clone()
        {
            return new World {
                _agents = _agents.Select( a => a.Clone() ).ToList(),
                _name = ( string ) _name.Clone()
            };
        }

        bool IWorld.Contains( IAgent agent )
        {
            return _agents.Any( a => a.Id == agent.Id );
        }

        public void Clear()
        {
            _agents.Clear();
        }

        string IWorld.GetName()
        {
            return _name;
        }

        uint IWorld.GetDepth()
        {
            return _holder == null ? Constants.BasicDepth : _holder.GetDepth() + 1;
        }

        #endregion


        #region IComponent

        private IComponent IComponent
        {
            get { return this; }
        }

        void IComponent.Init( IComposition composition ) {}

        IComponent IComponent.Clone()
        {
            return IWorld.Clone() as IComponent;
        }

        #endregion


        #region IIdentifiable

        Guid IIdentifiable.Id
        {
            get { return _id; }
        }

        #endregion


        #region IResearchable

        string IResearchable.GetDump()
        {
            var sb = new StringBuilder();
            sb.AppendFormat( "{0}\n", this );
            _agents.ForEach( a => { sb.AppendFormat( "  {0}\n", a ); } );
            return sb.ToString();
        }

        #endregion


        #region IAspect

        void IAspect.Verify()
        {
            Assert.That( _agents.AreUniqueBy( a => a.Id ), "World's {0} agents are not unique", IWorld.Id );
        }

        #endregion


        #region Ctor

        public World( IAgent holder = null, string name = null )
        {
            _holder = holder;
            _name = name ?? Constants.Worlds.Names.Default;
        }

        #endregion


        #region Overrides

        public override string ToString()
        {
            return string.Format( "{0} {{{1}}}", IWorld.GetName(), IWorld.Id );
        }

        #endregion


        #region Fields

        private List< IAgent > _agents = new List< IAgent >();
        private readonly Guid _id = Guid.NewGuid();
        private string _name;
        private IAgent _holder;

        #endregion


        #region Routines

        private void AddAgent( IAgent agent )
        {
            Assert.That( agent.Reality == null, "Agent [{0}] already belongs to the World [{1}]", agent, agent.Reality );
            Assert.That( !IWorld.Contains( agent ), "World [{1}] already contains the clone of [{0}]", agent, IWorld );

            _agents.Add( agent );
            agent.Reality = this;
        }

        #endregion
    }
}