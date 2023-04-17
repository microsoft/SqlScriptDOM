//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DbccStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        private static Dictionary<DbccCommand, String> _dbccCommandNames = new Dictionary<DbccCommand, String>()
        {
            { DbccCommand.ActiveCursors, CodeGenerationSupporter.ActiveCursors},
            { DbccCommand.AddExtendedProc, CodeGenerationSupporter.AddExtendedProc},
            { DbccCommand.AddInstance, CodeGenerationSupporter.AddInstance},
            { DbccCommand.AuditEvent, CodeGenerationSupporter.AuditEvent},
            { DbccCommand.AutoPilot, CodeGenerationSupporter.AutoPilot},
            { DbccCommand.Buffer, CodeGenerationSupporter.Buffer},
            { DbccCommand.Bytes, CodeGenerationSupporter.Bytes},
            { DbccCommand.CacheProfile, CodeGenerationSupporter.CacheProfile},
            { DbccCommand.CacheStats, CodeGenerationSupporter.CacheStats},
            { DbccCommand.CallFullText, CodeGenerationSupporter.CallFulltext},
            { DbccCommand.CheckAlloc, CodeGenerationSupporter.CheckAlloc},
            { DbccCommand.CheckCatalog, CodeGenerationSupporter.CheckCatalog},
            { DbccCommand.CheckConstraints, CodeGenerationSupporter.CheckConstraintsHint},
            { DbccCommand.CheckDB, CodeGenerationSupporter.CheckDb},
            { DbccCommand.CheckFileGroup, CodeGenerationSupporter.CheckFilegroup},
            { DbccCommand.CheckIdent, CodeGenerationSupporter.CheckIdent},
            { DbccCommand.CheckPrimaryFile, CodeGenerationSupporter.CheckPrimaryFile},
            { DbccCommand.CheckTable, CodeGenerationSupporter.CheckTable},
            { DbccCommand.CleanTable, CodeGenerationSupporter.CleanTable},
            { DbccCommand.ClearSpaceCaches, CodeGenerationSupporter.ClearSpaceCaches},
            { DbccCommand.CollectStats, CodeGenerationSupporter.CollectStats},
            { DbccCommand.ConcurrencyViolation, CodeGenerationSupporter.ConcurrencyViolation},
            { DbccCommand.CursorStats, CodeGenerationSupporter.CursorStats},
            { DbccCommand.DBRecover, CodeGenerationSupporter.DbRecover},
            { DbccCommand.DBReindex, CodeGenerationSupporter.DbReindex},
            { DbccCommand.DBReindexAll, CodeGenerationSupporter.DbReindexAll},
            { DbccCommand.DBRepair, CodeGenerationSupporter.DbRepair},
            { DbccCommand.DebugBreak, CodeGenerationSupporter.DebugBreak},
            { DbccCommand.DeleteInstance, CodeGenerationSupporter.DeleteInstance},
            { DbccCommand.DetachDB, CodeGenerationSupporter.DetachDb},
            { DbccCommand.DropCleanBuffers, CodeGenerationSupporter.DropCleanBuffers},
            { DbccCommand.DropExtendedProc, CodeGenerationSupporter.DropExtendedProc},
            { DbccCommand.DumpConfig, CodeGenerationSupporter.DumpConfig},
            { DbccCommand.DumpDBInfo, CodeGenerationSupporter.DumpDbInfo},
            { DbccCommand.DumpDBTable, CodeGenerationSupporter.DumpDbTable},
            { DbccCommand.DumpLock, CodeGenerationSupporter.DumpLock},
            { DbccCommand.DumpLog, CodeGenerationSupporter.DumpLog},
            { DbccCommand.DumpPage, CodeGenerationSupporter.DumpPage},
            { DbccCommand.DumpResource, CodeGenerationSupporter.DumpResource},
            { DbccCommand.DumpTrigger, CodeGenerationSupporter.DumpTrigger},
            { DbccCommand.ErrorLog, CodeGenerationSupporter.ErrorLog},
            { DbccCommand.ExtentInfo, CodeGenerationSupporter.ExtentInfo},
            { DbccCommand.FileHeader, CodeGenerationSupporter.FileHeader},
            { DbccCommand.FixAllocation, CodeGenerationSupporter.FixAllocation},
            { DbccCommand.Flush, CodeGenerationSupporter.Flush},
            { DbccCommand.FlushProcInDB, CodeGenerationSupporter.FlushProcInDb},
            { DbccCommand.ForceGhostCleanup, CodeGenerationSupporter.ForceGhostCleanup},
            { DbccCommand.Free, CodeGenerationSupporter.Free},
            { DbccCommand.FreeProcCache, CodeGenerationSupporter.FreeProcCache},
            { DbccCommand.FreeSessionCache, CodeGenerationSupporter.FreeSessionCache},
            { DbccCommand.FreeSystemCache, CodeGenerationSupporter.FreeSystemCache},
            { DbccCommand.FreezeIO, CodeGenerationSupporter.FreezeIo},
            { DbccCommand.Help, CodeGenerationSupporter.Help},
            { DbccCommand.IcecapQuery, CodeGenerationSupporter.IceCapQuery},
            { DbccCommand.IncrementInstance, CodeGenerationSupporter.IncrementInstance},
            { DbccCommand.Ind, CodeGenerationSupporter.Ind},
            { DbccCommand.IndexDefrag, CodeGenerationSupporter.IndexDefrag},
            { DbccCommand.InputBuffer, CodeGenerationSupporter.InputBuffer},
            { DbccCommand.InvalidateTextptr, CodeGenerationSupporter.InvalidateTextptr},
            { DbccCommand.InvalidateTextptrObjid, CodeGenerationSupporter.InvalidateTextptrObjid},
            { DbccCommand.Latch, CodeGenerationSupporter.Latch},
            { DbccCommand.LogInfo, CodeGenerationSupporter.LogInfo},
            { DbccCommand.MapAllocUnit, CodeGenerationSupporter.MapAllocUnit},
            { DbccCommand.MemObjList, CodeGenerationSupporter.MemObjList},
            { DbccCommand.MemoryMap, CodeGenerationSupporter.MemoryMap},
            { DbccCommand.MemoryStatus, CodeGenerationSupporter.MemoryStatus},
            { DbccCommand.Metadata, CodeGenerationSupporter.Metadata},
            { DbccCommand.MovePage, CodeGenerationSupporter.MovePage},
            { DbccCommand.NoTextptr, CodeGenerationSupporter.NoTextptr},
            { DbccCommand.OpenTran, CodeGenerationSupporter.OpenTran},
            { DbccCommand.OptimizerWhatIf, CodeGenerationSupporter.OptimizerWhatIf},
            { DbccCommand.OutputBuffer, CodeGenerationSupporter.OutputBuffer},
            { DbccCommand.PerfMonStats, CodeGenerationSupporter.PerfMonStats},
            { DbccCommand.PersistStackHash, CodeGenerationSupporter.PersistStackHash},
            { DbccCommand.PinTable, CodeGenerationSupporter.PinTable},
            { DbccCommand.ProcCache, CodeGenerationSupporter.ProcCache},
            { DbccCommand.PrtiPage, CodeGenerationSupporter.PrtiPage},
            { DbccCommand.ReadPage, CodeGenerationSupporter.ReadPage},
            { DbccCommand.RenameColumn, CodeGenerationSupporter.RenameColumn},
            { DbccCommand.RuleOff, CodeGenerationSupporter.RuleOff},
            { DbccCommand.RuleOn, CodeGenerationSupporter.RuleOn},
            { DbccCommand.SeMetadata, CodeGenerationSupporter.SeMetadata},
            { DbccCommand.SetCpuWeight, CodeGenerationSupporter.SetCpuWeight},
            { DbccCommand.SetInstance, CodeGenerationSupporter.SetInstance},
            { DbccCommand.SetIOWeight, CodeGenerationSupporter.SetIoWeight},
            { DbccCommand.ShowStatistics, CodeGenerationSupporter.ShowStatistics},
            { DbccCommand.ShowContig, CodeGenerationSupporter.ShowContig},
            { DbccCommand.ShowDBAffinity, CodeGenerationSupporter.ShowDbAffinity},
            { DbccCommand.ShowFileStats, CodeGenerationSupporter.ShowFileStats},
            { DbccCommand.ShowOffRules, CodeGenerationSupporter.ShowOffRules},
            { DbccCommand.ShowOnRules, CodeGenerationSupporter.ShowOnRules},
            { DbccCommand.ShowTableAffinity, CodeGenerationSupporter.ShowTableAffinity},
            { DbccCommand.ShowText, CodeGenerationSupporter.ShowText},
            { DbccCommand.ShowWeights, CodeGenerationSupporter.ShowWeights},
            { DbccCommand.ShrinkDatabase, CodeGenerationSupporter.ShrinkDatabase},
            { DbccCommand.ShrinkFile, CodeGenerationSupporter.ShrinkFile},
            { DbccCommand.SqlMgrStats, CodeGenerationSupporter.SqlMgrStats},
            { DbccCommand.SqlPerf, CodeGenerationSupporter.SqlPerf},
            { DbccCommand.StackDump, CodeGenerationSupporter.StackDump},
            { DbccCommand.Tec, CodeGenerationSupporter.Tec},
            { DbccCommand.ThawIO, CodeGenerationSupporter.ThawIo},
            { DbccCommand.ThrottleIO, CodeGenerationSupporter.ThrottleIo},
            { DbccCommand.TraceOff, CodeGenerationSupporter.TraceOff},
            { DbccCommand.TraceOn, CodeGenerationSupporter.TraceOn},
            { DbccCommand.TraceStatus, CodeGenerationSupporter.TraceStatus},
            { DbccCommand.UnpinTable, CodeGenerationSupporter.UnpinTable},
            { DbccCommand.UpdateUsage, CodeGenerationSupporter.UpdateUsage},
            { DbccCommand.UsePlan, CodeGenerationSupporter.UsePlan},
            { DbccCommand.UserOptions, CodeGenerationSupporter.UserOptions},
            { DbccCommand.WritePage, CodeGenerationSupporter.WritePage},        
        };

        private static Dictionary<DbccOptionKind, TokenGenerator> _dbccOptionsGenerators = new Dictionary<DbccOptionKind, TokenGenerator>()
        {
            { DbccOptionKind.AllErrorMessages, new IdentifierGenerator(CodeGenerationSupporter.AllErrorMessages)},
            { DbccOptionKind.CountRows, new IdentifierGenerator(CodeGenerationSupporter.CountRows)},
            { DbccOptionKind.NoInfoMessages, new IdentifierGenerator(CodeGenerationSupporter.NoInfoMessages)},
            { DbccOptionKind.TableResults, new IdentifierGenerator(CodeGenerationSupporter.TableResults)},
            { DbccOptionKind.TabLock, new IdentifierGenerator(CodeGenerationSupporter.TabLock)},
            { DbccOptionKind.StatHeader, new IdentifierGenerator(CodeGenerationSupporter.StatHeader)},
            { DbccOptionKind.DensityVector, new IdentifierGenerator(CodeGenerationSupporter.DensityVector)},
            { DbccOptionKind.HistogramSteps, new IdentifierGenerator(CodeGenerationSupporter.HistogramSteps)},
            { DbccOptionKind.EstimateOnly, new IdentifierGenerator(CodeGenerationSupporter.EstimateOnly)},
            { DbccOptionKind.Fast, new IdentifierGenerator(CodeGenerationSupporter.Fast)},
            { DbccOptionKind.AllLevels, new IdentifierGenerator(CodeGenerationSupporter.AllLevels)},
            { DbccOptionKind.AllIndexes, new IdentifierGenerator(CodeGenerationSupporter.AllIndexes)},
            { DbccOptionKind.PhysicalOnly, new IdentifierGenerator(CodeGenerationSupporter.PhysicalOnly)},
            { DbccOptionKind.AllConstraints, new IdentifierGenerator(CodeGenerationSupporter.AllConstraints)},
            { DbccOptionKind.StatsStream, new IdentifierGenerator(CodeGenerationSupporter.StatsStream)},
            { DbccOptionKind.Histogram, new IdentifierGenerator(CodeGenerationSupporter.Histogram)},
            { DbccOptionKind.DataPurity, new IdentifierGenerator(CodeGenerationSupporter.DataPurity)},
            { DbccOptionKind.MarkInUseForRemoval, new IdentifierGenerator(CodeGenerationSupporter.MarkInUseForRemoval)},
            { DbccOptionKind.ExtendedLogicalChecks, new IdentifierGenerator(CodeGenerationSupporter.ExtendedLogicalChecks)},
        };

        public override void ExplicitVisit(DbccStatement node)
        {
            GenerateKeyword(TSqlTokenType.Dbcc);

            // DLL name for FREE
            if (node.Command == DbccCommand.Free)
            {
                GenerateSpace();
                GenerateIdentifierWithoutCasing(node.DllName);
            }
            else
            {
                String commandName = GetValueForEnumKey(_dbccCommandNames, node.Command);
                if (commandName != null)
                {
                    GenerateSpaceAndIdentifier(commandName);
                }
            }

            if (node.ParenthesisRequired || node.Literals.Count > 0)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.Literals);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            if (node.Options != null && node.Options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();

                if (node.OptionsUseJoin)
                {
                    GenerateJoinSeparatedList(node.Options);
                }
                else
                {
                    GenerateCommaSeparatedList(node.Options);
                }
            }
        }

        public override void ExplicitVisit(DbccOption node)
        {
            TokenGenerator generator = GetValueForEnumKey(_dbccOptionsGenerators, node.OptionKind);
            GenerateToken(generator);
        }

        protected void GenerateJoinSeparatedList<T>(IList<T> list) where T : TSqlFragment
        {
            GenerateList(list, delegate()
            {
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.Join);
                GenerateSpace();
            });
        }
    }
}
