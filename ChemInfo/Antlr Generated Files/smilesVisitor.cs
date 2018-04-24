//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from smiles.g4 by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="smilesParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public interface IsmilesVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.smiles"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSmiles([NotNull] smilesParser.SmilesContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.chain"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitChain([NotNull] smilesParser.ChainContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.branched_atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBranched_atom([NotNull] smilesParser.Branched_atomContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.branch"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBranch([NotNull] smilesParser.BranchContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.ringbond"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRingbond([NotNull] smilesParser.RingbondContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.bond"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBond([NotNull] smilesParser.BondContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.dot"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDot([NotNull] smilesParser.DotContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAtom([NotNull] smilesParser.AtomContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSymbol([NotNull] smilesParser.SymbolContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.organic"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrganic([NotNull] smilesParser.OrganicContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.aromatic"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAromatic([NotNull] smilesParser.AromaticContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.halogen"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHalogen([NotNull] smilesParser.HalogenContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.wildcard"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWildcard([NotNull] smilesParser.WildcardContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.chiral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitChiral([NotNull] smilesParser.ChiralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.charge"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCharge([NotNull] smilesParser.ChargeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.hcount"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHcount([NotNull] smilesParser.HcountContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.atomclass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAtomclass([NotNull] smilesParser.AtomclassContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.isotope"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIsotope([NotNull] smilesParser.IsotopeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="smilesParser.element"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElement([NotNull] smilesParser.ElementContext context);
}
