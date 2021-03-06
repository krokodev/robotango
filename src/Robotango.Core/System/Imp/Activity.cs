// Robotango (c) 2015 Krokodev
// Robotango.Core
// Activity.cs

using System;
using Robotango.Common.Types.Types;
using Robotango.Common.Utils.Diagnostics.Debug;
using Robotango.Common.Utils.Diagnostics.Exceptions;
using Robotango.Common.Utils.Tools;

namespace Robotango.Core.System.Imp
{
    public class Activity<T> : IActivity
    {
        #region Data

        private readonly Action< IAgent, T > _action;
        private readonly string _name;

        #endregion


        #region Ctor

        public Activity( string name, Action< IAgent, T > action )
        {
            _name = name;
            _action = action;
        }

        #endregion


        #region IResearchable

        string IResearchable.Dump( int level )
        {
            return OutlineWriter.Line(
                level,
                "{0}({1},{2}) <{3}>",
                IActivity.Name,
                typeof( IAgent ).Name,
                typeof( T ).Name,
                typeof( Activity< T > ).Name
                );
        }

        #endregion


        #region IActivity

        private IActivity IActivity {
            get { return this; }
        }

        string IActivity.Name {
            get { return _name; }
        }

        void IActivity.Execute( IAgent agent, object arg )
        {
            Debug.Assert.That( arg is T,
                new ActivityArgumentException( "Expected {0} but was {1}", typeof( T ).Name, arg.GetType().Name ) );
            _action.Invoke( agent, ( T ) arg );
        }

        #endregion
    }
}