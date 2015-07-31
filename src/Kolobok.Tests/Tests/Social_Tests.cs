﻿// Kolobok (c) 2015 Krokodev
// Kolobok.Tests
// Social_Tests.cs

using Kolobok.Core.Types;
using Kolobok.Stuff;
using Kolobok.Utils;
using NUnit.Framework;

namespace Kolobok.Tests
{
    [TestFixture]
    public class Social_Tests : BaseTests
    {
        [Test]
        public void Social_can_query_question()
        {
            var alice = Factory.CreateAgent< IOwner, ISocial >();
            var question = alice.As< ISocial >().Ask< Colors >( world => world.Agent( alice ).As< IOwner >().GetFirst< IHat >().Color );

            Assert.AreEqual( alice.As< ISocial >(), question.Querist );
            Assert.NotNull( question.Querist );
        }

        [Test]
        public void Social_can_reply_answer()
        {
            var alice = Factory.CreateAgent< IOwner, ISocial >();
            var bob = Factory.CreateAgent< IOwner, ISocial >();
            var question = alice.As< ISocial >().Ask< Colors >( world => world.Agent( alice ).As< IOwner >().GetFirst< IHat >().Color );
            var answer = bob.As< ISocial >().Reply< Colors >( question );

            Assert.AreEqual( answer.Question, question );
            Assert.AreEqual( answer.Question.Querist, question.Querist );
            Assert.AreEqual( answer.Question.Querist, alice.As< ISocial >() );
            Assert.AreEqual( answer.Respondent, bob.As< ISocial >() );
            Assert.IsFalse( answer.Result.IsVaild );
        }

        [Test]
        public void Non_Rationals_answer_is_invalid()
        {
            var alice = Factory.CreateAgent< ISocial >();
            var bob = Factory.CreateAgent< ISocial >();

            var question = alice.As< ISocial >().Ask< bool >( world => world.Agent( bob ) != null );
            var answer = bob.As< ISocial >().Reply< bool >( question );

            Log( answer.Result.Exception );

            Assert.IsFalse( answer.Result.IsVaild );
        }

        [Test]
        public void Bob_does_not_know_question_theme_so_answer_is_invalid()
        {
            var alice = Factory.CreateAgent< ISocial >();
            var bob = Factory.CreateAgent< ISocial, IRational >();

            var question = alice.As< ISocial >().Ask< bool >( world => world.Agent( bob ) != null );
            var answer = bob.As< ISocial >().Reply< bool >( question );

            Log( answer.Result.Exception );

            Assert.IsFalse( answer.Result.IsVaild );
        }

        [Test]
        public void Bob_answers_about_Alicas_hat_color()
        {
            var alice = Factory.CreateAgent< IOwner, ISocial >();
            var bob = Factory.CreateAgent< ISocial, IRational >();

            alice.As< IOwner >().Has( new Hat() );
            alice.As< IOwner >().GetFirst< IHat >().Color = Colors.Red;

            bob.As< IRational >().Believes( world => { world.Add( alice ); } );
            bob.As< IRational >().Think();

            var question = alice.As< ISocial >().Ask< Colors >( world => world.Agent( alice ).As< IOwner >().GetFirst< IHat >().Color );
            var answer = bob.As< ISocial >().Reply< Colors >( question );

            Log( answer.Result.Value );

            Assert.AreEqual( Colors.Red, answer.Result.Value );
            Assert.IsTrue( answer.Result.IsVaild );
        }

        [Test]
        public void Bob_answers_according_his_beliefes()
        {
            var alice = Factory.CreateAgent< IOwner, ISocial >();
            var bob = Factory.CreateAgent< ISocial, IRational >();

            alice.As< IOwner >().Has( new Hat() );
            alice.As< IOwner >().GetFirst< IHat >().Color = Colors.Red;

            bob.As< IRational >().Believes( world => {
                var alicaImage = alice.Clone();
                world.Add( alicaImage );
                alicaImage.As< IOwner >().Has( new Hat() );
                alicaImage.As< IOwner >().GetFirst< IHat >().Color = Colors.Black;
            } );
            bob.As< IRational >().Think();

            var question = alice.As< ISocial >().Ask< Colors >( world => world.Agent( alice ).As< IOwner >().GetFirst< IHat >().Color );
            var answer = bob.As< ISocial >().Reply< Colors >( question );

            Log( answer.Result.Value );

            Assert.AreEqual( Colors.Black, answer.Result.Value );
        }
    }
}