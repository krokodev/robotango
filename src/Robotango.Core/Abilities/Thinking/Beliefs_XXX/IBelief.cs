// Robotango (c) 2015 Krokodev
// Robotango.Core
// IBelief.cs

using System;
using Robotango.Core.System;

namespace Robotango.Core.Abilities.Thinking.Beliefs_XXX
{
    public interface IBelief
    {
        Action< IReality > Essence { get; }
    }
}