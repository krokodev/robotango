﻿// Kolobok (c) 2015 Krokodev
// Kolobok.Tests
// Social_Tests.cs

using System.Linq;
using Kolobok.Core.Types;
using Kolobok.Stuff;
using Kolobok.Utils;
using NUnit.Framework;

namespace Kolobok.Tests
{
    [Ignore]
    [TestFixture]
    public class Social_Tests : BaseTests
    {
        [Test]
        public void Wise_agent_can_solve_color_of_its_hat_during_conversation()
        {
            const Hat.Colors aColor = Hat.Colors.White;
            const Hat.Colors bColor = Hat.Colors.Black;

            var w = Factory.CreateAgent< IWorld >();
            var a = Factory.CreateAgent< IRational, ISocial, IReflective, IComposition >();
            var b = Factory.CreateAgent< IRational, ISocial, IReflective, IComposition >();

            w.As< IWorld >().Contains( a, b );

            a.As< IComposition >().Has( new Hat() );
            b.As< IComposition >().Has( new Hat() );

            a.As< IComposition >().Get< Hat >().Color = aColor;
            b.As< IComposition >().Get< Hat >().Color = bColor;


            a.As< IRational >().Believes( world => world.Contains( a, b ));
            a.As< IRational >().Believes( world => world.Agent( a ).As< IComposition >().Get< Hat >().Color = Hat.Colors.Unknown );
            a.As< IRational >().Believes( world => world.Agent( b ).As< IComposition >().Get< Hat >().Color = bColor );

            b.As< IRational >().Believes( world => world.Contains( a, b ));
            b.As< IRational >().Believes( world => world.Agent( b ).As< IComposition >().Get< Hat >().Color = Hat.Colors.Unknown );
            b.As< IRational >().Believes( world => world.Agent( a ).As< IComposition >().Get< Hat >().Color = aColor );

            Assert.That(
                a.As< ISocial >()
                    .Replies< Hat.Colors >( world =>
                        world.Agent( a ).As< IComposition >().Get< Hat >().Color
                    )
                    == Hat.Colors.Unknown
                );

            // Some iterations
            foreach( var i in Enumerable.Range( 0, 10 ) ) {
                a.As< IRational >().Think();
                b.As< IRational >().Think();

                // a ask b about b's hat color
                a.As< IRational >().Believes( aWorld =>
                    aWorld.Agent( b ).As< IRational >().Believes( bWorld =>
                        bWorld.Agent( b ).As< IComposition >().Get< Hat >().Color
                            = b.As< ISocial >()
                                .Replies< Hat.Colors >( world =>
                                    world.Agent( b ).As< IComposition >().Get< Hat >().Color
                                )
                        )
                    );

                // ...
            }

            Assert.That(
                a.As< ISocial >()
                    .Replies< Hat.Colors >( world =>
                        world.Agent( a ).As< IComposition >().Get< Hat >().Color
                    )
                    == Hat.Colors.Black
                );
        }
    }
}