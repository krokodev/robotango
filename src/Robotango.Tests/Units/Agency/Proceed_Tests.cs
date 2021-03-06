﻿// Robotango (c) 2015 Krokodev
// Robotango.Tests
// Proceed_Tests.cs

using NUnit.Framework;
using Robotango.Core.Abilities.Active;
using Robotango.Core.Abilities.Desirous;
using Robotango.Core.Abilities.Thinking;
using Robotango.Core.Abilities.Virtual;
using Robotango.Core.Abilities.Virtual.Imp;
using Robotango.Core.Common;
using Robotango.Expressions.Terms;
using Robotango.Tests.Base;

namespace Robotango.Tests.Units.Agency
{
    [TestFixture]
    public class Proceed_Tests : BaseTests
    {
        [Test]
        public void Abilities_ordering_does_not_cause_on_the_proceed_success()
        {
            var world = Factory.CreateWorld();
            var alice =
                world.IReality.AddAgent( Factory.CreateAgent< IActive, IVirtual, IDesirous, IThinking >( "Alice" ) );
            var bob =
                world.IReality.AddAgent( Factory.CreateAgent< IThinking, IDesirous, IVirtual, IActive >( "Bob" ) );
            var a = new Location( "A" );
            var b = new Location( "B" );

            alice.As< IActive >().AddActivity( Lib.Activities.Movement );
            alice.As< IVirtual >().AddAttribute( new Position( a ) );
            alice.As< IActive >().AddIntention( Lib.Activities.Movement, alice, b );

            bob.As< IActive >().AddActivity( Lib.Activities.Movement );
            bob.As< IVirtual >().AddAttribute( new Position( a ) );
            bob.As< IActive >().AddIntention( Lib.Activities.Movement, bob, b );

            world.Proceed();

            Log( world.Dump() );

            Assert.That( alice.Get( Its.Virtual.Location ), Is.EqualTo( b ) );
            Assert.That( bob.Get( Its.Virtual.Location ), Is.EqualTo( b ) );
        }

        [Test]
        public void No_intentions_after_proceed()
        {
            var a = new Location( "A" );
            var b = new Location( "B" );
            var world = Factory.CreateWorld();
            var alice = world.IReality.AddAgent(
                Factory.CreateAgent< IVirtual, IThinking, IDesirous, IActive >( "Alice" )
                );
            alice.As< IActive >().AddActivity( Lib.Activities.Movement );

            alice.As< IVirtual >().AddAttribute( new Position( a ) );
            alice.As< IActive >().AddIntention( Lib.Activities.Movement, alice, b );
            alice.As< IThinking >().InnerReality.AddAgent( alice, "aAlice" );

            var dump1 = Log( world.Dump() );
            {
                world.Proceed();
            }
            var dump2 = Log( world.Dump() );

            Assert.That( dump1, Is.StringContaining( "MoveTo(Alice,B) <Intention`1>" ) );
            Assert.That( dump2, Is.Not.StringContaining( "MoveTo(Alice,B) <Intention`1>" ) );
        }
    }
}