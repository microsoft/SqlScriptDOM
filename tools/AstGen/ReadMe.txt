AstGen tool generates AST classes (spec or implementation - depends on /spec command line switch).
It also generates a visitor (see Visitor design pattern) to walk generated classes into separate file.

It reads input from an XML file in the following format:

<?xml version="1.0" ?> 
<Types>
   <Class Name = "NullableConstraint"> 
	<Member Name="Nullable" Type = "bool"/>
	<Implements Interface="ICollationSetter"/>
	<ModelBuilderSubstitution Type="string" Handler="string" VisitChildren="bool">
   </Class> 
	<Interface Name="ICollationSetter">
		<Member Name="Collation" Type="Identifier" Summary="Collation identifier"/>
	</Interface>
</Types> 

Implementation classes are written into <output code file>. 
Spec classes are written into into <output spec file>.
Needless to say, multiple classes and multiple members per class are supported.

Class attributes:
'Name' - mandatory
'Summary' - optional, default value is empty string
'Base' - optional, base type for class, default is TSqlFragment
'Abstract' - optional, default value is false 
		setting this attributes adds 'abstract' keyword to generated class definition
'IsSql80' - optional, default value is true, means, that class represents supported in SQL 2000 construct
'IsSql90' - optional, default value is false, means, that class represents supported in SQL 2005 construct

Interface attributes:
'Name' - mandatory
'Summary' - optional, default value is empty string
'Base' - optional, base type for interface, default is empty string
		
Member attributes:
'Name' - mandatory
'Type' - mandatory
'Summary' - optional, default value is empty string
'Collection' - optional, default value is false
		setting to true causes generation of collection property instead of regular one
'GenerateAdder' - optional, default value is true. Controls generation of Add<member name> for collection members. No effect if member is not a collection.
'GenerateUpdatePositionInfoCall' - optional, default is true
		Causes skipping generation of UpdatePositionInfo call in generated properties (which can't be generated for bool, int and enum types anyway)
'IsSql80' - optional, default value is true, means, that member represents supported in SQL 2000 construct
'IsSql90' - optional, default value is false, means, that member represents supported in SQL 2005 construct

ModelBuilderSubstitution attributes:
'Type' - Mandatory, name of the sub-visitor (will be generated)
'Handler' - Mandatory, Name of the handler for the sub-visitor once it has returned (this method will NOT be generated, just called)
'VisitChildren' - optional, true by default (specifies whether or not the ExplicitVisit method will be overridden in ModelBuilder
		
It is possible to specify summary comment as sub-node instead of attribute for both classes and members:
   <Class Name = "NullableConstraint"> 
	<Summary>
		Some summary
		(on multiple lines)
	</Summary>
	<Member Name="Nullable" Type = "bool" GenerateUpdatePositionInfoCall="false"/>
   </Class> 

So, here is another example which uses all the attributes:

<?xml version="1.0" ?> 
<Types>
   <Class Name = "NullableConstraint"> 
	<Member Name="Nullable" Type = "bool" GenerateUpdatePositionInfoCall="false"/>
   </Class> 
   <Class Name = "Column" Summary = "Represents a column that is used inside an expression.">
	<Member Name="Identifiers" Type="Identifier" Collection = "true"/>
	<Member Name="ColumnType" Type="ColumnType" Summary="Type of the column."/>
   </Class>
   <Class Name = "SimpleTableSource" BaseType = "SelectTableSource" Abstract="true"> 
	<Member Name="Alias" Type="Identifier" Summary = "As identifier. May be null."/>
   </Class> 
</Types> 
