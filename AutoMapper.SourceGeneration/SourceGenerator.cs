
using Microsoft.CodeAnalysis;

using System.Diagnostics;
using System.Text;

namespace AutoMapper.SourceGeneration;

[Generator]
public sealed class SourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    /*
     * 
     * Example Generated Source:
     * 
     *     using System;
     *     using {SOURCE_CLASS_NAMESPACE};
     *     using {TARGET_CLASS_NAMESPACE};
     *     
     *     namespace AutoMapper.GeneratedMappers;
     *     public static class {SOURCE_CLASS_NAME}Mapper
     *     {
     *         public static {TARGET_CLASS_NAME} As{TARGET_CLASS_NAME}({SOURCE_CLASS_NAME} model)
     *         {
     *             var target = new {TARGET_CLASS_NAME}();
     *             target.{PROP1} = source.{PROP1};
     *             target.{PROP2} = source.{PROP2};
     *
     *             return target;
     *         }
     *     }
     *     
     *     and, let's also generate the mapper as an extension method
     *     
     *     using System;
     *     using {SOURCE_CLASS_NAMESPACE};
     *     using {TARGET_CLASS_NAMESPACE};
     *     
     *     namespace AutoMapper.GeneratedMappers;
     *     public static class {SOURCE_CLASS_NAME}To{TARGET_CLASS_NAME}MappingExtensions
     *     {
     *         public static {TARGET_CLASS_NAME} As{TARGET_CLASS_NAME}(this {SOURCE_CLASS_NAME} model)
     *         {
     *             var target = new {TARGET_CLASS_NAME}();
     *             target.{PROP1} = source.{PROP1};
     *             target.{PROP2} = source.{PROP2};
     *
     *             return target;
     *         }
     *     }
     */

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not SyntaxReceiver syntaxReceiver)
            return;

        var sourceBuilder = new StringBuilder();

        foreach (var workItem in syntaxReceiver.WorkItems)
        {
            GenerateStaticMappingClass(sourceBuilder, workItem);
            context.AddSource($"{workItem.SourceClass.Name}To{workItem.TargetClass.Name}Mapper.cs", sourceBuilder.ToString());
            sourceBuilder.Clear();

            GenerateMappingExtensionsClass(sourceBuilder, workItem);
            context.AddSource($"{workItem.SourceClass.Name}To{workItem.TargetClass.Name}MappingExtensions.cs", sourceBuilder.ToString());
            sourceBuilder.Clear();
        }
    }

    private static void GenerateStaticMappingClass(StringBuilder sourceBuilder, WorkItem workItem)
    {
        sourceBuilder.AppendLine(@"using System;");
        sourceBuilder.AppendLine($@"using {workItem.SourceClass.ContainingNamespace.ToDisplayString()};");
        sourceBuilder.AppendLine($@"using {workItem.TargetClass.ContainingNamespace.ToDisplayString()};");
        sourceBuilder.AppendLine(@"namespace AutoMapper.GeneratedMappers;");
        sourceBuilder.AppendLine($@"public static class {workItem.SourceClass.Name}To{workItem.TargetClass.Name}Mapper");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine($@"public static {workItem.TargetClass.Name} As{workItem.TargetClass.Name}({workItem.SourceClass.Name} source)");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine($@"var target = new {workItem.TargetClass.Name}();");

        foreach (var property in workItem.PropertiesToMap)
        {
            var propertyName = property.Name;
            sourceBuilder.AppendLine($@"target.{propertyName} = source.{propertyName};");
        }

        sourceBuilder.AppendLine("return target;");
        sourceBuilder.AppendLine("}");
        sourceBuilder.AppendLine("}");
    }

    private static void GenerateMappingExtensionsClass(StringBuilder sourceBuilder, WorkItem workItem)
    {
        sourceBuilder.AppendLine(@"using System;");
        sourceBuilder.AppendLine($@"using {workItem.SourceClass.ContainingNamespace.ToDisplayString()};");
        sourceBuilder.AppendLine($@"using {workItem.TargetClass.ContainingNamespace.ToDisplayString()};");
        sourceBuilder.AppendLine(@"namespace AutoMapper.GeneratedMappingExtensions;");
        sourceBuilder.AppendLine($@"public static class {workItem.SourceClass.Name}To{workItem.TargetClass.Name}MappingExtensions");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine($@"public static {workItem.TargetClass.Name} As{workItem.TargetClass.Name}(this {workItem.SourceClass.Name} source)");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine($@"var target = new {workItem.TargetClass.Name}();");

        foreach (var property in workItem.PropertiesToMap)
        {
            var propertyName = property.Name;
            sourceBuilder.AppendLine($@"target.{propertyName} = source.{propertyName};");
        }

        sourceBuilder.AppendLine("return target;");
        sourceBuilder.AppendLine("}");
        sourceBuilder.AppendLine("}");
    }
}