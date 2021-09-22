using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoMapper.SourceGeneration
{
    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        private const string MappingAttributeName = "MapsToAttribute";

        private readonly List<WorkItem> workItems;

        public IReadOnlyList<WorkItem> WorkItems
            => this.workItems.AsReadOnly();

        public SyntaxReceiver()
        {
            this.workItems = new List<WorkItem>();
        }

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not ClassDeclarationSyntax sourceClassDeclarationSyntax)
                return;

            if (context.SemanticModel.GetDeclaredSymbol(sourceClassDeclarationSyntax) is not INamedTypeSymbol sourceClassTypeSymbol)
                return;

            var mapsToAttribute = sourceClassTypeSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == MappingAttributeName);

            if (mapsToAttribute?.ConstructorArguments[0].Value is not INamedTypeSymbol targetClassTypeSymbol)
                return;

            var sourceClassProperties = sourceClassTypeSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(property => property.DeclaredAccessibility == Accessibility.Public);

            var targetClassProperties = targetClassTypeSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(property => property.DeclaredAccessibility == Accessibility.Public);

            var propertiesToMap = sourceClassProperties.Intersect(targetClassProperties, PropertySymbolComparer.Default);

            this.workItems.Add(new WorkItem(sourceClassTypeSymbol, targetClassTypeSymbol, propertiesToMap));
        }
    }
}