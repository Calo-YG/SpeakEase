using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SpeakEase.FastEndpoint.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpeakEase.FastEndpoint
{
    [Generator(LanguageNames.CSharp)]
    public class FastEndpointSourceGenrate : IIncrementalGenerator
    {
        private const string FastEndpointTypeName = "FastEndpoint";
        private const string EndPointSuffix = "Endpoint";
        private const string AsyncSuffix = "Async";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // 获取编译对象
            var compilationProvider = context.CompilationProvider;

            // 收集当前项目的类信息
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: IsCandidateClass,
                    transform: GetSemanticTargetForGeneration)
                .Where(m => m != null)
                .Collect();

            // 注册源输出
            context.RegisterSourceOutput(
                compilationProvider.Combine(classDeclarations),
                (spc, source) =>
                {
                    var (compilation, classes) = source;

                    // 获取引用的程序集中的类型
                    var referencedClasses = GetReferencedClasses(compilation);

                    // 将当前项目和引用程序集的类合并
                    var allClasses = classes.AddRange(referencedClasses);

                    GenerateSource(spc, compilation, allClasses);
                });
        }

        private void GenerateSource(SourceProductionContext context, Compilation compilation, ImmutableArray<FastEndpointInfo> classes)
        {
            var classInfos = classes.Where(c => c != null).Select(c => c!).ToList();

            if (!classInfos.Any())
                return;

            // 生成 DI 注册代码
            var diRegistration = GenerateDiRegistration(classInfos);

            // 生成 API 映射代码
            var apiMappings = GenerateApiMappings(classInfos, compilation, context.CancellationToken);

            // 构建最终的源代码
            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine("// 这是由ZService.Analyzers自动生成的");
            sourceBuilder.AppendLine("using Microsoft.AspNetCore.Builder;");
            sourceBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sourceBuilder.AppendLine("using Microsoft.AspNetCore.Authorization;");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("namespace Microsoft.Extensions.DependencyInjection");
            sourceBuilder.AppendLine("{");
            sourceBuilder.AppendLine("    public static class FastedpointExtensions");
            sourceBuilder.AppendLine("    {");
            sourceBuilder.AppendLine(
                "        public static IServiceCollection AddFastedpoint(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine(diRegistration);
            sourceBuilder.AppendLine("            return services;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public static WebApplication MapFastedpoint(this WebApplication app)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine(apiMappings);
            sourceBuilder.AppendLine("            return app;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine("    }");
            sourceBuilder.AppendLine("}");

            // 添加生成的源代码
            context.AddSource("FastedpointExtensions.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        private static FastEndpointInfo GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;
            var model = context.SemanticModel;

            // 获取类的符号信息
            if (model.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol)
                return null;

            if (!InheritsFromFantasyService(classSymbol))
                return null;

            // 获取命名空间
            var namespaceName = GetFullNamespace(classSymbol.ContainingNamespace);
            // 获取类名
            var className = classSymbol.Name;

            // 获取类级别的特性
            var attributes = classSymbol.GetAttributes();

            // 获取 RouteAttribute
            var routeAttr = attributes.FirstOrDefault(a => a.AttributeClass?.Name == "RouteAttribute");

            var route = routeAttr?.ConstructorArguments.FirstOrDefault().Value as string ??
                        $"/api/{className.TrimEnd(EndPointSuffix)}";

            // 获取 Tags 属性
            var tagsAttr = attributes.FirstOrDefault(a => a.AttributeClass?.Name == "Tags");

            var tags = tagsAttr?.ConstructorArguments.FirstOrDefault().Value as string;

            // 获取 FilterAttributes
            var filterAttributes = attributes.Where(a =>
                a.AttributeClass?.Name == "FilterAttribute" || a.AttributeClass?.Name == "Filter").ToArray();

            var authorizeAttributes = attributes.Where(a =>
                a.AttributeClass?.Name?.EndsWith("AuthorizeAttribute") ?? false).ToArray();

            //获取summaryattribute
            var summaryAttributes = attributes.Where(a =>
                a.AttributeClass?.Name?.EndsWith("SummaryAttribute") ?? false).ToArray();


            // 获取所有公共非静态方法
            var methods = classSymbol.GetMembers().OfType<IMethodSymbol>()
                .Where(m => m.DeclaredAccessibility == Accessibility.Public &&
                           !m.IsStatic &&
                            m.MethodKind == MethodKind.Ordinary)
                .ToArray();

            // 获取继承的接口并匹配名称
            var interfaces = classSymbol.AllInterfaces
                .Where(i => i.Name.StartsWith("I") && i.Name.Substring(1) == className)
                .FirstOrDefault();

            var interFacesNamespace = GetFullNamespace(interfaces?.ContainingNamespace);

            return new FastEndpointInfo
            {
                Namespace = namespaceName,
                ClassName = className,
                InterfaceName = interfaces?.Name,
                InterfaceNameSpace = interFacesNamespace,
                Route = route,
                AuthorizationAttribute = authorizeAttributes?[0],
                Tags = tags,
                FilterAttribute = filterAttributes,
                Methods = methods,
                SummaryAttribute = summaryAttributes?[0]
            };
        }


        private static bool IsCandidateClass(SyntaxNode node, CancellationToken _)
        {
            return node is ClassDeclarationSyntax { BaseList: not null };
        }

        private static bool InheritsFromFantasyService(INamedTypeSymbol typeSymbol)
        {
            var baseType = typeSymbol.BaseType;
            while (baseType != null)
            {
                if (baseType.Name == FastEndpointTypeName)
                    return true;
                baseType = baseType.BaseType;
            }

            return false;
        }

        private static string GetFullNamespace(INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol == null || namespaceSymbol.IsGlobalNamespace)
                return string.Empty;

            var parts = new Stack<string>();
            var current = namespaceSymbol;
            while (current is { IsGlobalNamespace: false })
            {
                parts.Push(current.Name);
                current = current.ContainingNamespace;
            }

            return string.Join(".", parts);
        }

        /// <summary>
        /// Fastpoint 信息
        /// </summary>

        class FastEndpointInfo
        {
            /// <summary>
            /// 命名空间名称
            /// </summary>
            public string Namespace { get; set; }

            /// <summary>
            /// 类名
            /// </summary>
            public string ClassName { get; set; }

            /// <summary>
            /// 接口名称
            /// </summary>
            public string InterfaceName { get; set; }

            /// <summary>
            /// 接口命名空间名称
            /// </summary>
            public string InterfaceNameSpace { get; set; }

            /// <summary>
            /// 路由
            /// </summary>
            public string Route { get; set; }

            /// <summary>
            /// tag
            /// </summary>
            public string Tags { get; set; }

            /// <summary>
            /// 授权
            /// </summary>
            public AttributeData AuthorizationAttribute { get; set; }

            /// <summary>
            /// 过滤器属性
            /// </summary>
            public AttributeData[] FilterAttribute { get; set; }

            /// <summary>
            /// 路由
            /// </summary>
            public AttributeData RouteAttribute { get; set; }

            /// <summary>
            /// tag
            /// </summary>
            public AttributeData TagAttribute { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public AttributeData SummaryAttribute { get; set; }

            /// <summary>
            /// 你命
            /// </summary>
            public AttributeData AllowAnonymous { get; set; }

            /// <summary>
            /// 方法
            /// </summary>
            public IMethodSymbol[] Methods { get; set; }
        }

        private ImmutableArray<FastEndpointInfo> GetReferencedClasses(Compilation compilation)
        {
            var builder = ImmutableArray.CreateBuilder<FastEndpointInfo>();

            foreach (var reference in compilation.References)
            {
                var symbol = compilation.GetAssemblyOrModuleSymbol(reference);
                if (symbol is not IAssemblySymbol assemblySymbol)
                    continue;

                // 过滤掉微软的 System 程序集
                if (assemblySymbol.Name.StartsWith("System", StringComparison.OrdinalIgnoreCase) ||
                    assemblySymbol.Name.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                foreach (var type in assemblySymbol.GlobalNamespace.GetNamespaceTypes())
                {
                    // 检查类型是否继承自 FantasyService
                    if (InheritsFromFantasyService(type))
                    {
                        var classInfo = GetClassInfo(type);
                        if (classInfo != null)
                        {
                            builder.Add(classInfo);
                        }
                    }
                }
            }

            return builder.ToImmutable();
        }

        private static string GenerateDiRegistration(List<FastEndpointInfo> classInfos)
        {
            var sb = new StringBuilder();

            // 将类信息按命名空间分组
            var classInfosByNamespace = classInfos.GroupBy(c => c.Namespace);

            sb.AppendLine("            switch (lifetime)");
            sb.AppendLine("            {");
            sb.AppendLine("                case ServiceLifetime.Singleton:");
            var infosByNamespace = classInfosByNamespace as IGrouping<string, FastEndpointInfo>[] ?? classInfosByNamespace.ToArray();
            foreach (var group in infosByNamespace)
            {
                if (!string.IsNullOrEmpty(group.Key))
                {
                    sb.AppendLine($"                    // {group.Key}");
                }

                foreach (var classInfo in group)
                {
                    if (string.IsNullOrWhiteSpace(classInfo.InterfaceName))
                    {
                        sb.AppendLine($"                    services.AddSingleton<{classInfo.Namespace}.{classInfo.ClassName}>();");
                        continue;
                    }
                    sb.AppendLine($"                    services.AddSingleton<{classInfo.InterfaceNameSpace}.{classInfo.InterfaceName}, {classInfo.Namespace}.{classInfo.ClassName}>();");
                }
            }
            sb.AppendLine("                    break;");

            sb.AppendLine("                case ServiceLifetime.Scoped:");
            foreach (var group in infosByNamespace)
            {
                if (!string.IsNullOrEmpty(group.Key))
                {
                    sb.AppendLine($"                    // {group.Key}");
                }

                foreach (var classInfo in group)
                {
                    if (string.IsNullOrWhiteSpace(classInfo.InterfaceName))
                    {
                        sb.AppendLine($"                    services.AddScoped<{classInfo.Namespace}.{classInfo.ClassName}>();");
                        continue;
                    }
                    sb.AppendLine($"                    services.AddScoped<{classInfo.InterfaceNameSpace} . {classInfo.InterfaceName}, {classInfo.Namespace}.{classInfo.ClassName}>();");
                }
            }
            sb.AppendLine("                    break;");

            sb.AppendLine("                case ServiceLifetime.Transient:");
            foreach (var group in infosByNamespace)
            {
                if (!string.IsNullOrEmpty(group.Key))
                {
                    sb.AppendLine($"                    // {group.Key}");
                }

                foreach (var classInfo in group)
                {
                    if (string.IsNullOrWhiteSpace(classInfo.InterfaceName))
                    {
                        sb.AppendLine($"                    services.AddTransient<{classInfo.Namespace}.{classInfo.ClassName}>();");
                        continue;
                    }
                    sb.AppendLine($"                    services.AddTransient<{classInfo.InterfaceNameSpace} . {classInfo.InterfaceName}, {classInfo.Namespace}.{classInfo.ClassName}>();");
                }
            }
            sb.AppendLine("                    break;");

            sb.AppendLine("            }");

            return sb.ToString();
        }

        private string GenerateApiMappings(List<FastEndpointInfo> classInfos, Compilation compilation,
            CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();

            // 按命名空间分组
            var classInfosByNamespace = classInfos.GroupBy(c => c.Namespace);

            foreach (var group in classInfosByNamespace)
            {
                if (!string.IsNullOrEmpty(group.Key))
                {
                    sb.AppendLine($"            // {group.Key}");
                }

                foreach (var classInfo in group)
                {
                    // 确定服务实例名称
                    var instanceName = RemoveServiceSuffixAndLowercaseFirstLetter(classInfo.ClassName);

                    sb.AppendLine(
                        $"            var {instanceName} = app.MapGroup(\"{classInfo.Route}\"){GenerateClassAttributes(classInfo)};");

                    foreach (var methodCode in classInfo.Methods.Select(method => GenerateMethodMapping(method, classInfo, compilation, instanceName, cancellationToken)).Where(methodCode => !string.IsNullOrWhiteSpace(methodCode)))
                    {
                        sb.AppendLine(methodCode);
                    }

                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private string GenerateClassAttributes(FastEndpointInfo classInfo)
        {
            var sb = new StringBuilder();

            // 添加类级别的过滤器
            foreach (var filter in classInfo.FilterAttribute.Where(filterAttr => filterAttr.ConstructorArguments.Length > 0).Select(filterAttr => filterAttr.ConstructorArguments.FirstOrDefault().Values).SelectMany(filterTypes => filterTypes))
            {
                if (filter.Value is INamedTypeSymbol filterType)
                {
                    sb.AppendLine($".AddEndpointFilter<{filterType.ToDisplayString()}>()");
                }
            }

            // 处理授权属性
            if (classInfo.AuthorizationAttribute != null)
            {
                // 获取 AuthorizeAttribute 的 Roles 属性
                var rolesArg = classInfo.AuthorizationAttribute?.NamedArguments.FirstOrDefault(p => p.Key == "Roles");

                // 获取 AuthorizeAttribute 的 Policy 属性
                var policyArg = classInfo.AuthorizationAttribute?.NamedArguments.FirstOrDefault(p => p.Key == "Policy");

                var authArr = classInfo.AuthorizationAttribute?.ConstructorArguments
                    .Where(argument => argument.Kind == TypedConstantKind.Array && argument.Values.Length > 0)
                    .SelectMany(argument => argument.Values)
                    .Where(value => value.Value is string)
                    .Select(value => value.Value as string)
                    .ToArray() ?? Array.Empty<string>();

                if (!rolesArg.Equals(default) && !policyArg.Equals(default) &&
                    rolesArg.Value.Value is string roles && policyArg.Value.Value is string policy)
                {
                    sb.AppendLine($".RequireAuthorization(p => p.RequireRole(\"{roles}\").RequirePolicy(\"{policy}\"))");
                }
                else if (!rolesArg.Equals(default) && rolesArg.Value.Value is string role)
                {
                    sb.AppendLine($".RequireAuthorization(p => p.RequireRole(\"{role}\"))");
                }
                else if (!zAuthArr.Equals(default) && zAuthArr.Length > 0)
                {
                    sb.AppendLine($".RequireAuthorization(\"{string.Join(",", zAuthArr)}\")");
                }
                else if (!policyArg.Equals(default) && policyArg.Value.Value is string policyValue)
                {
                    sb.AppendLine($".RequireAuthorization(\"{policyValue}\")");
                }
                else
                {
                    sb.AppendLine(".RequireAuthorization()");
                }
            }
            // 添加标签
            if (!string.IsNullOrEmpty(classInfo.Tags))
            {
                sb.AppendLine($".WithTags(\"{classInfo.Tags}\")");
            }

            return sb.ToString();
        }

        private string GenerateMethodMapping(IMethodSymbol method, FastEndpointInfo classInfo, Compilation compilation,
            string instanceName, CancellationToken cancellationToken)
        {
            // 跳过具有 IgnoreRouteAttribute 的方法
            if (method.GetAttributes().Any(a => a.AttributeClass?.Name == "IgnoreRouteAttribute"))
                return string.Empty;

            // 确定 HTTP 方法和路由
            var (httpMethod, route) = DetermineHttpMethodAndRoute(method);

            // 获取方法级别的属性，排除 FilterAttribute ZAuthorizeAttribute
            var ignoreAttributes = new[] { "FilterAttribute", "AuthorizeAttribute", "AsyncStateMachineAttribute", "IgnoreAntiforgeryTokenAttribute" };
            var methodAttributes =
                method.GetAttributes().Where(a => !ignoreAttributes.Any(v => a.AttributeClass?.Name.EndsWith(v) ?? false)).ToList();

            // 构建属性字符串
            var attributesString = string.Join("\n", methodAttributes.Select(a => $"                [{a}]"));

            if (!string.IsNullOrEmpty(attributesString))
            {
                attributesString = "\n" + attributesString;
            }

            // 获取方法级别的过滤器
            var filterAttributes =
                method.GetAttributes().Where(a => a.AttributeClass?.Name == "FilterAttribute").ToList();

            var filterExtensions = new StringBuilder();
            foreach (var filter in filterAttributes.Where(filterAttr => filterAttr.ConstructorArguments.Length > 0).Select(filterAttr => filterAttr.ConstructorArguments[0].Values).SelectMany(filterTypes => filterTypes))
            {
                if (filter.Value is INamedTypeSymbol filterType)
                {
                    filterExtensions.AppendLine($"\r\n.AddEndpointFilter<{filterType.ToDisplayString()}>()");
                }
            }

            // 获取方法级别的过滤器
            var ignoreAntiforgerys =
                method.GetAttributes().Where(a => a.AttributeClass?.Name == "IgnoreAntiforgeryTokenAttribute").ToList();

            var ignoreAntExtensions = new StringBuilder();
            if (ignoreAntiforgerys.Any())
            {
                ignoreAntExtensions.AppendLine($"\r\n.DisableAntiforgery()");
            }
            // 获取方法级别的权限
            var zAuthExtensions = new StringBuilder();
            var zAuthorizeAttributes =
                method.GetAttributes()
                .Where(a => a.AttributeClass?.Name
                .EndsWith("AuthorizeAttribute") ?? false).ToList();
            if (zAuthorizeAttributes.Any())
            {
                // 获取 AuthorizeAttribute 的 Roles 属性
                var rolesArg = zAuthorizeAttributes
                    .SelectMany(a => a.NamedArguments)
                    .FirstOrDefault(n => n.Key == "Roles");

                // 获取 AuthorizeAttribute 的 Policy 属性
                var policyArg = zAuthorizeAttributes
                    .SelectMany(a => a.NamedArguments)
                    .FirstOrDefault(n => n.Key == "Policy");

                var zAuthArr = zAuthorizeAttributes
                    .SelectMany(attribute => attribute.ConstructorArguments)
                    .Where(argument => argument.Kind == TypedConstantKind.Array && argument.Values.Length > 0)
                    .SelectMany(argument => argument.Values) // 展开数组中的值
                    .Where(value => value.Value is string) // 筛选出 string 类型的值
                    .Select(value => value.Value as string) // 转换为 string
                    .ToArray();

                if (!rolesArg.Equals(default) && !policyArg.Equals(default) &&
                    rolesArg.Value.Value is string roles && policyArg.Value.Value is string policy)
                {
                    zAuthExtensions.AppendLine($"\r\n.RequireAuthorization(p => p.RequireRole(\"{roles}\").RequirePolicy(\"{policy}\"))");
                }
                else if (!rolesArg.Equals(default) && rolesArg.Value.Value is string role)
                {
                    zAuthExtensions.AppendLine($"\r\n.RequireAuthorization(p => p.RequireRole(\"{role}\"))");
                }
                else if (!zAuthArr.Equals(default) && zAuthArr.Length > 0)
                {
                    zAuthExtensions.AppendLine($"\r\n.RequireAuthorization(\"{string.Join(",", zAuthArr)}\")");
                }
                else if (!policyArg.Equals(default) && policyArg.Value.Value is string policyValue)
                {
                    zAuthExtensions.AppendLine($"\r\n.RequireAuthorization(\"{policyValue}\")");
                }
                else
                {
                    zAuthExtensions.AppendLine("\r\n.RequireAuthorization()");
                }
            }

            // 获取方法参数
            var parameters = method.Parameters;
            var parameterList = string.Join(", ", parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}"));
            var parameterNames = string.Join(", ", parameters.Select(p => p.Name));

            // 确定服务实例名称
            var serviceInstance = char.ToLowerInvariant('_') + RemoveServiceSuffixAndLowercaseFirstLetter(classInfo.ClassName);

            // 构建 Lambda 表达式
            string lambda;
            if (parameters.Length > 0)
            {
                if (string.IsNullOrWhiteSpace(classInfo.InterfaceName))
                    lambda =
                    $"async ({classInfo.Namespace}.{classInfo.ClassName} {serviceInstance}, {parameterList}) => await {serviceInstance}.{method.Name}({parameterNames})";

                else
                    lambda =
                        $"async ({classInfo.InterfaceNameSpace}.{classInfo.InterfaceName} {serviceInstance}, {parameterList}) => await {serviceInstance}.{method.Name}({parameterNames})";


            }
            else
            {
                if (string.IsNullOrWhiteSpace(classInfo.InterfaceName))
                    lambda =
                    $"async ({classInfo.Namespace}.{classInfo.ClassName} {serviceInstance}) => await {serviceInstance}.{method.Name}()";

                else
                    lambda =
                    $"async ({classInfo.InterfaceNameSpace}.{classInfo.InterfaceName} {serviceInstance}) => await {serviceInstance}.{method.Name}()";
            }



            // 组合所有部分生成方法代码
            var methodCode = $@"            {instanceName}.Map{httpMethod}(""{route}"",{attributesString}
                {lambda}){zAuthExtensions}{ignoreAntExtensions}{filterExtensions};";

            return methodCode;
        }

        private static string RemoveServiceSuffixAndLowercaseFirstLetter(string input)
        {
            // 1. 截断末尾的 "Service"
            if (input.EndsWith(EndPointSuffix))
            {
                input = input.TrimEnd(EndPointSuffix);
            }

            // 2. 确保字符串非空，处理首字母小写
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToLower(input[0]) + input.Substring(1);
        }

        private (string httpMethod, string route) DetermineHttpMethodAndRoute(IMethodSymbol method)
        {
            // 检查是否有 HTTP 方法特性
            var httpMethodAttr = method.GetAttributes().FirstOrDefault(a => a.AttributeClass != null &&
                                                                           (a.AttributeClass.Name == "HttpGetAttribute" ||
                                                                            a.AttributeClass.Name == "HttpPostAttribute" ||
                                                                            a.AttributeClass.Name == "HttpPutAttribute" ||
                                                                            a.AttributeClass.Name == "HttpDeleteAttribute"));

            if (httpMethodAttr is { AttributeClass: not null })
            {
                var attrName = httpMethodAttr.AttributeClass.Name;
                string httpMethod = attrName switch
                {
                    "HttpGetAttribute" => "Get",
                    "HttpPostAttribute" => "Post",
                    "HttpPutAttribute" => "Put",
                    "HttpDeleteAttribute" => "Delete",
                    _ => "Post"
                };

                // 获取路由参数，如果有
                var route = httpMethodAttr.ConstructorArguments.FirstOrDefault().Value as string ?? method.Name;

                return (httpMethod, route);
            }

            // 根据方法名推断 HTTP 方法和路由
            var methodName = method.Name;
            string inferredHttpMethod;
            string inferredRoute;

            if (methodName.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Get";
                inferredRoute = methodName.Substring(3);
            }
            else if (methodName.StartsWith("Remove", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Delete";
                inferredRoute = methodName.Substring(6);
            }
            else if (methodName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Delete";
                inferredRoute = methodName.Substring(6);
            }
            else if (methodName.StartsWith("Post", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Post";
                inferredRoute = methodName.Substring(4);
            }
            else if (methodName.StartsWith("Create", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Post";
                inferredRoute = methodName.Substring(6);
            }
            else if (methodName.StartsWith("Add", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Post";
                inferredRoute = methodName.Substring(3);
            }
            else if (methodName.StartsWith("Insert", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Post";
                inferredRoute = methodName.Substring(6);
            }
            else if (methodName.StartsWith("Put", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Put";
                inferredRoute = methodName.Substring(3);
            }
            else if (methodName.StartsWith("Update", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Put";
                inferredRoute = methodName.Substring(6);
            }
            else if (methodName.StartsWith("Modify", StringComparison.OrdinalIgnoreCase))
            {
                inferredHttpMethod = "Put";
                inferredRoute = methodName.Substring(6);
            }
            else
            {
                inferredHttpMethod = "Post";
                inferredRoute = methodName;
            }

            // 移除 'Async' 后缀
            if (inferredRoute.EndsWith(AsyncSuffix, StringComparison.OrdinalIgnoreCase))
            {
                inferredRoute = inferredRoute.Substring(0, inferredRoute.Length - AsyncSuffix.Length);
            }

            // 移除 'Service' 后缀
            if (inferredRoute.EndsWith(EndPointSuffix, StringComparison.OrdinalIgnoreCase))
            {
                inferredRoute = inferredRoute.Substring(0, inferredRoute.Length - EndPointSuffix.Length);
            }

            // 确保路由以 '/' 开头
            if (!inferredRoute.StartsWith("/"))
                inferredRoute = "/" + inferredRoute;

            return (inferredHttpMethod, inferredRoute);
        }
    }
}
