namespace AutoMapper.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class MapsToAttribute : Attribute
{
    public Type TargetType { get; }

    public MapsToAttribute(Type targetType)
    {
        TargetType = targetType 
            ?? throw new ArgumentNullException(nameof(targetType));
    }
}