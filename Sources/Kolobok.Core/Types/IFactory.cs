﻿// Kolobok (c) 2015 Krokodev
// Kolobok.Core
// IFactory.cs

namespace Kolobok.Core.Types
{
    public interface IFactory
    {
        IAgent CreateAgent<T1>();
        IAgent CreateAgent<T1, T2>();
        IAgent CreateAgent( params IComponent[] components );
        IComponent CreateComponent<T>();
    }
}