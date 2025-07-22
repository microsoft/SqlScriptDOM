//------------------------------------------------------------------------------
// <copyright file="TSqlFragmentVisitor.codegen.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Visitor for AST nodes
	/// </summary>
	partial class TSqlFragmentVisitor
	{
		/// <summary>
		/// Visitor for StatementList
		/// </summary>
		public virtual void Visit(StatementList node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StatementList
		/// </summary>
		public virtual void ExplicitVisit(StatementList node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteStatement
		/// </summary>
		public virtual void Visit(ExecuteStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteStatement
		/// </summary>
		public virtual void ExplicitVisit(ExecuteStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteOption
		/// </summary>
		public virtual void Visit(ExecuteOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteOption
		/// </summary>
		public virtual void ExplicitVisit(ExecuteOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ResultSetsExecuteOption
		/// </summary>
		public virtual void Visit(ResultSetsExecuteOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ResultSetsExecuteOption
		/// </summary>
		public virtual void ExplicitVisit(ResultSetsExecuteOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExecuteOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ResultSetDefinition
		/// </summary>
		public virtual void Visit(ResultSetDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ResultSetDefinition
		/// </summary>
		public virtual void ExplicitVisit(ResultSetDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InlineResultSetDefinition
		/// </summary>
		public virtual void Visit(InlineResultSetDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InlineResultSetDefinition
		/// </summary>
		public virtual void ExplicitVisit(InlineResultSetDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ResultSetDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ResultColumnDefinition
		/// </summary>
		public virtual void Visit(ResultColumnDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ResultColumnDefinition
		/// </summary>
		public virtual void ExplicitVisit(ResultColumnDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaObjectResultSetDefinition
		/// </summary>
		public virtual void Visit(SchemaObjectResultSetDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaObjectResultSetDefinition
		/// </summary>
		public virtual void ExplicitVisit(SchemaObjectResultSetDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ResultSetDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteSpecification
		/// </summary>
		public virtual void Visit(ExecuteSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteSpecification
		/// </summary>
		public virtual void ExplicitVisit(ExecuteSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteContext
		/// </summary>
		public virtual void Visit(ExecuteContext node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteContext
		/// </summary>
		public virtual void ExplicitVisit(ExecuteContext node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteParameter
		/// </summary>
		public virtual void Visit(ExecuteParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteParameter
		/// </summary>
		public virtual void ExplicitVisit(ExecuteParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecutableEntity
		/// </summary>
		public virtual void Visit(ExecutableEntity node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecutableEntity
		/// </summary>
		public virtual void ExplicitVisit(ExecutableEntity node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProcedureReferenceName
		/// </summary>
		public virtual void Visit(ProcedureReferenceName node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProcedureReferenceName
		/// </summary>
		public virtual void ExplicitVisit(ProcedureReferenceName node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecutableProcedureReference
		/// </summary>
		public virtual void Visit(ExecutableProcedureReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecutableProcedureReference
		/// </summary>
		public virtual void ExplicitVisit(ExecutableProcedureReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExecutableEntity) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecutableStringList
		/// </summary>
		public virtual void Visit(ExecutableStringList node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecutableStringList
		/// </summary>
		public virtual void ExplicitVisit(ExecutableStringList node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExecutableEntity) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AdHocDataSource
		/// </summary>
		public virtual void Visit(AdHocDataSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AdHocDataSource
		/// </summary>
		public virtual void ExplicitVisit(AdHocDataSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ViewOption
		/// </summary>
		public virtual void Visit(ViewOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ViewOption
		/// </summary>
		public virtual void ExplicitVisit(ViewOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterViewStatement
		/// </summary>
		public virtual void Visit(AlterViewStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterViewStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterViewStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ViewStatementBody) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateViewStatement
		/// </summary>
		public virtual void Visit(CreateViewStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateViewStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateViewStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ViewStatementBody) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateOrAlterViewStatement
		/// </summary>
		public virtual void Visit(CreateOrAlterViewStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateOrAlterViewStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateOrAlterViewStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ViewStatementBody) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ViewStatementBody
		/// </summary>
		public virtual void Visit(ViewStatementBody node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ViewStatementBody
		/// </summary>
		public virtual void ExplicitVisit(ViewStatementBody node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ViewForAppendOption
		/// </summary>
		public virtual void Visit(ViewForAppendOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ViewForAppendOption
		/// </summary>
		public virtual void ExplicitVisit(ViewForAppendOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ViewOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ViewDistributionOption
		/// </summary>
		public virtual void Visit(ViewDistributionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ViewDistributionOption
		/// </summary>
		public virtual void ExplicitVisit(ViewDistributionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ViewOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ViewDistributionPolicy
		/// </summary>
		public virtual void Visit(ViewDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ViewDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(ViewDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ViewRoundRobinDistributionPolicy
		/// </summary>
		public virtual void Visit(ViewRoundRobinDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ViewRoundRobinDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(ViewRoundRobinDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ViewDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ViewHashDistributionPolicy
		/// </summary>
		public virtual void Visit(ViewHashDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ViewHashDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(ViewHashDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ViewDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TriggerObject
		/// </summary>
		public virtual void Visit(TriggerObject node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TriggerObject
		/// </summary>
		public virtual void ExplicitVisit(TriggerObject node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TriggerOption
		/// </summary>
		public virtual void Visit(TriggerOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TriggerOption
		/// </summary>
		public virtual void ExplicitVisit(TriggerOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteAsTriggerOption
		/// </summary>
		public virtual void Visit(ExecuteAsTriggerOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteAsTriggerOption
		/// </summary>
		public virtual void ExplicitVisit(ExecuteAsTriggerOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TriggerOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TriggerAction
		/// </summary>
		public virtual void Visit(TriggerAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TriggerAction
		/// </summary>
		public virtual void ExplicitVisit(TriggerAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTriggerStatement
		/// </summary>
		public virtual void Visit(AlterTriggerStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTriggerStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTriggerStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TriggerStatementBody) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateTriggerStatement
		/// </summary>
		public virtual void Visit(CreateTriggerStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateTriggerStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateTriggerStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TriggerStatementBody) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateOrAlterTriggerStatement
		/// </summary>
		public virtual void Visit(CreateOrAlterTriggerStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateOrAlterTriggerStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateOrAlterTriggerStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TriggerStatementBody) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TriggerStatementBody
		/// </summary>
		public virtual void Visit(TriggerStatementBody node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TriggerStatementBody
		/// </summary>
		public virtual void ExplicitVisit(TriggerStatementBody node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for Identifier
		/// </summary>
		public virtual void Visit(Identifier node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for Identifier
		/// </summary>
		public virtual void ExplicitVisit(Identifier node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterProcedureStatement
		/// </summary>
		public virtual void Visit(AlterProcedureStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterProcedureStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterProcedureStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ProcedureStatementBody) node);
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateProcedureStatement
		/// </summary>
		public virtual void Visit(CreateProcedureStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateProcedureStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateProcedureStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ProcedureStatementBody) node);
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateOrAlterProcedureStatement
		/// </summary>
		public virtual void Visit(CreateOrAlterProcedureStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateOrAlterProcedureStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateOrAlterProcedureStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ProcedureStatementBody) node);
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProcedureReference
		/// </summary>
		public virtual void Visit(ProcedureReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProcedureReference
		/// </summary>
		public virtual void ExplicitVisit(ProcedureReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MethodSpecifier
		/// </summary>
		public virtual void Visit(MethodSpecifier node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MethodSpecifier
		/// </summary>
		public virtual void ExplicitVisit(MethodSpecifier node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProcedureStatementBody
		/// </summary>
		public virtual void Visit(ProcedureStatementBody node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProcedureStatementBody
		/// </summary>
		public virtual void ExplicitVisit(ProcedureStatementBody node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProcedureStatementBodyBase
		/// </summary>
		public virtual void Visit(ProcedureStatementBodyBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProcedureStatementBodyBase
		/// </summary>
		public virtual void ExplicitVisit(ProcedureStatementBodyBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FunctionStatementBody
		/// </summary>
		public virtual void Visit(FunctionStatementBody node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FunctionStatementBody
		/// </summary>
		public virtual void ExplicitVisit(FunctionStatementBody node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProcedureOption
		/// </summary>
		public virtual void Visit(ProcedureOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProcedureOption
		/// </summary>
		public virtual void ExplicitVisit(ProcedureOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteAsProcedureOption
		/// </summary>
		public virtual void Visit(ExecuteAsProcedureOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteAsProcedureOption
		/// </summary>
		public virtual void ExplicitVisit(ExecuteAsProcedureOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ProcedureOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FunctionOption
		/// </summary>
		public virtual void Visit(FunctionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FunctionOption
		/// </summary>
		public virtual void ExplicitVisit(FunctionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InlineFunctionOption
		/// </summary>
		public virtual void Visit(InlineFunctionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InlineFunctionOption
		/// </summary>
		public virtual void ExplicitVisit(InlineFunctionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteAsFunctionOption
		/// </summary>
		public virtual void Visit(ExecuteAsFunctionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteAsFunctionOption
		/// </summary>
		public virtual void ExplicitVisit(ExecuteAsFunctionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlNamespaces
		/// </summary>
		public virtual void Visit(XmlNamespaces node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlNamespaces
		/// </summary>
		public virtual void ExplicitVisit(XmlNamespaces node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlNamespacesElement
		/// </summary>
		public virtual void Visit(XmlNamespacesElement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlNamespacesElement
		/// </summary>
		public virtual void ExplicitVisit(XmlNamespacesElement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlNamespacesDefaultElement
		/// </summary>
		public virtual void Visit(XmlNamespacesDefaultElement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlNamespacesDefaultElement
		/// </summary>
		public virtual void ExplicitVisit(XmlNamespacesDefaultElement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((XmlNamespacesElement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlNamespacesAliasElement
		/// </summary>
		public virtual void Visit(XmlNamespacesAliasElement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlNamespacesAliasElement
		/// </summary>
		public virtual void ExplicitVisit(XmlNamespacesAliasElement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((XmlNamespacesElement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CommonTableExpression
		/// </summary>
		public virtual void Visit(CommonTableExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CommonTableExpression
		/// </summary>
		public virtual void ExplicitVisit(CommonTableExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WithCtesAndXmlNamespaces
		/// </summary>
		public virtual void Visit(WithCtesAndXmlNamespaces node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WithCtesAndXmlNamespaces
		/// </summary>
		public virtual void ExplicitVisit(WithCtesAndXmlNamespaces node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FunctionReturnType
		/// </summary>
		public virtual void Visit(FunctionReturnType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FunctionReturnType
		/// </summary>
		public virtual void ExplicitVisit(FunctionReturnType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableValuedFunctionReturnType
		/// </summary>
		public virtual void Visit(TableValuedFunctionReturnType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableValuedFunctionReturnType
		/// </summary>
		public virtual void ExplicitVisit(TableValuedFunctionReturnType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionReturnType) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DataTypeReference
		/// </summary>
		public virtual void Visit(DataTypeReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DataTypeReference
		/// </summary>
		public virtual void ExplicitVisit(DataTypeReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ParameterizedDataTypeReference
		/// </summary>
		public virtual void Visit(ParameterizedDataTypeReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ParameterizedDataTypeReference
		/// </summary>
		public virtual void ExplicitVisit(ParameterizedDataTypeReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataTypeReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SqlDataTypeReference
		/// </summary>
		public virtual void Visit(SqlDataTypeReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SqlDataTypeReference
		/// </summary>
		public virtual void ExplicitVisit(SqlDataTypeReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ParameterizedDataTypeReference) node);
				this.Visit((DataTypeReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UserDataTypeReference
		/// </summary>
		public virtual void Visit(UserDataTypeReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UserDataTypeReference
		/// </summary>
		public virtual void ExplicitVisit(UserDataTypeReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ParameterizedDataTypeReference) node);
				this.Visit((DataTypeReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlDataTypeReference
		/// </summary>
		public virtual void Visit(XmlDataTypeReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlDataTypeReference
		/// </summary>
		public virtual void ExplicitVisit(XmlDataTypeReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataTypeReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ScalarFunctionReturnType
		/// </summary>
		public virtual void Visit(ScalarFunctionReturnType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ScalarFunctionReturnType
		/// </summary>
		public virtual void ExplicitVisit(ScalarFunctionReturnType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionReturnType) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectFunctionReturnType
		/// </summary>
		public virtual void Visit(SelectFunctionReturnType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectFunctionReturnType
		/// </summary>
		public virtual void ExplicitVisit(SelectFunctionReturnType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionReturnType) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableDefinition
		/// </summary>
		public virtual void Visit(TableDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableDefinition
		/// </summary>
		public virtual void ExplicitVisit(TableDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeclareTableVariableBody
		/// </summary>
		public virtual void Visit(DeclareTableVariableBody node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeclareTableVariableBody
		/// </summary>
		public virtual void ExplicitVisit(DeclareTableVariableBody node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeclareTableVariableStatement
		/// </summary>
		public virtual void Visit(DeclareTableVariableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeclareTableVariableStatement
		/// </summary>
		public virtual void ExplicitVisit(DeclareTableVariableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for NamedTableReference
		/// </summary>
		public virtual void Visit(NamedTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for NamedTableReference
		/// </summary>
		public virtual void ExplicitVisit(NamedTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaObjectFunctionTableReference
		/// </summary>
		public virtual void Visit(SchemaObjectFunctionTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaObjectFunctionTableReference
		/// </summary>
		public virtual void ExplicitVisit(SchemaObjectFunctionTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableHint
		/// </summary>
		public virtual void Visit(TableHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableHint
		/// </summary>
		public virtual void ExplicitVisit(TableHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IndexTableHint
		/// </summary>
		public virtual void Visit(IndexTableHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IndexTableHint
		/// </summary>
		public virtual void ExplicitVisit(IndexTableHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableHint) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralTableHint
		/// </summary>
		public virtual void Visit(LiteralTableHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralTableHint
		/// </summary>
		public virtual void ExplicitVisit(LiteralTableHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableHint) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryDerivedTable
		/// </summary>
		public virtual void Visit(QueryDerivedTable node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryDerivedTable
		/// </summary>
		public virtual void ExplicitVisit(QueryDerivedTable node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InlineDerivedTable
		/// </summary>
		public virtual void Visit(InlineDerivedTable node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InlineDerivedTable
		/// </summary>
		public virtual void ExplicitVisit(InlineDerivedTable node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SubqueryComparisonPredicate
		/// </summary>
		public virtual void Visit(SubqueryComparisonPredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SubqueryComparisonPredicate
		/// </summary>
		public virtual void ExplicitVisit(SubqueryComparisonPredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExistsPredicate
		/// </summary>
		public virtual void Visit(ExistsPredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExistsPredicate
		/// </summary>
		public virtual void ExplicitVisit(ExistsPredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LikePredicate
		/// </summary>
		public virtual void Visit(LikePredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LikePredicate
		/// </summary>
		public virtual void ExplicitVisit(LikePredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DistinctPredicate
		/// </summary>
		public virtual void Visit(DistinctPredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DistinctPredicate
		/// </summary>
		public virtual void ExplicitVisit(DistinctPredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InPredicate
		/// </summary>
		public virtual void Visit(InPredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InPredicate
		/// </summary>
		public virtual void ExplicitVisit(InPredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextPredicate
		/// </summary>
		public virtual void Visit(FullTextPredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextPredicate
		/// </summary>
		public virtual void ExplicitVisit(FullTextPredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UserDefinedTypePropertyAccess
		/// </summary>
		public virtual void Visit(UserDefinedTypePropertyAccess node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UserDefinedTypePropertyAccess
		/// </summary>
		public virtual void ExplicitVisit(UserDefinedTypePropertyAccess node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StatementWithCtesAndXmlNamespaces
		/// </summary>
		public virtual void Visit(StatementWithCtesAndXmlNamespaces node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StatementWithCtesAndXmlNamespaces
		/// </summary>
		public virtual void ExplicitVisit(StatementWithCtesAndXmlNamespaces node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectStatement
		/// </summary>
		public virtual void Visit(SelectStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectStatement
		/// </summary>
		public virtual void ExplicitVisit(SelectStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((StatementWithCtesAndXmlNamespaces) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ForClause
		/// </summary>
		public virtual void Visit(ForClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ForClause
		/// </summary>
		public virtual void ExplicitVisit(ForClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BrowseForClause
		/// </summary>
		public virtual void Visit(BrowseForClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BrowseForClause
		/// </summary>
		public virtual void ExplicitVisit(BrowseForClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ForClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ReadOnlyForClause
		/// </summary>
		public virtual void Visit(ReadOnlyForClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ReadOnlyForClause
		/// </summary>
		public virtual void ExplicitVisit(ReadOnlyForClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ForClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlForClause
		/// </summary>
		public virtual void Visit(XmlForClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlForClause
		/// </summary>
		public virtual void ExplicitVisit(XmlForClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ForClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlForClauseOption
		/// </summary>
		public virtual void Visit(XmlForClauseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlForClauseOption
		/// </summary>
		public virtual void ExplicitVisit(XmlForClauseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ForClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for JsonForClause
		/// </summary>
		public virtual void Visit(JsonForClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for JsonForClause
		/// </summary>
		public virtual void ExplicitVisit(JsonForClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ForClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for JsonKeyValue
		/// </summary>
		public virtual void Visit(JsonKeyValue node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for JsonKeyValue
		/// </summary>
		public virtual void ExplicitVisit(JsonKeyValue node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for JsonForClauseOption
		/// </summary>
		public virtual void Visit(JsonForClauseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for JsonForClauseOption
		/// </summary>
		public virtual void ExplicitVisit(JsonForClauseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ForClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateForClause
		/// </summary>
		public virtual void Visit(UpdateForClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateForClause
		/// </summary>
		public virtual void ExplicitVisit(UpdateForClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ForClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OptimizerHint
		/// </summary>
		public virtual void Visit(OptimizerHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OptimizerHint
		/// </summary>
		public virtual void ExplicitVisit(OptimizerHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralOptimizerHint
		/// </summary>
		public virtual void Visit(LiteralOptimizerHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralOptimizerHint
		/// </summary>
		public virtual void ExplicitVisit(LiteralOptimizerHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OptimizerHint) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableHintsOptimizerHint
		/// </summary>
		public virtual void Visit(TableHintsOptimizerHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableHintsOptimizerHint
		/// </summary>
		public virtual void ExplicitVisit(TableHintsOptimizerHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OptimizerHint) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ForceSeekTableHint
		/// </summary>
		public virtual void Visit(ForceSeekTableHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ForceSeekTableHint
		/// </summary>
		public virtual void ExplicitVisit(ForceSeekTableHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableHint) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OptimizeForOptimizerHint
		/// </summary>
		public virtual void Visit(OptimizeForOptimizerHint node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OptimizeForOptimizerHint
		/// </summary>
		public virtual void ExplicitVisit(OptimizeForOptimizerHint node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OptimizerHint) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UseHintList
		/// </summary>
		public virtual void Visit(UseHintList node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UseHintList
		/// </summary>
		public virtual void ExplicitVisit(UseHintList node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OptimizerHint) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for VariableValuePair
		/// </summary>
		public virtual void Visit(VariableValuePair node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for VariableValuePair
		/// </summary>
		public virtual void ExplicitVisit(VariableValuePair node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WhenClause
		/// </summary>
		public virtual void Visit(WhenClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WhenClause
		/// </summary>
		public virtual void ExplicitVisit(WhenClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SimpleWhenClause
		/// </summary>
		public virtual void Visit(SimpleWhenClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SimpleWhenClause
		/// </summary>
		public virtual void ExplicitVisit(SimpleWhenClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WhenClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SearchedWhenClause
		/// </summary>
		public virtual void Visit(SearchedWhenClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SearchedWhenClause
		/// </summary>
		public virtual void ExplicitVisit(SearchedWhenClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WhenClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CaseExpression
		/// </summary>
		public virtual void Visit(CaseExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CaseExpression
		/// </summary>
		public virtual void ExplicitVisit(CaseExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SimpleCaseExpression
		/// </summary>
		public virtual void Visit(SimpleCaseExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SimpleCaseExpression
		/// </summary>
		public virtual void ExplicitVisit(SimpleCaseExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CaseExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SearchedCaseExpression
		/// </summary>
		public virtual void Visit(SearchedCaseExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SearchedCaseExpression
		/// </summary>
		public virtual void ExplicitVisit(SearchedCaseExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CaseExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for NullIfExpression
		/// </summary>
		public virtual void Visit(NullIfExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for NullIfExpression
		/// </summary>
		public virtual void ExplicitVisit(NullIfExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CoalesceExpression
		/// </summary>
		public virtual void Visit(CoalesceExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CoalesceExpression
		/// </summary>
		public virtual void ExplicitVisit(CoalesceExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IIfCall
		/// </summary>
		public virtual void Visit(IIfCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IIfCall
		/// </summary>
		public virtual void ExplicitVisit(IIfCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextTableReference
		/// </summary>
		public virtual void Visit(FullTextTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextTableReference
		/// </summary>
		public virtual void ExplicitVisit(FullTextTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SemanticTableReference
		/// </summary>
		public virtual void Visit(SemanticTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SemanticTableReference
		/// </summary>
		public virtual void ExplicitVisit(SemanticTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenXmlTableReference
		/// </summary>
		public virtual void Visit(OpenXmlTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenXmlTableReference
		/// </summary>
		public virtual void ExplicitVisit(OpenXmlTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenJsonTableReference
		/// </summary>
		public virtual void Visit(OpenJsonTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenJsonTableReference
		/// </summary>
		public virtual void ExplicitVisit(OpenJsonTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenRowsetTableReference
		/// </summary>
		public virtual void Visit(OpenRowsetTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenRowsetTableReference
		/// </summary>
		public virtual void ExplicitVisit(OpenRowsetTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InternalOpenRowset
		/// </summary>
		public virtual void Visit(InternalOpenRowset node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InternalOpenRowset
		/// </summary>
		public virtual void ExplicitVisit(InternalOpenRowset node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenRowsetCosmos
		/// </summary>
		public virtual void Visit(OpenRowsetCosmos node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenRowsetCosmos
		/// </summary>
		public virtual void ExplicitVisit(OpenRowsetCosmos node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BulkOpenRowset
		/// </summary>
		public virtual void Visit(BulkOpenRowset node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BulkOpenRowset
		/// </summary>
		public virtual void ExplicitVisit(BulkOpenRowset node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenRowsetColumnDefinition
		/// </summary>
		public virtual void Visit(OpenRowsetColumnDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenRowsetColumnDefinition
		/// </summary>
		public virtual void ExplicitVisit(OpenRowsetColumnDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnDefinitionBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenQueryTableReference
		/// </summary>
		public virtual void Visit(OpenQueryTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenQueryTableReference
		/// </summary>
		public virtual void ExplicitVisit(OpenQueryTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AdHocTableReference
		/// </summary>
		public virtual void Visit(AdHocTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AdHocTableReference
		/// </summary>
		public virtual void ExplicitVisit(AdHocTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaDeclarationItem
		/// </summary>
		public virtual void Visit(SchemaDeclarationItem node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaDeclarationItem
		/// </summary>
		public virtual void ExplicitVisit(SchemaDeclarationItem node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaDeclarationItemOpenjson
		/// </summary>
		public virtual void Visit(SchemaDeclarationItemOpenjson node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaDeclarationItemOpenjson
		/// </summary>
		public virtual void ExplicitVisit(SchemaDeclarationItemOpenjson node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SchemaDeclarationItem) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ConvertCall
		/// </summary>
		public virtual void Visit(ConvertCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ConvertCall
		/// </summary>
		public virtual void ExplicitVisit(ConvertCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TryConvertCall
		/// </summary>
		public virtual void Visit(TryConvertCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TryConvertCall
		/// </summary>
		public virtual void ExplicitVisit(TryConvertCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ParseCall
		/// </summary>
		public virtual void Visit(ParseCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ParseCall
		/// </summary>
		public virtual void ExplicitVisit(ParseCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TryParseCall
		/// </summary>
		public virtual void Visit(TryParseCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TryParseCall
		/// </summary>
		public virtual void ExplicitVisit(TryParseCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CastCall
		/// </summary>
		public virtual void Visit(CastCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CastCall
		/// </summary>
		public virtual void ExplicitVisit(CastCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TryCastCall
		/// </summary>
		public virtual void Visit(TryCastCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TryCastCall
		/// </summary>
		public virtual void ExplicitVisit(TryCastCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AtTimeZoneCall
		/// </summary>
		public virtual void Visit(AtTimeZoneCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AtTimeZoneCall
		/// </summary>
		public virtual void ExplicitVisit(AtTimeZoneCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FunctionCall
		/// </summary>
		public virtual void Visit(FunctionCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FunctionCall
		/// </summary>
		public virtual void ExplicitVisit(FunctionCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CallTarget
		/// </summary>
		public virtual void Visit(CallTarget node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CallTarget
		/// </summary>
		public virtual void ExplicitVisit(CallTarget node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExpressionCallTarget
		/// </summary>
		public virtual void Visit(ExpressionCallTarget node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExpressionCallTarget
		/// </summary>
		public virtual void ExplicitVisit(ExpressionCallTarget node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CallTarget) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MultiPartIdentifierCallTarget
		/// </summary>
		public virtual void Visit(MultiPartIdentifierCallTarget node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MultiPartIdentifierCallTarget
		/// </summary>
		public virtual void ExplicitVisit(MultiPartIdentifierCallTarget node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CallTarget) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UserDefinedTypeCallTarget
		/// </summary>
		public virtual void Visit(UserDefinedTypeCallTarget node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UserDefinedTypeCallTarget
		/// </summary>
		public virtual void ExplicitVisit(UserDefinedTypeCallTarget node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CallTarget) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LeftFunctionCall
		/// </summary>
		public virtual void Visit(LeftFunctionCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LeftFunctionCall
		/// </summary>
		public virtual void ExplicitVisit(LeftFunctionCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RightFunctionCall
		/// </summary>
		public virtual void Visit(RightFunctionCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RightFunctionCall
		/// </summary>
		public virtual void ExplicitVisit(RightFunctionCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PartitionFunctionCall
		/// </summary>
		public virtual void Visit(PartitionFunctionCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PartitionFunctionCall
		/// </summary>
		public virtual void ExplicitVisit(PartitionFunctionCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OverClause
		/// </summary>
		public virtual void Visit(OverClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OverClause
		/// </summary>
		public virtual void ExplicitVisit(OverClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WindowClause
		/// </summary>
		public virtual void Visit(WindowClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WindowClause
		/// </summary>
		public virtual void ExplicitVisit(WindowClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WindowDefinition
		/// </summary>
		public virtual void Visit(WindowDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WindowDefinition
		/// </summary>
		public virtual void ExplicitVisit(WindowDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ParameterlessCall
		/// </summary>
		public virtual void Visit(ParameterlessCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ParameterlessCall
		/// </summary>
		public virtual void ExplicitVisit(ParameterlessCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ScalarSubquery
		/// </summary>
		public virtual void Visit(ScalarSubquery node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ScalarSubquery
		/// </summary>
		public virtual void ExplicitVisit(ScalarSubquery node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OdbcFunctionCall
		/// </summary>
		public virtual void Visit(OdbcFunctionCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OdbcFunctionCall
		/// </summary>
		public virtual void ExplicitVisit(OdbcFunctionCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExtractFromExpression
		/// </summary>
		public virtual void Visit(ExtractFromExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExtractFromExpression
		/// </summary>
		public virtual void ExplicitVisit(ExtractFromExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OdbcConvertSpecification
		/// </summary>
		public virtual void Visit(OdbcConvertSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OdbcConvertSpecification
		/// </summary>
		public virtual void ExplicitVisit(OdbcConvertSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterFunctionStatement
		/// </summary>
		public virtual void Visit(AlterFunctionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterFunctionStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterFunctionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionStatementBody) node);
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BeginEndBlockStatement
		/// </summary>
		public virtual void Visit(BeginEndBlockStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BeginEndBlockStatement
		/// </summary>
		public virtual void ExplicitVisit(BeginEndBlockStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BeginEndAtomicBlockStatement
		/// </summary>
		public virtual void Visit(BeginEndAtomicBlockStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BeginEndAtomicBlockStatement
		/// </summary>
		public virtual void ExplicitVisit(BeginEndAtomicBlockStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BeginEndBlockStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AtomicBlockOption
		/// </summary>
		public virtual void Visit(AtomicBlockOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AtomicBlockOption
		/// </summary>
		public virtual void ExplicitVisit(AtomicBlockOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralAtomicBlockOption
		/// </summary>
		public virtual void Visit(LiteralAtomicBlockOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralAtomicBlockOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralAtomicBlockOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AtomicBlockOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentifierAtomicBlockOption
		/// </summary>
		public virtual void Visit(IdentifierAtomicBlockOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentifierAtomicBlockOption
		/// </summary>
		public virtual void ExplicitVisit(IdentifierAtomicBlockOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AtomicBlockOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffAtomicBlockOption
		/// </summary>
		public virtual void Visit(OnOffAtomicBlockOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffAtomicBlockOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffAtomicBlockOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AtomicBlockOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BeginTransactionStatement
		/// </summary>
		public virtual void Visit(BeginTransactionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BeginTransactionStatement
		/// </summary>
		public virtual void ExplicitVisit(BeginTransactionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TransactionStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BreakStatement
		/// </summary>
		public virtual void Visit(BreakStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BreakStatement
		/// </summary>
		public virtual void ExplicitVisit(BreakStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnWithSortOrder
		/// </summary>
		public virtual void Visit(ColumnWithSortOrder node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnWithSortOrder
		/// </summary>
		public virtual void ExplicitVisit(ColumnWithSortOrder node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CommitTransactionStatement
		/// </summary>
		public virtual void Visit(CommitTransactionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CommitTransactionStatement
		/// </summary>
		public virtual void ExplicitVisit(CommitTransactionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TransactionStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RollbackTransactionStatement
		/// </summary>
		public virtual void Visit(RollbackTransactionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RollbackTransactionStatement
		/// </summary>
		public virtual void ExplicitVisit(RollbackTransactionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TransactionStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SaveTransactionStatement
		/// </summary>
		public virtual void Visit(SaveTransactionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SaveTransactionStatement
		/// </summary>
		public virtual void ExplicitVisit(SaveTransactionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TransactionStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ContinueStatement
		/// </summary>
		public virtual void Visit(ContinueStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ContinueStatement
		/// </summary>
		public virtual void ExplicitVisit(ContinueStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateDefaultStatement
		/// </summary>
		public virtual void Visit(CreateDefaultStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateDefaultStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateDefaultStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateFunctionStatement
		/// </summary>
		public virtual void Visit(CreateFunctionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateFunctionStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateFunctionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionStatementBody) node);
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateOrAlterFunctionStatement
		/// </summary>
		public virtual void Visit(CreateOrAlterFunctionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateOrAlterFunctionStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateOrAlterFunctionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FunctionStatementBody) node);
				this.Visit((ProcedureStatementBodyBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateRuleStatement
		/// </summary>
		public virtual void Visit(CreateRuleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateRuleStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateRuleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeclareVariableElement
		/// </summary>
		public virtual void Visit(DeclareVariableElement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeclareVariableElement
		/// </summary>
		public virtual void ExplicitVisit(DeclareVariableElement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeclareVariableStatement
		/// </summary>
		public virtual void Visit(DeclareVariableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeclareVariableStatement
		/// </summary>
		public virtual void ExplicitVisit(DeclareVariableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GoToStatement
		/// </summary>
		public virtual void Visit(GoToStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GoToStatement
		/// </summary>
		public virtual void ExplicitVisit(GoToStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IfStatement
		/// </summary>
		public virtual void Visit(IfStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IfStatement
		/// </summary>
		public virtual void ExplicitVisit(IfStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LabelStatement
		/// </summary>
		public virtual void Visit(LabelStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LabelStatement
		/// </summary>
		public virtual void ExplicitVisit(LabelStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MultiPartIdentifier
		/// </summary>
		public virtual void Visit(MultiPartIdentifier node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MultiPartIdentifier
		/// </summary>
		public virtual void ExplicitVisit(MultiPartIdentifier node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaObjectName
		/// </summary>
		public virtual void Visit(SchemaObjectName node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaObjectName
		/// </summary>
		public virtual void ExplicitVisit(SchemaObjectName node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MultiPartIdentifier) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ChildObjectName
		/// </summary>
		public virtual void Visit(ChildObjectName node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ChildObjectName
		/// </summary>
		public virtual void ExplicitVisit(ChildObjectName node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SchemaObjectName) node);
				this.Visit((MultiPartIdentifier) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProcedureParameter
		/// </summary>
		public virtual void Visit(ProcedureParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProcedureParameter
		/// </summary>
		public virtual void ExplicitVisit(ProcedureParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DeclareVariableElement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TransactionStatement
		/// </summary>
		public virtual void Visit(TransactionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TransactionStatement
		/// </summary>
		public virtual void ExplicitVisit(TransactionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WhileStatement
		/// </summary>
		public virtual void Visit(WhileStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WhileStatement
		/// </summary>
		public virtual void ExplicitVisit(WhileStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeleteStatement
		/// </summary>
		public virtual void Visit(DeleteStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeleteStatement
		/// </summary>
		public virtual void ExplicitVisit(DeleteStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataModificationStatement) node);
				this.Visit((StatementWithCtesAndXmlNamespaces) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateDeleteSpecificationBase
		/// </summary>
		public virtual void Visit(UpdateDeleteSpecificationBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateDeleteSpecificationBase
		/// </summary>
		public virtual void ExplicitVisit(UpdateDeleteSpecificationBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataModificationSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeleteSpecification
		/// </summary>
		public virtual void Visit(DeleteSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeleteSpecification
		/// </summary>
		public virtual void ExplicitVisit(DeleteSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((UpdateDeleteSpecificationBase) node);
				this.Visit((DataModificationSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InsertStatement
		/// </summary>
		public virtual void Visit(InsertStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InsertStatement
		/// </summary>
		public virtual void ExplicitVisit(InsertStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataModificationStatement) node);
				this.Visit((StatementWithCtesAndXmlNamespaces) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InsertSpecification
		/// </summary>
		public virtual void Visit(InsertSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InsertSpecification
		/// </summary>
		public virtual void ExplicitVisit(InsertSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataModificationSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateStatement
		/// </summary>
		public virtual void Visit(UpdateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateStatement
		/// </summary>
		public virtual void ExplicitVisit(UpdateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataModificationStatement) node);
				this.Visit((StatementWithCtesAndXmlNamespaces) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateSpecification
		/// </summary>
		public virtual void Visit(UpdateSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateSpecification
		/// </summary>
		public virtual void ExplicitVisit(UpdateSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((UpdateDeleteSpecificationBase) node);
				this.Visit((DataModificationSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSchemaStatement
		/// </summary>
		public virtual void Visit(CreateSchemaStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSchemaStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSchemaStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WaitForStatement
		/// </summary>
		public virtual void Visit(WaitForStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WaitForStatement
		/// </summary>
		public virtual void ExplicitVisit(WaitForStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ReadTextStatement
		/// </summary>
		public virtual void Visit(ReadTextStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ReadTextStatement
		/// </summary>
		public virtual void ExplicitVisit(ReadTextStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateTextStatement
		/// </summary>
		public virtual void Visit(UpdateTextStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateTextStatement
		/// </summary>
		public virtual void ExplicitVisit(UpdateTextStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TextModificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WriteTextStatement
		/// </summary>
		public virtual void Visit(WriteTextStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WriteTextStatement
		/// </summary>
		public virtual void ExplicitVisit(WriteTextStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TextModificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TextModificationStatement
		/// </summary>
		public virtual void Visit(TextModificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TextModificationStatement
		/// </summary>
		public virtual void ExplicitVisit(TextModificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LineNoStatement
		/// </summary>
		public virtual void Visit(LineNoStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LineNoStatement
		/// </summary>
		public virtual void ExplicitVisit(LineNoStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityStatement
		/// </summary>
		public virtual void Visit(SecurityStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityStatement
		/// </summary>
		public virtual void ExplicitVisit(SecurityStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GrantStatement
		/// </summary>
		public virtual void Visit(GrantStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GrantStatement
		/// </summary>
		public virtual void ExplicitVisit(GrantStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DenyStatement
		/// </summary>
		public virtual void Visit(DenyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DenyStatement
		/// </summary>
		public virtual void ExplicitVisit(DenyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RevokeStatement
		/// </summary>
		public virtual void Visit(RevokeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RevokeStatement
		/// </summary>
		public virtual void ExplicitVisit(RevokeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterAuthorizationStatement
		/// </summary>
		public virtual void Visit(AlterAuthorizationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterAuthorizationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterAuthorizationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for Permission
		/// </summary>
		public virtual void Visit(Permission node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for Permission
		/// </summary>
		public virtual void ExplicitVisit(Permission node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityTargetObject
		/// </summary>
		public virtual void Visit(SecurityTargetObject node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityTargetObject
		/// </summary>
		public virtual void ExplicitVisit(SecurityTargetObject node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityTargetObjectName
		/// </summary>
		public virtual void Visit(SecurityTargetObjectName node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityTargetObjectName
		/// </summary>
		public virtual void ExplicitVisit(SecurityTargetObjectName node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityPrincipal
		/// </summary>
		public virtual void Visit(SecurityPrincipal node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityPrincipal
		/// </summary>
		public virtual void ExplicitVisit(SecurityPrincipal node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityStatementBody80
		/// </summary>
		public virtual void Visit(SecurityStatementBody80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityStatementBody80
		/// </summary>
		public virtual void ExplicitVisit(SecurityStatementBody80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GrantStatement80
		/// </summary>
		public virtual void Visit(GrantStatement80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GrantStatement80
		/// </summary>
		public virtual void ExplicitVisit(GrantStatement80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityStatementBody80) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DenyStatement80
		/// </summary>
		public virtual void Visit(DenyStatement80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DenyStatement80
		/// </summary>
		public virtual void ExplicitVisit(DenyStatement80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityStatementBody80) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RevokeStatement80
		/// </summary>
		public virtual void Visit(RevokeStatement80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RevokeStatement80
		/// </summary>
		public virtual void ExplicitVisit(RevokeStatement80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityStatementBody80) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityElement80
		/// </summary>
		public virtual void Visit(SecurityElement80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityElement80
		/// </summary>
		public virtual void ExplicitVisit(SecurityElement80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CommandSecurityElement80
		/// </summary>
		public virtual void Visit(CommandSecurityElement80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CommandSecurityElement80
		/// </summary>
		public virtual void ExplicitVisit(CommandSecurityElement80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityElement80) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PrivilegeSecurityElement80
		/// </summary>
		public virtual void Visit(PrivilegeSecurityElement80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PrivilegeSecurityElement80
		/// </summary>
		public virtual void ExplicitVisit(PrivilegeSecurityElement80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityElement80) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for Privilege80
		/// </summary>
		public virtual void Visit(Privilege80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for Privilege80
		/// </summary>
		public virtual void ExplicitVisit(Privilege80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityUserClause80
		/// </summary>
		public virtual void Visit(SecurityUserClause80 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityUserClause80
		/// </summary>
		public virtual void ExplicitVisit(SecurityUserClause80 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SqlCommandIdentifier
		/// </summary>
		public virtual void Visit(SqlCommandIdentifier node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SqlCommandIdentifier
		/// </summary>
		public virtual void ExplicitVisit(SqlCommandIdentifier node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Identifier) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetClause
		/// </summary>
		public virtual void Visit(SetClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetClause
		/// </summary>
		public virtual void ExplicitVisit(SetClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AssignmentSetClause
		/// </summary>
		public virtual void Visit(AssignmentSetClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AssignmentSetClause
		/// </summary>
		public virtual void ExplicitVisit(AssignmentSetClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FunctionCallSetClause
		/// </summary>
		public virtual void Visit(FunctionCallSetClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FunctionCallSetClause
		/// </summary>
		public virtual void ExplicitVisit(FunctionCallSetClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetClause) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InsertSource
		/// </summary>
		public virtual void Visit(InsertSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InsertSource
		/// </summary>
		public virtual void ExplicitVisit(InsertSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ValuesInsertSource
		/// </summary>
		public virtual void Visit(ValuesInsertSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ValuesInsertSource
		/// </summary>
		public virtual void ExplicitVisit(ValuesInsertSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((InsertSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectInsertSource
		/// </summary>
		public virtual void Visit(SelectInsertSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectInsertSource
		/// </summary>
		public virtual void ExplicitVisit(SelectInsertSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((InsertSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteInsertSource
		/// </summary>
		public virtual void Visit(ExecuteInsertSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteInsertSource
		/// </summary>
		public virtual void ExplicitVisit(ExecuteInsertSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((InsertSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RowValue
		/// </summary>
		public virtual void Visit(RowValue node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RowValue
		/// </summary>
		public virtual void ExplicitVisit(RowValue node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PrintStatement
		/// </summary>
		public virtual void Visit(PrintStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PrintStatement
		/// </summary>
		public virtual void ExplicitVisit(PrintStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateCall
		/// </summary>
		public virtual void Visit(UpdateCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateCall
		/// </summary>
		public virtual void ExplicitVisit(UpdateCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TSEqualCall
		/// </summary>
		public virtual void Visit(TSEqualCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TSEqualCall
		/// </summary>
		public virtual void ExplicitVisit(TSEqualCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PrimaryExpression
		/// </summary>
		public virtual void Visit(PrimaryExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PrimaryExpression
		/// </summary>
		public virtual void ExplicitVisit(PrimaryExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for Literal
		/// </summary>
		public virtual void Visit(Literal node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for Literal
		/// </summary>
		public virtual void ExplicitVisit(Literal node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IntegerLiteral
		/// </summary>
		public virtual void Visit(IntegerLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IntegerLiteral
		/// </summary>
		public virtual void ExplicitVisit(IntegerLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for NumericLiteral
		/// </summary>
		public virtual void Visit(NumericLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for NumericLiteral
		/// </summary>
		public virtual void ExplicitVisit(NumericLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RealLiteral
		/// </summary>
		public virtual void Visit(RealLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RealLiteral
		/// </summary>
		public virtual void ExplicitVisit(RealLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MoneyLiteral
		/// </summary>
		public virtual void Visit(MoneyLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MoneyLiteral
		/// </summary>
		public virtual void ExplicitVisit(MoneyLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BinaryLiteral
		/// </summary>
		public virtual void Visit(BinaryLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BinaryLiteral
		/// </summary>
		public virtual void ExplicitVisit(BinaryLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StringLiteral
		/// </summary>
		public virtual void Visit(StringLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StringLiteral
		/// </summary>
		public virtual void ExplicitVisit(StringLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for NullLiteral
		/// </summary>
		public virtual void Visit(NullLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for NullLiteral
		/// </summary>
		public virtual void ExplicitVisit(NullLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentifierLiteral
		/// </summary>
		public virtual void Visit(IdentifierLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentifierLiteral
		/// </summary>
		public virtual void ExplicitVisit(IdentifierLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DefaultLiteral
		/// </summary>
		public virtual void Visit(DefaultLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DefaultLiteral
		/// </summary>
		public virtual void ExplicitVisit(DefaultLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxLiteral
		/// </summary>
		public virtual void Visit(MaxLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxLiteral
		/// </summary>
		public virtual void ExplicitVisit(MaxLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OdbcLiteral
		/// </summary>
		public virtual void Visit(OdbcLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OdbcLiteral
		/// </summary>
		public virtual void ExplicitVisit(OdbcLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Literal) node);
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralRange
		/// </summary>
		public virtual void Visit(LiteralRange node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralRange
		/// </summary>
		public virtual void ExplicitVisit(LiteralRange node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ValueExpression
		/// </summary>
		public virtual void Visit(ValueExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ValueExpression
		/// </summary>
		public virtual void ExplicitVisit(ValueExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for VariableReference
		/// </summary>
		public virtual void Visit(VariableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for VariableReference
		/// </summary>
		public virtual void ExplicitVisit(VariableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OptionValue
		/// </summary>
		public virtual void Visit(OptionValue node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OptionValue
		/// </summary>
		public virtual void ExplicitVisit(OptionValue node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffOptionValue
		/// </summary>
		public virtual void Visit(OnOffOptionValue node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffOptionValue
		/// </summary>
		public virtual void ExplicitVisit(OnOffOptionValue node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OptionValue) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralOptionValue
		/// </summary>
		public virtual void Visit(LiteralOptionValue node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralOptionValue
		/// </summary>
		public virtual void ExplicitVisit(LiteralOptionValue node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OptionValue) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GlobalVariableExpression
		/// </summary>
		public virtual void Visit(GlobalVariableExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GlobalVariableExpression
		/// </summary>
		public virtual void ExplicitVisit(GlobalVariableExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ValueExpression) node);
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentifierOrValueExpression
		/// </summary>
		public virtual void Visit(IdentifierOrValueExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentifierOrValueExpression
		/// </summary>
		public virtual void ExplicitVisit(IdentifierOrValueExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentifierOrScalarExpression
		/// </summary>
		public virtual void Visit(IdentifierOrScalarExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentifierOrScalarExpression
		/// </summary>
		public virtual void ExplicitVisit(IdentifierOrScalarExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaObjectNameOrValueExpression
		/// </summary>
		public virtual void Visit(SchemaObjectNameOrValueExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaObjectNameOrValueExpression
		/// </summary>
		public virtual void ExplicitVisit(SchemaObjectNameOrValueExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ParenthesisExpression
		/// </summary>
		public virtual void Visit(ParenthesisExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ParenthesisExpression
		/// </summary>
		public virtual void ExplicitVisit(ParenthesisExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnReferenceExpression
		/// </summary>
		public virtual void Visit(ColumnReferenceExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnReferenceExpression
		/// </summary>
		public virtual void ExplicitVisit(ColumnReferenceExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for NextValueForExpression
		/// </summary>
		public virtual void Visit(NextValueForExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for NextValueForExpression
		/// </summary>
		public virtual void ExplicitVisit(NextValueForExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrimaryExpression) node);
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SequenceStatement
		/// </summary>
		public virtual void Visit(SequenceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SequenceStatement
		/// </summary>
		public virtual void ExplicitVisit(SequenceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SequenceOption
		/// </summary>
		public virtual void Visit(SequenceOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SequenceOption
		/// </summary>
		public virtual void ExplicitVisit(SequenceOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DataTypeSequenceOption
		/// </summary>
		public virtual void Visit(DataTypeSequenceOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DataTypeSequenceOption
		/// </summary>
		public virtual void ExplicitVisit(DataTypeSequenceOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SequenceOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ScalarExpressionSequenceOption
		/// </summary>
		public virtual void Visit(ScalarExpressionSequenceOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ScalarExpressionSequenceOption
		/// </summary>
		public virtual void ExplicitVisit(ScalarExpressionSequenceOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SequenceOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSequenceStatement
		/// </summary>
		public virtual void Visit(CreateSequenceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSequenceStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSequenceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SequenceStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterSequenceStatement
		/// </summary>
		public virtual void Visit(AlterSequenceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterSequenceStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterSequenceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SequenceStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSequenceStatement
		/// </summary>
		public virtual void Visit(DropSequenceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSequenceStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSequenceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityPolicyStatement
		/// </summary>
		public virtual void Visit(SecurityPolicyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityPolicyStatement
		/// </summary>
		public virtual void ExplicitVisit(SecurityPolicyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityPredicateAction
		/// </summary>
		public virtual void Visit(SecurityPredicateAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityPredicateAction
		/// </summary>
		public virtual void ExplicitVisit(SecurityPredicateAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecurityPolicyOption
		/// </summary>
		public virtual void Visit(SecurityPolicyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecurityPolicyOption
		/// </summary>
		public virtual void ExplicitVisit(SecurityPolicyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSecurityPolicyStatement
		/// </summary>
		public virtual void Visit(CreateSecurityPolicyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSecurityPolicyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSecurityPolicyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityPolicyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterSecurityPolicyStatement
		/// </summary>
		public virtual void Visit(AlterSecurityPolicyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterSecurityPolicyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterSecurityPolicyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SecurityPolicyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSecurityPolicyStatement
		/// </summary>
		public virtual void Visit(DropSecurityPolicyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSecurityPolicyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSecurityPolicyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateColumnMasterKeyStatement
		/// </summary>
		public virtual void Visit(CreateColumnMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateColumnMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateColumnMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnMasterKeyParameter
		/// </summary>
		public virtual void Visit(ColumnMasterKeyParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnMasterKeyParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnMasterKeyParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnMasterKeyStoreProviderNameParameter
		/// </summary>
		public virtual void Visit(ColumnMasterKeyStoreProviderNameParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnMasterKeyStoreProviderNameParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnMasterKeyStoreProviderNameParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnMasterKeyParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnMasterKeyPathParameter
		/// </summary>
		public virtual void Visit(ColumnMasterKeyPathParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnMasterKeyPathParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnMasterKeyPathParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnMasterKeyParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnMasterKeyEnclaveComputationsParameter
		/// </summary>
		public virtual void Visit(ColumnMasterKeyEnclaveComputationsParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnMasterKeyEnclaveComputationsParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnMasterKeyEnclaveComputationsParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnMasterKeyParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropColumnMasterKeyStatement
		/// </summary>
		public virtual void Visit(DropColumnMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropColumnMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropColumnMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(ColumnEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateColumnEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(CreateColumnEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateColumnEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateColumnEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterColumnEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(AlterColumnEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterColumnEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterColumnEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropColumnEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(DropColumnEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropColumnEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropColumnEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionKeyValue
		/// </summary>
		public virtual void Visit(ColumnEncryptionKeyValue node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionKeyValue
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionKeyValue node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionKeyValueParameter
		/// </summary>
		public virtual void Visit(ColumnEncryptionKeyValueParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionKeyValueParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionKeyValueParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnMasterKeyNameParameter
		/// </summary>
		public virtual void Visit(ColumnMasterKeyNameParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnMasterKeyNameParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnMasterKeyNameParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionKeyValueParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionAlgorithmNameParameter
		/// </summary>
		public virtual void Visit(ColumnEncryptionAlgorithmNameParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionAlgorithmNameParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionAlgorithmNameParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionKeyValueParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EncryptedValueParameter
		/// </summary>
		public virtual void Visit(EncryptedValueParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EncryptedValueParameter
		/// </summary>
		public virtual void ExplicitVisit(EncryptedValueParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionKeyValueParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableStatement
		/// </summary>
		public virtual void Visit(ExternalTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableOption
		/// </summary>
		public virtual void Visit(ExternalTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableLiteralOrIdentifierOption
		/// </summary>
		public virtual void Visit(ExternalTableLiteralOrIdentifierOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableLiteralOrIdentifierOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableLiteralOrIdentifierOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalTableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableDistributionOption
		/// </summary>
		public virtual void Visit(ExternalTableDistributionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableDistributionOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableDistributionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalTableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableRejectTypeOption
		/// </summary>
		public virtual void Visit(ExternalTableRejectTypeOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableRejectTypeOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableRejectTypeOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalTableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableDistributionPolicy
		/// </summary>
		public virtual void Visit(ExternalTableDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableReplicatedDistributionPolicy
		/// </summary>
		public virtual void Visit(ExternalTableReplicatedDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableReplicatedDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableReplicatedDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalTableDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableRoundRobinDistributionPolicy
		/// </summary>
		public virtual void Visit(ExternalTableRoundRobinDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableRoundRobinDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableRoundRobinDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalTableDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableShardedDistributionPolicy
		/// </summary>
		public virtual void Visit(ExternalTableShardedDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableShardedDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableShardedDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalTableDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalTableStatement
		/// </summary>
		public virtual void Visit(CreateExternalTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalTableStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalTableStatement
		/// </summary>
		public virtual void Visit(DropExternalTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalTableStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalDataSourceStatement
		/// </summary>
		public virtual void Visit(ExternalDataSourceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalDataSourceStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalDataSourceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalDataSourceOption
		/// </summary>
		public virtual void Visit(ExternalDataSourceOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalDataSourceOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalDataSourceOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalDataSourceLiteralOrIdentifierOption
		/// </summary>
		public virtual void Visit(ExternalDataSourceLiteralOrIdentifierOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalDataSourceLiteralOrIdentifierOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalDataSourceLiteralOrIdentifierOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalDataSourceOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalDataSourceStatement
		/// </summary>
		public virtual void Visit(CreateExternalDataSourceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalDataSourceStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalDataSourceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalDataSourceStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterExternalDataSourceStatement
		/// </summary>
		public virtual void Visit(AlterExternalDataSourceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterExternalDataSourceStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterExternalDataSourceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalDataSourceStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalDataSourceStatement
		/// </summary>
		public virtual void Visit(DropExternalDataSourceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalDataSourceStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalDataSourceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalStreamStatement
		/// </summary>
		public virtual void Visit(ExternalStreamStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalStreamStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalStreamStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalStreamOption
		/// </summary>
		public virtual void Visit(ExternalStreamOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalStreamOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalStreamOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalStreamLiteralOrIdentifierOption
		/// </summary>
		public virtual void Visit(ExternalStreamLiteralOrIdentifierOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalStreamLiteralOrIdentifierOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalStreamLiteralOrIdentifierOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalStreamOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalStreamStatement
		/// </summary>
		public virtual void Visit(CreateExternalStreamStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalStreamStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalStreamStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalStreamStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalStreamStatement
		/// </summary>
		public virtual void Visit(DropExternalStreamStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalStreamStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalStreamStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalFileFormatStatement
		/// </summary>
		public virtual void Visit(ExternalFileFormatStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalFileFormatStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalFileFormatStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalFileFormatOption
		/// </summary>
		public virtual void Visit(ExternalFileFormatOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalFileFormatOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalFileFormatOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalFileFormatLiteralOption
		/// </summary>
		public virtual void Visit(ExternalFileFormatLiteralOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalFileFormatLiteralOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalFileFormatLiteralOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalFileFormatOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalFileFormatUseDefaultTypeOption
		/// </summary>
		public virtual void Visit(ExternalFileFormatUseDefaultTypeOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalFileFormatUseDefaultTypeOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalFileFormatUseDefaultTypeOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalFileFormatOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalFileFormatContainerOption
		/// </summary>
		public virtual void Visit(ExternalFileFormatContainerOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalFileFormatContainerOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalFileFormatContainerOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalFileFormatOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalFileFormatStatement
		/// </summary>
		public virtual void Visit(CreateExternalFileFormatStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalFileFormatStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalFileFormatStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalFileFormatStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalFileFormatStatement
		/// </summary>
		public virtual void Visit(DropExternalFileFormatStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalFileFormatStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalFileFormatStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalStreamingJobStatement
		/// </summary>
		public virtual void Visit(ExternalStreamingJobStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalStreamingJobStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalStreamingJobStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalStreamingJobStatement
		/// </summary>
		public virtual void Visit(CreateExternalStreamingJobStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalStreamingJobStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalStreamingJobStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalStreamingJobStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalStreamingJobStatement
		/// </summary>
		public virtual void Visit(DropExternalStreamingJobStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalStreamingJobStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalStreamingJobStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AssemblyStatement
		/// </summary>
		public virtual void Visit(AssemblyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AssemblyStatement
		/// </summary>
		public virtual void ExplicitVisit(AssemblyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateAssemblyStatement
		/// </summary>
		public virtual void Visit(CreateAssemblyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateAssemblyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateAssemblyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AssemblyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterAssemblyStatement
		/// </summary>
		public virtual void Visit(AlterAssemblyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterAssemblyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterAssemblyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AssemblyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AssemblyOption
		/// </summary>
		public virtual void Visit(AssemblyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AssemblyOption
		/// </summary>
		public virtual void ExplicitVisit(AssemblyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffAssemblyOption
		/// </summary>
		public virtual void Visit(OnOffAssemblyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffAssemblyOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffAssemblyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AssemblyOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PermissionSetAssemblyOption
		/// </summary>
		public virtual void Visit(PermissionSetAssemblyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PermissionSetAssemblyOption
		/// </summary>
		public virtual void ExplicitVisit(PermissionSetAssemblyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AssemblyOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AddFileSpec
		/// </summary>
		public virtual void Visit(AddFileSpec node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AddFileSpec
		/// </summary>
		public virtual void ExplicitVisit(AddFileSpec node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateXmlSchemaCollectionStatement
		/// </summary>
		public virtual void Visit(CreateXmlSchemaCollectionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateXmlSchemaCollectionStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateXmlSchemaCollectionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterXmlSchemaCollectionStatement
		/// </summary>
		public virtual void Visit(AlterXmlSchemaCollectionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterXmlSchemaCollectionStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterXmlSchemaCollectionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropXmlSchemaCollectionStatement
		/// </summary>
		public virtual void Visit(DropXmlSchemaCollectionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropXmlSchemaCollectionStatement
		/// </summary>
		public virtual void ExplicitVisit(DropXmlSchemaCollectionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AssemblyName
		/// </summary>
		public virtual void Visit(AssemblyName node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AssemblyName
		/// </summary>
		public virtual void ExplicitVisit(AssemblyName node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableStatement
		/// </summary>
		public virtual void Visit(AlterTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableAlterPartitionStatement
		/// </summary>
		public virtual void Visit(AlterTableAlterPartitionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableAlterPartitionStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableAlterPartitionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableRebuildStatement
		/// </summary>
		public virtual void Visit(AlterTableRebuildStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableRebuildStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableRebuildStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableChangeTrackingModificationStatement
		/// </summary>
		public virtual void Visit(AlterTableChangeTrackingModificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableChangeTrackingModificationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableChangeTrackingModificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableFileTableNamespaceStatement
		/// </summary>
		public virtual void Visit(AlterTableFileTableNamespaceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableFileTableNamespaceStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableFileTableNamespaceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableSetStatement
		/// </summary>
		public virtual void Visit(AlterTableSetStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableSetStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableSetStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableAddClusterByStatement
		/// </summary>
		public virtual void Visit(AlterTableAddClusterByStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableAddClusterByStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableAddClusterByStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableOption
		/// </summary>
		public virtual void Visit(TableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableOption
		/// </summary>
		public virtual void ExplicitVisit(TableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LockEscalationTableOption
		/// </summary>
		public virtual void Visit(LockEscalationTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LockEscalationTableOption
		/// </summary>
		public virtual void ExplicitVisit(LockEscalationTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileStreamOnTableOption
		/// </summary>
		public virtual void Visit(FileStreamOnTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileStreamOnTableOption
		/// </summary>
		public virtual void ExplicitVisit(FileStreamOnTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileTableDirectoryTableOption
		/// </summary>
		public virtual void Visit(FileTableDirectoryTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileTableDirectoryTableOption
		/// </summary>
		public virtual void ExplicitVisit(FileTableDirectoryTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileTableCollateFileNameTableOption
		/// </summary>
		public virtual void Visit(FileTableCollateFileNameTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileTableCollateFileNameTableOption
		/// </summary>
		public virtual void ExplicitVisit(FileTableCollateFileNameTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileTableConstraintNameTableOption
		/// </summary>
		public virtual void Visit(FileTableConstraintNameTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileTableConstraintNameTableOption
		/// </summary>
		public virtual void ExplicitVisit(FileTableConstraintNameTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MemoryOptimizedTableOption
		/// </summary>
		public virtual void Visit(MemoryOptimizedTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MemoryOptimizedTableOption
		/// </summary>
		public virtual void ExplicitVisit(MemoryOptimizedTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DurabilityTableOption
		/// </summary>
		public virtual void Visit(DurabilityTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DurabilityTableOption
		/// </summary>
		public virtual void ExplicitVisit(DurabilityTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteDataArchiveTableOption
		/// </summary>
		public virtual void Visit(RemoteDataArchiveTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteDataArchiveTableOption
		/// </summary>
		public virtual void ExplicitVisit(RemoteDataArchiveTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteDataArchiveAlterTableOption
		/// </summary>
		public virtual void Visit(RemoteDataArchiveAlterTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteDataArchiveAlterTableOption
		/// </summary>
		public virtual void ExplicitVisit(RemoteDataArchiveAlterTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteDataArchiveDatabaseOption
		/// </summary>
		public virtual void Visit(RemoteDataArchiveDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteDataArchiveDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(RemoteDataArchiveDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteDataArchiveDatabaseSetting
		/// </summary>
		public virtual void Visit(RemoteDataArchiveDatabaseSetting node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteDataArchiveDatabaseSetting
		/// </summary>
		public virtual void ExplicitVisit(RemoteDataArchiveDatabaseSetting node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteDataArchiveDbServerSetting
		/// </summary>
		public virtual void Visit(RemoteDataArchiveDbServerSetting node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteDataArchiveDbServerSetting
		/// </summary>
		public virtual void ExplicitVisit(RemoteDataArchiveDbServerSetting node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RemoteDataArchiveDatabaseSetting) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteDataArchiveDbCredentialSetting
		/// </summary>
		public virtual void Visit(RemoteDataArchiveDbCredentialSetting node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteDataArchiveDbCredentialSetting
		/// </summary>
		public virtual void ExplicitVisit(RemoteDataArchiveDbCredentialSetting node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RemoteDataArchiveDatabaseSetting) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteDataArchiveDbFederatedServiceAccountSetting
		/// </summary>
		public virtual void Visit(RemoteDataArchiveDbFederatedServiceAccountSetting node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteDataArchiveDbFederatedServiceAccountSetting
		/// </summary>
		public virtual void ExplicitVisit(RemoteDataArchiveDbFederatedServiceAccountSetting node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RemoteDataArchiveDatabaseSetting) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RetentionPeriodDefinition
		/// </summary>
		public virtual void Visit(RetentionPeriodDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RetentionPeriodDefinition
		/// </summary>
		public virtual void ExplicitVisit(RetentionPeriodDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SystemVersioningTableOption
		/// </summary>
		public virtual void Visit(SystemVersioningTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SystemVersioningTableOption
		/// </summary>
		public virtual void ExplicitVisit(SystemVersioningTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LedgerTableOption
		/// </summary>
		public virtual void Visit(LedgerTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LedgerTableOption
		/// </summary>
		public virtual void ExplicitVisit(LedgerTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LedgerViewOption
		/// </summary>
		public virtual void Visit(LedgerViewOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LedgerViewOption
		/// </summary>
		public virtual void ExplicitVisit(LedgerViewOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DataRetentionTableOption
		/// </summary>
		public virtual void Visit(DataRetentionTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DataRetentionTableOption
		/// </summary>
		public virtual void ExplicitVisit(DataRetentionTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableAddTableElementStatement
		/// </summary>
		public virtual void Visit(AlterTableAddTableElementStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableAddTableElementStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableAddTableElementStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableConstraintModificationStatement
		/// </summary>
		public virtual void Visit(AlterTableConstraintModificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableConstraintModificationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableConstraintModificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableSwitchStatement
		/// </summary>
		public virtual void Visit(AlterTableSwitchStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableSwitchStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableSwitchStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableSwitchOption
		/// </summary>
		public virtual void Visit(TableSwitchOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableSwitchOption
		/// </summary>
		public virtual void ExplicitVisit(TableSwitchOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LowPriorityLockWaitTableSwitchOption
		/// </summary>
		public virtual void Visit(LowPriorityLockWaitTableSwitchOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LowPriorityLockWaitTableSwitchOption
		/// </summary>
		public virtual void ExplicitVisit(LowPriorityLockWaitTableSwitchOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableSwitchOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TruncateTargetTableSwitchOption
		/// </summary>
		public virtual void Visit(TruncateTargetTableSwitchOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TruncateTargetTableSwitchOption
		/// </summary>
		public virtual void ExplicitVisit(TruncateTargetTableSwitchOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableSwitchOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropClusteredConstraintOption
		/// </summary>
		public virtual void Visit(DropClusteredConstraintOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropClusteredConstraintOption
		/// </summary>
		public virtual void ExplicitVisit(DropClusteredConstraintOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropClusteredConstraintStateOption
		/// </summary>
		public virtual void Visit(DropClusteredConstraintStateOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropClusteredConstraintStateOption
		/// </summary>
		public virtual void ExplicitVisit(DropClusteredConstraintStateOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropClusteredConstraintOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropClusteredConstraintValueOption
		/// </summary>
		public virtual void Visit(DropClusteredConstraintValueOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropClusteredConstraintValueOption
		/// </summary>
		public virtual void ExplicitVisit(DropClusteredConstraintValueOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropClusteredConstraintOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropClusteredConstraintMoveOption
		/// </summary>
		public virtual void Visit(DropClusteredConstraintMoveOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropClusteredConstraintMoveOption
		/// </summary>
		public virtual void ExplicitVisit(DropClusteredConstraintMoveOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropClusteredConstraintOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropClusteredConstraintWaitAtLowPriorityLockOption
		/// </summary>
		public virtual void Visit(DropClusteredConstraintWaitAtLowPriorityLockOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropClusteredConstraintWaitAtLowPriorityLockOption
		/// </summary>
		public virtual void ExplicitVisit(DropClusteredConstraintWaitAtLowPriorityLockOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropClusteredConstraintOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableDropTableElement
		/// </summary>
		public virtual void Visit(AlterTableDropTableElement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableDropTableElement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableDropTableElement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableDropTableElementStatement
		/// </summary>
		public virtual void Visit(AlterTableDropTableElementStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableDropTableElementStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableDropTableElementStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableTriggerModificationStatement
		/// </summary>
		public virtual void Visit(AlterTableTriggerModificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableTriggerModificationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableTriggerModificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EnableDisableTriggerStatement
		/// </summary>
		public virtual void Visit(EnableDisableTriggerStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EnableDisableTriggerStatement
		/// </summary>
		public virtual void ExplicitVisit(EnableDisableTriggerStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TryCatchStatement
		/// </summary>
		public virtual void Visit(TryCatchStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TryCatchStatement
		/// </summary>
		public virtual void ExplicitVisit(TryCatchStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateTypeStatement
		/// </summary>
		public virtual void Visit(CreateTypeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateTypeStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateTypeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateTypeUdtStatement
		/// </summary>
		public virtual void Visit(CreateTypeUdtStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateTypeUdtStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateTypeUdtStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateTypeStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateTypeUddtStatement
		/// </summary>
		public virtual void Visit(CreateTypeUddtStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateTypeUddtStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateTypeUddtStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateTypeStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSynonymStatement
		/// </summary>
		public virtual void Visit(CreateSynonymStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSynonymStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSynonymStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteAsClause
		/// </summary>
		public virtual void Visit(ExecuteAsClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteAsClause
		/// </summary>
		public virtual void ExplicitVisit(ExecuteAsClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueueOption
		/// </summary>
		public virtual void Visit(QueueOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueueOption
		/// </summary>
		public virtual void ExplicitVisit(QueueOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueueStateOption
		/// </summary>
		public virtual void Visit(QueueStateOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueueStateOption
		/// </summary>
		public virtual void ExplicitVisit(QueueStateOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueueOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueueProcedureOption
		/// </summary>
		public virtual void Visit(QueueProcedureOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueueProcedureOption
		/// </summary>
		public virtual void ExplicitVisit(QueueProcedureOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueueOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueueValueOption
		/// </summary>
		public virtual void Visit(QueueValueOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueueValueOption
		/// </summary>
		public virtual void ExplicitVisit(QueueValueOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueueOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueueExecuteAsOption
		/// </summary>
		public virtual void Visit(QueueExecuteAsOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueueExecuteAsOption
		/// </summary>
		public virtual void ExplicitVisit(QueueExecuteAsOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueueOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RouteOption
		/// </summary>
		public virtual void Visit(RouteOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RouteOption
		/// </summary>
		public virtual void ExplicitVisit(RouteOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RouteStatement
		/// </summary>
		public virtual void Visit(RouteStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RouteStatement
		/// </summary>
		public virtual void ExplicitVisit(RouteStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterRouteStatement
		/// </summary>
		public virtual void Visit(AlterRouteStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterRouteStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterRouteStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RouteStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateRouteStatement
		/// </summary>
		public virtual void Visit(CreateRouteStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateRouteStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateRouteStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RouteStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueueStatement
		/// </summary>
		public virtual void Visit(QueueStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueueStatement
		/// </summary>
		public virtual void ExplicitVisit(QueueStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterQueueStatement
		/// </summary>
		public virtual void Visit(AlterQueueStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterQueueStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterQueueStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueueStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateQueueStatement
		/// </summary>
		public virtual void Visit(CreateQueueStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateQueueStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateQueueStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueueStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IndexDefinition
		/// </summary>
		public virtual void Visit(IndexDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IndexDefinition
		/// </summary>
		public virtual void ExplicitVisit(IndexDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SystemTimePeriodDefinition
		/// </summary>
		public virtual void Visit(SystemTimePeriodDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SystemTimePeriodDefinition
		/// </summary>
		public virtual void ExplicitVisit(SystemTimePeriodDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IndexStatement
		/// </summary>
		public virtual void Visit(IndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IndexStatement
		/// </summary>
		public virtual void ExplicitVisit(IndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IndexType
		/// </summary>
		public virtual void Visit(IndexType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IndexType
		/// </summary>
		public virtual void ExplicitVisit(IndexType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PartitionSpecifier
		/// </summary>
		public virtual void Visit(PartitionSpecifier node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PartitionSpecifier
		/// </summary>
		public virtual void ExplicitVisit(PartitionSpecifier node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterIndexStatement
		/// </summary>
		public virtual void Visit(AlterIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateXmlIndexStatement
		/// </summary>
		public virtual void Visit(CreateXmlIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateXmlIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateXmlIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSelectiveXmlIndexStatement
		/// </summary>
		public virtual void Visit(CreateSelectiveXmlIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSelectiveXmlIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSelectiveXmlIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileGroupOrPartitionScheme
		/// </summary>
		public virtual void Visit(FileGroupOrPartitionScheme node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileGroupOrPartitionScheme
		/// </summary>
		public virtual void ExplicitVisit(FileGroupOrPartitionScheme node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateIndexStatement
		/// </summary>
		public virtual void Visit(CreateIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IndexOption
		/// </summary>
		public virtual void Visit(IndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IndexOption
		/// </summary>
		public virtual void ExplicitVisit(IndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IndexStateOption
		/// </summary>
		public virtual void Visit(IndexStateOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IndexStateOption
		/// </summary>
		public virtual void ExplicitVisit(IndexStateOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IndexExpressionOption
		/// </summary>
		public virtual void Visit(IndexExpressionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IndexExpressionOption
		/// </summary>
		public virtual void ExplicitVisit(IndexExpressionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxDurationOption
		/// </summary>
		public virtual void Visit(MaxDurationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxDurationOption
		/// </summary>
		public virtual void ExplicitVisit(MaxDurationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WaitAtLowPriorityOption
		/// </summary>
		public virtual void Visit(WaitAtLowPriorityOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WaitAtLowPriorityOption
		/// </summary>
		public virtual void ExplicitVisit(WaitAtLowPriorityOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnlineIndexOption
		/// </summary>
		public virtual void Visit(OnlineIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnlineIndexOption
		/// </summary>
		public virtual void ExplicitVisit(OnlineIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexStateOption) node);
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IgnoreDupKeyIndexOption
		/// </summary>
		public virtual void Visit(IgnoreDupKeyIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IgnoreDupKeyIndexOption
		/// </summary>
		public virtual void ExplicitVisit(IgnoreDupKeyIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexStateOption) node);
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OrderIndexOption
		/// </summary>
		public virtual void Visit(OrderIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OrderIndexOption
		/// </summary>
		public virtual void ExplicitVisit(OrderIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnlineIndexLowPriorityLockWaitOption
		/// </summary>
		public virtual void Visit(OnlineIndexLowPriorityLockWaitOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnlineIndexLowPriorityLockWaitOption
		/// </summary>
		public virtual void ExplicitVisit(OnlineIndexLowPriorityLockWaitOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LowPriorityLockWaitOption
		/// </summary>
		public virtual void Visit(LowPriorityLockWaitOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LowPriorityLockWaitOption
		/// </summary>
		public virtual void ExplicitVisit(LowPriorityLockWaitOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LowPriorityLockWaitMaxDurationOption
		/// </summary>
		public virtual void Visit(LowPriorityLockWaitMaxDurationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LowPriorityLockWaitMaxDurationOption
		/// </summary>
		public virtual void ExplicitVisit(LowPriorityLockWaitMaxDurationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((LowPriorityLockWaitOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LowPriorityLockWaitAbortAfterWaitOption
		/// </summary>
		public virtual void Visit(LowPriorityLockWaitAbortAfterWaitOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LowPriorityLockWaitAbortAfterWaitOption
		/// </summary>
		public virtual void ExplicitVisit(LowPriorityLockWaitAbortAfterWaitOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((LowPriorityLockWaitOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextIndexColumn
		/// </summary>
		public virtual void Visit(FullTextIndexColumn node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextIndexColumn
		/// </summary>
		public virtual void ExplicitVisit(FullTextIndexColumn node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateFullTextIndexStatement
		/// </summary>
		public virtual void Visit(CreateFullTextIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateFullTextIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateFullTextIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextIndexOption
		/// </summary>
		public virtual void Visit(FullTextIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextIndexOption
		/// </summary>
		public virtual void ExplicitVisit(FullTextIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ChangeTrackingFullTextIndexOption
		/// </summary>
		public virtual void Visit(ChangeTrackingFullTextIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ChangeTrackingFullTextIndexOption
		/// </summary>
		public virtual void ExplicitVisit(ChangeTrackingFullTextIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FullTextIndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StopListFullTextIndexOption
		/// </summary>
		public virtual void Visit(StopListFullTextIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StopListFullTextIndexOption
		/// </summary>
		public virtual void ExplicitVisit(StopListFullTextIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FullTextIndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SearchPropertyListFullTextIndexOption
		/// </summary>
		public virtual void Visit(SearchPropertyListFullTextIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SearchPropertyListFullTextIndexOption
		/// </summary>
		public virtual void ExplicitVisit(SearchPropertyListFullTextIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FullTextIndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextCatalogAndFileGroup
		/// </summary>
		public virtual void Visit(FullTextCatalogAndFileGroup node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextCatalogAndFileGroup
		/// </summary>
		public virtual void ExplicitVisit(FullTextCatalogAndFileGroup node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventTypeGroupContainer
		/// </summary>
		public virtual void Visit(EventTypeGroupContainer node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventTypeGroupContainer
		/// </summary>
		public virtual void ExplicitVisit(EventTypeGroupContainer node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventTypeContainer
		/// </summary>
		public virtual void Visit(EventTypeContainer node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventTypeContainer
		/// </summary>
		public virtual void ExplicitVisit(EventTypeContainer node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EventTypeGroupContainer) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventGroupContainer
		/// </summary>
		public virtual void Visit(EventGroupContainer node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventGroupContainer
		/// </summary>
		public virtual void ExplicitVisit(EventGroupContainer node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EventTypeGroupContainer) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateEventNotificationStatement
		/// </summary>
		public virtual void Visit(CreateEventNotificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateEventNotificationStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateEventNotificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventNotificationObjectScope
		/// </summary>
		public virtual void Visit(EventNotificationObjectScope node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventNotificationObjectScope
		/// </summary>
		public virtual void ExplicitVisit(EventNotificationObjectScope node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MasterKeyStatement
		/// </summary>
		public virtual void Visit(MasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(MasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateMasterKeyStatement
		/// </summary>
		public virtual void Visit(CreateMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MasterKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterMasterKeyStatement
		/// </summary>
		public virtual void Visit(AlterMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MasterKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ApplicationRoleOption
		/// </summary>
		public virtual void Visit(ApplicationRoleOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ApplicationRoleOption
		/// </summary>
		public virtual void ExplicitVisit(ApplicationRoleOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ApplicationRoleStatement
		/// </summary>
		public virtual void Visit(ApplicationRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ApplicationRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(ApplicationRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateApplicationRoleStatement
		/// </summary>
		public virtual void Visit(CreateApplicationRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateApplicationRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateApplicationRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ApplicationRoleStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterApplicationRoleStatement
		/// </summary>
		public virtual void Visit(AlterApplicationRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterApplicationRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterApplicationRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ApplicationRoleStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RoleStatement
		/// </summary>
		public virtual void Visit(RoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RoleStatement
		/// </summary>
		public virtual void ExplicitVisit(RoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateRoleStatement
		/// </summary>
		public virtual void Visit(CreateRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RoleStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterRoleStatement
		/// </summary>
		public virtual void Visit(AlterRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RoleStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterRoleAction
		/// </summary>
		public virtual void Visit(AlterRoleAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterRoleAction
		/// </summary>
		public virtual void ExplicitVisit(AlterRoleAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RenameAlterRoleAction
		/// </summary>
		public virtual void Visit(RenameAlterRoleAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RenameAlterRoleAction
		/// </summary>
		public virtual void ExplicitVisit(RenameAlterRoleAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterRoleAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AddMemberAlterRoleAction
		/// </summary>
		public virtual void Visit(AddMemberAlterRoleAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AddMemberAlterRoleAction
		/// </summary>
		public virtual void ExplicitVisit(AddMemberAlterRoleAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterRoleAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropMemberAlterRoleAction
		/// </summary>
		public virtual void Visit(DropMemberAlterRoleAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropMemberAlterRoleAction
		/// </summary>
		public virtual void ExplicitVisit(DropMemberAlterRoleAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterRoleAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateServerRoleStatement
		/// </summary>
		public virtual void Visit(CreateServerRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateServerRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateServerRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateRoleStatement) node);
				this.Visit((RoleStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerRoleStatement
		/// </summary>
		public virtual void Visit(AlterServerRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterRoleStatement) node);
				this.Visit((RoleStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropServerRoleStatement
		/// </summary>
		public virtual void Visit(DropServerRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropServerRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(DropServerRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UserLoginOption
		/// </summary>
		public virtual void Visit(UserLoginOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UserLoginOption
		/// </summary>
		public virtual void ExplicitVisit(UserLoginOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UserStatement
		/// </summary>
		public virtual void Visit(UserStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UserStatement
		/// </summary>
		public virtual void ExplicitVisit(UserStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateUserStatement
		/// </summary>
		public virtual void Visit(CreateUserStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateUserStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateUserStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((UserStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterUserStatement
		/// </summary>
		public virtual void Visit(AlterUserStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterUserStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterUserStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((UserStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StatisticsOption
		/// </summary>
		public virtual void Visit(StatisticsOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StatisticsOption
		/// </summary>
		public virtual void ExplicitVisit(StatisticsOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ResampleStatisticsOption
		/// </summary>
		public virtual void Visit(ResampleStatisticsOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ResampleStatisticsOption
		/// </summary>
		public virtual void ExplicitVisit(ResampleStatisticsOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((StatisticsOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StatisticsPartitionRange
		/// </summary>
		public virtual void Visit(StatisticsPartitionRange node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StatisticsPartitionRange
		/// </summary>
		public virtual void ExplicitVisit(StatisticsPartitionRange node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffStatisticsOption
		/// </summary>
		public virtual void Visit(OnOffStatisticsOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffStatisticsOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffStatisticsOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((StatisticsOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralStatisticsOption
		/// </summary>
		public virtual void Visit(LiteralStatisticsOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralStatisticsOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralStatisticsOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((StatisticsOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateStatisticsStatement
		/// </summary>
		public virtual void Visit(CreateStatisticsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateStatisticsStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateStatisticsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateStatisticsStatement
		/// </summary>
		public virtual void Visit(UpdateStatisticsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateStatisticsStatement
		/// </summary>
		public virtual void ExplicitVisit(UpdateStatisticsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ReturnStatement
		/// </summary>
		public virtual void Visit(ReturnStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ReturnStatement
		/// </summary>
		public virtual void ExplicitVisit(ReturnStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeclareCursorStatement
		/// </summary>
		public virtual void Visit(DeclareCursorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeclareCursorStatement
		/// </summary>
		public virtual void ExplicitVisit(DeclareCursorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CursorDefinition
		/// </summary>
		public virtual void Visit(CursorDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CursorDefinition
		/// </summary>
		public virtual void ExplicitVisit(CursorDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CursorOption
		/// </summary>
		public virtual void Visit(CursorOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CursorOption
		/// </summary>
		public virtual void ExplicitVisit(CursorOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetVariableStatement
		/// </summary>
		public virtual void Visit(SetVariableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetVariableStatement
		/// </summary>
		public virtual void ExplicitVisit(SetVariableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CursorId
		/// </summary>
		public virtual void Visit(CursorId node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CursorId
		/// </summary>
		public virtual void ExplicitVisit(CursorId node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CursorStatement
		/// </summary>
		public virtual void Visit(CursorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CursorStatement
		/// </summary>
		public virtual void ExplicitVisit(CursorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenCursorStatement
		/// </summary>
		public virtual void Visit(OpenCursorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenCursorStatement
		/// </summary>
		public virtual void ExplicitVisit(OpenCursorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CursorStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CloseCursorStatement
		/// </summary>
		public virtual void Visit(CloseCursorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CloseCursorStatement
		/// </summary>
		public virtual void ExplicitVisit(CloseCursorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CursorStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CryptoMechanism
		/// </summary>
		public virtual void Visit(CryptoMechanism node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CryptoMechanism
		/// </summary>
		public virtual void ExplicitVisit(CryptoMechanism node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenSymmetricKeyStatement
		/// </summary>
		public virtual void Visit(OpenSymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenSymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(OpenSymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CloseSymmetricKeyStatement
		/// </summary>
		public virtual void Visit(CloseSymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CloseSymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CloseSymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenMasterKeyStatement
		/// </summary>
		public virtual void Visit(OpenMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(OpenMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CloseMasterKeyStatement
		/// </summary>
		public virtual void Visit(CloseMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CloseMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CloseMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeallocateCursorStatement
		/// </summary>
		public virtual void Visit(DeallocateCursorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeallocateCursorStatement
		/// </summary>
		public virtual void ExplicitVisit(DeallocateCursorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CursorStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FetchType
		/// </summary>
		public virtual void Visit(FetchType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FetchType
		/// </summary>
		public virtual void ExplicitVisit(FetchType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FetchCursorStatement
		/// </summary>
		public virtual void Visit(FetchCursorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FetchCursorStatement
		/// </summary>
		public virtual void ExplicitVisit(FetchCursorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CursorStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WhereClause
		/// </summary>
		public virtual void Visit(WhereClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WhereClause
		/// </summary>
		public virtual void ExplicitVisit(WhereClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropUnownedObjectStatement
		/// </summary>
		public virtual void Visit(DropUnownedObjectStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropUnownedObjectStatement
		/// </summary>
		public virtual void ExplicitVisit(DropUnownedObjectStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropObjectsStatement
		/// </summary>
		public virtual void Visit(DropObjectsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropObjectsStatement
		/// </summary>
		public virtual void ExplicitVisit(DropObjectsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropDatabaseStatement
		/// </summary>
		public virtual void Visit(DropDatabaseStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropDatabaseStatement
		/// </summary>
		public virtual void ExplicitVisit(DropDatabaseStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropChildObjectsStatement
		/// </summary>
		public virtual void Visit(DropChildObjectsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropChildObjectsStatement
		/// </summary>
		public virtual void ExplicitVisit(DropChildObjectsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropIndexStatement
		/// </summary>
		public virtual void Visit(DropIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(DropIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropIndexClauseBase
		/// </summary>
		public virtual void Visit(DropIndexClauseBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropIndexClauseBase
		/// </summary>
		public virtual void ExplicitVisit(DropIndexClauseBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackwardsCompatibleDropIndexClause
		/// </summary>
		public virtual void Visit(BackwardsCompatibleDropIndexClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackwardsCompatibleDropIndexClause
		/// </summary>
		public virtual void ExplicitVisit(BackwardsCompatibleDropIndexClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropIndexClauseBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropIndexClause
		/// </summary>
		public virtual void Visit(DropIndexClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropIndexClause
		/// </summary>
		public virtual void ExplicitVisit(DropIndexClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropIndexClauseBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MoveToDropIndexOption
		/// </summary>
		public virtual void Visit(MoveToDropIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MoveToDropIndexOption
		/// </summary>
		public virtual void ExplicitVisit(MoveToDropIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileStreamOnDropIndexOption
		/// </summary>
		public virtual void Visit(FileStreamOnDropIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileStreamOnDropIndexOption
		/// </summary>
		public virtual void ExplicitVisit(FileStreamOnDropIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropStatisticsStatement
		/// </summary>
		public virtual void Visit(DropStatisticsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropStatisticsStatement
		/// </summary>
		public virtual void ExplicitVisit(DropStatisticsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropChildObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropTableStatement
		/// </summary>
		public virtual void Visit(DropTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropTableStatement
		/// </summary>
		public virtual void ExplicitVisit(DropTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropProcedureStatement
		/// </summary>
		public virtual void Visit(DropProcedureStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropProcedureStatement
		/// </summary>
		public virtual void ExplicitVisit(DropProcedureStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropFunctionStatement
		/// </summary>
		public virtual void Visit(DropFunctionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropFunctionStatement
		/// </summary>
		public virtual void ExplicitVisit(DropFunctionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropViewStatement
		/// </summary>
		public virtual void Visit(DropViewStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropViewStatement
		/// </summary>
		public virtual void ExplicitVisit(DropViewStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropDefaultStatement
		/// </summary>
		public virtual void Visit(DropDefaultStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropDefaultStatement
		/// </summary>
		public virtual void ExplicitVisit(DropDefaultStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropRuleStatement
		/// </summary>
		public virtual void Visit(DropRuleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropRuleStatement
		/// </summary>
		public virtual void ExplicitVisit(DropRuleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropTriggerStatement
		/// </summary>
		public virtual void Visit(DropTriggerStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropTriggerStatement
		/// </summary>
		public virtual void ExplicitVisit(DropTriggerStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSchemaStatement
		/// </summary>
		public virtual void Visit(DropSchemaStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSchemaStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSchemaStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RaiseErrorLegacyStatement
		/// </summary>
		public virtual void Visit(RaiseErrorLegacyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RaiseErrorLegacyStatement
		/// </summary>
		public virtual void ExplicitVisit(RaiseErrorLegacyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RaiseErrorStatement
		/// </summary>
		public virtual void Visit(RaiseErrorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RaiseErrorStatement
		/// </summary>
		public virtual void ExplicitVisit(RaiseErrorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ThrowStatement
		/// </summary>
		public virtual void Visit(ThrowStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ThrowStatement
		/// </summary>
		public virtual void ExplicitVisit(ThrowStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UseStatement
		/// </summary>
		public virtual void Visit(UseStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UseStatement
		/// </summary>
		public virtual void ExplicitVisit(UseStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for KillStatement
		/// </summary>
		public virtual void Visit(KillStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for KillStatement
		/// </summary>
		public virtual void ExplicitVisit(KillStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for KillQueryNotificationSubscriptionStatement
		/// </summary>
		public virtual void Visit(KillQueryNotificationSubscriptionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for KillQueryNotificationSubscriptionStatement
		/// </summary>
		public virtual void ExplicitVisit(KillQueryNotificationSubscriptionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for KillStatsJobStatement
		/// </summary>
		public virtual void Visit(KillStatsJobStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for KillStatsJobStatement
		/// </summary>
		public virtual void ExplicitVisit(KillStatsJobStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CheckpointStatement
		/// </summary>
		public virtual void Visit(CheckpointStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CheckpointStatement
		/// </summary>
		public virtual void ExplicitVisit(CheckpointStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ReconfigureStatement
		/// </summary>
		public virtual void Visit(ReconfigureStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ReconfigureStatement
		/// </summary>
		public virtual void ExplicitVisit(ReconfigureStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ShutdownStatement
		/// </summary>
		public virtual void Visit(ShutdownStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ShutdownStatement
		/// </summary>
		public virtual void ExplicitVisit(ShutdownStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetUserStatement
		/// </summary>
		public virtual void Visit(SetUserStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetUserStatement
		/// </summary>
		public virtual void ExplicitVisit(SetUserStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TruncateTableStatement
		/// </summary>
		public virtual void Visit(TruncateTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TruncateTableStatement
		/// </summary>
		public virtual void ExplicitVisit(TruncateTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetOnOffStatement
		/// </summary>
		public virtual void Visit(SetOnOffStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetOnOffStatement
		/// </summary>
		public virtual void ExplicitVisit(SetOnOffStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PredicateSetStatement
		/// </summary>
		public virtual void Visit(PredicateSetStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PredicateSetStatement
		/// </summary>
		public virtual void ExplicitVisit(PredicateSetStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetOnOffStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetStatisticsStatement
		/// </summary>
		public virtual void Visit(SetStatisticsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetStatisticsStatement
		/// </summary>
		public virtual void ExplicitVisit(SetStatisticsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetOnOffStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetRowCountStatement
		/// </summary>
		public virtual void Visit(SetRowCountStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetRowCountStatement
		/// </summary>
		public virtual void ExplicitVisit(SetRowCountStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetOffsetsStatement
		/// </summary>
		public virtual void Visit(SetOffsetsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetOffsetsStatement
		/// </summary>
		public virtual void ExplicitVisit(SetOffsetsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetOnOffStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetCommand
		/// </summary>
		public virtual void Visit(SetCommand node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetCommand
		/// </summary>
		public virtual void ExplicitVisit(SetCommand node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GeneralSetCommand
		/// </summary>
		public virtual void Visit(GeneralSetCommand node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GeneralSetCommand
		/// </summary>
		public virtual void ExplicitVisit(GeneralSetCommand node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetCommand) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetFipsFlaggerCommand
		/// </summary>
		public virtual void Visit(SetFipsFlaggerCommand node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetFipsFlaggerCommand
		/// </summary>
		public virtual void ExplicitVisit(SetFipsFlaggerCommand node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetCommand) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetCommandStatement
		/// </summary>
		public virtual void Visit(SetCommandStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetCommandStatement
		/// </summary>
		public virtual void ExplicitVisit(SetCommandStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetTransactionIsolationLevelStatement
		/// </summary>
		public virtual void Visit(SetTransactionIsolationLevelStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetTransactionIsolationLevelStatement
		/// </summary>
		public virtual void ExplicitVisit(SetTransactionIsolationLevelStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetTextSizeStatement
		/// </summary>
		public virtual void Visit(SetTextSizeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetTextSizeStatement
		/// </summary>
		public virtual void ExplicitVisit(SetTextSizeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetIdentityInsertStatement
		/// </summary>
		public virtual void Visit(SetIdentityInsertStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetIdentityInsertStatement
		/// </summary>
		public virtual void ExplicitVisit(SetIdentityInsertStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SetOnOffStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetErrorLevelStatement
		/// </summary>
		public virtual void Visit(SetErrorLevelStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetErrorLevelStatement
		/// </summary>
		public virtual void ExplicitVisit(SetErrorLevelStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateDatabaseStatement
		/// </summary>
		public virtual void Visit(CreateDatabaseStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateDatabaseStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateDatabaseStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileDeclaration
		/// </summary>
		public virtual void Visit(FileDeclaration node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileDeclaration
		/// </summary>
		public virtual void ExplicitVisit(FileDeclaration node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileDeclarationOption
		/// </summary>
		public virtual void Visit(FileDeclarationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileDeclarationOption
		/// </summary>
		public virtual void ExplicitVisit(FileDeclarationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for NameFileDeclarationOption
		/// </summary>
		public virtual void Visit(NameFileDeclarationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for NameFileDeclarationOption
		/// </summary>
		public virtual void ExplicitVisit(NameFileDeclarationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FileDeclarationOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileNameFileDeclarationOption
		/// </summary>
		public virtual void Visit(FileNameFileDeclarationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileNameFileDeclarationOption
		/// </summary>
		public virtual void ExplicitVisit(FileNameFileDeclarationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FileDeclarationOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SizeFileDeclarationOption
		/// </summary>
		public virtual void Visit(SizeFileDeclarationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SizeFileDeclarationOption
		/// </summary>
		public virtual void ExplicitVisit(SizeFileDeclarationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FileDeclarationOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxSizeFileDeclarationOption
		/// </summary>
		public virtual void Visit(MaxSizeFileDeclarationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxSizeFileDeclarationOption
		/// </summary>
		public virtual void ExplicitVisit(MaxSizeFileDeclarationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FileDeclarationOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileGrowthFileDeclarationOption
		/// </summary>
		public virtual void Visit(FileGrowthFileDeclarationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileGrowthFileDeclarationOption
		/// </summary>
		public virtual void ExplicitVisit(FileGrowthFileDeclarationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FileDeclarationOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileGroupDefinition
		/// </summary>
		public virtual void Visit(FileGroupDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileGroupDefinition
		/// </summary>
		public virtual void ExplicitVisit(FileGroupDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseScopedConfigurationStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseScopedConfigurationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseScopedConfigurationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseScopedConfigurationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseScopedConfigurationSetStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseScopedConfigurationSetStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseScopedConfigurationSetStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseScopedConfigurationSetStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseScopedConfigurationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseScopedConfigurationClearStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseScopedConfigurationClearStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseScopedConfigurationClearStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseScopedConfigurationClearStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseScopedConfigurationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DatabaseConfigurationClearOption
		/// </summary>
		public virtual void Visit(DatabaseConfigurationClearOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DatabaseConfigurationClearOption
		/// </summary>
		public virtual void ExplicitVisit(DatabaseConfigurationClearOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DatabaseConfigurationSetOption
		/// </summary>
		public virtual void Visit(DatabaseConfigurationSetOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DatabaseConfigurationSetOption
		/// </summary>
		public virtual void ExplicitVisit(DatabaseConfigurationSetOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffPrimaryConfigurationOption
		/// </summary>
		public virtual void Visit(OnOffPrimaryConfigurationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffPrimaryConfigurationOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffPrimaryConfigurationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseConfigurationSetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxDopConfigurationOption
		/// </summary>
		public virtual void Visit(MaxDopConfigurationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxDopConfigurationOption
		/// </summary>
		public virtual void ExplicitVisit(MaxDopConfigurationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseConfigurationSetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DWCompatibilityLevelConfigurationOption
		/// </summary>
		public virtual void Visit(DWCompatibilityLevelConfigurationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DWCompatibilityLevelConfigurationOption
		/// </summary>
		public virtual void ExplicitVisit(DWCompatibilityLevelConfigurationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseConfigurationSetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GenericConfigurationOption
		/// </summary>
		public virtual void Visit(GenericConfigurationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GenericConfigurationOption
		/// </summary>
		public virtual void ExplicitVisit(GenericConfigurationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseConfigurationSetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseCollateStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseCollateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseCollateStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseCollateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseRebuildLogStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseRebuildLogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseRebuildLogStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseRebuildLogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseAddFileStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseAddFileStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseAddFileStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseAddFileStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseAddFileGroupStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseAddFileGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseAddFileGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseAddFileGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseRemoveFileGroupStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseRemoveFileGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseRemoveFileGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseRemoveFileGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseRemoveFileStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseRemoveFileStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseRemoveFileStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseRemoveFileStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseModifyNameStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseModifyNameStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseModifyNameStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseModifyNameStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseModifyFileStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseModifyFileStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseModifyFileStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseModifyFileStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseModifyFileGroupStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseModifyFileGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseModifyFileGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseModifyFileGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseTermination
		/// </summary>
		public virtual void Visit(AlterDatabaseTermination node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseTermination
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseTermination node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseSetStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseSetStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseSetStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseSetStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterDatabaseStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DatabaseOption
		/// </summary>
		public virtual void Visit(DatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(DatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffDatabaseOption
		/// </summary>
		public virtual void Visit(OnOffDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutoCreateStatisticsDatabaseOption
		/// </summary>
		public virtual void Visit(AutoCreateStatisticsDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutoCreateStatisticsDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(AutoCreateStatisticsDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OnOffDatabaseOption) node);
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ContainmentDatabaseOption
		/// </summary>
		public virtual void Visit(ContainmentDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ContainmentDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(ContainmentDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for HadrDatabaseOption
		/// </summary>
		public virtual void Visit(HadrDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for HadrDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(HadrDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for HadrAvailabilityGroupDatabaseOption
		/// </summary>
		public virtual void Visit(HadrAvailabilityGroupDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for HadrAvailabilityGroupDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(HadrAvailabilityGroupDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((HadrDatabaseOption) node);
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DelayedDurabilityDatabaseOption
		/// </summary>
		public virtual void Visit(DelayedDurabilityDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DelayedDurabilityDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(DelayedDurabilityDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CursorDefaultDatabaseOption
		/// </summary>
		public virtual void Visit(CursorDefaultDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CursorDefaultDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(CursorDefaultDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RecoveryDatabaseOption
		/// </summary>
		public virtual void Visit(RecoveryDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RecoveryDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(RecoveryDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TargetRecoveryTimeDatabaseOption
		/// </summary>
		public virtual void Visit(TargetRecoveryTimeDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TargetRecoveryTimeDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(TargetRecoveryTimeDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PageVerifyDatabaseOption
		/// </summary>
		public virtual void Visit(PageVerifyDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PageVerifyDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(PageVerifyDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PartnerDatabaseOption
		/// </summary>
		public virtual void Visit(PartnerDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PartnerDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(PartnerDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WitnessDatabaseOption
		/// </summary>
		public virtual void Visit(WitnessDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WitnessDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(WitnessDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ParameterizationDatabaseOption
		/// </summary>
		public virtual void Visit(ParameterizationDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ParameterizationDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(ParameterizationDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralDatabaseOption
		/// </summary>
		public virtual void Visit(LiteralDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentifierDatabaseOption
		/// </summary>
		public virtual void Visit(IdentifierDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentifierDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(IdentifierDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ChangeTrackingDatabaseOption
		/// </summary>
		public virtual void Visit(ChangeTrackingDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ChangeTrackingDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(ChangeTrackingDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ChangeTrackingOptionDetail
		/// </summary>
		public virtual void Visit(ChangeTrackingOptionDetail node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ChangeTrackingOptionDetail
		/// </summary>
		public virtual void ExplicitVisit(ChangeTrackingOptionDetail node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutoCleanupChangeTrackingOptionDetail
		/// </summary>
		public virtual void Visit(AutoCleanupChangeTrackingOptionDetail node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutoCleanupChangeTrackingOptionDetail
		/// </summary>
		public virtual void ExplicitVisit(AutoCleanupChangeTrackingOptionDetail node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ChangeTrackingOptionDetail) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ChangeRetentionChangeTrackingOptionDetail
		/// </summary>
		public virtual void Visit(ChangeRetentionChangeTrackingOptionDetail node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ChangeRetentionChangeTrackingOptionDetail
		/// </summary>
		public virtual void ExplicitVisit(ChangeRetentionChangeTrackingOptionDetail node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ChangeTrackingOptionDetail) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AcceleratedDatabaseRecoveryDatabaseOption
		/// </summary>
		public virtual void Visit(AcceleratedDatabaseRecoveryDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AcceleratedDatabaseRecoveryDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(AcceleratedDatabaseRecoveryDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreDatabaseOption
		/// </summary>
		public virtual void Visit(QueryStoreDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreOption
		/// </summary>
		public virtual void Visit(QueryStoreOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreDesiredStateOption
		/// </summary>
		public virtual void Visit(QueryStoreDesiredStateOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreDesiredStateOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreDesiredStateOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreCapturePolicyOption
		/// </summary>
		public virtual void Visit(QueryStoreCapturePolicyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreCapturePolicyOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreCapturePolicyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreSizeCleanupPolicyOption
		/// </summary>
		public virtual void Visit(QueryStoreSizeCleanupPolicyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreSizeCleanupPolicyOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreSizeCleanupPolicyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreDataFlushIntervalOption
		/// </summary>
		public virtual void Visit(QueryStoreDataFlushIntervalOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreDataFlushIntervalOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreDataFlushIntervalOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreIntervalLengthOption
		/// </summary>
		public virtual void Visit(QueryStoreIntervalLengthOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreIntervalLengthOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreIntervalLengthOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreMaxStorageSizeOption
		/// </summary>
		public virtual void Visit(QueryStoreMaxStorageSizeOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreMaxStorageSizeOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreMaxStorageSizeOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreMaxPlansPerQueryOption
		/// </summary>
		public virtual void Visit(QueryStoreMaxPlansPerQueryOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreMaxPlansPerQueryOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreMaxPlansPerQueryOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreTimeCleanupPolicyOption
		/// </summary>
		public virtual void Visit(QueryStoreTimeCleanupPolicyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreTimeCleanupPolicyOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreTimeCleanupPolicyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryStoreWaitStatsCaptureOption
		/// </summary>
		public virtual void Visit(QueryStoreWaitStatsCaptureOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryStoreWaitStatsCaptureOption
		/// </summary>
		public virtual void ExplicitVisit(QueryStoreWaitStatsCaptureOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryStoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutomaticTuningDatabaseOption
		/// </summary>
		public virtual void Visit(AutomaticTuningDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutomaticTuningDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(AutomaticTuningDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutomaticTuningOption
		/// </summary>
		public virtual void Visit(AutomaticTuningOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutomaticTuningOption
		/// </summary>
		public virtual void ExplicitVisit(AutomaticTuningOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutomaticTuningForceLastGoodPlanOption
		/// </summary>
		public virtual void Visit(AutomaticTuningForceLastGoodPlanOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutomaticTuningForceLastGoodPlanOption
		/// </summary>
		public virtual void ExplicitVisit(AutomaticTuningForceLastGoodPlanOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AutomaticTuningOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutomaticTuningCreateIndexOption
		/// </summary>
		public virtual void Visit(AutomaticTuningCreateIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutomaticTuningCreateIndexOption
		/// </summary>
		public virtual void ExplicitVisit(AutomaticTuningCreateIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AutomaticTuningOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutomaticTuningDropIndexOption
		/// </summary>
		public virtual void Visit(AutomaticTuningDropIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutomaticTuningDropIndexOption
		/// </summary>
		public virtual void ExplicitVisit(AutomaticTuningDropIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AutomaticTuningOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AutomaticTuningMaintainIndexOption
		/// </summary>
		public virtual void Visit(AutomaticTuningMaintainIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AutomaticTuningMaintainIndexOption
		/// </summary>
		public virtual void ExplicitVisit(AutomaticTuningMaintainIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AutomaticTuningOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileStreamDatabaseOption
		/// </summary>
		public virtual void Visit(FileStreamDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileStreamDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(FileStreamDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CatalogCollationOption
		/// </summary>
		public virtual void Visit(CatalogCollationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CatalogCollationOption
		/// </summary>
		public virtual void ExplicitVisit(CatalogCollationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LedgerOption
		/// </summary>
		public virtual void Visit(LedgerOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LedgerOption
		/// </summary>
		public virtual void ExplicitVisit(LedgerOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxSizeDatabaseOption
		/// </summary>
		public virtual void Visit(MaxSizeDatabaseOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxSizeDatabaseOption
		/// </summary>
		public virtual void ExplicitVisit(MaxSizeDatabaseOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableAlterIndexStatement
		/// </summary>
		public virtual void Visit(AlterTableAlterIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableAlterIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableAlterIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterTableAlterColumnStatement
		/// </summary>
		public virtual void Visit(AlterTableAlterColumnStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterTableAlterColumnStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterTableAlterColumnStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterTableStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnDefinition
		/// </summary>
		public virtual void Visit(ColumnDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnDefinition
		/// </summary>
		public virtual void ExplicitVisit(ColumnDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnDefinitionBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionDefinition
		/// </summary>
		public virtual void Visit(ColumnEncryptionDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionDefinition
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionDefinitionParameter
		/// </summary>
		public virtual void Visit(ColumnEncryptionDefinitionParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionDefinitionParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionDefinitionParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionKeyNameParameter
		/// </summary>
		public virtual void Visit(ColumnEncryptionKeyNameParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionKeyNameParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionKeyNameParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionDefinitionParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionTypeParameter
		/// </summary>
		public virtual void Visit(ColumnEncryptionTypeParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionTypeParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionTypeParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionDefinitionParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnEncryptionAlgorithmParameter
		/// </summary>
		public virtual void Visit(ColumnEncryptionAlgorithmParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnEncryptionAlgorithmParameter
		/// </summary>
		public virtual void ExplicitVisit(ColumnEncryptionAlgorithmParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ColumnEncryptionDefinitionParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentityOptions
		/// </summary>
		public virtual void Visit(IdentityOptions node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentityOptions
		/// </summary>
		public virtual void ExplicitVisit(IdentityOptions node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnStorageOptions
		/// </summary>
		public virtual void Visit(ColumnStorageOptions node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnStorageOptions
		/// </summary>
		public virtual void ExplicitVisit(ColumnStorageOptions node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ConstraintDefinition
		/// </summary>
		public virtual void Visit(ConstraintDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ConstraintDefinition
		/// </summary>
		public virtual void ExplicitVisit(ConstraintDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateTableStatement
		/// </summary>
		public virtual void Visit(CreateTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateTableStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FederationScheme
		/// </summary>
		public virtual void Visit(FederationScheme node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FederationScheme
		/// </summary>
		public virtual void ExplicitVisit(FederationScheme node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableDataCompressionOption
		/// </summary>
		public virtual void Visit(TableDataCompressionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableDataCompressionOption
		/// </summary>
		public virtual void ExplicitVisit(TableDataCompressionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableXmlCompressionOption
		/// </summary>
		public virtual void Visit(TableXmlCompressionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableXmlCompressionOption
		/// </summary>
		public virtual void ExplicitVisit(TableXmlCompressionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableDistributionOption
		/// </summary>
		public virtual void Visit(TableDistributionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableDistributionOption
		/// </summary>
		public virtual void ExplicitVisit(TableDistributionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableDistributionPolicy
		/// </summary>
		public virtual void Visit(TableDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(TableDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableReplicateDistributionPolicy
		/// </summary>
		public virtual void Visit(TableReplicateDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableReplicateDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(TableReplicateDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableRoundRobinDistributionPolicy
		/// </summary>
		public virtual void Visit(TableRoundRobinDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableRoundRobinDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(TableRoundRobinDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableHashDistributionPolicy
		/// </summary>
		public virtual void Visit(TableHashDistributionPolicy node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableHashDistributionPolicy
		/// </summary>
		public virtual void ExplicitVisit(TableHashDistributionPolicy node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableDistributionPolicy) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClusterByTableOption
		/// </summary>
		public virtual void Visit(ClusterByTableOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClusterByTableOption
		/// </summary>
		public virtual void ExplicitVisit(ClusterByTableOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableIndexOption
		/// </summary>
		public virtual void Visit(TableIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableIndexOption
		/// </summary>
		public virtual void ExplicitVisit(TableIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableIndexType
		/// </summary>
		public virtual void Visit(TableIndexType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableIndexType
		/// </summary>
		public virtual void ExplicitVisit(TableIndexType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableClusteredIndexType
		/// </summary>
		public virtual void Visit(TableClusteredIndexType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableClusteredIndexType
		/// </summary>
		public virtual void ExplicitVisit(TableClusteredIndexType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableIndexType) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableNonClusteredIndexType
		/// </summary>
		public virtual void Visit(TableNonClusteredIndexType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableNonClusteredIndexType
		/// </summary>
		public virtual void ExplicitVisit(TableNonClusteredIndexType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableIndexType) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TablePartitionOption
		/// </summary>
		public virtual void Visit(TablePartitionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TablePartitionOption
		/// </summary>
		public virtual void ExplicitVisit(TablePartitionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PartitionSpecifications
		/// </summary>
		public virtual void Visit(PartitionSpecifications node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PartitionSpecifications
		/// </summary>
		public virtual void ExplicitVisit(PartitionSpecifications node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TablePartitionOptionSpecifications
		/// </summary>
		public virtual void Visit(TablePartitionOptionSpecifications node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TablePartitionOptionSpecifications
		/// </summary>
		public virtual void ExplicitVisit(TablePartitionOptionSpecifications node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PartitionSpecifications) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LocationOption
		/// </summary>
		public virtual void Visit(LocationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LocationOption
		/// </summary>
		public virtual void ExplicitVisit(LocationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RenameEntityStatement
		/// </summary>
		public virtual void Visit(RenameEntityStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RenameEntityStatement
		/// </summary>
		public virtual void ExplicitVisit(RenameEntityStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CopyStatement
		/// </summary>
		public virtual void Visit(CopyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CopyStatement
		/// </summary>
		public virtual void ExplicitVisit(CopyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CopyStatementOptionBase
		/// </summary>
		public virtual void Visit(CopyStatementOptionBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CopyStatementOptionBase
		/// </summary>
		public virtual void ExplicitVisit(CopyStatementOptionBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CopyOption
		/// </summary>
		public virtual void Visit(CopyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CopyOption
		/// </summary>
		public virtual void ExplicitVisit(CopyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CopyCredentialOption
		/// </summary>
		public virtual void Visit(CopyCredentialOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CopyCredentialOption
		/// </summary>
		public virtual void ExplicitVisit(CopyCredentialOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CopyStatementOptionBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SingleValueTypeCopyOption
		/// </summary>
		public virtual void Visit(SingleValueTypeCopyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SingleValueTypeCopyOption
		/// </summary>
		public virtual void ExplicitVisit(SingleValueTypeCopyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CopyStatementOptionBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ListTypeCopyOption
		/// </summary>
		public virtual void Visit(ListTypeCopyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ListTypeCopyOption
		/// </summary>
		public virtual void ExplicitVisit(ListTypeCopyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CopyStatementOptionBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CopyColumnOption
		/// </summary>
		public virtual void Visit(CopyColumnOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CopyColumnOption
		/// </summary>
		public virtual void ExplicitVisit(CopyColumnOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CopyStatementOptionBase) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DataCompressionOption
		/// </summary>
		public virtual void Visit(DataCompressionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DataCompressionOption
		/// </summary>
		public virtual void ExplicitVisit(DataCompressionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for XmlCompressionOption
		/// </summary>
		public virtual void Visit(XmlCompressionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for XmlCompressionOption
		/// </summary>
		public virtual void ExplicitVisit(XmlCompressionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CompressionPartitionRange
		/// </summary>
		public virtual void Visit(CompressionPartitionRange node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CompressionPartitionRange
		/// </summary>
		public virtual void ExplicitVisit(CompressionPartitionRange node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CheckConstraintDefinition
		/// </summary>
		public virtual void Visit(CheckConstraintDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CheckConstraintDefinition
		/// </summary>
		public virtual void ExplicitVisit(CheckConstraintDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ConstraintDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DefaultConstraintDefinition
		/// </summary>
		public virtual void Visit(DefaultConstraintDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DefaultConstraintDefinition
		/// </summary>
		public virtual void ExplicitVisit(DefaultConstraintDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ConstraintDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ForeignKeyConstraintDefinition
		/// </summary>
		public virtual void Visit(ForeignKeyConstraintDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ForeignKeyConstraintDefinition
		/// </summary>
		public virtual void ExplicitVisit(ForeignKeyConstraintDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ConstraintDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for NullableConstraintDefinition
		/// </summary>
		public virtual void Visit(NullableConstraintDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for NullableConstraintDefinition
		/// </summary>
		public virtual void ExplicitVisit(NullableConstraintDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ConstraintDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphConnectionBetweenNodes
		/// </summary>
		public virtual void Visit(GraphConnectionBetweenNodes node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphConnectionBetweenNodes
		/// </summary>
		public virtual void ExplicitVisit(GraphConnectionBetweenNodes node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphConnectionConstraintDefinition
		/// </summary>
		public virtual void Visit(GraphConnectionConstraintDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphConnectionConstraintDefinition
		/// </summary>
		public virtual void ExplicitVisit(GraphConnectionConstraintDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ConstraintDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UniqueConstraintDefinition
		/// </summary>
		public virtual void Visit(UniqueConstraintDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UniqueConstraintDefinition
		/// </summary>
		public virtual void ExplicitVisit(UniqueConstraintDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ConstraintDefinition) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupStatement
		/// </summary>
		public virtual void Visit(BackupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupStatement
		/// </summary>
		public virtual void ExplicitVisit(BackupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupDatabaseStatement
		/// </summary>
		public virtual void Visit(BackupDatabaseStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupDatabaseStatement
		/// </summary>
		public virtual void ExplicitVisit(BackupDatabaseStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BackupStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupTransactionLogStatement
		/// </summary>
		public virtual void Visit(BackupTransactionLogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupTransactionLogStatement
		/// </summary>
		public virtual void ExplicitVisit(BackupTransactionLogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BackupStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RestoreStatement
		/// </summary>
		public virtual void Visit(RestoreStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RestoreStatement
		/// </summary>
		public virtual void ExplicitVisit(RestoreStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RestoreOption
		/// </summary>
		public virtual void Visit(RestoreOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RestoreOption
		/// </summary>
		public virtual void ExplicitVisit(RestoreOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ScalarExpressionRestoreOption
		/// </summary>
		public virtual void Visit(ScalarExpressionRestoreOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ScalarExpressionRestoreOption
		/// </summary>
		public virtual void ExplicitVisit(ScalarExpressionRestoreOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RestoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MoveRestoreOption
		/// </summary>
		public virtual void Visit(MoveRestoreOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MoveRestoreOption
		/// </summary>
		public virtual void ExplicitVisit(MoveRestoreOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RestoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StopRestoreOption
		/// </summary>
		public virtual void Visit(StopRestoreOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StopRestoreOption
		/// </summary>
		public virtual void ExplicitVisit(StopRestoreOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RestoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileStreamRestoreOption
		/// </summary>
		public virtual void Visit(FileStreamRestoreOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileStreamRestoreOption
		/// </summary>
		public virtual void ExplicitVisit(FileStreamRestoreOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RestoreOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupOption
		/// </summary>
		public virtual void Visit(BackupOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupOption
		/// </summary>
		public virtual void ExplicitVisit(BackupOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupEncryptionOption
		/// </summary>
		public virtual void Visit(BackupEncryptionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupEncryptionOption
		/// </summary>
		public virtual void ExplicitVisit(BackupEncryptionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BackupOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeviceInfo
		/// </summary>
		public virtual void Visit(DeviceInfo node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeviceInfo
		/// </summary>
		public virtual void ExplicitVisit(DeviceInfo node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MirrorToClause
		/// </summary>
		public virtual void Visit(MirrorToClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MirrorToClause
		/// </summary>
		public virtual void ExplicitVisit(MirrorToClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupRestoreFileInfo
		/// </summary>
		public virtual void Visit(BackupRestoreFileInfo node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupRestoreFileInfo
		/// </summary>
		public virtual void ExplicitVisit(BackupRestoreFileInfo node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BulkInsertBase
		/// </summary>
		public virtual void Visit(BulkInsertBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BulkInsertBase
		/// </summary>
		public virtual void ExplicitVisit(BulkInsertBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BulkInsertStatement
		/// </summary>
		public virtual void Visit(BulkInsertStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BulkInsertStatement
		/// </summary>
		public virtual void ExplicitVisit(BulkInsertStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BulkInsertBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InsertBulkStatement
		/// </summary>
		public virtual void Visit(InsertBulkStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InsertBulkStatement
		/// </summary>
		public virtual void ExplicitVisit(InsertBulkStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BulkInsertBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BulkInsertOption
		/// </summary>
		public virtual void Visit(BulkInsertOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BulkInsertOption
		/// </summary>
		public virtual void ExplicitVisit(BulkInsertOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OpenRowsetCosmosOption
		/// </summary>
		public virtual void Visit(OpenRowsetCosmosOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OpenRowsetCosmosOption
		/// </summary>
		public virtual void ExplicitVisit(OpenRowsetCosmosOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralOpenRowsetCosmosOption
		/// </summary>
		public virtual void Visit(LiteralOpenRowsetCosmosOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralOpenRowsetCosmosOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralOpenRowsetCosmosOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((OpenRowsetCosmosOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralBulkInsertOption
		/// </summary>
		public virtual void Visit(LiteralBulkInsertOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralBulkInsertOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralBulkInsertOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BulkInsertOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OrderBulkInsertOption
		/// </summary>
		public virtual void Visit(OrderBulkInsertOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OrderBulkInsertOption
		/// </summary>
		public virtual void ExplicitVisit(OrderBulkInsertOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BulkInsertOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ColumnDefinitionBase
		/// </summary>
		public virtual void Visit(ColumnDefinitionBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ColumnDefinitionBase
		/// </summary>
		public virtual void ExplicitVisit(ColumnDefinitionBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalTableColumnDefinition
		/// </summary>
		public virtual void Visit(ExternalTableColumnDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalTableColumnDefinition
		/// </summary>
		public virtual void ExplicitVisit(ExternalTableColumnDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InsertBulkColumnDefinition
		/// </summary>
		public virtual void Visit(InsertBulkColumnDefinition node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InsertBulkColumnDefinition
		/// </summary>
		public virtual void ExplicitVisit(InsertBulkColumnDefinition node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DbccStatement
		/// </summary>
		public virtual void Visit(DbccStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DbccStatement
		/// </summary>
		public virtual void ExplicitVisit(DbccStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DbccOption
		/// </summary>
		public virtual void Visit(DbccOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DbccOption
		/// </summary>
		public virtual void ExplicitVisit(DbccOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DbccNamedLiteral
		/// </summary>
		public virtual void Visit(DbccNamedLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DbccNamedLiteral
		/// </summary>
		public virtual void ExplicitVisit(DbccNamedLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateAsymmetricKeyStatement
		/// </summary>
		public virtual void Visit(CreateAsymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateAsymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateAsymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreatePartitionFunctionStatement
		/// </summary>
		public virtual void Visit(CreatePartitionFunctionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreatePartitionFunctionStatement
		/// </summary>
		public virtual void ExplicitVisit(CreatePartitionFunctionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PartitionParameterType
		/// </summary>
		public virtual void Visit(PartitionParameterType node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PartitionParameterType
		/// </summary>
		public virtual void ExplicitVisit(PartitionParameterType node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreatePartitionSchemeStatement
		/// </summary>
		public virtual void Visit(CreatePartitionSchemeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreatePartitionSchemeStatement
		/// </summary>
		public virtual void ExplicitVisit(CreatePartitionSchemeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteServiceBindingStatementBase
		/// </summary>
		public virtual void Visit(RemoteServiceBindingStatementBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteServiceBindingStatementBase
		/// </summary>
		public virtual void ExplicitVisit(RemoteServiceBindingStatementBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RemoteServiceBindingOption
		/// </summary>
		public virtual void Visit(RemoteServiceBindingOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RemoteServiceBindingOption
		/// </summary>
		public virtual void ExplicitVisit(RemoteServiceBindingOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffRemoteServiceBindingOption
		/// </summary>
		public virtual void Visit(OnOffRemoteServiceBindingOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffRemoteServiceBindingOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffRemoteServiceBindingOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RemoteServiceBindingOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UserRemoteServiceBindingOption
		/// </summary>
		public virtual void Visit(UserRemoteServiceBindingOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UserRemoteServiceBindingOption
		/// </summary>
		public virtual void ExplicitVisit(UserRemoteServiceBindingOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RemoteServiceBindingOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateRemoteServiceBindingStatement
		/// </summary>
		public virtual void Visit(CreateRemoteServiceBindingStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateRemoteServiceBindingStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateRemoteServiceBindingStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RemoteServiceBindingStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterRemoteServiceBindingStatement
		/// </summary>
		public virtual void Visit(AlterRemoteServiceBindingStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterRemoteServiceBindingStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterRemoteServiceBindingStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((RemoteServiceBindingStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EncryptionSource
		/// </summary>
		public virtual void Visit(EncryptionSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EncryptionSource
		/// </summary>
		public virtual void ExplicitVisit(EncryptionSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AssemblyEncryptionSource
		/// </summary>
		public virtual void Visit(AssemblyEncryptionSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AssemblyEncryptionSource
		/// </summary>
		public virtual void ExplicitVisit(AssemblyEncryptionSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EncryptionSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FileEncryptionSource
		/// </summary>
		public virtual void Visit(FileEncryptionSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FileEncryptionSource
		/// </summary>
		public virtual void ExplicitVisit(FileEncryptionSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EncryptionSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProviderEncryptionSource
		/// </summary>
		public virtual void Visit(ProviderEncryptionSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProviderEncryptionSource
		/// </summary>
		public virtual void ExplicitVisit(ProviderEncryptionSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EncryptionSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CertificateStatementBase
		/// </summary>
		public virtual void Visit(CertificateStatementBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CertificateStatementBase
		/// </summary>
		public virtual void ExplicitVisit(CertificateStatementBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterCertificateStatement
		/// </summary>
		public virtual void Visit(AlterCertificateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterCertificateStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterCertificateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CertificateStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateCertificateStatement
		/// </summary>
		public virtual void Visit(CreateCertificateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateCertificateStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateCertificateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CertificateStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CertificateOption
		/// </summary>
		public virtual void Visit(CertificateOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CertificateOption
		/// </summary>
		public virtual void ExplicitVisit(CertificateOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateContractStatement
		/// </summary>
		public virtual void Visit(CreateContractStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateContractStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateContractStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ContractMessage
		/// </summary>
		public virtual void Visit(ContractMessage node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ContractMessage
		/// </summary>
		public virtual void ExplicitVisit(ContractMessage node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CredentialStatement
		/// </summary>
		public virtual void Visit(CredentialStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CredentialStatement
		/// </summary>
		public virtual void ExplicitVisit(CredentialStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateCredentialStatement
		/// </summary>
		public virtual void Visit(CreateCredentialStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateCredentialStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateCredentialStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CredentialStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterCredentialStatement
		/// </summary>
		public virtual void Visit(AlterCredentialStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterCredentialStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterCredentialStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CredentialStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MessageTypeStatementBase
		/// </summary>
		public virtual void Visit(MessageTypeStatementBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MessageTypeStatementBase
		/// </summary>
		public virtual void ExplicitVisit(MessageTypeStatementBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateMessageTypeStatement
		/// </summary>
		public virtual void Visit(CreateMessageTypeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateMessageTypeStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateMessageTypeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MessageTypeStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterMessageTypeStatement
		/// </summary>
		public virtual void Visit(AlterMessageTypeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterMessageTypeStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterMessageTypeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MessageTypeStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateAggregateStatement
		/// </summary>
		public virtual void Visit(CreateAggregateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateAggregateStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateAggregateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateEndpointStatement
		/// </summary>
		public virtual void Visit(CreateEndpointStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateEndpointStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateEndpointStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterCreateEndpointStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterEndpointStatement
		/// </summary>
		public virtual void Visit(AlterEndpointStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterEndpointStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterEndpointStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterCreateEndpointStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterCreateEndpointStatementBase
		/// </summary>
		public virtual void Visit(AlterCreateEndpointStatementBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterCreateEndpointStatementBase
		/// </summary>
		public virtual void ExplicitVisit(AlterCreateEndpointStatementBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EndpointAffinity
		/// </summary>
		public virtual void Visit(EndpointAffinity node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EndpointAffinity
		/// </summary>
		public virtual void ExplicitVisit(EndpointAffinity node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EndpointProtocolOption
		/// </summary>
		public virtual void Visit(EndpointProtocolOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EndpointProtocolOption
		/// </summary>
		public virtual void ExplicitVisit(EndpointProtocolOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralEndpointProtocolOption
		/// </summary>
		public virtual void Visit(LiteralEndpointProtocolOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralEndpointProtocolOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralEndpointProtocolOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EndpointProtocolOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuthenticationEndpointProtocolOption
		/// </summary>
		public virtual void Visit(AuthenticationEndpointProtocolOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuthenticationEndpointProtocolOption
		/// </summary>
		public virtual void ExplicitVisit(AuthenticationEndpointProtocolOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EndpointProtocolOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PortsEndpointProtocolOption
		/// </summary>
		public virtual void Visit(PortsEndpointProtocolOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PortsEndpointProtocolOption
		/// </summary>
		public virtual void ExplicitVisit(PortsEndpointProtocolOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EndpointProtocolOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CompressionEndpointProtocolOption
		/// </summary>
		public virtual void Visit(CompressionEndpointProtocolOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CompressionEndpointProtocolOption
		/// </summary>
		public virtual void ExplicitVisit(CompressionEndpointProtocolOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EndpointProtocolOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ListenerIPEndpointProtocolOption
		/// </summary>
		public virtual void Visit(ListenerIPEndpointProtocolOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ListenerIPEndpointProtocolOption
		/// </summary>
		public virtual void ExplicitVisit(ListenerIPEndpointProtocolOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EndpointProtocolOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IPv4
		/// </summary>
		public virtual void Visit(IPv4 node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IPv4
		/// </summary>
		public virtual void ExplicitVisit(IPv4 node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SoapMethod
		/// </summary>
		public virtual void Visit(SoapMethod node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SoapMethod
		/// </summary>
		public virtual void ExplicitVisit(SoapMethod node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PayloadOption
		/// </summary>
		public virtual void Visit(PayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PayloadOption
		/// </summary>
		public virtual void ExplicitVisit(PayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EnabledDisabledPayloadOption
		/// </summary>
		public virtual void Visit(EnabledDisabledPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EnabledDisabledPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(EnabledDisabledPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WsdlPayloadOption
		/// </summary>
		public virtual void Visit(WsdlPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WsdlPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(WsdlPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LoginTypePayloadOption
		/// </summary>
		public virtual void Visit(LoginTypePayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LoginTypePayloadOption
		/// </summary>
		public virtual void ExplicitVisit(LoginTypePayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralPayloadOption
		/// </summary>
		public virtual void Visit(LiteralPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SessionTimeoutPayloadOption
		/// </summary>
		public virtual void Visit(SessionTimeoutPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SessionTimeoutPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(SessionTimeoutPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaPayloadOption
		/// </summary>
		public virtual void Visit(SchemaPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(SchemaPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CharacterSetPayloadOption
		/// </summary>
		public virtual void Visit(CharacterSetPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CharacterSetPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(CharacterSetPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RolePayloadOption
		/// </summary>
		public virtual void Visit(RolePayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RolePayloadOption
		/// </summary>
		public virtual void ExplicitVisit(RolePayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuthenticationPayloadOption
		/// </summary>
		public virtual void Visit(AuthenticationPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuthenticationPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(AuthenticationPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EncryptionPayloadOption
		/// </summary>
		public virtual void Visit(EncryptionPayloadOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EncryptionPayloadOption
		/// </summary>
		public virtual void ExplicitVisit(EncryptionPayloadOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PayloadOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SymmetricKeyStatement
		/// </summary>
		public virtual void Visit(SymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(SymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSymmetricKeyStatement
		/// </summary>
		public virtual void Visit(CreateSymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SymmetricKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for KeyOption
		/// </summary>
		public virtual void Visit(KeyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for KeyOption
		/// </summary>
		public virtual void ExplicitVisit(KeyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for KeySourceKeyOption
		/// </summary>
		public virtual void Visit(KeySourceKeyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for KeySourceKeyOption
		/// </summary>
		public virtual void ExplicitVisit(KeySourceKeyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((KeyOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlgorithmKeyOption
		/// </summary>
		public virtual void Visit(AlgorithmKeyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlgorithmKeyOption
		/// </summary>
		public virtual void ExplicitVisit(AlgorithmKeyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((KeyOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentityValueKeyOption
		/// </summary>
		public virtual void Visit(IdentityValueKeyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentityValueKeyOption
		/// </summary>
		public virtual void ExplicitVisit(IdentityValueKeyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((KeyOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProviderKeyNameKeyOption
		/// </summary>
		public virtual void Visit(ProviderKeyNameKeyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProviderKeyNameKeyOption
		/// </summary>
		public virtual void ExplicitVisit(ProviderKeyNameKeyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((KeyOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreationDispositionKeyOption
		/// </summary>
		public virtual void Visit(CreationDispositionKeyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreationDispositionKeyOption
		/// </summary>
		public virtual void ExplicitVisit(CreationDispositionKeyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((KeyOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterSymmetricKeyStatement
		/// </summary>
		public virtual void Visit(AlterSymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterSymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterSymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SymmetricKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextCatalogStatement
		/// </summary>
		public virtual void Visit(FullTextCatalogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextCatalogStatement
		/// </summary>
		public virtual void ExplicitVisit(FullTextCatalogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextCatalogOption
		/// </summary>
		public virtual void Visit(FullTextCatalogOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextCatalogOption
		/// </summary>
		public virtual void ExplicitVisit(FullTextCatalogOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffFullTextCatalogOption
		/// </summary>
		public virtual void Visit(OnOffFullTextCatalogOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffFullTextCatalogOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffFullTextCatalogOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FullTextCatalogOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateFullTextCatalogStatement
		/// </summary>
		public virtual void Visit(CreateFullTextCatalogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateFullTextCatalogStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateFullTextCatalogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FullTextCatalogStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterFullTextCatalogStatement
		/// </summary>
		public virtual void Visit(AlterFullTextCatalogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterFullTextCatalogStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterFullTextCatalogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((FullTextCatalogStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterCreateServiceStatementBase
		/// </summary>
		public virtual void Visit(AlterCreateServiceStatementBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterCreateServiceStatementBase
		/// </summary>
		public virtual void ExplicitVisit(AlterCreateServiceStatementBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateServiceStatement
		/// </summary>
		public virtual void Visit(CreateServiceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateServiceStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateServiceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterCreateServiceStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServiceStatement
		/// </summary>
		public virtual void Visit(AlterServiceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServiceStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServiceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterCreateServiceStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ServiceContract
		/// </summary>
		public virtual void Visit(ServiceContract node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ServiceContract
		/// </summary>
		public virtual void ExplicitVisit(ServiceContract node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BinaryExpression
		/// </summary>
		public virtual void Visit(BinaryExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BinaryExpression
		/// </summary>
		public virtual void ExplicitVisit(BinaryExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BuiltInFunctionTableReference
		/// </summary>
		public virtual void Visit(BuiltInFunctionTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BuiltInFunctionTableReference
		/// </summary>
		public virtual void ExplicitVisit(BuiltInFunctionTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GlobalFunctionTableReference
		/// </summary>
		public virtual void Visit(GlobalFunctionTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GlobalFunctionTableReference
		/// </summary>
		public virtual void ExplicitVisit(GlobalFunctionTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ComputeClause
		/// </summary>
		public virtual void Visit(ComputeClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ComputeClause
		/// </summary>
		public virtual void ExplicitVisit(ComputeClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ComputeFunction
		/// </summary>
		public virtual void Visit(ComputeFunction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ComputeFunction
		/// </summary>
		public virtual void ExplicitVisit(ComputeFunction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PivotedTableReference
		/// </summary>
		public virtual void Visit(PivotedTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PivotedTableReference
		/// </summary>
		public virtual void ExplicitVisit(PivotedTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UnpivotedTableReference
		/// </summary>
		public virtual void Visit(UnpivotedTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UnpivotedTableReference
		/// </summary>
		public virtual void ExplicitVisit(UnpivotedTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UnqualifiedJoin
		/// </summary>
		public virtual void Visit(UnqualifiedJoin node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UnqualifiedJoin
		/// </summary>
		public virtual void ExplicitVisit(UnqualifiedJoin node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((JoinTableReference) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableSampleClause
		/// </summary>
		public virtual void Visit(TableSampleClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableSampleClause
		/// </summary>
		public virtual void ExplicitVisit(TableSampleClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ScalarExpression
		/// </summary>
		public virtual void Visit(ScalarExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ScalarExpression
		/// </summary>
		public virtual void ExplicitVisit(ScalarExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanExpression
		/// </summary>
		public virtual void Visit(BooleanExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanExpression
		/// </summary>
		public virtual void ExplicitVisit(BooleanExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanNotExpression
		/// </summary>
		public virtual void Visit(BooleanNotExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanNotExpression
		/// </summary>
		public virtual void ExplicitVisit(BooleanNotExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanParenthesisExpression
		/// </summary>
		public virtual void Visit(BooleanParenthesisExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanParenthesisExpression
		/// </summary>
		public virtual void ExplicitVisit(BooleanParenthesisExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanComparisonExpression
		/// </summary>
		public virtual void Visit(BooleanComparisonExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanComparisonExpression
		/// </summary>
		public virtual void ExplicitVisit(BooleanComparisonExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanBinaryExpression
		/// </summary>
		public virtual void Visit(BooleanBinaryExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanBinaryExpression
		/// </summary>
		public virtual void ExplicitVisit(BooleanBinaryExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanIsNullExpression
		/// </summary>
		public virtual void Visit(BooleanIsNullExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanIsNullExpression
		/// </summary>
		public virtual void ExplicitVisit(BooleanIsNullExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphMatchPredicate
		/// </summary>
		public virtual void Visit(GraphMatchPredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphMatchPredicate
		/// </summary>
		public virtual void ExplicitVisit(GraphMatchPredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphMatchLastNodePredicate
		/// </summary>
		public virtual void Visit(GraphMatchLastNodePredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphMatchLastNodePredicate
		/// </summary>
		public virtual void ExplicitVisit(GraphMatchLastNodePredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphMatchNodeExpression
		/// </summary>
		public virtual void Visit(GraphMatchNodeExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphMatchNodeExpression
		/// </summary>
		public virtual void ExplicitVisit(GraphMatchNodeExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphMatchRecursivePredicate
		/// </summary>
		public virtual void Visit(GraphMatchRecursivePredicate node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphMatchRecursivePredicate
		/// </summary>
		public virtual void ExplicitVisit(GraphMatchRecursivePredicate node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphMatchExpression
		/// </summary>
		public virtual void Visit(GraphMatchExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphMatchExpression
		/// </summary>
		public virtual void ExplicitVisit(GraphMatchExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphMatchCompositeExpression
		/// </summary>
		public virtual void Visit(GraphMatchCompositeExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphMatchCompositeExpression
		/// </summary>
		public virtual void ExplicitVisit(GraphMatchCompositeExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GraphRecursiveMatchQuantifier
		/// </summary>
		public virtual void Visit(GraphRecursiveMatchQuantifier node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GraphRecursiveMatchQuantifier
		/// </summary>
		public virtual void ExplicitVisit(GraphRecursiveMatchQuantifier node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExpressionWithSortOrder
		/// </summary>
		public virtual void Visit(ExpressionWithSortOrder node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExpressionWithSortOrder
		/// </summary>
		public virtual void ExplicitVisit(ExpressionWithSortOrder node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GroupByClause
		/// </summary>
		public virtual void Visit(GroupByClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GroupByClause
		/// </summary>
		public virtual void ExplicitVisit(GroupByClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GroupingSpecification
		/// </summary>
		public virtual void Visit(GroupingSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GroupingSpecification
		/// </summary>
		public virtual void ExplicitVisit(GroupingSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExpressionGroupingSpecification
		/// </summary>
		public virtual void Visit(ExpressionGroupingSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExpressionGroupingSpecification
		/// </summary>
		public virtual void ExplicitVisit(ExpressionGroupingSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((GroupingSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CompositeGroupingSpecification
		/// </summary>
		public virtual void Visit(CompositeGroupingSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CompositeGroupingSpecification
		/// </summary>
		public virtual void ExplicitVisit(CompositeGroupingSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((GroupingSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CubeGroupingSpecification
		/// </summary>
		public virtual void Visit(CubeGroupingSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CubeGroupingSpecification
		/// </summary>
		public virtual void ExplicitVisit(CubeGroupingSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((GroupingSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RollupGroupingSpecification
		/// </summary>
		public virtual void Visit(RollupGroupingSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RollupGroupingSpecification
		/// </summary>
		public virtual void ExplicitVisit(RollupGroupingSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((GroupingSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GrandTotalGroupingSpecification
		/// </summary>
		public virtual void Visit(GrandTotalGroupingSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GrandTotalGroupingSpecification
		/// </summary>
		public virtual void ExplicitVisit(GrandTotalGroupingSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((GroupingSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GroupingSetsGroupingSpecification
		/// </summary>
		public virtual void Visit(GroupingSetsGroupingSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GroupingSetsGroupingSpecification
		/// </summary>
		public virtual void ExplicitVisit(GroupingSetsGroupingSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((GroupingSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OutputClause
		/// </summary>
		public virtual void Visit(OutputClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OutputClause
		/// </summary>
		public virtual void ExplicitVisit(OutputClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OutputIntoClause
		/// </summary>
		public virtual void Visit(OutputIntoClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OutputIntoClause
		/// </summary>
		public virtual void ExplicitVisit(OutputIntoClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for HavingClause
		/// </summary>
		public virtual void Visit(HavingClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for HavingClause
		/// </summary>
		public virtual void ExplicitVisit(HavingClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentityFunctionCall
		/// </summary>
		public virtual void Visit(IdentityFunctionCall node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentityFunctionCall
		/// </summary>
		public virtual void ExplicitVisit(IdentityFunctionCall node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for JoinParenthesisTableReference
		/// </summary>
		public virtual void Visit(JoinParenthesisTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for JoinParenthesisTableReference
		/// </summary>
		public virtual void ExplicitVisit(JoinParenthesisTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OrderByClause
		/// </summary>
		public virtual void Visit(OrderByClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OrderByClause
		/// </summary>
		public virtual void ExplicitVisit(OrderByClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for JoinTableReference
		/// </summary>
		public virtual void Visit(JoinTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for JoinTableReference
		/// </summary>
		public virtual void ExplicitVisit(JoinTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QualifiedJoin
		/// </summary>
		public virtual void Visit(QualifiedJoin node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QualifiedJoin
		/// </summary>
		public virtual void ExplicitVisit(QualifiedJoin node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((JoinTableReference) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OdbcQualifiedJoinTableReference
		/// </summary>
		public virtual void Visit(OdbcQualifiedJoinTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OdbcQualifiedJoinTableReference
		/// </summary>
		public virtual void ExplicitVisit(OdbcQualifiedJoinTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryExpression
		/// </summary>
		public virtual void Visit(QueryExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryExpression
		/// </summary>
		public virtual void ExplicitVisit(QueryExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueryParenthesisExpression
		/// </summary>
		public virtual void Visit(QueryParenthesisExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueryParenthesisExpression
		/// </summary>
		public virtual void ExplicitVisit(QueryParenthesisExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QuerySpecification
		/// </summary>
		public virtual void Visit(QuerySpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QuerySpecification
		/// </summary>
		public virtual void ExplicitVisit(QuerySpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FromClause
		/// </summary>
		public virtual void Visit(FromClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FromClause
		/// </summary>
		public virtual void ExplicitVisit(FromClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PredictTableReference
		/// </summary>
		public virtual void Visit(PredictTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PredictTableReference
		/// </summary>
		public virtual void ExplicitVisit(PredictTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectElement
		/// </summary>
		public virtual void Visit(SelectElement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectElement
		/// </summary>
		public virtual void ExplicitVisit(SelectElement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectScalarExpression
		/// </summary>
		public virtual void Visit(SelectScalarExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectScalarExpression
		/// </summary>
		public virtual void ExplicitVisit(SelectScalarExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SelectElement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectStarExpression
		/// </summary>
		public virtual void Visit(SelectStarExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectStarExpression
		/// </summary>
		public virtual void ExplicitVisit(SelectStarExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SelectElement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectSetVariable
		/// </summary>
		public virtual void Visit(SelectSetVariable node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectSetVariable
		/// </summary>
		public virtual void ExplicitVisit(SelectSetVariable node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SelectElement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableReference
		/// </summary>
		public virtual void Visit(TableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableReference
		/// </summary>
		public virtual void ExplicitVisit(TableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableReferenceWithAlias
		/// </summary>
		public virtual void Visit(TableReferenceWithAlias node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableReferenceWithAlias
		/// </summary>
		public virtual void ExplicitVisit(TableReferenceWithAlias node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TableReferenceWithAliasAndColumns
		/// </summary>
		public virtual void Visit(TableReferenceWithAliasAndColumns node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TableReferenceWithAliasAndColumns
		/// </summary>
		public virtual void ExplicitVisit(TableReferenceWithAliasAndColumns node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DataModificationTableReference
		/// </summary>
		public virtual void Visit(DataModificationTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DataModificationTableReference
		/// </summary>
		public virtual void ExplicitVisit(DataModificationTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ChangeTableChangesTableReference
		/// </summary>
		public virtual void Visit(ChangeTableChangesTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ChangeTableChangesTableReference
		/// </summary>
		public virtual void ExplicitVisit(ChangeTableChangesTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ChangeTableVersionTableReference
		/// </summary>
		public virtual void Visit(ChangeTableVersionTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ChangeTableVersionTableReference
		/// </summary>
		public virtual void ExplicitVisit(ChangeTableVersionTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanTernaryExpression
		/// </summary>
		public virtual void Visit(BooleanTernaryExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanTernaryExpression
		/// </summary>
		public virtual void ExplicitVisit(BooleanTernaryExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TopRowFilter
		/// </summary>
		public virtual void Visit(TopRowFilter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TopRowFilter
		/// </summary>
		public virtual void ExplicitVisit(TopRowFilter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OffsetClause
		/// </summary>
		public virtual void Visit(OffsetClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OffsetClause
		/// </summary>
		public virtual void ExplicitVisit(OffsetClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UnaryExpression
		/// </summary>
		public virtual void Visit(UnaryExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UnaryExpression
		/// </summary>
		public virtual void ExplicitVisit(UnaryExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BinaryQueryExpression
		/// </summary>
		public virtual void Visit(BinaryQueryExpression node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BinaryQueryExpression
		/// </summary>
		public virtual void ExplicitVisit(BinaryQueryExpression node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((QueryExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for VariableTableReference
		/// </summary>
		public virtual void Visit(VariableTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for VariableTableReference
		/// </summary>
		public virtual void ExplicitVisit(VariableTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for VariableMethodCallTableReference
		/// </summary>
		public virtual void Visit(VariableMethodCallTableReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for VariableMethodCallTableReference
		/// </summary>
		public virtual void ExplicitVisit(VariableMethodCallTableReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TableReferenceWithAliasAndColumns) node);
				this.Visit((TableReferenceWithAlias) node);
				this.Visit((TableReference) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropPartitionFunctionStatement
		/// </summary>
		public virtual void Visit(DropPartitionFunctionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropPartitionFunctionStatement
		/// </summary>
		public virtual void ExplicitVisit(DropPartitionFunctionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropPartitionSchemeStatement
		/// </summary>
		public virtual void Visit(DropPartitionSchemeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropPartitionSchemeStatement
		/// </summary>
		public virtual void ExplicitVisit(DropPartitionSchemeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSynonymStatement
		/// </summary>
		public virtual void Visit(DropSynonymStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSynonymStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSynonymStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropAggregateStatement
		/// </summary>
		public virtual void Visit(DropAggregateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropAggregateStatement
		/// </summary>
		public virtual void ExplicitVisit(DropAggregateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropAssemblyStatement
		/// </summary>
		public virtual void Visit(DropAssemblyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropAssemblyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropAssemblyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropObjectsStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropApplicationRoleStatement
		/// </summary>
		public virtual void Visit(DropApplicationRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropApplicationRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(DropApplicationRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropFullTextCatalogStatement
		/// </summary>
		public virtual void Visit(DropFullTextCatalogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropFullTextCatalogStatement
		/// </summary>
		public virtual void ExplicitVisit(DropFullTextCatalogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropFullTextIndexStatement
		/// </summary>
		public virtual void Visit(DropFullTextIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropFullTextIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(DropFullTextIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropLoginStatement
		/// </summary>
		public virtual void Visit(DropLoginStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropLoginStatement
		/// </summary>
		public virtual void ExplicitVisit(DropLoginStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropRoleStatement
		/// </summary>
		public virtual void Visit(DropRoleStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropRoleStatement
		/// </summary>
		public virtual void ExplicitVisit(DropRoleStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropTypeStatement
		/// </summary>
		public virtual void Visit(DropTypeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropTypeStatement
		/// </summary>
		public virtual void ExplicitVisit(DropTypeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropUserStatement
		/// </summary>
		public virtual void Visit(DropUserStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropUserStatement
		/// </summary>
		public virtual void ExplicitVisit(DropUserStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropMasterKeyStatement
		/// </summary>
		public virtual void Visit(DropMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSymmetricKeyStatement
		/// </summary>
		public virtual void Visit(DropSymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropAsymmetricKeyStatement
		/// </summary>
		public virtual void Visit(DropAsymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropAsymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropAsymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropCertificateStatement
		/// </summary>
		public virtual void Visit(DropCertificateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropCertificateStatement
		/// </summary>
		public virtual void ExplicitVisit(DropCertificateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropCredentialStatement
		/// </summary>
		public virtual void Visit(DropCredentialStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropCredentialStatement
		/// </summary>
		public virtual void ExplicitVisit(DropCredentialStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterPartitionFunctionStatement
		/// </summary>
		public virtual void Visit(AlterPartitionFunctionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterPartitionFunctionStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterPartitionFunctionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterPartitionSchemeStatement
		/// </summary>
		public virtual void Visit(AlterPartitionSchemeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterPartitionSchemeStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterPartitionSchemeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterFullTextIndexStatement
		/// </summary>
		public virtual void Visit(AlterFullTextIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterFullTextIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterFullTextIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterFullTextIndexAction
		/// </summary>
		public virtual void Visit(AlterFullTextIndexAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterFullTextIndexAction
		/// </summary>
		public virtual void ExplicitVisit(AlterFullTextIndexAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SimpleAlterFullTextIndexAction
		/// </summary>
		public virtual void Visit(SimpleAlterFullTextIndexAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SimpleAlterFullTextIndexAction
		/// </summary>
		public virtual void ExplicitVisit(SimpleAlterFullTextIndexAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterFullTextIndexAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetStopListAlterFullTextIndexAction
		/// </summary>
		public virtual void Visit(SetStopListAlterFullTextIndexAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetStopListAlterFullTextIndexAction
		/// </summary>
		public virtual void ExplicitVisit(SetStopListAlterFullTextIndexAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterFullTextIndexAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SetSearchPropertyListAlterFullTextIndexAction
		/// </summary>
		public virtual void Visit(SetSearchPropertyListAlterFullTextIndexAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SetSearchPropertyListAlterFullTextIndexAction
		/// </summary>
		public virtual void ExplicitVisit(SetSearchPropertyListAlterFullTextIndexAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterFullTextIndexAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropAlterFullTextIndexAction
		/// </summary>
		public virtual void Visit(DropAlterFullTextIndexAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropAlterFullTextIndexAction
		/// </summary>
		public virtual void ExplicitVisit(DropAlterFullTextIndexAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterFullTextIndexAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AddAlterFullTextIndexAction
		/// </summary>
		public virtual void Visit(AddAlterFullTextIndexAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AddAlterFullTextIndexAction
		/// </summary>
		public virtual void ExplicitVisit(AddAlterFullTextIndexAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterFullTextIndexAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterColumnAlterFullTextIndexAction
		/// </summary>
		public virtual void Visit(AlterColumnAlterFullTextIndexAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterColumnAlterFullTextIndexAction
		/// </summary>
		public virtual void ExplicitVisit(AlterColumnAlterFullTextIndexAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterFullTextIndexAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSearchPropertyListStatement
		/// </summary>
		public virtual void Visit(CreateSearchPropertyListStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSearchPropertyListStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSearchPropertyListStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterSearchPropertyListStatement
		/// </summary>
		public virtual void Visit(AlterSearchPropertyListStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterSearchPropertyListStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterSearchPropertyListStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SearchPropertyListAction
		/// </summary>
		public virtual void Visit(SearchPropertyListAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SearchPropertyListAction
		/// </summary>
		public virtual void ExplicitVisit(SearchPropertyListAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AddSearchPropertyListAction
		/// </summary>
		public virtual void Visit(AddSearchPropertyListAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AddSearchPropertyListAction
		/// </summary>
		public virtual void ExplicitVisit(AddSearchPropertyListAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SearchPropertyListAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSearchPropertyListAction
		/// </summary>
		public virtual void Visit(DropSearchPropertyListAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSearchPropertyListAction
		/// </summary>
		public virtual void ExplicitVisit(DropSearchPropertyListAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SearchPropertyListAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSearchPropertyListStatement
		/// </summary>
		public virtual void Visit(DropSearchPropertyListStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSearchPropertyListStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSearchPropertyListStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateLoginStatement
		/// </summary>
		public virtual void Visit(CreateLoginStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateLoginStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateLoginStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateLoginSource
		/// </summary>
		public virtual void Visit(CreateLoginSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateLoginSource
		/// </summary>
		public virtual void ExplicitVisit(CreateLoginSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PasswordCreateLoginSource
		/// </summary>
		public virtual void Visit(PasswordCreateLoginSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PasswordCreateLoginSource
		/// </summary>
		public virtual void ExplicitVisit(PasswordCreateLoginSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateLoginSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PrincipalOption
		/// </summary>
		public virtual void Visit(PrincipalOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PrincipalOption
		/// </summary>
		public virtual void ExplicitVisit(PrincipalOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffPrincipalOption
		/// </summary>
		public virtual void Visit(OnOffPrincipalOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffPrincipalOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffPrincipalOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrincipalOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralPrincipalOption
		/// </summary>
		public virtual void Visit(LiteralPrincipalOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralPrincipalOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralPrincipalOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrincipalOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentifierPrincipalOption
		/// </summary>
		public virtual void Visit(IdentifierPrincipalOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentifierPrincipalOption
		/// </summary>
		public virtual void ExplicitVisit(IdentifierPrincipalOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrincipalOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WindowsCreateLoginSource
		/// </summary>
		public virtual void Visit(WindowsCreateLoginSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WindowsCreateLoginSource
		/// </summary>
		public virtual void ExplicitVisit(WindowsCreateLoginSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateLoginSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalCreateLoginSource
		/// </summary>
		public virtual void Visit(ExternalCreateLoginSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalCreateLoginSource
		/// </summary>
		public virtual void ExplicitVisit(ExternalCreateLoginSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateLoginSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CertificateCreateLoginSource
		/// </summary>
		public virtual void Visit(CertificateCreateLoginSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CertificateCreateLoginSource
		/// </summary>
		public virtual void ExplicitVisit(CertificateCreateLoginSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateLoginSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AsymmetricKeyCreateLoginSource
		/// </summary>
		public virtual void Visit(AsymmetricKeyCreateLoginSource node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AsymmetricKeyCreateLoginSource
		/// </summary>
		public virtual void ExplicitVisit(AsymmetricKeyCreateLoginSource node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateLoginSource) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PasswordAlterPrincipalOption
		/// </summary>
		public virtual void Visit(PasswordAlterPrincipalOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PasswordAlterPrincipalOption
		/// </summary>
		public virtual void ExplicitVisit(PasswordAlterPrincipalOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((PrincipalOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterLoginStatement
		/// </summary>
		public virtual void Visit(AlterLoginStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterLoginStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterLoginStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterLoginOptionsStatement
		/// </summary>
		public virtual void Visit(AlterLoginOptionsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterLoginOptionsStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterLoginOptionsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterLoginStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterLoginEnableDisableStatement
		/// </summary>
		public virtual void Visit(AlterLoginEnableDisableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterLoginEnableDisableStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterLoginEnableDisableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterLoginStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterLoginAddDropCredentialStatement
		/// </summary>
		public virtual void Visit(AlterLoginAddDropCredentialStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterLoginAddDropCredentialStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterLoginAddDropCredentialStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterLoginStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RevertStatement
		/// </summary>
		public virtual void Visit(RevertStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RevertStatement
		/// </summary>
		public virtual void ExplicitVisit(RevertStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropContractStatement
		/// </summary>
		public virtual void Visit(DropContractStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropContractStatement
		/// </summary>
		public virtual void ExplicitVisit(DropContractStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropEndpointStatement
		/// </summary>
		public virtual void Visit(DropEndpointStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropEndpointStatement
		/// </summary>
		public virtual void ExplicitVisit(DropEndpointStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropMessageTypeStatement
		/// </summary>
		public virtual void Visit(DropMessageTypeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropMessageTypeStatement
		/// </summary>
		public virtual void ExplicitVisit(DropMessageTypeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropQueueStatement
		/// </summary>
		public virtual void Visit(DropQueueStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropQueueStatement
		/// </summary>
		public virtual void ExplicitVisit(DropQueueStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropRemoteServiceBindingStatement
		/// </summary>
		public virtual void Visit(DropRemoteServiceBindingStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropRemoteServiceBindingStatement
		/// </summary>
		public virtual void ExplicitVisit(DropRemoteServiceBindingStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropRouteStatement
		/// </summary>
		public virtual void Visit(DropRouteStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropRouteStatement
		/// </summary>
		public virtual void ExplicitVisit(DropRouteStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropServiceStatement
		/// </summary>
		public virtual void Visit(DropServiceStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropServiceStatement
		/// </summary>
		public virtual void ExplicitVisit(DropServiceStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SignatureStatementBase
		/// </summary>
		public virtual void Visit(SignatureStatementBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SignatureStatementBase
		/// </summary>
		public virtual void ExplicitVisit(SignatureStatementBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AddSignatureStatement
		/// </summary>
		public virtual void Visit(AddSignatureStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AddSignatureStatement
		/// </summary>
		public virtual void ExplicitVisit(AddSignatureStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SignatureStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSignatureStatement
		/// </summary>
		public virtual void Visit(DropSignatureStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSignatureStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSignatureStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SignatureStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropEventNotificationStatement
		/// </summary>
		public virtual void Visit(DropEventNotificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropEventNotificationStatement
		/// </summary>
		public virtual void ExplicitVisit(DropEventNotificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExecuteAsStatement
		/// </summary>
		public virtual void Visit(ExecuteAsStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExecuteAsStatement
		/// </summary>
		public virtual void ExplicitVisit(ExecuteAsStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EndConversationStatement
		/// </summary>
		public virtual void Visit(EndConversationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EndConversationStatement
		/// </summary>
		public virtual void ExplicitVisit(EndConversationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MoveConversationStatement
		/// </summary>
		public virtual void Visit(MoveConversationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MoveConversationStatement
		/// </summary>
		public virtual void ExplicitVisit(MoveConversationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GetConversationGroupStatement
		/// </summary>
		public virtual void Visit(GetConversationGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GetConversationGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(GetConversationGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WaitForSupportedStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ReceiveStatement
		/// </summary>
		public virtual void Visit(ReceiveStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ReceiveStatement
		/// </summary>
		public virtual void ExplicitVisit(ReceiveStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WaitForSupportedStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SendStatement
		/// </summary>
		public virtual void Visit(SendStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SendStatement
		/// </summary>
		public virtual void ExplicitVisit(SendStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WaitForSupportedStatement
		/// </summary>
		public virtual void Visit(WaitForSupportedStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WaitForSupportedStatement
		/// </summary>
		public virtual void ExplicitVisit(WaitForSupportedStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterSchemaStatement
		/// </summary>
		public virtual void Visit(AlterSchemaStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterSchemaStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterSchemaStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterAsymmetricKeyStatement
		/// </summary>
		public virtual void Visit(AlterAsymmetricKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterAsymmetricKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterAsymmetricKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServiceMasterKeyStatement
		/// </summary>
		public virtual void Visit(AlterServiceMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServiceMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServiceMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BeginConversationTimerStatement
		/// </summary>
		public virtual void Visit(BeginConversationTimerStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BeginConversationTimerStatement
		/// </summary>
		public virtual void ExplicitVisit(BeginConversationTimerStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BeginDialogStatement
		/// </summary>
		public virtual void Visit(BeginDialogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BeginDialogStatement
		/// </summary>
		public virtual void ExplicitVisit(BeginDialogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DialogOption
		/// </summary>
		public virtual void Visit(DialogOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DialogOption
		/// </summary>
		public virtual void ExplicitVisit(DialogOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ScalarExpressionDialogOption
		/// </summary>
		public virtual void Visit(ScalarExpressionDialogOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ScalarExpressionDialogOption
		/// </summary>
		public virtual void ExplicitVisit(ScalarExpressionDialogOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DialogOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffDialogOption
		/// </summary>
		public virtual void Visit(OnOffDialogOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffDialogOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffDialogOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DialogOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupCertificateStatement
		/// </summary>
		public virtual void Visit(BackupCertificateStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupCertificateStatement
		/// </summary>
		public virtual void ExplicitVisit(BackupCertificateStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CertificateStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupRestoreMasterKeyStatementBase
		/// </summary>
		public virtual void Visit(BackupRestoreMasterKeyStatementBase node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupRestoreMasterKeyStatementBase
		/// </summary>
		public virtual void ExplicitVisit(BackupRestoreMasterKeyStatementBase node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupServiceMasterKeyStatement
		/// </summary>
		public virtual void Visit(BackupServiceMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupServiceMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(BackupServiceMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BackupRestoreMasterKeyStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RestoreServiceMasterKeyStatement
		/// </summary>
		public virtual void Visit(RestoreServiceMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RestoreServiceMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(RestoreServiceMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BackupRestoreMasterKeyStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BackupMasterKeyStatement
		/// </summary>
		public virtual void Visit(BackupMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BackupMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(BackupMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BackupRestoreMasterKeyStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RestoreMasterKeyStatement
		/// </summary>
		public virtual void Visit(RestoreMasterKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RestoreMasterKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(RestoreMasterKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BackupRestoreMasterKeyStatementBase) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ScalarExpressionSnippet
		/// </summary>
		public virtual void Visit(ScalarExpressionSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ScalarExpressionSnippet
		/// </summary>
		public virtual void ExplicitVisit(ScalarExpressionSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BooleanExpressionSnippet
		/// </summary>
		public virtual void Visit(BooleanExpressionSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BooleanExpressionSnippet
		/// </summary>
		public virtual void ExplicitVisit(BooleanExpressionSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StatementListSnippet
		/// </summary>
		public virtual void Visit(StatementListSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StatementListSnippet
		/// </summary>
		public virtual void ExplicitVisit(StatementListSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((StatementList) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectStatementSnippet
		/// </summary>
		public virtual void Visit(SelectStatementSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectStatementSnippet
		/// </summary>
		public virtual void ExplicitVisit(SelectStatementSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SelectStatement) node);
				this.Visit((StatementWithCtesAndXmlNamespaces) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SchemaObjectNameSnippet
		/// </summary>
		public virtual void Visit(SchemaObjectNameSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SchemaObjectNameSnippet
		/// </summary>
		public virtual void ExplicitVisit(SchemaObjectNameSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SchemaObjectName) node);
				this.Visit((MultiPartIdentifier) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TSqlFragmentSnippet
		/// </summary>
		public virtual void Visit(TSqlFragmentSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TSqlFragmentSnippet
		/// </summary>
		public virtual void ExplicitVisit(TSqlFragmentSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TSqlStatementSnippet
		/// </summary>
		public virtual void Visit(TSqlStatementSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TSqlStatementSnippet
		/// </summary>
		public virtual void ExplicitVisit(TSqlStatementSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for IdentifierSnippet
		/// </summary>
		public virtual void Visit(IdentifierSnippet node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for IdentifierSnippet
		/// </summary>
		public virtual void ExplicitVisit(IdentifierSnippet node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((Identifier) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TSqlScript
		/// </summary>
		public virtual void Visit(TSqlScript node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TSqlScript
		/// </summary>
		public virtual void ExplicitVisit(TSqlScript node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TSqlBatch
		/// </summary>
		public virtual void Visit(TSqlBatch node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TSqlBatch
		/// </summary>
		public virtual void ExplicitVisit(TSqlBatch node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TSqlStatement
		/// </summary>
		public virtual void Visit(TSqlStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TSqlStatement
		/// </summary>
		public virtual void ExplicitVisit(TSqlStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DataModificationStatement
		/// </summary>
		public virtual void Visit(DataModificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DataModificationStatement
		/// </summary>
		public virtual void ExplicitVisit(DataModificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((StatementWithCtesAndXmlNamespaces) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DataModificationSpecification
		/// </summary>
		public virtual void Visit(DataModificationSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DataModificationSpecification
		/// </summary>
		public virtual void ExplicitVisit(DataModificationSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MergeStatement
		/// </summary>
		public virtual void Visit(MergeStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MergeStatement
		/// </summary>
		public virtual void ExplicitVisit(MergeStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataModificationStatement) node);
				this.Visit((StatementWithCtesAndXmlNamespaces) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MergeSpecification
		/// </summary>
		public virtual void Visit(MergeSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MergeSpecification
		/// </summary>
		public virtual void ExplicitVisit(MergeSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DataModificationSpecification) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MergeActionClause
		/// </summary>
		public virtual void Visit(MergeActionClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MergeActionClause
		/// </summary>
		public virtual void ExplicitVisit(MergeActionClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MergeAction
		/// </summary>
		public virtual void Visit(MergeAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MergeAction
		/// </summary>
		public virtual void ExplicitVisit(MergeAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UpdateMergeAction
		/// </summary>
		public virtual void Visit(UpdateMergeAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UpdateMergeAction
		/// </summary>
		public virtual void ExplicitVisit(UpdateMergeAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MergeAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DeleteMergeAction
		/// </summary>
		public virtual void Visit(DeleteMergeAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DeleteMergeAction
		/// </summary>
		public virtual void ExplicitVisit(DeleteMergeAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MergeAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for InsertMergeAction
		/// </summary>
		public virtual void Visit(InsertMergeAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for InsertMergeAction
		/// </summary>
		public virtual void ExplicitVisit(InsertMergeAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((MergeAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateTypeTableStatement
		/// </summary>
		public virtual void Visit(CreateTypeTableStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateTypeTableStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateTypeTableStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((CreateTypeStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SensitivityClassificationStatement
		/// </summary>
		public virtual void Visit(SensitivityClassificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SensitivityClassificationStatement
		/// </summary>
		public virtual void ExplicitVisit(SensitivityClassificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SensitivityClassificationOption
		/// </summary>
		public virtual void Visit(SensitivityClassificationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SensitivityClassificationOption
		/// </summary>
		public virtual void ExplicitVisit(SensitivityClassificationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AddSensitivityClassificationStatement
		/// </summary>
		public virtual void Visit(AddSensitivityClassificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AddSensitivityClassificationStatement
		/// </summary>
		public virtual void ExplicitVisit(AddSensitivityClassificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SensitivityClassificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropSensitivityClassificationStatement
		/// </summary>
		public virtual void Visit(DropSensitivityClassificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropSensitivityClassificationStatement
		/// </summary>
		public virtual void ExplicitVisit(DropSensitivityClassificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SensitivityClassificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditSpecificationStatement
		/// </summary>
		public virtual void Visit(AuditSpecificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditSpecificationStatement
		/// </summary>
		public virtual void ExplicitVisit(AuditSpecificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditSpecificationPart
		/// </summary>
		public virtual void Visit(AuditSpecificationPart node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditSpecificationPart
		/// </summary>
		public virtual void ExplicitVisit(AuditSpecificationPart node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditSpecificationDetail
		/// </summary>
		public virtual void Visit(AuditSpecificationDetail node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditSpecificationDetail
		/// </summary>
		public virtual void ExplicitVisit(AuditSpecificationDetail node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditActionSpecification
		/// </summary>
		public virtual void Visit(AuditActionSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditActionSpecification
		/// </summary>
		public virtual void ExplicitVisit(AuditActionSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditSpecificationDetail) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DatabaseAuditAction
		/// </summary>
		public virtual void Visit(DatabaseAuditAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DatabaseAuditAction
		/// </summary>
		public virtual void ExplicitVisit(DatabaseAuditAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditActionGroupReference
		/// </summary>
		public virtual void Visit(AuditActionGroupReference node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditActionGroupReference
		/// </summary>
		public virtual void ExplicitVisit(AuditActionGroupReference node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditSpecificationDetail) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateDatabaseAuditSpecificationStatement
		/// </summary>
		public virtual void Visit(CreateDatabaseAuditSpecificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateDatabaseAuditSpecificationStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateDatabaseAuditSpecificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditSpecificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseAuditSpecificationStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseAuditSpecificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseAuditSpecificationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseAuditSpecificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditSpecificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropDatabaseAuditSpecificationStatement
		/// </summary>
		public virtual void Visit(DropDatabaseAuditSpecificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropDatabaseAuditSpecificationStatement
		/// </summary>
		public virtual void ExplicitVisit(DropDatabaseAuditSpecificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateServerAuditSpecificationStatement
		/// </summary>
		public virtual void Visit(CreateServerAuditSpecificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateServerAuditSpecificationStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateServerAuditSpecificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditSpecificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerAuditSpecificationStatement
		/// </summary>
		public virtual void Visit(AlterServerAuditSpecificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerAuditSpecificationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerAuditSpecificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditSpecificationStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropServerAuditSpecificationStatement
		/// </summary>
		public virtual void Visit(DropServerAuditSpecificationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropServerAuditSpecificationStatement
		/// </summary>
		public virtual void ExplicitVisit(DropServerAuditSpecificationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ServerAuditStatement
		/// </summary>
		public virtual void Visit(ServerAuditStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ServerAuditStatement
		/// </summary>
		public virtual void ExplicitVisit(ServerAuditStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateServerAuditStatement
		/// </summary>
		public virtual void Visit(CreateServerAuditStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateServerAuditStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateServerAuditStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ServerAuditStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerAuditStatement
		/// </summary>
		public virtual void Visit(AlterServerAuditStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerAuditStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerAuditStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ServerAuditStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropServerAuditStatement
		/// </summary>
		public virtual void Visit(DropServerAuditStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropServerAuditStatement
		/// </summary>
		public virtual void ExplicitVisit(DropServerAuditStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditTarget
		/// </summary>
		public virtual void Visit(AuditTarget node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditTarget
		/// </summary>
		public virtual void ExplicitVisit(AuditTarget node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditOption
		/// </summary>
		public virtual void Visit(AuditOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditOption
		/// </summary>
		public virtual void ExplicitVisit(AuditOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for QueueDelayAuditOption
		/// </summary>
		public virtual void Visit(QueueDelayAuditOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for QueueDelayAuditOption
		/// </summary>
		public virtual void ExplicitVisit(QueueDelayAuditOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditGuidAuditOption
		/// </summary>
		public virtual void Visit(AuditGuidAuditOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditGuidAuditOption
		/// </summary>
		public virtual void ExplicitVisit(AuditGuidAuditOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnFailureAuditOption
		/// </summary>
		public virtual void Visit(OnFailureAuditOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnFailureAuditOption
		/// </summary>
		public virtual void ExplicitVisit(OnFailureAuditOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OperatorAuditOption
		/// </summary>
		public virtual void Visit(OperatorAuditOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OperatorAuditOption
		/// </summary>
		public virtual void ExplicitVisit(OperatorAuditOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for StateAuditOption
		/// </summary>
		public virtual void Visit(StateAuditOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for StateAuditOption
		/// </summary>
		public virtual void ExplicitVisit(StateAuditOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AuditTargetOption
		/// </summary>
		public virtual void Visit(AuditTargetOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AuditTargetOption
		/// </summary>
		public virtual void ExplicitVisit(AuditTargetOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxSizeAuditTargetOption
		/// </summary>
		public virtual void Visit(MaxSizeAuditTargetOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxSizeAuditTargetOption
		/// </summary>
		public virtual void ExplicitVisit(MaxSizeAuditTargetOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditTargetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for RetentionDaysAuditTargetOption
		/// </summary>
		public virtual void Visit(RetentionDaysAuditTargetOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for RetentionDaysAuditTargetOption
		/// </summary>
		public virtual void ExplicitVisit(RetentionDaysAuditTargetOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditTargetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxRolloverFilesAuditTargetOption
		/// </summary>
		public virtual void Visit(MaxRolloverFilesAuditTargetOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxRolloverFilesAuditTargetOption
		/// </summary>
		public virtual void ExplicitVisit(MaxRolloverFilesAuditTargetOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditTargetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralAuditTargetOption
		/// </summary>
		public virtual void Visit(LiteralAuditTargetOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralAuditTargetOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralAuditTargetOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditTargetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffAuditTargetOption
		/// </summary>
		public virtual void Visit(OnOffAuditTargetOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffAuditTargetOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffAuditTargetOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AuditTargetOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(DatabaseEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(DatabaseEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateDatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(CreateDatabaseEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateDatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateDatabaseEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseEncryptionKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterDatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(AlterDatabaseEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterDatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterDatabaseEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseEncryptionKeyStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropDatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void Visit(DropDatabaseEncryptionKeyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropDatabaseEncryptionKeyStatement
		/// </summary>
		public virtual void ExplicitVisit(DropDatabaseEncryptionKeyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ResourcePoolStatement
		/// </summary>
		public virtual void Visit(ResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(ResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ResourcePoolParameter
		/// </summary>
		public virtual void Visit(ResourcePoolParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ResourcePoolParameter
		/// </summary>
		public virtual void ExplicitVisit(ResourcePoolParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ResourcePoolAffinitySpecification
		/// </summary>
		public virtual void Visit(ResourcePoolAffinitySpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ResourcePoolAffinitySpecification
		/// </summary>
		public virtual void ExplicitVisit(ResourcePoolAffinitySpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateResourcePoolStatement
		/// </summary>
		public virtual void Visit(CreateResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ResourcePoolStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterResourcePoolStatement
		/// </summary>
		public virtual void Visit(AlterResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ResourcePoolStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropResourcePoolStatement
		/// </summary>
		public virtual void Visit(DropResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(DropResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalResourcePoolStatement
		/// </summary>
		public virtual void Visit(ExternalResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalResourcePoolParameter
		/// </summary>
		public virtual void Visit(ExternalResourcePoolParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalResourcePoolParameter
		/// </summary>
		public virtual void ExplicitVisit(ExternalResourcePoolParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalResourcePoolAffinitySpecification
		/// </summary>
		public virtual void Visit(ExternalResourcePoolAffinitySpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalResourcePoolAffinitySpecification
		/// </summary>
		public virtual void ExplicitVisit(ExternalResourcePoolAffinitySpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalResourcePoolStatement
		/// </summary>
		public virtual void Visit(CreateExternalResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalResourcePoolStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterExternalResourcePoolStatement
		/// </summary>
		public virtual void Visit(AlterExternalResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterExternalResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterExternalResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalResourcePoolStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalResourcePoolStatement
		/// </summary>
		public virtual void Visit(DropExternalResourcePoolStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalResourcePoolStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalResourcePoolStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WorkloadGroupStatement
		/// </summary>
		public virtual void Visit(WorkloadGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WorkloadGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(WorkloadGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WorkloadGroupResourceParameter
		/// </summary>
		public virtual void Visit(WorkloadGroupResourceParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WorkloadGroupResourceParameter
		/// </summary>
		public virtual void ExplicitVisit(WorkloadGroupResourceParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadGroupParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WorkloadGroupImportanceParameter
		/// </summary>
		public virtual void Visit(WorkloadGroupImportanceParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WorkloadGroupImportanceParameter
		/// </summary>
		public virtual void ExplicitVisit(WorkloadGroupImportanceParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadGroupParameter) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WorkloadGroupParameter
		/// </summary>
		public virtual void Visit(WorkloadGroupParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WorkloadGroupParameter
		/// </summary>
		public virtual void ExplicitVisit(WorkloadGroupParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateWorkloadGroupStatement
		/// </summary>
		public virtual void Visit(CreateWorkloadGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateWorkloadGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateWorkloadGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadGroupStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterWorkloadGroupStatement
		/// </summary>
		public virtual void Visit(AlterWorkloadGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterWorkloadGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterWorkloadGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadGroupStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropWorkloadGroupStatement
		/// </summary>
		public virtual void Visit(DropWorkloadGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropWorkloadGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(DropWorkloadGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WorkloadClassifierStatement
		/// </summary>
		public virtual void Visit(WorkloadClassifierStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WorkloadClassifierStatement
		/// </summary>
		public virtual void ExplicitVisit(WorkloadClassifierStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WorkloadClassifierOption
		/// </summary>
		public virtual void Visit(WorkloadClassifierOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WorkloadClassifierOption
		/// </summary>
		public virtual void ExplicitVisit(WorkloadClassifierOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClassifierWorkloadGroupOption
		/// </summary>
		public virtual void Visit(ClassifierWorkloadGroupOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClassifierWorkloadGroupOption
		/// </summary>
		public virtual void ExplicitVisit(ClassifierWorkloadGroupOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClassifierMemberNameOption
		/// </summary>
		public virtual void Visit(ClassifierMemberNameOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClassifierMemberNameOption
		/// </summary>
		public virtual void ExplicitVisit(ClassifierMemberNameOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClassifierWlmLabelOption
		/// </summary>
		public virtual void Visit(ClassifierWlmLabelOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClassifierWlmLabelOption
		/// </summary>
		public virtual void ExplicitVisit(ClassifierWlmLabelOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClassifierImportanceOption
		/// </summary>
		public virtual void Visit(ClassifierImportanceOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClassifierImportanceOption
		/// </summary>
		public virtual void ExplicitVisit(ClassifierImportanceOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClassifierWlmContextOption
		/// </summary>
		public virtual void Visit(ClassifierWlmContextOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClassifierWlmContextOption
		/// </summary>
		public virtual void ExplicitVisit(ClassifierWlmContextOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClassifierStartTimeOption
		/// </summary>
		public virtual void Visit(ClassifierStartTimeOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClassifierStartTimeOption
		/// </summary>
		public virtual void ExplicitVisit(ClassifierStartTimeOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ClassifierEndTimeOption
		/// </summary>
		public virtual void Visit(ClassifierEndTimeOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ClassifierEndTimeOption
		/// </summary>
		public virtual void ExplicitVisit(ClassifierEndTimeOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WlmTimeLiteral
		/// </summary>
		public virtual void Visit(WlmTimeLiteral node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WlmTimeLiteral
		/// </summary>
		public virtual void ExplicitVisit(WlmTimeLiteral node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateWorkloadClassifierStatement
		/// </summary>
		public virtual void Visit(CreateWorkloadClassifierStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateWorkloadClassifierStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateWorkloadClassifierStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((WorkloadClassifierStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropWorkloadClassifierStatement
		/// </summary>
		public virtual void Visit(DropWorkloadClassifierStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropWorkloadClassifierStatement
		/// </summary>
		public virtual void ExplicitVisit(DropWorkloadClassifierStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BrokerPriorityStatement
		/// </summary>
		public virtual void Visit(BrokerPriorityStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BrokerPriorityStatement
		/// </summary>
		public virtual void ExplicitVisit(BrokerPriorityStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BrokerPriorityParameter
		/// </summary>
		public virtual void Visit(BrokerPriorityParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BrokerPriorityParameter
		/// </summary>
		public virtual void ExplicitVisit(BrokerPriorityParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateBrokerPriorityStatement
		/// </summary>
		public virtual void Visit(CreateBrokerPriorityStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateBrokerPriorityStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateBrokerPriorityStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BrokerPriorityStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterBrokerPriorityStatement
		/// </summary>
		public virtual void Visit(AlterBrokerPriorityStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterBrokerPriorityStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterBrokerPriorityStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BrokerPriorityStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropBrokerPriorityStatement
		/// </summary>
		public virtual void Visit(DropBrokerPriorityStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropBrokerPriorityStatement
		/// </summary>
		public virtual void ExplicitVisit(DropBrokerPriorityStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateFullTextStopListStatement
		/// </summary>
		public virtual void Visit(CreateFullTextStopListStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateFullTextStopListStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateFullTextStopListStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterFullTextStopListStatement
		/// </summary>
		public virtual void Visit(AlterFullTextStopListStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterFullTextStopListStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterFullTextStopListStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FullTextStopListAction
		/// </summary>
		public virtual void Visit(FullTextStopListAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FullTextStopListAction
		/// </summary>
		public virtual void ExplicitVisit(FullTextStopListAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropFullTextStopListStatement
		/// </summary>
		public virtual void Visit(DropFullTextStopListStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropFullTextStopListStatement
		/// </summary>
		public virtual void ExplicitVisit(DropFullTextStopListStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateCryptographicProviderStatement
		/// </summary>
		public virtual void Visit(CreateCryptographicProviderStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateCryptographicProviderStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateCryptographicProviderStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterCryptographicProviderStatement
		/// </summary>
		public virtual void Visit(AlterCryptographicProviderStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterCryptographicProviderStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterCryptographicProviderStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropCryptographicProviderStatement
		/// </summary>
		public virtual void Visit(DropCryptographicProviderStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropCryptographicProviderStatement
		/// </summary>
		public virtual void ExplicitVisit(DropCryptographicProviderStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventSessionObjectName
		/// </summary>
		public virtual void Visit(EventSessionObjectName node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventSessionObjectName
		/// </summary>
		public virtual void ExplicitVisit(EventSessionObjectName node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventSessionStatement
		/// </summary>
		public virtual void Visit(EventSessionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventSessionStatement
		/// </summary>
		public virtual void ExplicitVisit(EventSessionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateEventSessionStatement
		/// </summary>
		public virtual void Visit(CreateEventSessionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateEventSessionStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateEventSessionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EventSessionStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventDeclaration
		/// </summary>
		public virtual void Visit(EventDeclaration node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventDeclaration
		/// </summary>
		public virtual void ExplicitVisit(EventDeclaration node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventDeclarationSetParameter
		/// </summary>
		public virtual void Visit(EventDeclarationSetParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventDeclarationSetParameter
		/// </summary>
		public virtual void ExplicitVisit(EventDeclarationSetParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SourceDeclaration
		/// </summary>
		public virtual void Visit(SourceDeclaration node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SourceDeclaration
		/// </summary>
		public virtual void ExplicitVisit(SourceDeclaration node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ScalarExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventDeclarationCompareFunctionParameter
		/// </summary>
		public virtual void Visit(EventDeclarationCompareFunctionParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventDeclarationCompareFunctionParameter
		/// </summary>
		public virtual void ExplicitVisit(EventDeclarationCompareFunctionParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((BooleanExpression) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TargetDeclaration
		/// </summary>
		public virtual void Visit(TargetDeclaration node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TargetDeclaration
		/// </summary>
		public virtual void ExplicitVisit(TargetDeclaration node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SessionOption
		/// </summary>
		public virtual void Visit(SessionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SessionOption
		/// </summary>
		public virtual void ExplicitVisit(SessionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for EventRetentionSessionOption
		/// </summary>
		public virtual void Visit(EventRetentionSessionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for EventRetentionSessionOption
		/// </summary>
		public virtual void ExplicitVisit(EventRetentionSessionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SessionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MemoryPartitionSessionOption
		/// </summary>
		public virtual void Visit(MemoryPartitionSessionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MemoryPartitionSessionOption
		/// </summary>
		public virtual void ExplicitVisit(MemoryPartitionSessionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SessionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralSessionOption
		/// </summary>
		public virtual void Visit(LiteralSessionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralSessionOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralSessionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SessionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for MaxDispatchLatencySessionOption
		/// </summary>
		public virtual void Visit(MaxDispatchLatencySessionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for MaxDispatchLatencySessionOption
		/// </summary>
		public virtual void ExplicitVisit(MaxDispatchLatencySessionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SessionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for OnOffSessionOption
		/// </summary>
		public virtual void Visit(OnOffSessionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for OnOffSessionOption
		/// </summary>
		public virtual void ExplicitVisit(OnOffSessionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SessionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterEventSessionStatement
		/// </summary>
		public virtual void Visit(AlterEventSessionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterEventSessionStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterEventSessionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((EventSessionStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropEventSessionStatement
		/// </summary>
		public virtual void Visit(DropEventSessionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropEventSessionStatement
		/// </summary>
		public virtual void ExplicitVisit(DropEventSessionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterResourceGovernorStatement
		/// </summary>
		public virtual void Visit(AlterResourceGovernorStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterResourceGovernorStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterResourceGovernorStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateSpatialIndexStatement
		/// </summary>
		public virtual void Visit(CreateSpatialIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateSpatialIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateSpatialIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SpatialIndexOption
		/// </summary>
		public virtual void Visit(SpatialIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SpatialIndexOption
		/// </summary>
		public virtual void ExplicitVisit(SpatialIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SpatialIndexRegularOption
		/// </summary>
		public virtual void Visit(SpatialIndexRegularOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SpatialIndexRegularOption
		/// </summary>
		public virtual void ExplicitVisit(SpatialIndexRegularOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SpatialIndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BoundingBoxSpatialIndexOption
		/// </summary>
		public virtual void Visit(BoundingBoxSpatialIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BoundingBoxSpatialIndexOption
		/// </summary>
		public virtual void ExplicitVisit(BoundingBoxSpatialIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SpatialIndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for BoundingBoxParameter
		/// </summary>
		public virtual void Visit(BoundingBoxParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for BoundingBoxParameter
		/// </summary>
		public virtual void ExplicitVisit(BoundingBoxParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GridsSpatialIndexOption
		/// </summary>
		public virtual void Visit(GridsSpatialIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GridsSpatialIndexOption
		/// </summary>
		public virtual void ExplicitVisit(GridsSpatialIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SpatialIndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for GridParameter
		/// </summary>
		public virtual void Visit(GridParameter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for GridParameter
		/// </summary>
		public virtual void ExplicitVisit(GridParameter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CellsPerObjectSpatialIndexOption
		/// </summary>
		public virtual void Visit(CellsPerObjectSpatialIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CellsPerObjectSpatialIndexOption
		/// </summary>
		public virtual void ExplicitVisit(CellsPerObjectSpatialIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((SpatialIndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationStatement
		/// </summary>
		public virtual void Visit(AlterServerConfigurationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ProcessAffinityRange
		/// </summary>
		public virtual void Visit(ProcessAffinityRange node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ProcessAffinityRange
		/// </summary>
		public virtual void ExplicitVisit(ProcessAffinityRange node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((LiteralRange) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationSetExternalAuthenticationStatement
		/// </summary>
		public virtual void Visit(AlterServerConfigurationSetExternalAuthenticationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationSetExternalAuthenticationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationSetExternalAuthenticationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationExternalAuthenticationOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationExternalAuthenticationOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationExternalAuthenticationOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationExternalAuthenticationOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationExternalAuthenticationContainerOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationExternalAuthenticationContainerOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationExternalAuthenticationContainerOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationExternalAuthenticationContainerOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterServerConfigurationExternalAuthenticationOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationSetBufferPoolExtensionStatement
		/// </summary>
		public virtual void Visit(AlterServerConfigurationSetBufferPoolExtensionStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationSetBufferPoolExtensionStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationSetBufferPoolExtensionStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationBufferPoolExtensionOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationBufferPoolExtensionOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationBufferPoolExtensionOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationBufferPoolExtensionOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationBufferPoolExtensionContainerOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationBufferPoolExtensionContainerOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationBufferPoolExtensionContainerOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationBufferPoolExtensionContainerOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterServerConfigurationBufferPoolExtensionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationBufferPoolExtensionSizeOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationBufferPoolExtensionSizeOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationBufferPoolExtensionSizeOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationBufferPoolExtensionSizeOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterServerConfigurationBufferPoolExtensionOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationSetDiagnosticsLogStatement
		/// </summary>
		public virtual void Visit(AlterServerConfigurationSetDiagnosticsLogStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationSetDiagnosticsLogStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationSetDiagnosticsLogStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationDiagnosticsLogOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationDiagnosticsLogOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationDiagnosticsLogOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationDiagnosticsLogOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationDiagnosticsLogMaxSizeOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationDiagnosticsLogMaxSizeOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationDiagnosticsLogMaxSizeOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationDiagnosticsLogMaxSizeOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterServerConfigurationDiagnosticsLogOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationSetFailoverClusterPropertyStatement
		/// </summary>
		public virtual void Visit(AlterServerConfigurationSetFailoverClusterPropertyStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationSetFailoverClusterPropertyStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationSetFailoverClusterPropertyStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationFailoverClusterPropertyOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationFailoverClusterPropertyOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationFailoverClusterPropertyOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationFailoverClusterPropertyOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationSetHadrClusterStatement
		/// </summary>
		public virtual void Visit(AlterServerConfigurationSetHadrClusterStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationSetHadrClusterStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationSetHadrClusterStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationHadrClusterOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationHadrClusterOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationHadrClusterOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationHadrClusterOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationSetSoftNumaStatement
		/// </summary>
		public virtual void Visit(AlterServerConfigurationSetSoftNumaStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationSetSoftNumaStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationSetSoftNumaStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterServerConfigurationSoftNumaOption
		/// </summary>
		public virtual void Visit(AlterServerConfigurationSoftNumaOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterServerConfigurationSoftNumaOption
		/// </summary>
		public virtual void ExplicitVisit(AlterServerConfigurationSoftNumaOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AvailabilityGroupStatement
		/// </summary>
		public virtual void Visit(AvailabilityGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AvailabilityGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(AvailabilityGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateAvailabilityGroupStatement
		/// </summary>
		public virtual void Visit(CreateAvailabilityGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateAvailabilityGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateAvailabilityGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityGroupStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterAvailabilityGroupStatement
		/// </summary>
		public virtual void Visit(AlterAvailabilityGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterAvailabilityGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterAvailabilityGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityGroupStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AvailabilityReplica
		/// </summary>
		public virtual void Visit(AvailabilityReplica node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AvailabilityReplica
		/// </summary>
		public virtual void ExplicitVisit(AvailabilityReplica node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AvailabilityReplicaOption
		/// </summary>
		public virtual void Visit(AvailabilityReplicaOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AvailabilityReplicaOption
		/// </summary>
		public virtual void ExplicitVisit(AvailabilityReplicaOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralReplicaOption
		/// </summary>
		public virtual void Visit(LiteralReplicaOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralReplicaOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralReplicaOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityReplicaOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AvailabilityModeReplicaOption
		/// </summary>
		public virtual void Visit(AvailabilityModeReplicaOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AvailabilityModeReplicaOption
		/// </summary>
		public virtual void ExplicitVisit(AvailabilityModeReplicaOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityReplicaOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for FailoverModeReplicaOption
		/// </summary>
		public virtual void Visit(FailoverModeReplicaOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for FailoverModeReplicaOption
		/// </summary>
		public virtual void ExplicitVisit(FailoverModeReplicaOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityReplicaOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for PrimaryRoleReplicaOption
		/// </summary>
		public virtual void Visit(PrimaryRoleReplicaOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for PrimaryRoleReplicaOption
		/// </summary>
		public virtual void ExplicitVisit(PrimaryRoleReplicaOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityReplicaOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SecondaryRoleReplicaOption
		/// </summary>
		public virtual void Visit(SecondaryRoleReplicaOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SecondaryRoleReplicaOption
		/// </summary>
		public virtual void ExplicitVisit(SecondaryRoleReplicaOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityReplicaOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AvailabilityGroupOption
		/// </summary>
		public virtual void Visit(AvailabilityGroupOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AvailabilityGroupOption
		/// </summary>
		public virtual void ExplicitVisit(AvailabilityGroupOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for LiteralAvailabilityGroupOption
		/// </summary>
		public virtual void Visit(LiteralAvailabilityGroupOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for LiteralAvailabilityGroupOption
		/// </summary>
		public virtual void ExplicitVisit(LiteralAvailabilityGroupOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AvailabilityGroupOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterAvailabilityGroupAction
		/// </summary>
		public virtual void Visit(AlterAvailabilityGroupAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterAvailabilityGroupAction
		/// </summary>
		public virtual void ExplicitVisit(AlterAvailabilityGroupAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterAvailabilityGroupFailoverAction
		/// </summary>
		public virtual void Visit(AlterAvailabilityGroupFailoverAction node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterAvailabilityGroupFailoverAction
		/// </summary>
		public virtual void ExplicitVisit(AlterAvailabilityGroupFailoverAction node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((AlterAvailabilityGroupAction) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterAvailabilityGroupFailoverOption
		/// </summary>
		public virtual void Visit(AlterAvailabilityGroupFailoverOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterAvailabilityGroupFailoverOption
		/// </summary>
		public virtual void ExplicitVisit(AlterAvailabilityGroupFailoverOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropAvailabilityGroupStatement
		/// </summary>
		public virtual void Visit(DropAvailabilityGroupStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropAvailabilityGroupStatement
		/// </summary>
		public virtual void ExplicitVisit(DropAvailabilityGroupStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateFederationStatement
		/// </summary>
		public virtual void Visit(CreateFederationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateFederationStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateFederationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterFederationStatement
		/// </summary>
		public virtual void Visit(AlterFederationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterFederationStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterFederationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropFederationStatement
		/// </summary>
		public virtual void Visit(DropFederationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropFederationStatement
		/// </summary>
		public virtual void ExplicitVisit(DropFederationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DropUnownedObjectStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for UseFederationStatement
		/// </summary>
		public virtual void Visit(UseFederationStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for UseFederationStatement
		/// </summary>
		public virtual void ExplicitVisit(UseFederationStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DiskStatement
		/// </summary>
		public virtual void Visit(DiskStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DiskStatement
		/// </summary>
		public virtual void ExplicitVisit(DiskStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DiskStatementOption
		/// </summary>
		public virtual void Visit(DiskStatementOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DiskStatementOption
		/// </summary>
		public virtual void ExplicitVisit(DiskStatementOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateColumnStoreIndexStatement
		/// </summary>
		public virtual void Visit(CreateColumnStoreIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateColumnStoreIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateColumnStoreIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateJsonIndexStatement
		/// </summary>
		public virtual void Visit(CreateJsonIndexStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateJsonIndexStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateJsonIndexStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WindowFrameClause
		/// </summary>
		public virtual void Visit(WindowFrameClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WindowFrameClause
		/// </summary>
		public virtual void ExplicitVisit(WindowFrameClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WindowDelimiter
		/// </summary>
		public virtual void Visit(WindowDelimiter node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WindowDelimiter
		/// </summary>
		public virtual void ExplicitVisit(WindowDelimiter node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for WithinGroupClause
		/// </summary>
		public virtual void Visit(WithinGroupClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for WithinGroupClause
		/// </summary>
		public virtual void ExplicitVisit(WithinGroupClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for SelectiveXmlIndexPromotedPath
		/// </summary>
		public virtual void Visit(SelectiveXmlIndexPromotedPath node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for SelectiveXmlIndexPromotedPath
		/// </summary>
		public virtual void ExplicitVisit(SelectiveXmlIndexPromotedPath node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for TemporalClause
		/// </summary>
		public virtual void Visit(TemporalClause node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for TemporalClause
		/// </summary>
		public virtual void ExplicitVisit(TemporalClause node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CompressionDelayIndexOption
		/// </summary>
		public virtual void Visit(CompressionDelayIndexOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CompressionDelayIndexOption
		/// </summary>
		public virtual void ExplicitVisit(CompressionDelayIndexOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((IndexOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalLibraryStatement
		/// </summary>
		public virtual void Visit(ExternalLibraryStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalLibraryStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalLibraryStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalLibraryStatement
		/// </summary>
		public virtual void Visit(CreateExternalLibraryStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalLibraryStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalLibraryStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalLibraryStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterExternalLibraryStatement
		/// </summary>
		public virtual void Visit(AlterExternalLibraryStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterExternalLibraryStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterExternalLibraryStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalLibraryStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalLibraryFileOption
		/// </summary>
		public virtual void Visit(ExternalLibraryFileOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalLibraryFileOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalLibraryFileOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalLibraryStatement
		/// </summary>
		public virtual void Visit(DropExternalLibraryStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalLibraryStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalLibraryStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalLanguageStatement
		/// </summary>
		public virtual void Visit(ExternalLanguageStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalLanguageStatement
		/// </summary>
		public virtual void ExplicitVisit(ExternalLanguageStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for CreateExternalLanguageStatement
		/// </summary>
		public virtual void Visit(CreateExternalLanguageStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for CreateExternalLanguageStatement
		/// </summary>
		public virtual void ExplicitVisit(CreateExternalLanguageStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalLanguageStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for AlterExternalLanguageStatement
		/// </summary>
		public virtual void Visit(AlterExternalLanguageStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for AlterExternalLanguageStatement
		/// </summary>
		public virtual void ExplicitVisit(AlterExternalLanguageStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((ExternalLanguageStatement) node);
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ExternalLanguageFileOption
		/// </summary>
		public virtual void Visit(ExternalLanguageFileOption node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ExternalLanguageFileOption
		/// </summary>
		public virtual void ExplicitVisit(ExternalLanguageFileOption node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for DropExternalLanguageStatement
		/// </summary>
		public virtual void Visit(DropExternalLanguageStatement node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for DropExternalLanguageStatement
		/// </summary>
		public virtual void ExplicitVisit(DropExternalLanguageStatement node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((TSqlStatement) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

		/// <summary>
		/// Visitor for ElasticPoolSpecification
		/// </summary>
		public virtual void Visit(ElasticPoolSpecification node)
		{
			if (!this.VisitBaseType)
			{
				this.Visit((TSqlFragment) node);
			}
		}

		/// <summary>
		/// Explicit Visitor for ElasticPoolSpecification
		/// </summary>
		public virtual void ExplicitVisit(ElasticPoolSpecification node)
		{
			if (this.VisitBaseType)
			{
				this.Visit((DatabaseOption) node);
				this.Visit((TSqlFragment) node);
			}

			this.Visit(node);
			node.AcceptChildren(this);
		}

	}
}

