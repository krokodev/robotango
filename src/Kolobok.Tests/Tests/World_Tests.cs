﻿// Kolobok (c) 2015 Krokodev
// Kolobok.Tests
// World_Tests.cs

using Kolobok.Core.Common;
using Kolobok.Core.Diagnostics;
using Kolobok.Core.Types;
using Kolobok.Utils;
using NUnit.Framework;

namespace Kolobok.Tests
{
    [TestFixture]
    public class World_Tests : BaseTests
    {
        [Test, ExpectedException( typeof( KolobokException ) )]
        public void Agent_belongs_only_one_world()
        {
            var agent = Factory.CreateAgent< IRational >();
            var world = Factory.CreateAgent< IWorld >();
            var image = agent.As< IRational >().Imaginary;

            world.As< IWorld >().Add( agent );
            image.Add( agent );
        }

        [Test, ExpectedException( typeof( KolobokException ) )]
        public void World_contains_each_agent_only_once()
        {
            var agent = Factory.CreateAgent< IRational >();
            var world = Factory.CreateAgent< IWorld >();

            world.As< IWorld >().Add( agent );
            world.As< IWorld >().Add( agent );
        }

        [Test, ExpectedException( typeof( KolobokException ) )]
        public void World_contains_only_unique_agents()
        {
            var agent = Factory.CreateAgent< IRational >();
            var world = Factory.CreateAgent< IWorld >();

            world.As< IWorld >().Add( agent.Clone() );
            world.As< IWorld >().Add( agent.Clone() );
        }

        [Test]
        public void Cloned_worlds_have_different_id()
        {
            var w1 = Factory.CreateAgent< IWorld >().As< IWorld >();
            var w2 = w1.Clone();
            Log( "{0}\n{1}", w1.Id, w2.Id );
            Assert.AreNotEqual( w1.Id, w2.Id );
        }

        [Test]
        public void Cloned_worlds_have_the_same_name()
        {
            var w1 = Factory.CreateAgent< IRational >().As< IRational >().Imaginary;
            var w2 = w1.Clone();
            Log( "{0}\n{1}", w1.GetName(), w2.GetName() );
            Assert.AreEqual( w1.GetName(), w2.GetName() );
        }

        [Test]
        public void Imaginary_agent_has_the_same_id()
        {
            var alice = Factory.CreateAgent< IRational, IOwner >();
            alice.As< IRational >().Believes( world => world.Add( alice.Clone() ) );
            alice.As< IRational >().Think();
            Log( alice );
            Log( alice.As< IRational >().Imaginary.Agent( alice ) );
            Assert.AreEqual( alice.Id, alice.As< IRational >().Imaginary.Agent( alice ).Id );
        }

        [Test]
        public void Imaginary_agent_has_the_same_name()
        {
            const string name = "Alice";
            var alice = Factory.CreateAgent< IRational, IOwner >( name );
            alice.As< IRational >().Believes( world => world.Add( alice.Clone() ) );
            alice.As< IRational >().Think();
            Log( alice.Name );
            Log( alice.As< IRational >().Imaginary.Agent( alice ).Name );
            Assert.AreEqual( name, alice.Name );
            Assert.AreEqual( alice.Name, alice.As< IRational >().Imaginary.Agent( alice ).Name );
        }

        [Test]
        public void World_has_default_name()
        {
            var world = Factory.CreateAgent< IWorld >().As< IWorld >();
            Log( world );
            Assert.AreEqual( Constants.Worlds.Names.Default, world.GetName() );
        }

        [Test]
        public void World_has_depth()
        {
            var world = Factory.CreateAgent< IWorld >().As< IWorld >();
            Assert.AreEqual( Constants.Depth.Basic, world.GetDepth() );
        }

        [Test]
        public void World_agents_have_the_same_depts()
        {
            var matrix = Factory.CreateAgent< IRational >( "Matrix" );
            var alice = Factory.CreateAgent< IRational >( "Alice" );
            var bob = Factory.CreateAgent< IRational >( "Bob" );

            matrix.As< IRational >().Imaginary.Add( alice );
            matrix.As< IRational >().Imaginary.Add( bob );

            Assert.AreEqual( matrix.GetDepth() + 1, matrix.As< IRational >().Imaginary.GetDepth() );
            Assert.AreEqual( matrix.As< IRational >().Imaginary.GetDepth(), alice.GetDepth() );
            Assert.AreEqual( matrix.As< IRational >().Imaginary.GetDepth(), bob.GetDepth() );
        }

        [Ignore]
        [Test]
        public void Agents_Imaginary_world_has_proper_name()
        {
            var alice = Factory.CreateAgent< IRational >( "Alice" );
            var aworld = alice.As< IRational >().Imaginary;
            Log( aworld.GetName() );
            Assert.AreEqual(
                string.Format(
                    "{0}|{1}.{2}",
                    alice.Name,
                    alice.GetDepth(),
                    Constants.Worlds.Names.Imaginary
                    ),
                aworld.GetName() );
        }

        [Ignore]
        [Test]
        public void Worlds_name_represents_its_position_in_hierarchy()
        {
            var universe = Factory.CreateAgent< IWorld >( "Universe" );
            var alice = Factory.CreateAgent< IRational >( "Alice" );
            var bob = Factory.CreateAgent< IRational >( "Bob" );

            universe.As< IWorld >().Add( alice );
            alice.As< IRational >().Believes( iworld => iworld.Add( bob.Clone() ) );
            alice.As< IRational >().Think();
            alice.As< IRational >().Imaginary.Agent( bob ).As< IRational >().Believes( iworld => iworld.Add( alice.Clone() ) );
            alice.As< IRational >().Imaginary.Agent( bob ).As< IRational >().Think();

            var uabaWorld =
                universe.As< IWorld >()
                    .Agent( alice ).As< IRational >().Imaginary
                    .Agent( bob ).As< IRational >().Imaginary
                    .Agent( alice ).As< IRational >().Imaginary;

            // FullName: Universe[Alice].Imaginary[Bob].Imaginary[Alice].Imaginary
            // Name: Alice|3.Imaginary
            Log( uabaWorld );

            Assert.AreNotEqual( Constants.Worlds.Names.Default, uabaWorld.GetName() );
            Assert.AreEqual( "Alice|3.Imaginary", uabaWorld.GetName() );
        }

        [Test]
        public void Worlds_full_name_describes_its_hierarchy()
        {
            Assert.Ignore();
        }
    }
}