CREATE VECTOR INDEX IX_Vector_Basic
    ON dbo.Documents (VectorData);

CREATE VECTOR INDEX IX_Vector_Cosine
    ON dbo.Documents (VectorData) WITH (METRIC = 'COSINE');

CREATE VECTOR INDEX IX_Vector_Dot
    ON dbo.Documents (VectorData) WITH (METRIC = 'DOT');

CREATE VECTOR INDEX IX_Vector_Euclidean
    ON dbo.Documents (VectorData) WITH (METRIC = 'EUCLIDEAN');

CREATE VECTOR INDEX IX_Vector_DiskANN
    ON dbo.Documents (VectorData) WITH (TYPE = 'DISKANN');

CREATE VECTOR INDEX IX_Vector_Complete
    ON dbo.Documents (VectorData) WITH (METRIC = 'COSINE', TYPE = 'DISKANN');

CREATE VECTOR INDEX IX_Vector_MaxDop
    ON dbo.Documents (VectorData) WITH (MAXDOP = 4);

CREATE VECTOR INDEX IX_Vector_AllOptions
    ON dbo.Documents (VectorData) WITH (METRIC = 'DOT', TYPE = 'DISKANN', MAXDOP = 8);

CREATE VECTOR INDEX IX_Vector_Schema
    ON MySchema.MyTable (VectorColumn) WITH (METRIC = 'EUCLIDEAN');

CREATE VECTOR INDEX [IX Vector Index]
    ON [dbo].[Documents] ([Vector Data]) WITH (METRIC = 'COSINE');

CREATE VECTOR INDEX IX_Vector_Filegroup
    ON dbo.Documents (VectorData) WITH (METRIC = 'COSINE')
    ON [PRIMARY];

CREATE VECTOR INDEX IX_Vector_DefaultFG
    ON dbo.Documents (VectorData) WITH (METRIC = 'DOT')
    ON "default";