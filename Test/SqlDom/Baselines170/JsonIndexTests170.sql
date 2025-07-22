CREATE JSON INDEX IX_JSON_Basic
    ON dbo.Users (JsonData);

CREATE JSON INDEX IX_JSON_SinglePath
    ON dbo.Users (JsonData)
    FOR ('$.name');

CREATE JSON INDEX IX_JSON_MultiplePaths
    ON dbo.Users (JsonData)
    FOR ('$.name', '$.email', '$.age');

CREATE JSON INDEX IX_JSON_WithOptions
    ON dbo.Users (JsonData) WITH (FILLFACTOR = 90, ONLINE = OFF);

CREATE JSON INDEX IX_JSON_Complete
    ON dbo.Users (JsonData)
    FOR ('$.profile.name', '$.profile.email') WITH (MAXDOP = 4, DATA_COMPRESSION = ROW);

CREATE JSON INDEX IX_JSON_Filegroup
    ON dbo.Users (JsonData)
    ON [PRIMARY];

CREATE JSON INDEX IX_JSON_Full
    ON dbo.Users (JsonData)
    FOR ('$.orders[*].amount', '$.orders[*].date') WITH (DROP_EXISTING = OFF, ALLOW_ROW_LOCKS = ON)
    ON [JsonIndexes];

CREATE JSON INDEX IX_JSON_Schema
    ON MySchema.MyTable (JsonColumn)
    FOR ('$.properties.value');

CREATE JSON INDEX [IX JSON Index]
    ON [dbo].[Users] ([Json Data])
    FOR ('$.data.attributes');

CREATE JSON INDEX IX_JSON_Complex
    ON dbo.Documents (Content)
    FOR ('$.metadata.title', '$.content.sections[*].text', '$.tags[*]');