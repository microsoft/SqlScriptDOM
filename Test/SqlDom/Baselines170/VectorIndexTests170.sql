CREATE VECTOR INDEX IX_Vector_Basic
    ON dbo.Documents (VectorData);

CREATE VECTOR INDEX IX_Vector_Cosine
    ON dbo.Documents (VectorData) WITH (METRIC = 'cosine');

CREATE VECTOR INDEX IX_Vector_Dot
    ON dbo.Documents (VectorData) WITH (METRIC = 'dot');

CREATE VECTOR INDEX IX_Vector_Euclidean
    ON dbo.Documents (VectorData) WITH (METRIC = 'euclidean');

CREATE VECTOR INDEX IX_Vector_DiskANN
    ON dbo.Documents (VectorData) WITH (TYPE = 'DiskANN');

CREATE VECTOR INDEX IX_Vector_Complete
    ON dbo.Documents (VectorData) WITH (METRIC = 'cosine', TYPE = 'DiskANN');

CREATE VECTOR INDEX IX_Vector_MaxDop
    ON dbo.Documents (VectorData) WITH (MAXDOP = 4);

CREATE VECTOR INDEX IX_Vector_AllOptions
    ON dbo.Documents (VectorData) WITH (METRIC = 'dot', TYPE = 'DiskANN', MAXDOP = 8);

CREATE VECTOR INDEX IX_Vector_Schema
    ON MySchema.MyTable (VectorColumn) WITH (METRIC = 'euclidean');

CREATE VECTOR INDEX [IX Vector Index]
    ON [dbo].[Documents] ([Vector Data]) WITH (METRIC = 'cosine');

CREATE VECTOR INDEX IX_Vector_Filegroup
    ON dbo.Documents (VectorData) WITH (METRIC = 'cosine')
    ON [PRIMARY];

CREATE VECTOR INDEX IX_Vector_DefaultFG
    ON dbo.Documents (VectorData) WITH (METRIC = 'dot')
    ON "default";