﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiagnosticAnalyzerAndCodeFix.Maintainability
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    class AV1525 : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AV1525";
        internal const string Description = "Don’t make explicit comparisons to true or false";
        internal const string MessageFormat = "Don’t make explicit comparisons to true or false";
        internal const string Category = "Maintainability";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.EqualsExpression);
        }

        public void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = context.Node as BinaryExpressionSyntax;

            if (binaryExpression == null)
                return;

            if (binaryExpression.Left.IsKind(SyntaxKind.TrueLiteralExpression) ||
               binaryExpression.Left.IsKind(SyntaxKind.FalseLiteralExpression) ||
               binaryExpression.Right.IsKind(SyntaxKind.TrueLiteralExpression) ||
               binaryExpression.Right.IsKind(SyntaxKind.FalseLiteralExpression))
            {
                var diagnostic = Diagnostic.Create(Rule, binaryExpression.GetLocation());

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}