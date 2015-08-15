// Robotango (c) 2015 Krokodev
// Robotango.Core
// Deciding.cs

using Robotango.Common.Domain.Types.Compositions;
using Robotango.Core.Agency.Imp;

// Here: Deciding

namespace Robotango.Core.Abilities.Imp
{
    internal class Deciding : Ability, IDeciding
    {
        protected override IComponent Clone()
        {
            return new Deciding();
        }
    }
}