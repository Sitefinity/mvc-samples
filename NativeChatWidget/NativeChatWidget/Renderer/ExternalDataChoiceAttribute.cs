using Progress.Sitefinity.Renderer.Designers.Attributes;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NativeChatWidget.Renderer
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DataContract(Name = "Choice")]
    internal class ExternalDataChoiceAttribute : ChoiceAttribute
    {
    }
}
