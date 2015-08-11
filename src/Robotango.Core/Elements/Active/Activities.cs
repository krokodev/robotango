// Robotango (c) 2015 Krokodev
// Robotango.Core
// Activities.cs

using Robotango.Common.Domain.Types.Properties;
using Robotango.Core.Elements.Virtual;
using Robotango.Core.Interfaces.Abilities;

namespace Robotango.Core.Elements.Active
{
    public static class Activities
    {
        public static class Virtual
        {
            public static readonly IActivity Move = new Activity< ILocation >(
                "MoveTo",
                ( agent, location ) => {
                    agent
                        .As< IVirtual >()
                        .GetAttribute< IPosition >()
                        .Location = location;
                } );
        }
    }
}