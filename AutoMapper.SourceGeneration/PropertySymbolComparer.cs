using Microsoft.CodeAnalysis;

namespace AutoMapper.SourceGeneration
{
    internal class PropertySymbolComparer : IEqualityComparer<IPropertySymbol>
    {
        private PropertySymbolComparer()
        {
        }

        public static PropertySymbolComparer Default { get; } = new PropertySymbolComparer();

        public bool Equals(IPropertySymbol x, IPropertySymbol y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(IPropertySymbol obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}