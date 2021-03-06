﻿// Robotango (c) 2015 Krokodev
// Robotango.Tests
// Desirous_Tests.cs

using NUnit.Framework;
using Robotango.Core.Abilities.Desirous;
using Robotango.Core.Abilities.Desirous.Imp;
using Robotango.Core.Abilities.Thinking;
using Robotango.Core.Abilities.Virtual;
using Robotango.Core.Abilities.Virtual.Imp;
using Robotango.Tests.Base;

namespace Robotango.Tests.Units.Abilities
{
    [TestFixture]
    public class Desirous_Tests : BaseTests
    {
        [Test]
        public void Agent_can_be_Desirous()
        {
            Factory.CreateAgent< IDesirous, IThinking >();
        }

        [Test]
        public void Desirous_agent_may_be_not_Thinking()
        {
            Factory.CreateAgent< IDesirous >();
        }

        [Test]
        public void Desirous_dump_contains_desires()
        {
            var agent = Factory.CreateAgent< IDesirous, IThinking >();
            agent.As< IDesirous >().AddDesire( new EmptyDesire() );

            var dump = Log( agent.Dump() );

            Assert.That( dump, Is.StringContaining( "<DesirousAbility>" ) );
            Assert.That( dump, Is.StringContaining( "Desires" ) );
            Assert.That( dump, Is.StringContaining( "Nothing(,INothing )" ) );
        }

        [Test]
        public void Desire_can_be_satisfied()
        {
            var agent = Factory.CreateAgent< IDesirous, IThinking >();
            IDesire desire = new ExistingDesire( agent );
            agent.As< IDesirous >().AddDesire( desire );

            Log( agent.Dump() );

            Assert.False( desire.IsSatisfiedIn( agent.As< IThinking >().InnerReality ) );

            agent.As< IThinking >().InnerReality.AddAgent( agent );

            Assert.That( desire.IsSatisfiedIn( agent.As< IThinking >().InnerReality ), Is.True );
        }

        [Test]
        public void Alice_desires_to_be_in_location_B_and_is_satisfied_by_suggestion_that_she_is()
        {
            var world = Factory.CreateWorld();
            var alice = world.IReality.AddAgent( Factory.CreateAgent< IVirtual, IDesirous, IThinking >( "Alice" ) );

            var a = new Location( "A" );
            var b = new Location( "B" );

            alice.As< IVirtual >().AddAttribute( new Position( a ) );
            var herself = alice.As< IThinking >().InnerReality.AddAgent( alice );

            IDesire desire = new LocationDesire( alice, b );
            alice.As< IDesirous >().AddDesire( desire );

            Log( world.Dump() );

            alice.As< IVirtual >().GetAttribute< IPosition >().Location = b;
            Assert.False( desire.IsSatisfiedIn( alice.As< IThinking >().InnerReality ) );

            alice.As< IVirtual >().GetAttribute< IPosition >().Location = a;
            herself.As< IVirtual >().GetAttribute< IPosition >().Location = b;
            Assert.True( desire.IsSatisfiedIn( alice.As< IThinking >().InnerReality ) );
        }

        [Test]
        public void Desire_can_be_named_and_names_are_dumped()
        {
            var a = new Location( "A" );
            var agent = Factory.CreateAgent< IDesirous, IThinking >();

            agent.As< IDesirous >().AddDesire( new EmptyDesire() );
            agent.As< IDesirous >().AddDesire( new ExistingDesire( agent ) );
            agent.As< IDesirous >().AddDesire( new LocationDesire( agent, a ) );

            var dump = Log( agent.Dump() );

            Assert.That( dump, Is.StringContaining( "SomeAgent" ) );
            Assert.That( dump, Is.StringContaining( "Nothing(,INothing )" ) );
            Assert.That( dump, Is.StringContaining( "Existing(SomeAgent,INothing )" ) );
            Assert.That( dump, Is.StringContaining( "InLocation(SomeAgent,ILocation A)" ) );
        }
    }
}