// Robotango (c) 2015 Krokodev
// Robotango.Common
// IComponent.cs

namespace Robotango.Common.Domain.Types.Compositions
{
    public interface IComponent
    {
        IComponent Clone();
        void Init( IComposite composition );
    }
}