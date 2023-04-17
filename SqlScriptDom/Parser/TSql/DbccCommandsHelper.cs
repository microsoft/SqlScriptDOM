//------------------------------------------------------------------------------
// <copyright file="DbccCommandsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class DbccCommandsHelper : OptionsHelper<DbccCommand>
    {
        private DbccCommandsHelper()
        {
            AddOptionMapping(DbccCommand.ActiveCursors, CodeGenerationSupporter.ActiveCursors);
            AddOptionMapping(DbccCommand.AddExtendedProc, CodeGenerationSupporter.AddExtendedProc);
            AddOptionMapping(DbccCommand.AddInstance, CodeGenerationSupporter.AddInstance);
            AddOptionMapping(DbccCommand.AuditEvent, CodeGenerationSupporter.AuditEvent);
            AddOptionMapping(DbccCommand.AutoPilot, CodeGenerationSupporter.AutoPilot);
            AddOptionMapping(DbccCommand.Buffer, CodeGenerationSupporter.Buffer);
            AddOptionMapping(DbccCommand.Bytes, CodeGenerationSupporter.Bytes);
            AddOptionMapping(DbccCommand.CacheProfile, CodeGenerationSupporter.CacheProfile);
            AddOptionMapping(DbccCommand.CacheStats, CodeGenerationSupporter.CacheStats);
            AddOptionMapping(DbccCommand.CallFullText, CodeGenerationSupporter.CallFulltext);
            AddOptionMapping(DbccCommand.CheckAlloc, CodeGenerationSupporter.CheckAlloc);
            AddOptionMapping(DbccCommand.CheckCatalog, CodeGenerationSupporter.CheckCatalog);
            AddOptionMapping(DbccCommand.CheckConstraints, CodeGenerationSupporter.CheckConstraints);
            AddOptionMapping(DbccCommand.CheckDB, CodeGenerationSupporter.CheckDb);
            AddOptionMapping(DbccCommand.CheckFileGroup, CodeGenerationSupporter.CheckFilegroup);
            AddOptionMapping(DbccCommand.CheckIdent, CodeGenerationSupporter.CheckIdent);
            AddOptionMapping(DbccCommand.CheckPrimaryFile, CodeGenerationSupporter.CheckPrimaryFile);
            AddOptionMapping(DbccCommand.CheckTable, CodeGenerationSupporter.CheckTable);
            AddOptionMapping(DbccCommand.CleanTable, CodeGenerationSupporter.CleanTable);
            AddOptionMapping(DbccCommand.ClearSpaceCaches, CodeGenerationSupporter.ClearSpaceCaches);
            AddOptionMapping(DbccCommand.CollectStats, CodeGenerationSupporter.CollectStats);
            AddOptionMapping(DbccCommand.ConcurrencyViolation, CodeGenerationSupporter.ConcurrencyViolation);
            AddOptionMapping(DbccCommand.CursorStats, CodeGenerationSupporter.CursorStats);
            AddOptionMapping(DbccCommand.DBRecover, CodeGenerationSupporter.DbRecover);
            AddOptionMapping(DbccCommand.DBReindex, CodeGenerationSupporter.DbReindex);
            AddOptionMapping(DbccCommand.DBReindexAll, CodeGenerationSupporter.DbReindexAll);
            AddOptionMapping(DbccCommand.DBRepair, CodeGenerationSupporter.DbRepair);
            AddOptionMapping(DbccCommand.DebugBreak, CodeGenerationSupporter.DebugBreak);
            AddOptionMapping(DbccCommand.DeleteInstance, CodeGenerationSupporter.DeleteInstance);
            AddOptionMapping(DbccCommand.DetachDB, CodeGenerationSupporter.DetachDb);
            AddOptionMapping(DbccCommand.DropCleanBuffers, CodeGenerationSupporter.DropCleanBuffers);
            AddOptionMapping(DbccCommand.DropExtendedProc, CodeGenerationSupporter.DropExtendedProc);
            AddOptionMapping(DbccCommand.DumpConfig, CodeGenerationSupporter.DumpConfig);
            AddOptionMapping(DbccCommand.DumpDBInfo, CodeGenerationSupporter.DumpDbInfo);
            AddOptionMapping(DbccCommand.DumpDBTable, CodeGenerationSupporter.DumpDbTable);
            AddOptionMapping(DbccCommand.DumpLock, CodeGenerationSupporter.DumpLock);
            AddOptionMapping(DbccCommand.DumpLog, CodeGenerationSupporter.DumpLog);
            AddOptionMapping(DbccCommand.DumpPage, CodeGenerationSupporter.DumpPage);
            AddOptionMapping(DbccCommand.DumpResource, CodeGenerationSupporter.DumpResource);
            AddOptionMapping(DbccCommand.DumpTrigger, CodeGenerationSupporter.DumpTrigger);
            AddOptionMapping(DbccCommand.ErrorLog, CodeGenerationSupporter.ErrorLog);
            AddOptionMapping(DbccCommand.ExtentInfo, CodeGenerationSupporter.ExtentInfo);
            AddOptionMapping(DbccCommand.FileHeader, CodeGenerationSupporter.FileHeader);
            AddOptionMapping(DbccCommand.FixAllocation, CodeGenerationSupporter.FixAllocation);
            AddOptionMapping(DbccCommand.Flush, CodeGenerationSupporter.Flush);
            AddOptionMapping(DbccCommand.FlushProcInDB, CodeGenerationSupporter.FlushProcInDb);
            AddOptionMapping(DbccCommand.ForceGhostCleanup, CodeGenerationSupporter.ForceGhostCleanup);
            AddOptionMapping(DbccCommand.FreeProcCache, CodeGenerationSupporter.FreeProcCache);
            AddOptionMapping(DbccCommand.FreeSessionCache, CodeGenerationSupporter.FreeSessionCache);
            AddOptionMapping(DbccCommand.FreeSystemCache, CodeGenerationSupporter.FreeSystemCache);
            AddOptionMapping(DbccCommand.FreezeIO, CodeGenerationSupporter.FreezeIo);
            AddOptionMapping(DbccCommand.Help, CodeGenerationSupporter.Help);
            AddOptionMapping(DbccCommand.IcecapQuery, CodeGenerationSupporter.IceCapQuery);
            AddOptionMapping(DbccCommand.IncrementInstance, CodeGenerationSupporter.IncrementInstance);
            AddOptionMapping(DbccCommand.Ind, CodeGenerationSupporter.Ind);
            AddOptionMapping(DbccCommand.IndexDefrag, CodeGenerationSupporter.IndexDefrag);
            AddOptionMapping(DbccCommand.InputBuffer, CodeGenerationSupporter.InputBuffer);
            AddOptionMapping(DbccCommand.InvalidateTextptr, CodeGenerationSupporter.InvalidateTextptr);
            AddOptionMapping(DbccCommand.InvalidateTextptrObjid, CodeGenerationSupporter.InvalidateTextptrObjid);
            AddOptionMapping(DbccCommand.Latch, CodeGenerationSupporter.Latch);
            AddOptionMapping(DbccCommand.LogInfo, CodeGenerationSupporter.LogInfo);
            AddOptionMapping(DbccCommand.MapAllocUnit, CodeGenerationSupporter.MapAllocUnit);
            AddOptionMapping(DbccCommand.MemObjList, CodeGenerationSupporter.MemObjList);
            AddOptionMapping(DbccCommand.MemoryMap, CodeGenerationSupporter.MemoryMap);
            AddOptionMapping(DbccCommand.MemoryStatus, CodeGenerationSupporter.MemoryStatus);
            AddOptionMapping(DbccCommand.Metadata, CodeGenerationSupporter.Metadata);
            AddOptionMapping(DbccCommand.MovePage, CodeGenerationSupporter.MovePage);
            AddOptionMapping(DbccCommand.NoTextptr, CodeGenerationSupporter.NoTextptr);
            AddOptionMapping(DbccCommand.OpenTran, CodeGenerationSupporter.OpenTran);
            AddOptionMapping(DbccCommand.OptimizerWhatIf, CodeGenerationSupporter.OptimizerWhatIf);
            AddOptionMapping(DbccCommand.OutputBuffer, CodeGenerationSupporter.OutputBuffer);
            AddOptionMapping(DbccCommand.PerfMonStats, CodeGenerationSupporter.PerfMonStats);
            AddOptionMapping(DbccCommand.PersistStackHash, CodeGenerationSupporter.PersistStackHash);
            AddOptionMapping(DbccCommand.PinTable, CodeGenerationSupporter.PinTable);
            AddOptionMapping(DbccCommand.ProcCache, CodeGenerationSupporter.ProcCache);
            AddOptionMapping(DbccCommand.PrtiPage, CodeGenerationSupporter.PrtiPage);
            AddOptionMapping(DbccCommand.ReadPage, CodeGenerationSupporter.ReadPage);
            AddOptionMapping(DbccCommand.RenameColumn, CodeGenerationSupporter.RenameColumn);
            AddOptionMapping(DbccCommand.RuleOff, CodeGenerationSupporter.RuleOff);
            AddOptionMapping(DbccCommand.RuleOn, CodeGenerationSupporter.RuleOn);
            AddOptionMapping(DbccCommand.SeMetadata, CodeGenerationSupporter.SeMetadata);
            AddOptionMapping(DbccCommand.SetCpuWeight, CodeGenerationSupporter.SetCpuWeight);
            AddOptionMapping(DbccCommand.SetInstance, CodeGenerationSupporter.SetInstance);
            AddOptionMapping(DbccCommand.SetIOWeight, CodeGenerationSupporter.SetIoWeight);
            AddOptionMapping(DbccCommand.ShowStatistics, CodeGenerationSupporter.ShowStatistics);
            AddOptionMapping(DbccCommand.ShowContig, CodeGenerationSupporter.ShowContig);
            AddOptionMapping(DbccCommand.ShowDBAffinity, CodeGenerationSupporter.ShowDbAffinity);
            AddOptionMapping(DbccCommand.ShowFileStats, CodeGenerationSupporter.ShowFileStats);
            AddOptionMapping(DbccCommand.ShowOffRules, CodeGenerationSupporter.ShowOffRules);
            AddOptionMapping(DbccCommand.ShowOnRules, CodeGenerationSupporter.ShowOnRules);
            AddOptionMapping(DbccCommand.ShowTableAffinity, CodeGenerationSupporter.ShowTableAffinity);
            AddOptionMapping(DbccCommand.ShowText, CodeGenerationSupporter.ShowText);
            AddOptionMapping(DbccCommand.ShowWeights, CodeGenerationSupporter.ShowWeights);
            AddOptionMapping(DbccCommand.ShrinkDatabase, CodeGenerationSupporter.ShrinkDatabase);
            AddOptionMapping(DbccCommand.ShrinkFile, CodeGenerationSupporter.ShrinkFile);
            AddOptionMapping(DbccCommand.SqlMgrStats, CodeGenerationSupporter.SqlMgrStats);
            AddOptionMapping(DbccCommand.SqlPerf, CodeGenerationSupporter.SqlPerf);
            AddOptionMapping(DbccCommand.StackDump, CodeGenerationSupporter.StackDump);
            AddOptionMapping(DbccCommand.Tec, CodeGenerationSupporter.Tec);
            AddOptionMapping(DbccCommand.ThawIO, CodeGenerationSupporter.ThawIo);
            AddOptionMapping(DbccCommand.ThrottleIO, CodeGenerationSupporter.ThrottleIo);
            AddOptionMapping(DbccCommand.TraceOff, CodeGenerationSupporter.TraceOff);
            AddOptionMapping(DbccCommand.TraceOn, CodeGenerationSupporter.TraceOn);
            AddOptionMapping(DbccCommand.TraceStatus, CodeGenerationSupporter.TraceStatus);
            AddOptionMapping(DbccCommand.UnpinTable, CodeGenerationSupporter.UnpinTable);
            AddOptionMapping(DbccCommand.UpdateUsage, CodeGenerationSupporter.UpdateUsage);
            AddOptionMapping(DbccCommand.UsePlan, CodeGenerationSupporter.UsePlan);
            AddOptionMapping(DbccCommand.UserOptions, CodeGenerationSupporter.UserOptions);
            AddOptionMapping(DbccCommand.WritePage, CodeGenerationSupporter.WritePage);
        }

        internal static readonly DbccCommandsHelper Instance = new DbccCommandsHelper();
    }
}
