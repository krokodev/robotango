﻿// Robotango (c) 2015 Krokodev
// Robotango.Core
// Attribute.cs

using Robotango.Common.Domain.Types.Properties;
using Robotango.Common.Utils.Tools;

namespace Robotango.Core.Elements.Virtual
{
    public abstract class Attribute<T> : IAttribute
        where T : IAttribute, new()
    {
        #region IAttribute

        IAttributeHolder IAttribute.Holder { get; set; }

        IAttribute IAttribute.Clone()
        {
            return Clone();
        }

        #endregion


        #region IResearchable

        string IResearchable.Dump( int level )
        {
            return Dump( level );
        }

        #endregion


        #region Overrides

        protected virtual string Dump( int level )
        {
            return OutlineWriter.Line(
                level,
                "{0}=[{2}] <{1}>",
                typeof( T ).Name,
                typeof( Attribute< T > ).Name,
                GetDumpContent() );
        }

        protected virtual IAttribute Clone()
        {
            return new T();
        }

        protected virtual string GetDumpContent()
        {
            return "";
        }

        #endregion
    }
}