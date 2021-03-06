// Robotango (c) 2015 Krokodev
// Robotango.Core
// IAttribute.cs

using Robotango.Common.Types.Types;

namespace Robotango.Core.Abilities.Virtual
{
    public interface IAttribute : IResearchable
    {
        IAttribute Clone();
        IAttributeHolder Holder { get; set; }
    }
}