﻿// Robotango (c) 2015 Krokodev
// Robotango.Common
// IComposite.cs

namespace Robotango.Common.Types.Compositions
{
    public interface IComposite
    {
        T GetComponent<T>() where T : IComponent;
        bool HasComponent<T>() where T : IComponent;
    }
}