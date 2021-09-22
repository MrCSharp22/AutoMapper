
using Microsoft.CodeAnalysis;

namespace AutoMapper.SourceGeneration
{
    internal class WorkItem
    {
        public INamedTypeSymbol SourceClass { get; }

        public INamedTypeSymbol TargetClass { get; }

        public IReadOnlyList<IPropertySymbol> PropertiesToMap {  get; }

        public WorkItem(INamedTypeSymbol sourceClass, 
            INamedTypeSymbol targetClass, 
            IEnumerable<IPropertySymbol> propertiesToMap)
        {
            this.SourceClass = sourceClass;
            this.TargetClass = targetClass;

            this.PropertiesToMap = propertiesToMap
                .ToList()
                .AsReadOnly();
        }
    }
}
